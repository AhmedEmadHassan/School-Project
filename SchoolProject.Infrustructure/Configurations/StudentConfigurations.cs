using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolProject.Data.Entities;

namespace SchoolProject.Infrustructure.Configurations
{
    public class StudentConfigurations : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder
                .HasOne(s => s.Department)
                .WithMany(d => d.Students)
                .HasForeignKey(s => s.DID)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasMany(s => s.Subjects)
                .WithMany(su => su.Students)
                .UsingEntity<StudentSubject>(
                    j => j.HasOne(ss => ss.Subject).WithMany(su => su.StudentsSubjects).HasForeignKey(ss => ss.SubID),
                    j => j.HasOne(ss => ss.Student).WithMany(s => s.StudentSubjects).HasForeignKey(ss => ss.StudID),
                    j =>
                    {
                        j.HasKey(ss => new { ss.StudID, ss.SubID });
                        j.ToTable("StudentSubject");
                    }
                );
        }
    }
}
