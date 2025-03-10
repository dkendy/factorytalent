
using System.Net.Http.Headers;
using FactoryTalent.Modules.Users.Domain.Users;
using Microsoft.Extensions.Configuration;
using FactoryTalent.Modules.Users.Application.Abstractions.Data;
using Newtonsoft.Json;
using System.Text;
using FactoryTalent.Modules.Users.Infrastructure.Identity;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using FactoryTalent.Modules.Users.Application.Abstractions.Helper;
using FactoryTalent.Modules.Users.Application.Abstractions.Identity;
using FactoryTalent.Common.Domain;
using System.Threading;
using Polly;
using FactoryTalent.Modules.Users.Application.Users.RegisterUser;
using MediatR;
using Polly.Telemetry;
using FactoryTalent.Modules.Users.Infrastructure.Users;
using System.Net;

namespace FactoryTalent.Modules.Users.Infrastructure.Database.DataSeeder;

public class DataSeeder
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServiceProvider _provider;
    private readonly IIdentityProviderService _identityProviderService;


    public DataSeeder(IServiceProvider provider, IUserRepository userRepository, IUnitOfWork unitOfWork,
        IIdentityProviderService identityProviderService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _provider = provider;
        _identityProviderService = identityProviderService;
    }



    public async Task SeedAsync()
    {

        KeyCloakOptions keycloakOptions = _provider
                   .GetRequiredService<IOptions<KeyCloakOptions>>().Value;

        string usersJson = "";

        Polly.Retry.AsyncRetryPolicy retryPolicy = Policy
            .Handle<HttpRequestException>(ex => ShouldRetry(ex))
            .WaitAndRetryAsync(30,
                retryAttempt => TimeSpan.FromSeconds(Math.Min(30, Math.Pow(2, retryAttempt))),
                (HttpRequestException, timeSpan) =>
                {
                    Console.WriteLine($"Keycloak not available, retrying in {timeSpan.TotalSeconds} seconds... Error: {HttpRequestException.Message}");
                });

        await retryPolicy.ExecuteAsync(async () =>
        {
            Result<string> result = await _identityProviderService.RegisterUserAsync(
                new UserModel(keycloakOptions.UserAdmin, keycloakOptions.PasswordAdm, "Adm", "Adm"),
                new CancellationToken());

            if (result.IsSuccess)
            {
                Console.WriteLine("Admin user registered successfully.");
                usersJson = result.Value;
            }
            else
            {
                Result<string> resultSearch = await _identityProviderService.GetUserAsync(
                    keycloakOptions.UserAdmin,
                new CancellationToken());
                if (resultSearch.IsSuccess)
                {
                    Console.WriteLine("Admin user registered successfully.");
                    usersJson = resultSearch.Value;
                }
            }

        });


        await _userRepository.DeleteByEmailAsync(keycloakOptions.UserAdmin);
        _userRepository.Insert(User.Create(keycloakOptions.UserAdmin, "Adm", "Adm", CpfGenerator.Create(), string.Empty, DateTime.Now, null, usersJson, new List<string>(), Role.Administrator));
        await _unitOfWork.SaveChangesAsync();



    }

    private static bool ShouldRetry(HttpRequestException ex)
    {
        if (ex.StatusCode == HttpStatusCode.Conflict)
        {
            return false;
        }
        return true;
    }
}



