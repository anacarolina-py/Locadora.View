using Locadora.Models;
using Locadora.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Validadacoes;
using Locadora.Controller;


namespace Utils.Menus
{
    public class VeiculoMenu
    {
        private VeiculoController Controller = new VeiculoController();

        private void InsertService()
        {
            string? input = Validar.ValidarInputString("Nome da categoria do Veículo: ");
            if (input == null) return;
            var category = new CategoriaController().BuscaCategoriaPorNome(input);
            if (category == null)
            {
                Console.WriteLine($"\nNão existe categoria com este nome cadastrado!");
                return;
            }

            string? plate = Validar.ValidarInputString("Placa: ");
            if (plate == null) return;

            string? mark = Validar.ValidarInputString("Marca: ");
            if (mark == null) return;

            string? model = Validar.ValidarInputString("Modelo: ");
            if (model == null) return;

            int year = Validar.ValidarInputInt("Ano do Veículo: ");
            if (year == 0) return;

            Veiculo vehicle = new Veiculo(category.CategoriaID, plate, mark, model, year, EStatusVeiculo.Disponível.ToString());

            try
            {
                Controller.AdicionarVeiculo(vehicle);
                Console.WriteLine("\n   >>>   Veículo inserido com sucesso!");
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
                var list = Controller.ListarTodosVeiculos();

                foreach (var vehicle in list)
                {
                    Console.WriteLine(vehicle);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void UpdateStatusService()
        {
            string? plate = Validar.ValidarInputString("Informe a placa para busca do veículo: ");
            if (plate == null) return;

            try
            {
                var vlr = Controller.BuscarVeiculoPorPlaca(plate);
                if (vlr is null)
                {
                    Console.WriteLine("\nNão existe veículo com essa placa cadastrado!");
                    return;
                }

                Console.WriteLine("\n\n                                    =-=-=   >  Veículo  <   =-=-=\n");
                Console.WriteLine(vlr);

                int? vehicleStatus = Validar.ValidarInputInt("\n Informe o novo status [1] Disponivel | [2] Alugado | [3] Manutencao: ");
                if (vehicleStatus == 0 || vehicleStatus is not 1 && vehicleStatus is not 2 && vehicleStatus is not 3) return;

                if (vehicleStatus == 1)
                    Controller.AtualizarStatusVeiculo(EStatusVeiculo.Disponível.ToString(), plate);
                else if (vehicleStatus == 2)
                    Controller.AtualizarStatusVeiculo(EStatusVeiculo.Alugado.ToString(), plate);
                else if (vehicleStatus == 3)
                    Controller.AtualizarStatusVeiculo(EStatusVeiculo.Manutencao.ToString(), plate);


                Console.WriteLine("\n >>>  Status atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void DeleteService()
        {
            string? plate = Validar.ValidarInputString("Informe a placa para busca do veículo: ");
            if (plate == null) return;

            try
            {
                var vlr = Controller.BuscarVeiculoPorPlaca(plate);
                if (vlr is null)
                {
                    Console.WriteLine("\nNão existe veículo com essa placa cadastrado!");
                    return;
                }

                Console.WriteLine("\n=-=-=   >  Veículo  <   =-=-=\n");
                Console.WriteLine(vlr + "\n");

                Console.Write("Tem certeza que deseja deletar o veículo? [S/N]: ");
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

                Controller.DeletarVeiculo(vlr.VeiculoID);
                Console.WriteLine("\n >>>  Veículo deletado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public void MenuVeiculo()
        {
            var listCategory = new CategoriaMenu();

            int opcao = 0;
            do
            {
                Console.Clear();
                Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=|");
                Console.WriteLine(" |                 VEÍCULOS               |");
                Console.WriteLine(" |----------------------------------------|");
                Console.WriteLine(" | [ 1 ] Cadastrar Veículo                |");
                Console.WriteLine(" | [ 2 ] Exibir Veículos                  |");
                Console.WriteLine(" | [ 3 ] Atualizar Veículo                |");
                Console.WriteLine(" | [ 4 ] Exibir CAtegorias                |");
                Console.WriteLine(" | [ 5 ] Deletar Veículo                  |");
                Console.WriteLine(" | [ 6 ] Voltar                            |");
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
                        UpdateStatusService();
                        break;
                    case 4:
                        listCategory.SelectAllService();
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
