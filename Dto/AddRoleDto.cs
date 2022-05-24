using System.ComponentModel.DataAnnotations;

namespace HospitalManagmentSytem.Dto
{
    public class AddRoleDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
