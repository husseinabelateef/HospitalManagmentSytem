using HospitalManagmentSytem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HospitalManagmentSytem.Repositorys
{
    public class AppointmentRepository : IAppointment
    {
        private readonly HospitalContext context;
        private readonly IPatient patienRepo;

        public AppointmentRepository(HospitalContext context , IPatient patienRepo)
        {
            this.context = context;
            this.patienRepo = patienRepo;
        }

        public string cancelAppointment(string ssn, int appointmentId)
        {
            var patient = patienRepo.GetById(ssn);
            if(patient == null)
            {
                return "Patient doesn't Exist";
            }
            var appoinment = this.GetById(appointmentId);
            if (appoinment == null)
                return "appointment doesn't Exist";
            if(appoinment.StartDate > DateTime.Now && appoinment.StartDate.Day > DateTime.Now.Day)
            {
                appoinment.IsConfirmed = false;
                context.Update(appoinment);
                context.SaveChanges();
                return "canceled Successfully";
            }
            return "Cann't cancel appointment at the same day";
        }

        public string confirmAppointment(string ssn, int appointmentId)
        {
            var patient = patienRepo.GetById(ssn);
            if (patient == null)
            {
                return "Patient doesn't Exist";
            }
            var appoinment = this.GetById(appointmentId);
            if (appoinment == null)
                return "appointment doesn't Exist";

            appoinment.IsConfirmed = true;
            context.Update(appoinment);
            context.SaveChanges();
            return "Confirmed Successfully";
        }

        public int create(appointment item)
        {
            try
            {
                context.appointments.Add(item);
                return context.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new DbUpdateException(ex.Message);
            }
        }

        public int delete(int id)
        {
            try
            {
                var item = this.GetById(id);
                context.appointments.Remove(item);
                return context.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new DbUpdateException(ex.Message);
            }
        }

        public List<appointment> GetAll()
        {
            try
            {
                return context.appointments.ToList();

            }
            catch(ArgumentNullException ex)
            {
                throw new ArgumentNullException(ex.Message);
            }
            
        }

        public appointment GetById(int id)
        {
                return context.appointments.FirstOrDefault(x=> x.Id == id);
        }

        public List<appointment> patientAppoinments(string ssn)
        {
               return context.appointments.Where(x => x.patientId == ssn).ToList(); 
        }

        public int update(int id, appointment item)
        {
            var old = this.GetById(id);
            try
            {
                if (old == null)
                    return 0;
                context.Update(old);
                old.StartDate = item.StartDate;
                old.EndDate = item.EndDate;
                old.IsConfirmed = item.IsConfirmed;
                old.patientId = item.patientId;
                return context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DbUpdateException(ex.Message);
            }
        }
    }
}
