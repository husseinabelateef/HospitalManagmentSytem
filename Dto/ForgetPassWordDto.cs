using System.ComponentModel.DataAnnotations;

namespace HospitalManagmentSytem.Dto
{
    public class ForgetPassWordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
