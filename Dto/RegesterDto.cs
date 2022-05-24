using System.ComponentModel.DataAnnotations;

namespace HospitalManagmentSytem.Dto
{
    public class RegesterDto
    {
        
        [Required]
        public string username { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string phoneNumber { get; set; }
        [Required]
        public string confirmPassword { get; set; }
        [Required]
        public string password { get; set; }


    }
}
