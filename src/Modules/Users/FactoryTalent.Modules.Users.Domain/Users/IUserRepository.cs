﻿namespace FactoryTalent.Modules.Users.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<User?> GetByCPFAsync(string cpf, CancellationToken cancellationToken = default);

    void Insert(User user);
}
