using SchoolProject.Data.Commons;
using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Data.Entities
{
    public class Subjects : GeneralLocalizableEntity
    {
        public Subjects()
        {
            StudentsSubjects = new HashSet<StudentSubject>();
            DepartmetsSubjects = new HashSet<DepartmetSubject>();
        }
        [Key]
        public int SubID { get; set; }
        [StringLength(500)]
        public string SubjectNameEn { get; set; }
        [StringLength(500)]
        public string SubjectNameAr { get; set; }
        public DateTime Period { get; set; }
        public virtual ICollection<StudentSubject> StudentsSubjects { get; set; }
        public virtual ICollection<DepartmetSubject> DepartmetsSubjects { get; set; }
    }
}
