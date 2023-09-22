using System.ComponentModel.DataAnnotations;

namespace StudentDetails.Areas.LOC_Student.Models
{
        public class StudentModel
        {
            public int? StudentID { get; set; }

            public int BranchID { get; set; }

            public int CityID { get; set; }

            public string StudentName { get; set; }

            public string MobileNoStudent { get; set; }

            public string Email { get; set; }

            public string MobileNoFather { get; set; }

            public string Address { get; set; }

            public DateTime BirthDate { get; set; }

            public int Age { get; set; }

            public Boolean IsActive { get; set; }

            public string Gender { get; set; }

            public string Password { get; set; }

            public DateTime Created { get; set; }
            public DateTime Modified { get; set; }

        }
}


