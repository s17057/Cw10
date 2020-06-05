using System;
using System.Collections.Generic;

namespace Cw10.Models
{
    public partial class Enrollment
    {
        public Enrollment()
        {
            StudentApbd = new HashSet<StudentApbd>();
        }

        public int IdEnrollment { get; set; }
        public int? Semester { get; set; }
        public int? IdStudy { get; set; }
        public DateTime? StartDate { get; set; }

        public virtual Studies IdStudyNavigation { get; set; }
        public virtual ICollection<StudentApbd> StudentApbd { get; set; }
    }
}
