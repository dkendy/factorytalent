using FactoryTalent.Common.Infrastructure.Inbox;
using FactoryTalent.Common.Infrastructure.Outbox;
using FactoryTalent.Modules.Users.Application.Abstractions.Data;
using FactoryTalent.Modules.Users.Domain.Users;
using FactoryTalent.Modules.Users.Infrastructure.Users;
using Microsoft.EntityFrameworkCore;

namespace FactoryTalent.Modules.Users.Infrastructure.Database;

public sealed class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<User> Users { get; set; }

    public DbSet<Contact> Contacts { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Users); 
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new PermissionConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new ContactConfiguration());

     


    }
}
