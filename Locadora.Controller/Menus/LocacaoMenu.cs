using Locadora.Models;
using Locadora.Models.Enums;
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

        private void InsertService()
        {
            Cliente cliente = null;
            Veiculo veiculo = null;

            //buscar cliente
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

            // buscar veículo
            string? placa = Validar.ValidarInputString("Digite a placa do veículo: ");
            if (placa == null) return;

            veiculo = controllerVeiculo.BuscarVeiculoPorPlaca(placa);
            if (veiculo is null)
            {
                Console.WriteLine("\nNão existe veículo com essa placa!");
                return;
            }

            if (veiculo.StatusVeiculo != EStatusVeiculo.Disponivel.ToString())
            {
                Console.WriteLine($"Veículo '{veiculo.Placa}' não está disponível.");
                return;
            }

            Console.WriteLine("\n=-=-=   >  Veículo <   =-=-=\n");
            Console.WriteLine(veiculo + "\n");

            // Selecionar funcionários
            Console.WriteLine("\n=-=-= Seleção de Funcionários =-=-=\n");

            var funcionarios = funcionarioController.ListarTodosFuncionarios();
            if (funcionarios.Count == 0)
            {
                Console.WriteLine("Nenhum funcionário encontrado!");
                return;
            }

            foreach (var f in funcionarios)
                Console.WriteLine($"{f.FuncionarioID} - {f.Nome}");

            List<int> funcionariosEscolhidos = new List<int>();

            while (true)
            {
                int funcId = Validar.ValidarInputInt("Digite o ID do funcionário que participará (0 para finalizar): ");

                if (funcId == 0) break;

                var existe = funcionarios.Exists(f => f.FuncionarioID == funcId);
                if (!existe)
                {
                    Console.WriteLine("Funcionário não encontrado!");
                    continue;
                }

                funcionariosEscolhidos.Add(funcId);
            }

            if (funcionariosEscolhidos.Count == 0)
            {
                Console.WriteLine("\nÉ obrigatório vincular pelo menos 1 funcionário.");
                return;
            }

            // Informações de locação
            int diarias = Validar.ValidarInputInt("Informe o número de diárias: ");
            if (diarias <= 0) return;

    
            var locacao = new Locacao(
                cliente.ClienteID,
                veiculo.VeiculoID,
                DateTime.Now,
                DateTime.Now.AddDays(diarias),
                veiculo.Categoria.Diaria,
                "Ativa"
            );

            try
            {
                controllerLocacao.AdicionarLocacao(locacao, funcionariosEscolhidos);

                Console.WriteLine("\n >>> Locação realizada com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
            Guid id = Validar.ValidarInputGuid("Informe o ID da locação: ");
            if (id == Guid.Empty) return;

            try
            {
                var locacao = controllerLocacao.BuscarLocacaoPorId(id);
                if (locacao is null)
                {
                    Console.WriteLine("Não existe locação com esse ID!");
                    return;
                }

                if (locacao.Status != EStatusLocacao.Ativa)
                {
                    Console.WriteLine("Só é possível cancelar uma locação ativa.");
                    return;
                }

                controllerLocacao.AtualizarStatusLocacao(id, "Cancelada");
                controllerLocacao.AtualizarDataDevolucaoRealLocacao(id, DateTime.Now);

                Console.WriteLine("\n >>> Locação cancelada com sucesso!");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void UpdateLocacaoService()
        {
            Guid id = Validar.ValidarInputGuid("Informe o ID da locação: ");
            if (id == Guid.Empty) return;

            try
            {
                var locacao = controllerLocacao.BuscarLocacaoPorId(id);
                if (locacao is null)
                {
                    Console.WriteLine("Não existe locação com esse ID!");
                    return;
                }

                if (locacao.Status != EStatusLocacao.Ativa)
                {
                    Console.WriteLine("Somente locações ativas podem ser finalizadas.");
                    return;
                }

                decimal valorFinal = locacao.CalcularValorFinal();

                controllerLocacao.AtualizarStatusLocacao(id, "Finalizada");
                controllerLocacao.AtualizarDataDevolucaoRealLocacao(id, DateTime.Now);

                Console.WriteLine($"\nValor total: R$ {valorFinal}");
                Console.WriteLine("\n >>> Locação finalizada com sucesso!\n");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
