using FactoryTalent.Modules.Users.Domain.Users;
using FactoryTalent.Modules.Users.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace FactoryTalent.Modules.Users.Infrastructure.Users;

internal sealed class UserRepository(UsersDbContext context) : IUserRepository
{
    public async Task<User?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Users.SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await context.Users.SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> GetByCPFAsync(string cpf, CancellationToken cancellationToken = default)
    {
        return await context.Users.SingleOrDefaultAsync(u => u.CPF == cpf, cancellationToken);
    }

    public void Insert(User user)
    {
        foreach (Role role in user.Roles)
        {
            context.Attach(role);
        }
 
        context.Contacts.AddRangeAsync(user.Contacs);

        context.Users.Add(user);
    }
}
