
using System.Globalization;


namespace FactoryTalent.Modules.Users.Application.Abstractions.Helper;
public sealed class CpfGenerator
{
    public static string Create()
    {

        var random = new Random();
        string semente;

        do
        {
#pragma warning disable CA5394 // Do not use insecure randomness
            semente = random.Next(1, 999999999).ToString(CultureInfo.CurrentCulture).PadLeft(9, '0');
#pragma warning restore CA5394 // Do not use insecure randomness
        }
        while (
            semente is "000000000"
            or "111111111"
            or "222222222"
            or "333333333"
            or "444444444"
            or "555555555"
            or "666666666"
            or "777777777"
            or "888888888"
            or "999999999"
        );

        semente += CalcularDigitoVerificador(semente).ToString(CultureInfo.CurrentCulture);
        semente += CalcularDigitoVerificador(semente).ToString(CultureInfo.CurrentCulture);
        return semente;

    }

    public static int CalcularDigitoVerificador(string semente)
    {
        int soma = 0;
        int[] multiplicadores = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

        int iFinal = multiplicadores.Length;
        int iInicial = iFinal - semente.Length;

        for (int i = iInicial; i < iFinal; i++)
        {
            soma += int.Parse(semente[i - iInicial].ToString(), CultureInfo.CurrentCulture) * multiplicadores[i];
        }

        int resto = soma % 11;

        resto = resto < 2 ? 0 : 11 - resto;

        return resto;

    }
}
