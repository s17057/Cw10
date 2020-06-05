using Cw10.DTO.Requests;
using Cw10.DTO.Responses;
using Cw10.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Cw10.ExtensionMethods;

namespace Cw10.Services
{
    public class SqlServerStudentDbService : IStudentDbService
    {
        public EnrollStudentResponse EnrollStudent(EnrollStudentRequest request)
        {
            var response = new EnrollStudentResponse();
            var db = new s17057Context();
            db.Database.BeginTransaction();
            int study;
            try
            {
                study = db.Studies.Where(s => s.Name.Equals(request.Studies)).Select(s => s.IdStudy).First();
            }
            catch (InvalidOperationException ex)
            {
                db.Database.RollbackTransaction();
                throw new ArgumentException("Studia " + request.Studies + " nie isnieją");
            }
            response.IdStudies = study;
            response.Semester = 1;
            response.IndexNumber = request.IndexNumber;
            try
            {
                var enrollment = db.Enrollment.Where(e => e.Semester == 1 && e.IdStudy == study).Select(e => new { 
                    id = e.IdEnrollment,
                    startDate = e.StartDate
                }).First();
                response.IdEnrollment = enrollment.id;
                response.StartDate = enrollment.startDate.Value;
            }
            catch (InvalidOperationException ex)
            {
                response.IdEnrollment = db.Enrollment.Max(e => e.IdEnrollment) + 1;
                response.StartDate = DateTime.Now;
                var newEnrollment = new Enrollment()
                {
                    IdEnrollment = response.IdEnrollment,
                    Semester = 1,
                    IdStudy = study,
                    StartDate = response.StartDate
                };
            }
            finally
            {
                var stud = new StudentApbd();
                stud.SetProperty("FirstName", request.FirstName);
                stud.SetProperty("IndexNumber", request.IndexNumber);
                stud.SetProperty("LastName", request.LastName);
                stud.SetProperty("BirthDate", request.BirthDate);
                stud.SetProperty("IdEnrollment", response.IdEnrollment);

                var salt = PasswordHelper.CreateSalt();
                stud.SetProperty("Salt", salt);
                stud.SetProperty("Pswd", PasswordHelper.GenerateSaltedHash(request.Password, salt));
                try
                {
                    db.StudentApbd.Add(stud);
                    db.SaveChanges();
                    db.Database.CommitTransaction();
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc.Message);
                    db.Database.RollbackTransaction();
                    throw new ArgumentException("Duplikat numeru indeksu");
                }
            }
            return response;

        }

        public PromoteStudentsResponse PromoteStudents(PromoteStudentsRequest request)
        {
            var db = new s17057Context();
            SqlParameter studiesParam = new SqlParameter("@Studies", request.Studies);
            SqlParameter semesterParam = new SqlParameter("@Semester", request.Semester);
            var resp = db.Enrollment.FromSqlRaw("EXEC PromoteStudents @Studies, @Semester", studiesParam, semesterParam).AsEnumerable().First();
            if (resp != null)
            {
                return new PromoteStudentsResponse
                {
                    IdEnrollment = resp.IdEnrollment,
                    IdStudy = resp.IdStudy.Value,
                    Semester = resp.Semester.Value,
                    StartDate = resp.StartDate.Value
                };
            }
            else
            {
                return null;
            }
        }
        public Student GetStudent(String id)
        {

            using (var connetion = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=s17057;Integrated Security=True"))
            using (var command = new SqlCommand())
            {
                var st = new Student();
                command.Connection = connetion;
                connetion.Open();
                command.CommandText = "SELECT * FROM StudentAPBD s LEFT JOIN ENROLLMENT e ON s.IdEnrollment = e.IdEnrollment LEFT JOIN STUDIES st on e.IdStudy = st.IdStudy WHERE IndexNumber LIKE @id";
                command.Parameters.AddWithValue("id", id);
                var dr = command.ExecuteReader();
                if (dr.Read())
                {
                    return new Student
                    {
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        IndexNumber = dr["IndexNumber"].ToString(),
                        BirthDate = Convert.ToDateTime(dr["BirthDate"].ToString()),
                        Studies = dr["Name"].ToString(),
                        Semester = Convert.ToInt32(dr["Semester"].ToString())
                    };
                }
                else
                {
                    return null;
                }
            }
        }
        public IEnumerable<Student> GetStudents()
        {

            using (var connetion = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=s17057;Integrated Security=True"))
            using (var command = new SqlCommand())
            {
                var list = new List<Student>();
                command.Connection = connetion;
                connetion.Open();
                command.CommandText = "SELECT * FROM StudentAPBD s LEFT JOIN ENROLLMENT e ON s.IdEnrollment = e.IdEnrollment LEFT JOIN STUDIES st on e.IdStudy = st.IdStudy";
                var dr = command.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        list.Add(new Student
                        {
                            FirstName = dr["FirstName"].ToString(),
                            LastName = dr["LastName"].ToString(),
                            IndexNumber = dr["IndexNumber"].ToString(),
                            BirthDate = Convert.ToDateTime(dr["BirthDate"].ToString()),
                            Studies = dr["Name"].ToString(),
                            Semester = Convert.ToInt32(dr["Semester"].ToString())
                        });
                    }
                    return list;
                }
                else
                {
                    return null;
                }

            }
        }
        public Boolean CheckStudent(String indexNumber)
        {
            using (var connetion = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=s17057;Integrated Security=True"))
            using (var command = new SqlCommand())
            {
                command.Connection = connetion;
                connetion.Open();
                command.CommandText = "SELECT * FROM StudentAPBD WHERE IndexNumber LIKE @indexNumber";
                command.Parameters.AddWithValue("indexNumber", indexNumber);
                var dr = command.ExecuteReader();
                return dr.HasRows;
            }
        }
    }
}
