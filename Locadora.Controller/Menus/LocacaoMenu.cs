using Locadora.Controller.Interfaces;
using Locadora.Models;
using Locadora.Models.Enums;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using Utils.Validadacoes;

namespace Locadora.Controller.Menus
{
    public class MenuLocacao
    {
        private LocacaoController controllerLocacao = new LocacaoController();
        private VeiculoController controllerVeiculo = new VeiculoController();
        private ClienteController clienteController = new ClienteController();
        private FuncionarioController funcionarioController = new FuncionarioController();
        private LocacaoFuncionarioController locacaoFuncionariosController = new LocacaoFuncionarioController();

        private void InsertService()
        {
            Cliente cliente = null;
            Veiculo veiculo = null;

            // Buscar cliente
            string? email = Validar.ValidarInputString("Digite o email do cliente: ");
            if (email == null) return;

            cliente = clienteController.BuscarClientePorEmail(email);
            if (cliente is null)
            {
                Console.WriteLine("\nNão existe cliente com esse email cadastrado!");
                return;
            }

            Console.WriteLine("\n=-=-=   >  Cliente Encontrado  <   =-=-=\n");
            Console.WriteLine(cliente + "\n");

            // Buscar veículo
            string? placa = Validar.ValidarInputString("Digite a placa do veículo: ");
            if (placa == null) return;

            veiculo = controllerVeiculo.BuscarVeiculoPorPlaca(placa);
            if (veiculo is null)
            {
                Console.WriteLine("\nNão existe veículo com essa placa!");
                return;
            }

            if (!veiculo.StatusVeiculo.Equals("Disponível", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Veículo '{veiculo.Placa}' não está disponível.");
                return;
            }

            Console.WriteLine("\n=-=-=   >  Veículo <   =-=-=\n");
            Console.WriteLine(veiculo + "\n");

            int diarias = Validar.ValidarInputInt("Informe o número de diárias: ");
            if (diarias <= 0) return;

            if (veiculo.Categoria == null)
            {
                Console.WriteLine("Erro: categoria do veículo não encontrada.");
                return;
            }

            DateTime dataLocacao = DateTime.Now;
            DateTime dataDevolucaoPrevista = dataLocacao.AddDays(diarias);

            var locacao = new Locacao(
                cliente.ClienteID,
                veiculo.VeiculoID,
                dataLocacao,
                dataDevolucaoPrevista,
                veiculo.Categoria.Diaria,
                "Ativa"
            );

            try
            {
                controllerLocacao.AdicionarLocacao(locacao, null);
                Console.WriteLine("\n>>> Locação criada com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao criar locação: " + ex.Message);
                return;
            }

            // Selecionar funcionarios
            Console.WriteLine("\n=-=-= Seleção de Funcionários =-=-=\n");

            var funcionarios = funcionarioController.ListarTodosFuncionarios();
            foreach (var f in funcionarios)
                Console.WriteLine($"{f.FuncionarioID} - {f.Nome}");

            List<int> funcionariosEscolhidos = new();

            while (true)
            {
                int funcId = Validar.ValidarInputInt("Digite o ID do funcionário que participará (0 para finalizar): ");

                if (funcId == 0) break;

                if (!funcionarios.Exists(f => f.FuncionarioID == funcId))
                {
                    Console.WriteLine("Funcionário não encontrado!");
                    continue;
                }

                try
                {
                    locacaoFuncionariosController.AssociarFuncionario(locacao.LocacaoID, funcId);
                    funcionariosEscolhidos.Add(funcId);
                    Console.WriteLine($"Funcionário {funcId} associado!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao associar funcionário: " + ex.Message);
                }
            }

            if (funcionariosEscolhidos.Count == 0)
            {
                Console.WriteLine("\nÉ obrigatório selecionar ao menos 1 funcionário!");
                return;
            }

            Console.WriteLine("\n >>> Locação registrada com sucesso!");
        }


        private void SelectAllService()
        {
            Console.Clear();

            try
            {
                var list = controllerLocacao.ListarLocacoes();

                foreach (var locacao in list)
                {
                    Console.WriteLine(locacao);
                    Console.WriteLine("------------------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void UpdateCancelLocacaoService()
        {
            Guid id = Validar.ValidarInputGuid("Informe o id para busca da locação: ");
            if (id == Guid.Empty) return;

            try
            {
                var locacao = controllerLocacao.BuscarLocacaoPorId(id);
                if (locacao is null)
                {
                    Console.WriteLine("\nNão existe locação cadastrada com esse id!");
                    return;
                }

                if (locacao.Status != "Ativa")
                {
                    Console.WriteLine($"\nStatus da locacao: {locacao.Status}");
                    Console.WriteLine("\nSó é possível cancelar uma locação ativa. Operação cancelada!");
                    return;
                }

                if (locacao.DataLocacao.Date < DateTime.Now.Date)
                {
                    Console.WriteLine("\nNão é possível cancelar: essa locação já está ativa a mais de um dia. Por favor, finalize a locação.");
                    return;
                }

                Console.WriteLine("\n=-=-=   >  Locação  <   =-=-=\n");
                Console.WriteLine(locacao + "\n");

                // Atualiza locação
                locacao.SetStatus(EStatusLocacao.Cancelada.ToString());
                locacao.SetDataDevolucaoReal(null);
                locacao.SetValorTotal(0m);

                controllerLocacao.AtualizarDataDevolucaoRealLocacao(locacao, null);
                controllerLocacao.AtualizarStatusLocacao(locacao, EStatusLocacao.Cancelada.ToString());
                controllerLocacao.AtualizarValorTotalLocacao(locacao);

                // Busca a placa do veículo diretamente pelo VeiculoID
                string placaVeiculo = controllerVeiculo.BuscarPlacaPorId(locacao.VeiculoID);
                controllerVeiculo.AtualizarStatusVeiculo(EStatusVeiculo.Disponível.ToString(), placaVeiculo);

                Console.WriteLine($"Valor total da locação: R$ 0");
                Console.WriteLine("\n >>>  Locacao cancelada com sucesso!\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao cancelar locação: " + ex.Message);
            }
        }


        private void UpdateLocacaoService()
        {
            Guid id = Validar.ValidarInputGuid("Informe o id para busca da locação: ");
            if (id == Guid.Empty) return;

            try
            {
                // Busca a locação pelo ID
                var locacao = controllerLocacao.BuscarLocacaoPorId(id);
                if (locacao is null)
                {
                    Console.WriteLine("\nNão existe locação cadastrada com esse id!");
                    return;
                }

                if (locacao.Status != "Ativa")
                {
                    Console.WriteLine($"\nStatus da locacao: {locacao.Status}");
                    Console.WriteLine("\nSó é possível finalizar uma locação ativa. Operação cancelada!");
                    return;
                }

                Console.WriteLine("\n=-=-=   >  Locação  <   =-=-=\n");
                Console.WriteLine(locacao + "\n");

                // Atualiza dados da locação
                locacao.SetStatus(EStatusLocacao.Finalizada.ToString());
                locacao.SetDataDevolucaoReal(DateTime.Now);

                // Calcula o valor total da locação e atualiza o objeto
                decimal valorTotal = locacao.CalcularValorFinal();
                locacao.SetValorTotal(valorTotal);

                // Atualiza no banco
                controllerLocacao.AtualizarDataDevolucaoRealLocacao(locacao, DateTime.Now);
                controllerLocacao.AtualizarStatusLocacao(locacao, EStatusLocacao.Finalizada.ToString());
                controllerLocacao.AtualizarValorTotalLocacao(locacao);

                // Atualiza status do veículo para disponível
                string placaVeiculo = controllerVeiculo.BuscarPlacaPorId(locacao.VeiculoID);
                controllerVeiculo.AtualizarStatusVeiculo(EStatusVeiculo.Disponível.ToString(), placaVeiculo);

                Console.WriteLine($"Valor total da locação: R$ {valorTotal}");
                Console.WriteLine("\n >>>  Locação finalizada com sucesso!\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao finalizar locação: " + ex.Message);
            }
        }




        public void LocacaoMenu()
        {
            int opcao = 0;

            do
            {
                Console.Clear();
                Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-|");
                Console.WriteLine(" |                 LOCAÇÕES               |");
                Console.WriteLine(" |----------------------------------------|");
                Console.WriteLine(" | [ 1 ] Criar Locação                    |");
                Console.WriteLine(" | [ 2 ] Listar Locações                  |");
                Console.WriteLine(" | [ 3 ] Finalizar Locação                |");
                Console.WriteLine(" | [ 4 ] Cancelar Locação                 |");
                Console.WriteLine(" | [ 5 ] Voltar                           |");
                Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-|");
                Console.WriteLine();

                opcao = Validar.ValidarInputInt("Escolha uma opção: ");

                Console.WriteLine("---------------------------------------");

                switch (opcao)
                {
                    case 1: InsertService(); break;
                    case 2: SelectAllService(); break;
                    case 3: UpdateLocacaoService(); break;
                    case 4: UpdateCancelLocacaoService(); break;
                    case 5: return;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }

                Console.Write("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();

            } while (true);
        }
    }
}
