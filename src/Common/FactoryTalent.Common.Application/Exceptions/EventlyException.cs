using FactoryTalent.Common.Domain;

namespace FactoryTalent.Common.Application.Exceptions;

public sealed class FactoryTalentException : Exception
{
    public FactoryTalentException(string requestName, Error? error = default, Exception? innerException = default)
        : base("Application exception", innerException)
    {
        RequestName = requestName;
        Error = error;
    }

    public string RequestName { get; }

    public Error? Error { get; }
}
