using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace HospitalManagmentSytem.Models
{
    public class appointment
    {
        public int Id { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsConfirmed { get; set; }
        [ForeignKey("Patient")]
        public string patientId { get; set; }
        [JsonIgnore]
        public virtual  Patient Patient { get; set; }
    }
}
