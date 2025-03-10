using System.Threading;
using FactoryTalent.Modules.Users.Domain.Users;
using FactoryTalent.Modules.Users.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace FactoryTalent.Modules.Users.Infrastructure.Users;

internal sealed class UserRepository(UsersDbContext context) : IUserRepository
{
    public async Task<User?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Users.Include(c=>c.Contacts).SingleOrDefaultAsync(u => u.Id == id, cancellationToken);

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

        context.Contacts.AddRangeAsync(user.Contacts);

        context.Users.Add(user);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        User user = await GetAsync(id, cancellationToken);
        if(user is null)
        {

            return;
        }

        List<Contact> contacts = await context.Contacts.Where(u => u.UserId == id).ToListAsync(cancellationToken);

        foreach (Contact contact in contacts)
        {
            context.Contacts.Remove(contact);
        }

        
        context.Users.Remove(user); 
    }

    public async Task DeleteByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        User user = await GetByEmailAsync(email, cancellationToken);
        if (user is null)
        { 
            return;
        }
        await DeleteAsync(user.Id, cancellationToken);

    }
}
