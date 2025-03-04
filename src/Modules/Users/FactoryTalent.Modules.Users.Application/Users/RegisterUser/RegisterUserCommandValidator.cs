using System.Text.RegularExpressions;
using FluentValidation;

namespace FactoryTalent.Modules.Users.Application.Users.RegisterUser;

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty();
        RuleFor(c => c.LastName).NotEmpty();
        RuleFor(c => c.Email).EmailAddress();
        RuleFor(c => c.Password).MinimumLength(6);
        RuleFor(c => c.birthdate).NotNull()
            .Must(BeAtLeast18YearsOld).WithMessage("User must be at least 18 years old.");
        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Must(ContainUppercase).WithMessage("Password must contain at least one uppercase letter.")
            .Must(ContainDigit).WithMessage("Password must contain at least one number.")
            .Must(ContainSpecialCharacter).WithMessage("Password must contain at least one special character (!@#$%^&*).");
        RuleFor(user => user.CPF)
            .NotEmpty().WithMessage("CPF is required.")
            .Must(BeOnlyDigits).WithMessage("CPF must contain only numbers.")
            .Length(11).WithMessage("CPF must have 11 digits.")
            .Must(NotBeBlacklisted).WithMessage("Invalid CPF.")
            .Must(IsValidCpfAlgorithm).WithMessage("Invalid CPF.");

    }

    private bool ContainUppercase(string password) => Regex.IsMatch(password, "[A-Z]");
    private bool ContainDigit(string password) => Regex.IsMatch(password, "[0-9]");

    private bool ContainSpecialCharacter(string password) => Regex.IsMatch(password, "[!@#$%^&*]");

    private bool BeAtLeast18YearsOld(DateTime date)
    {

        DateTime today = DateTime.Today;
        DateTime minDate = today.AddYears(-18);

        return date.Date <= minDate;
    }

    private bool BeOnlyDigits(string cpf)
    {
        return Regex.IsMatch(cpf, @"^\d{11}$");
    }

    private bool NotBeBlacklisted(string cpf)
    {
        string[] invalidCpfs = {
            "00000000000", "11111111111", "22222222222", "33333333333",
            "44444444444", "55555555555", "66666666666", "77777777777",
            "88888888888", "99999999999"
        };
        return !invalidCpfs.Contains(cpf);
    }

    private bool IsValidCpfAlgorithm(string cpf)
    {
        if (cpf.Length != 11)
        { 
            return false; 
        }

        int[] multipliers1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multipliers2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        int firstDigit = CalculateCpfDigit(cpf.Substring(0, 9), multipliers1);
        int secondDigit = CalculateCpfDigit(cpf.Substring(0, 9) + firstDigit, multipliers2);

        return cpf.EndsWith($"{firstDigit}{secondDigit}", StringComparison.CurrentCulture);
    }

    private int CalculateCpfDigit(string cpf, int[] multipliers)
    {
        int sum = cpf.Select((digit, index) => (digit - '0') * multipliers[index]).Sum();
        int remainder = sum % 11;
        return remainder < 2 ? 0 : 11 - remainder;
    }

}
