using System.Reflection.Emit;
using FactoryTalent.Modules.Users.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FactoryTalent.Modules.Users.Infrastructure.Users;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName).HasMaxLength(200);

        builder.Property(u => u.LastName).HasMaxLength(200);

        builder.Property(u => u.BirthDay).IsRequired(); 

        builder.Property(u => u.Email).HasMaxLength(300); 
         
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.Address).HasMaxLength(300);
        builder.Property(u => u.Address).IsRequired(false);


        builder.Property(u => u.CPF).HasMaxLength(11);

        builder.HasIndex(u => u.CPF).IsUnique();

        builder.HasIndex(u => u.IdentityId);

        builder.HasMany(c=>c.Contacts)
            .WithOne()
            .HasForeignKey(p => p.UserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(f => f.Superior) 
            .WithMany(f => f.Subordinates)
            .HasForeignKey(f => f.SuperiorId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
         
    }


}
