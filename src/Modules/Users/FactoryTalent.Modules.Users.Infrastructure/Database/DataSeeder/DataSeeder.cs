 
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

namespace FactoryTalent.Modules.Users.Infrastructure.Database.DataSeeder;

public class DataSeeder
{ 
    private readonly IServiceProvider _provider; 
    private readonly ISender _sender;


    public DataSeeder(IServiceProvider provider, ISender sender)
    {
        _sender = sender;
         _provider = provider; 
    }

    public async Task SeedAsync()
    {

        KeyCloakOptions keycloakOptions = _provider
                   .GetRequiredService<IOptions<KeyCloakOptions>>().Value;

        Result<Guid> result = await _sender.Send(new RegisterUserCommand(
              keycloakOptions.UserAdmin,
              keycloakOptions.PasswordAdm,
              "Adm",
              "Adm",
              CpfGenerator.Create(),
              DateTime.Now.AddYears(-18),
              null,
              "-",
              new List<string>(),
              Role.Administrator
              ));


        if (!result.IsSuccess && result.Error.Code != "Users.ArgumentError")
        {
            throw new ArgumentException(result.Error.Description);
        }
         

    }
}
 


