using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Data.Entities
{
    public class DepartmentSubject
    {
        [Key]
        public int DeptSubID { get; set; }
        public int DID { get; set; }
        public int SubID { get; set; }

        [ForeignKey("DID")]
        [InverseProperty(nameof(Department.DepartmentSubjects))]
        public virtual Department Department { get; set; }

        [ForeignKey("SubID")]
        [InverseProperty(nameof(Subjects.DepartmentsSubjects))]
        public virtual Subjects Subjects { get; set; }
    }
}
