using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolProject.Data.Entities;

namespace SchoolProject.Infrustructure.Configurations
{
    internal class InstructorConfigurations : IEntityTypeConfiguration<Instructor>
    {

        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.HasKey(x => x.InsId);
            builder
                .HasOne(i => i.Department)
                .WithMany(d => d.Instructors)
                .HasForeignKey(i => i.DID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
