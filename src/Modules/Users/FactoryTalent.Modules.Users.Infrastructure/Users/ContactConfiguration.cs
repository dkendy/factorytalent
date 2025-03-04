using System.Reflection.Emit;
using FactoryTalent.Modules.Users.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FactoryTalent.Modules.Users.Infrastructure.Users;

internal sealed class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.PhoneNumber).HasMaxLength(100);
         
    }


}
