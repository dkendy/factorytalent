using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryTalent.Modules.Users.TestUtilities.Helper;
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
            semente == "000000000"
            || semente == "111111111"
            || semente == "222222222"
            || semente == "333333333"
            || semente == "444444444"
            || semente == "555555555"
            || semente == "666666666"
            || semente == "777777777"
            || semente == "888888888"
            || semente == "999999999"
        );

        semente += CalcularDigitoVerificador(semente).ToString(CultureInfo.CurrentCulture);
        semente += CalcularDigitoVerificador(semente).ToString(CultureInfo.CurrentCulture);
        return semente;

    }

    public static int CalcularDigitoVerificador(string semente)
    {
        int soma = 0;
        int resto = 0;
        int[] multiplicadores = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        int iFinal = multiplicadores.Length;
        int iInicial = iFinal - semente.Length;

        for (int i = iInicial; i < iFinal; i++)
        {
            soma += int.Parse(semente[i - iInicial].ToString(),CultureInfo.CurrentCulture) * multiplicadores[i]; 
        }

        resto = soma % 11;

        resto = resto < 2 ? 0 : 11 - resto;

        return resto;

    }
}
