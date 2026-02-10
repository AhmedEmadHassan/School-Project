using SchoolProject.Data.Commons;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Data.Entities
{
    public class Student : GeneralLocalizableEntity
    {
        public Student()
        {
            StudentSubjects = new HashSet<StudentSubject>();
            Subjects = new HashSet<Subject>();
        }
        [Key]
        public int StudID { get; set; }
        [StringLength(200)]
        public string NameEn { get; set; }
        [StringLength(200)]
        public string NameAr { get; set; }
        [StringLength(500)]
        public string AddressEn { get; set; }
        [StringLength(500)]
        public string AddressAr { get; set; }
        [StringLength(500)]
        public string Phone { get; set; }
        public int? DID { get; set; }

        [ForeignKey("DID")]
        [InverseProperty(nameof(Department.Students))]
        public virtual Department Department { get; set; }
        // Subjects Many to Many Relation
        [InverseProperty("Students")]
        public virtual ICollection<Subject> Subjects { get; set; }

        // explicit join navigation for Student-Subject
        public virtual ICollection<StudentSubject> StudentSubjects { get; set; }
    }
}
