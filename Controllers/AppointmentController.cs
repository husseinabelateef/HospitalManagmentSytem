using HospitalManagmentSytem.Models;
using HospitalManagmentSytem.Repositorys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HospitalManagmentSytem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointment appointmentRepo;

        public AppointmentController(IAppointment appointmentRepo)
        {
            this.appointmentRepo = appointmentRepo;
        }
        
        [HttpGet]
        [Authorize]
        public IActionResult getAll()
        {
            try
            {
                var list = appointmentRepo.GetAll();
                if (list == null)
                {
                    return BadRequest("Empty appointmen");
                }
                return Ok(list);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpGet("{id:int}", Name = "getOne")]//api/appointment/3
        public IActionResult getByID(int id)
        {
            appointment appointment = this.appointmentRepo.GetById(id);
            if (appointment == null)
            {
                return BadRequest("Empty appointment");
            }
            return Ok(appointment);
        }
        [Authorize]
        [HttpPost]
        public IActionResult New(appointment appointment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var res = appointmentRepo.create(appointment);

                    string url = Url.Link("getOne", new { id = appointment.Id });
                    return Created(url, appointment);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(ModelState);
        }
        [Authorize]
        [HttpPatch]
        public IActionResult Update(int id, appointment appointment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var res = appointmentRepo.update(id, appointment);
                    if (res == 0)
                        return BadRequest("Not Found appointment");

                    return StatusCode(204, "Data Saved");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(ModelState);
        }
        [Authorize]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var result = this.appointmentRepo.delete(id);
                if (result > 0)
                    return Ok("Deleted SucessFully");
                return BadRequest("couldn't delete");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("patientAppointments")]
        public IActionResult patienAppointments (string ssn)
        {
           var res =  this.appointmentRepo.patientAppoinments(ssn);
            if (res.Count > 0)
                return Ok(res);
            return BadRequest("Not Assigned to Appointments");
        }
        [HttpGet("confirmAppointment")]
        public IActionResult confirmAppointment(string ssn ,int appointmentId)
        {
            var res = this.appointmentRepo.confirmAppointment(ssn,appointmentId);
            
                return Ok(res);
            
        }
        [HttpGet("cancelAppointment")]
        public IActionResult cancelAppointment(string ssn, int appointmentId)
        {
            var res = this.appointmentRepo.cancelAppointment(ssn, appointmentId);

            return Ok(res);

        }
    }
}
