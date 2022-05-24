using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HospitalManagmentSytem.Models
{
    public class Patient
    {
        [Key]
        public string SSN { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Insuranceid { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        [JsonIgnore]
        public virtual ICollection<appointment> Appointments { get; set; }
    }
}
