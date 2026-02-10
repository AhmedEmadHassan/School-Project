using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Data.Entities
{
    public class Ins_Subject
    {
        public int InsId { get; set; }
        public int SubID { get; set; }

        [ForeignKey(nameof(InsId))]
        public virtual Instructor Instructor { get; set; }

        [ForeignKey(nameof(SubID))]
        public virtual Subjects Subject { get; set; }
    }
}
