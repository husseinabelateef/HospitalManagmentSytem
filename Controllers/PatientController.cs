using HospitalManagmentSytem.Models;
using HospitalManagmentSytem.Repositorys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;
namespace HospitalManagmentSytem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class PatientController : ControllerBase
    {
        private readonly IPatient patientRepo;

        public PatientController(IPatient patientRepo)
        {
            this.patientRepo = patientRepo;
        }
        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult getAll()
        {
            try {
                var list = patientRepo.GetAll();
                if (list == null)
                {
                    return BadRequest("Empty patient");
                }
                return Ok(list);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "User")]
        [HttpGet("{ssn}", Name = "getOneRoute")]//api/Patient/3
       
        public IActionResult getByID(string ssn)
        {
            Patient patient = this.patientRepo.GetById(ssn);
            if (patient == null)
            {
                return BadRequest("Empty patient");
            }
            return Ok(patient);
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        public IActionResult New(Patient patient)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var res = patientRepo.create(patient); 
                    string url = Url.Link("getOneRoute", new { ssn = patient.SSN });
                    return Created(url, patient);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(ModelState);
        }
        [Authorize(Roles = "User")]
        [HttpPatch]
        public IActionResult Update(string id , Patient patient)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var res = patientRepo.update(id, patient);
                    if(res == 0)
                        return BadRequest("Not Found Patiet");

                    return StatusCode(204, "Data Saved");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(ModelState);
        }
        [Authorize(Roles ="User")]
        [HttpDelete]
        public IActionResult Delete(string id)
        {
            try
            {
               var result =  this.patientRepo.delete(id);
                if (result > 0)
                    return Ok("Deleted SucessFully");
                return BadRequest("couldn't delete");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
