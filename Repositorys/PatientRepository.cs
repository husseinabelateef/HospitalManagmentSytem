using HospitalManagmentSytem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HospitalManagmentSytem.Repositorys
{
    public class PatientRepository : IPatient
    {
        private readonly HospitalContext context;

        public PatientRepository(HospitalContext context)
        {
            this.context = context;
        }
        public int create(Patient item)
        {
            try
            {
                context.Patients.Add(item);
                return context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DbUpdateException(ex.Message);
            }
        }

        public int delete(string id)
        {
            try
            {
                var item = this.GetById(id);
                context.Patients.Remove(item);
                return context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DbUpdateException(ex.Message);
            }
        }

        public List<Patient> GetAll()
        {
            try
            {
                return context.Patients.ToList();

            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException(ex.Message);
            }

        }

        public Patient GetById(string id)
        {
            return context.Patients.FirstOrDefault(x=>x.SSN == id);
        }

        public int update(string id, Patient item)
        {
            var old = this.GetById(id);
            try
            {
                if (old == null)
                    return 0;
                context.Update(old);
                old.Phone = item.Phone;
                old.Insuranceid = item.Insuranceid;
                old.Address = item.Address;
                old.Name = item.Name;
                return context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DbUpdateException(ex.Message);
            }
        }
    }
}
