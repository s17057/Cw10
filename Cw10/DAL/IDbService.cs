using Cw10.Models;
using System.Collections.Generic;

namespace Cw10.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudents();
    }
}
