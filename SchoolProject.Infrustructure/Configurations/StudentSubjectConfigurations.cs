using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolProject.Data.Entities;

namespace SchoolProject.Infrustructure.Configurations
{
    public class StudentSubjectConfigurations : IEntityTypeConfiguration<StudentSubject>
    {
        public void Configure(EntityTypeBuilder<StudentSubject> builder)
        {
            builder.HasKey(ss => new { ss.StudID, ss.SubID });

            builder.HasOne(ss => ss.Student)
                   .WithMany(s => s.StudentSubjects)
                   .HasForeignKey(ss => ss.StudID);

            builder.HasOne(ss => ss.Subject)
                   .WithMany(su => su.StudentsSubjects)
                   .HasForeignKey(ss => ss.SubID);
        }
    }
}
