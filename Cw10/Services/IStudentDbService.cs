using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw10.DTO.Requests;
using Cw10.DTO.Responses;
using Cw10.Models;

namespace Cw10.Services
{
    public interface IStudentDbService
    {
        EnrollStudentResponse EnrollStudent(EnrollStudentRequest request);
        PromoteStudentsResponse PromoteStudents(PromoteStudentsRequest request);
        Student GetStudent(String id);
        IEnumerable<Student> GetStudents();
        Boolean CheckStudent(String indexNumber);
    }
}
