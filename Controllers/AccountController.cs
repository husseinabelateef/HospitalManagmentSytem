using HospitalManagmentSytem.Repositorys;
using HospitalManagmentSytem.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using HospitalManagmentSytem.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;

namespace HospitalManagmentSytem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthRepository _authService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IEmailSender emailSender;

        public AccountController(IAuthRepository authService,
            UserManager<User> userManager, RoleManager<IdentityRole> roleManager
            , IEmailSender emailSender)
        {
            _authService = authService;
            _userManager = userManager;
            this.roleManager = roleManager;
            this.emailSender = emailSender;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegesterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authService.RegisterAsync(model);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> GetTokenAsync([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }
              
        
        
        [HttpGet]
        public async Task<IActionResult> getAllUsers()
        {
            var result = await _userManager.Users.ToListAsync();
            return Ok(result);
        }
        [HttpPost("/forgetPassword")]
        public async Task<IActionResult> ForgetPassword( ForgetPassWordDto Input)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist 
                    return BadRequest("User Doesn't Exist");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = $"url +{code}";

                await emailSender.SendEmailAsync(
                    Input.Email,
                    "Reset Password",
                    $"Please reset your password by {code}");

                return Ok("Email Sent Succesfully");
            }

            return BadRequest("Email not Valid");
        }
        [HttpPost("/resetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Password or Email Not Formated Well");
            }

            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return BadRequest("the user does not exist");
            }

            var result = await _userManager.ResetPasswordAsync(user, input.Code, input.Password);
            if (result.Succeeded)
            {
                return Ok("Reset Password sucessfully");
            }
            string errorMsg = "";
            foreach (var error in result.Errors)
            {
                errorMsg+= error.Description;
            }
            return BadRequest(errorMsg);
        }
    }
    
    }

