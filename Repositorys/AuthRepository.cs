using HospitalManagmentSytem.Dto;
using HospitalManagmentSytem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagmentSytem.Repositorys
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<User> _userManager;
        

        public AuthRepository(IConfiguration Configuration,
            UserManager<User> userManager )
        {
            configuration = Configuration;
            _userManager = userManager;
           
        }

        public async Task<AuthDto> LoginAsync(LoginDto model)
        {
            var authModel = new AuthDto();

            var user = await _userManager.FindByEmailAsync(model.email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.password))
            {
                authModel.Message = "Email or Password is incorrect!";
                return authModel;
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.username = user.UserName;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();

            return authModel;
        }

        public async Task<AuthDto> RegisterAsync(RegesterDto model)
        {
            if (await _userManager.FindByEmailAsync(model.email) is not null)
                return new AuthDto { Message = "Email is already registered!" };
            if (await _userManager.FindByNameAsync(model.username) is not null)
                return new AuthDto { Message = "UserName is already registered!" };
            var user = new User
            {
                
                UserName = model.username,
                Email = model.email,
                PhoneNumber = model.phoneNumber,
                
            };
            var result = await _userManager.CreateAsync(user, model.password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return new AuthDto { Message = errors };
            }
            await _userManager.AddToRoleAsync(user, "User");
            var jwtSecurityToken = await CreateJwtToken(user);
            return new AuthDto
            {
                Email = user.Email,
                username  = user.UserName,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
               
            };
        }

        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddDays(double.Parse(configuration["JWT:DurationInDays"])),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
