using Locadora.Controller;
using Locadora.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Validadacoes;

namespace Utils.Menus
{
    public class FuncionarioMenu
    {
        private FuncionarioController Controller = new FuncionarioController();

        private void InsertService()
        {
            string? name = Validar.ValidarInputString("Nome: ");
            if (name == null) return;

            string? cpf = Validar.ValidarInputString("CPF: ");
            if (cpf == null) return;

            string? email = Validar.ValidarInputString("Email: ");
            if (email == null) return;

            decimal? salary = Validar.ValidarInputDecimalOpcional("Salário (opcional): ");

            Funcionario employeer = new Funcionario(name, cpf, email, salary);

            try
            {
                Controller.AdicionarFuncionario(employeer);
                Console.WriteLine("\n   >>>   Funcionario inserido com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void SelectAllService()
        {
            Console.Clear();
            Console.WriteLine();

            try
            {
                var list = Controller.ListarTodosFuncionarios();

                foreach (var employeer in list)
                {
                    Console.WriteLine(employeer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void UpdatePhoneService()
        {
            string? email = Validar.ValidarInputString("Informe o email para busca do funcionario: ");
            if (email == null) return;

            try
            {
                var vlr = Controller.BuscarFuncionarioEmail(email);
                if (vlr is null)
                {
                    Console.WriteLine("\nNão existe funcionario com esse email cadastrado!");
                    return;
                }

                Console.WriteLine("\n=-=-=   >  Funcionario  <   =-=-=\n");
                Console.WriteLine(vlr + "\n");

                decimal salary = Validar.ValidarInputDecimal("Informe o novo salário: ");
                if (salary == 0) return;

                Controller.AtualizarSalarioFuncionario(email, salary);

                Console.WriteLine("\n >>>  Salário atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void DeleteService()
        {
            string? email = Validar.ValidarInputString("Informe o email para busca do funcionario: ");
            if (email == null) return;

            try
            {
                var vlr = Controller.BuscarFuncionarioEmail(email);
                if (vlr is null)
                {
                    Console.WriteLine("\nNão existe funcionario com esse email cadastrado!");
                    return;
                }

                Console.WriteLine("\n=-=-=-=   >   Funcionario   <   =-=-=-=\n");
                Console.WriteLine(vlr + "\n");

                Console.Write("Tem certeza que deseja deletar o funcionario? [S/N]: ");
                string res = Console.ReadLine()!.ToUpper();

                while (res is not "S" && res is not "N")
                {
                    Console.Write("Error! Informe apenas [S/N] pra continuar: ");
                    res = Console.ReadLine()!.ToUpper();
                }

                if (res == "N")
                {
                    Console.WriteLine("\nEncerrando a operação de deletar...");
                    return;
                }

                Controller.DeletarFuncionario(email);
                Console.WriteLine("\n >>>  Funcionario deletado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public void MenuFuncionario()
        {
            int opcao = 0;
            do
            {
                Console.Clear();
                Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=|");
                Console.WriteLine(" |               FUNCIONÁRIOS             |");
                Console.WriteLine(" |----------------------------------------|");
                Console.WriteLine(" | [ 1 ] Cadastrar Funcionário            |");
                Console.WriteLine(" | [ 2 ] Exibir Funcionário               |");
                Console.WriteLine(" | [ 3 ] Atualizar Salário                |");
                Console.WriteLine(" | [ 4 ] Deletar Funcionário              |");
                Console.WriteLine(" | [ 5 ] Voltar                           |");              
                Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=|");
                Console.WriteLine();
                Console.Write("  >>> Informe o menu desejado: ");
                string entrada = Console.ReadLine()!;
                bool conversao = int.TryParse(entrada, out opcao);
                Console.WriteLine("---------------------------------------");

                switch (opcao)
                {
                    case 1:
                        InsertService();
                        break;
                    case 2:
                        SelectAllService();
                        break;
                    case 3:
                        UpdatePhoneService();
                        break;
                    case 4:
                        DeleteService();
                        break;
                    case 5:
                        return;
                    default:
                        Console.WriteLine("\nOpção Inválida. Tente novamente.");
                        break;
                }

                Console.Write("\n  >  Pressione qualquer Tecla para prosseguir ");
                Console.ReadLine();

            } while (true);
        }

    }
}
