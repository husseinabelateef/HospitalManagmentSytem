using HospitalManagmentSytem.Models;
using System.Collections.Generic;

namespace HospitalManagmentSytem.Repositorys
{
    public interface IAppointment:ICrudRepository<appointment , int>
    {
        public List<appointment> patientAppoinments(string ssn);
        public string cancelAppointment(string ssn , int appointmentId);
        public string confirmAppointment(string ssn, int appointmentId);
    }
}
