using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cw10.Models;
using System.Reflection;
using Cw10.Services;
using Cw10.ExtensionMethods;

namespace Cw10.Controllers
{
    [ApiController]
    [Route("api/studentEntity")]
    public class StudentsEntityController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStudents()
        {
            var db = new s17057Context();

            var res = db.StudentApbd.Select(stud => new
            {
                IndexNumber = stud.IndexNumber,
                FirstName = stud.FirstName,
                LastName = stud.LastName,
                BirthDate = stud.BirthDate,
                IdEnrollment = stud.IdEnrollment

            }).ToList();
            return Ok(res);
        }
        [Route("UpdateStudent")]
        [HttpPost("{Index}")]
        public IActionResult UpdateStudent([FromQuery] String Index, [FromBody] StudentApbd req)
        {
            var t1 = Index;
            var t2 = req;
            var db = new s17057Context();
            try
            {
                var stud = db.StudentApbd.Where(stud => stud.IndexNumber.Equals(Index)).First();
                //update
                stud.SetProperty("FirstName", req.FirstName);
                stud.SetProperty("IndexNumber", req.IndexNumber);
                stud.SetProperty("LastName", req.LastName);
                stud.SetProperty("BirthDate", req.BirthDate);
                stud.SetProperty("IdEnrollment", req.IdEnrollment);
                if(req.Pswd != null)
                {
                    var salt = stud.Salt;
                    stud.SetProperty("Pswd", PasswordHelper.GenerateSaltedHash(req.Pswd, salt));
                }
                db.SaveChanges();
                return Ok("Dane zaktualizowane pomyślnie!");
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest("Niepoprawny numer indexu!");
            }

        }
        [Route("DeleteStudent")]
        [HttpDelete("{index}")]
        public IActionResult DeleteStudent([FromQuery] String index)
        {
            var db = new s17057Context();
            var stud = db.StudentApbd.Where(stud => stud.IndexNumber.Equals(index)).First();
            if (stud == null)
            {
                return BadRequest("Niepoprawny numer indexu!");
            }
            else
            {
                //delete,
                db.Remove(stud);
                db.SaveChanges();
                return Ok("Dane usunięte pomyślnie!");
            }
        }
        [Route("AddStudent")]
        [HttpPost]
        public IActionResult InsertStudent(StudentApbd req)
        {
            var db = new s17057Context();
            var stud = new StudentApbd();
            stud.SetProperty("FirstName", req.FirstName);
            stud.SetProperty("IndexNumber", req.IndexNumber);
            stud.SetProperty("LastName", req.LastName);
            stud.SetProperty("BirthDate", req.BirthDate);
            stud.SetProperty("IdEnrollment", req.IdEnrollment);

            var salt = PasswordHelper.CreateSalt();
            stud.SetProperty("Salt", salt);
            stud.SetProperty("Pswd", PasswordHelper.GenerateSaltedHash(req.Pswd, salt));
            db.StudentApbd.Add(req);
            db.SaveChanges();
            return Ok("Insert zakończony pomyślnie");
        }
    }
}