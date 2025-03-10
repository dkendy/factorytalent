using FactoryTalent.Modules.Users.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FactoryTalent.Modules.Users.Infrastructure.Users;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");

        builder.HasKey(p => p.Code);

        builder.Property(p => p.Code).HasMaxLength(100);

        builder.HasData(
            Permission.GetUser,
            Permission.ModifyUser,
            Permission.AddUser,
            Permission.DeleteUser);

        builder
            .HasMany<Role>()
            .WithMany()
            .UsingEntity(joinBuilder =>
            {
                joinBuilder.ToTable("role_permissions");

                joinBuilder.HasData(
                     
                    CreateRolePermission(Role.Manager, Permission.GetUser),
                    CreateRolePermission(Role.Manager, Permission.ModifyUser),
                    CreateRolePermission(Role.Manager, Permission.AddUser),
                    CreateRolePermission(Role.Manager, Permission.DeleteUser),

                    CreateRolePermission(Role.Administrator, Permission.GetUser),
                    CreateRolePermission(Role.Administrator, Permission.ModifyUser),
                    CreateRolePermission(Role.Administrator, Permission.AddUser),
                    CreateRolePermission(Role.Administrator, Permission.DeleteUser),

                    CreateRolePermission(Role.Employee, Permission.GetUser),
                    CreateRolePermission(Role.Employee, Permission.ModifyUser), 

                    CreateRolePermission(Role.Intern, Permission.GetUser),
                    CreateRolePermission(Role.Intern, Permission.ModifyUser) );


    });
    }

    private static object CreateRolePermission(Role role, Permission permission)
    {
        return new
        {
            RoleName = role.Name,
            PermissionCode = permission.Code
        };
    }
}
