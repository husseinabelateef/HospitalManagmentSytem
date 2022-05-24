using System.Threading.Tasks;
using HospitalManagmentSytem.Dto;
namespace HospitalManagmentSytem.Repositorys
{
    public interface IAuthRepository
    {
        Task<AuthDto> RegisterAsync(RegesterDto model);
        Task<AuthDto> LoginAsync(LoginDto model);

    }
}
