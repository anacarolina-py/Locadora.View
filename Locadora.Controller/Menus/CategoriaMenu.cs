using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Validadacoes;
using Locadora.Models;
using Locadora.Controller;


namespace Utils.Menus
{
    public class CategoriaMenu
    {
        private CategoriaController Controller = new CategoriaController();

        private void InsertService()
        {
            string? name = Validar.ValidarInputString("Categoria: ");
            if (name == null) return;

            decimal daily = Validar.ValidarInputDecimal("Valor da Diária: R$ ");
            if (daily == 0) return;

            string? description = Validar.ValidarInputOpcional("Descrição (opcional): ");

            Categoria category = new Categoria(name, daily, description);

            try
            {
                Controller.AdicionarCategoria(category);
                Console.WriteLine("\n >>>  Categoria inserido com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public void SelectAllService()
        {
            Console.Clear();
            Console.WriteLine();

            try
            {
                var list = Controller.ListarCategorias();

                foreach (var category in list)
                {
                    Console.WriteLine(category);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void UpdateDescriptionService()
        {
            string? name = Validar.ValidarInputString("Informe o nome da categoria para busca: ");
            if (name == null) return;

            try
            {
                var vlr = Controller.BuscaCategoriaPorNome(name);
                if (vlr is null)
                {
                    Console.WriteLine("\nNão existe categoria com esse nome cadastrado!");
                    return;
                }

                Console.WriteLine("\n\n                        =-=-=   >  Categoria  <   =-=-=\n");
                Console.WriteLine(vlr + "\n");

                string? description = Validar.ValidarInputString(" > Informe a nova descrição: ");
                if (description == null) return;

                Controller.AtualizarDescricaoCategoria(description, name);

                Console.WriteLine("\n >>>  Descrição atualizada com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void UpdateDailyService()
        {
            string? name = Validar.ValidarInputString("Informe o nome da categoria para busca: ");
            if (name == null) return;

            try
            {
                var vlr = Controller.BuscaCategoriaPorNome(name);
                if (vlr is null)
                {
                    Console.WriteLine("\nNão existe categoria com esse nome cadastrado!");
                    return;
                }

                Console.WriteLine("\n\n                        =-=-=   >  Categoria  <   =-=-=\n");
                Console.WriteLine(vlr);

                decimal daily = Validar.ValidarInputDecimal("\n > Informe o novo Valor da Diária: R$ ");
                if (daily == 0) return;

                Controller.AtualizarDiariaCategoria(daily, name);

                Console.WriteLine("\n >>>  Valor atualizado com sucesso!");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        private void DeleteService()
        {
            string? name = Validar.ValidarInputString("Informe o nome da categoria para busca: ");
            if (name == null) return;

            try
            {
                var vlr = Controller.BuscaCategoriaPorNome(name);
                if (vlr is null)
                {
                    Console.WriteLine("\nNão existe categoria com esse nome cadastrado!");
                    return;
                }

                Console.WriteLine("\n=-=-=   >  Categoria  <   =-=-=\n");
                Console.WriteLine(vlr);

                Console.Write("Tem certeza que deseja deletar a categoria? [S/N]: ");
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

                Controller.DeletarCategoria(name);
                Console.WriteLine("\n >>>  Categoria deletado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public void MenuCategoria()
        {
            int opcao = 0;
            do
            {
                Console.Clear();
                Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-==-|");
                Console.WriteLine(" |                 CATEGORIAS             |");
                Console.WriteLine(" |----------------------------------------|");
                Console.WriteLine(" | [ 1 ] Cadastrar Categoria              |");
                Console.WriteLine(" | [ 2 ] Exibir Categoria                 |");
                Console.WriteLine(" | [ 3 ] Atualizar Descrição              |");
                Console.WriteLine(" | [ 4 ] Atualizar Diária                 |");
                Console.WriteLine(" | [ 5 ] Deletar Categoria                |");
                Console.WriteLine(" | [ 6 ] Voltar                           |");
                Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=--=-|");
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
                        UpdateDescriptionService();
                        break;
                    case 4:
                        UpdateDailyService();
                        break;
                    case 5:
                        DeleteService();
                        break;
                    case 6:
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

