using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Validadacoes
{
    public class Validar
    {
        public static string? ValidarInputString(string text)
        {
            Console.Write(text);
            string input = Console.ReadLine()!;

            while (string.IsNullOrWhiteSpace(input))
            {
                Console.Write("\nCampo obrigatório! Digite novamente ou [9] para sair: ");
                input = Console.ReadLine()!;

                if (input == "9")
                    return null;
            }
            return input;
        }

        public static string? ValidarInputOpcional(string text)
        {
            Console.Write(text);
            string input = Console.ReadLine()!;

            if (string.IsNullOrWhiteSpace(input))
                return null;

            return input;
        }

        public static decimal ValidarInputDecimal(string text)
        {
            Console.Write(text);
            string input = Console.ReadLine()!;

            var validation = Decimal.TryParse(input, out decimal result);

            while (!validation)
            {
                Console.Write("\nInválido! Digite apenas numerico ou [S] para sair: ");
                input = Console.ReadLine()!;

                if (input.ToUpper() == "S")
                    return 0;

                validation = Decimal.TryParse(input, out result);
            }
            return result;
        }


        public static decimal? ValidarInputDecimalOpcional(string text)
        {
            Console.Write(text);
            string input = Console.ReadLine()!;

            if (string.IsNullOrWhiteSpace(input)) return null;

            var validation = Decimal.TryParse(input, out decimal result);

            while (!validation)
            {
                Console.Write("\nInválido! Digite apenas numerico ou [S] para sair: ");
                input = Console.ReadLine()!;

                if (input.ToUpper() == "S")
                    return null;

                validation = Decimal.TryParse(input, out result);
            }
            return result;
        }


        public static int ValidarInputInt(string text)
        {
            Console.Write(text);
            string input = Console.ReadLine()!;

            var validation = int.TryParse(input, out int result);

            while (!validation)
            {
                Console.Write("\nInválido! Digite apenas numerico ou [S] para sair: ");
                input = Console.ReadLine()!;

                if (input.ToUpper() == "S")
                    return 0;

                validation = int.TryParse(input, out result);
            }
            return result;
        }

        public static DateOnly ValidarInputDateOnly(string text)
        {
            Console.Write(text);
            string input = Console.ReadLine()!;
            DateOnly data;

            while (!DateOnly.TryParseExact(input, "dd/MM/yyyy", out data))
            {
                Console.Write("\nData inválida! Digite novamente [dd/MM/yyyy]: ");
                input = Console.ReadLine()!;
            }
            return data;
        }

    }
}
