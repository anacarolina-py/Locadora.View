
using Locadora.Controller;
using Locadora.Controller;
using Locadora.Controller.Crud;
using Locadora.Controller.Menu;
using Locadora.Models;
using Locadora.Models.Enums;
using Microsoft.Data.SqlClient;
using Utils.DataBases;
using Utils.Menus;


//Funcionario func = new Funcionario("Bruce Wanny", "285646688", "batman@uol.com");


//var funcController = new FuncionarioController();


#region INSERT Funcionario
//try
//{
//    funcController.AdicionarFuncionario(func);
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.Message);
//}
#endregion



#region SELECT ALL Funcionarios
//try
//{
//    var lista = funcController.ListarTodosFuncionarios();

//    foreach (var f in lista)
//    {
//        Console.WriteLine(f);
//    }
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.Message);
//}
#endregion


#region UPDATE and Busca de Salario
//try
//{
//    funcController.AtualizarSalarioFuncionario("batman@uol.com", 1300.90m);
//    Console.WriteLine(funcController.BuscarFuncionarioEmail("batman@uol.com"));
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.Message);
//}
#endregion


#region DELETE Funcionario
//try
//{
//    funcController.DeletarFuncionario("batman@uol.com");
//    Console.WriteLine("Funcionario deletado com sucesso!");
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.Message);
//}
#endregion



//   ----------------                      <<<   Locação   >>>                      ----------------                      


//var locacaoController = new LocacaoController();


#region INSERT Veiculo
//try
//{
//    var veiculo = new Veiculo(1, "XYZ-9876", "Chevrolet", "S10", 2025, EStatusVeiculo.Disponivel.ToString());
//    veiculoController.AdicionarVeiculo(veiculo);
//}
//catch (Exception ex)
//{
//    Console.WriteLine("Erro ao criar veículo: " + ex.Message);
//}
#endregion


#region SELECT ALL Veiculos
//try
//{
//    var veiculos = veiculoController.ListarTodosVeiculos();

//    foreach (var item in veiculos)
//    {
//        Console.WriteLine(item);
//    }
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.Message);
//}
#endregion


#region SELECT BY PLACA
//try
//{
//    Console.WriteLine(veiculoController.BuscarVeiculoPlaca("MNO7890"));
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.Message);
//}
#endregion


#region DELETE Veiculo
//try
//{
//    var veiculo = veiculoController.BuscarVeiculoPlaca("XYZ-9876");

//    veiculoController.DeletarVeiculo(veiculo.VeiculoID);
//    Console.WriteLine("Veiculo deletado com sucesso!");
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.Message);
//}
#endregion


#region UPDATE Status Veículo
//try
//{
//    veiculoController.AtualizarStatusVeiculo(EStatusVeiculo.Manutencao.ToString(), "MNO7890");
//    Console.WriteLine(veiculoController.BuscarVeiculoPlaca("MNO7890"));
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.Message);
//}
#endregion




var customer = new ClienteMenu();
var category = new CategoriaMenu();
var vehicle = new VeiculoMenu();
var employeer = new FuncionarioMenu();

int opcao = 0;
do
{
    Console.Clear();
    Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=|");
    Console.WriteLine(" |       >    Sistema Locadora de Veículos    <       |");
    Console.WriteLine(" |----------------------------------------------------|");
    Console.WriteLine(" | [ 1 ] Locar Veículo      |   [ 2 ] Menu Exibir     |");
    Console.WriteLine(" | [ 3 ] Menu Veículo       |   [ 4 ] Menu Categoria  |");
    Console.WriteLine(" | [ 5 ] Menu Funcionario   |   [ 6 ] Menu Cliente    |");
    Console.WriteLine(" | [ 7 ] Sair               |                         |");
    Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=|");
    Console.WriteLine();
    Console.Write("  >>> Informe o menu desejado: ");
    string entrada = Console.ReadLine()!;
    bool conversao = int.TryParse(entrada, out opcao);
    Console.WriteLine("---------------------------------------");

    switch (opcao)
    {
        case 1:
            //
            break;
        case 2:
            //
            break;
        case 3:
            vehicle.MenuVeiculo();
            break;
        case 4:
            category.MenuCategoria();
            break;
        case 5:
            employeer.MenuFuncionario();
            break;
        case 6:
            customer.MenuCliente();
            break;
        case 7:
            Console.WriteLine("Encerrando o programa...");
            return;
        default:
            Console.WriteLine("\nOpção Inválida. Tente novamente.");
            break;
    }

    Console.Write("\n  >  Pressione qualquer Tecla para prosseguir ");
    Console.ReadLine();

} while (true);









































//Cliente cliente = new Cliente("Querie", "querie@email.com", "887891422");
//Documento documento = new Documento("RG", "2345885888", new DateOnly(2020, 1, 1), new DateOnly(2030, 2, 20));

////Console.WriteLine(cliente);

//var clienteController = new ClienteController();

var categoriaController = new CategoriaController();

var veiculoController = new VeiculoController();

var funcionarioController = new FuncionarioController();

var locacaoController = new LocacaoController();


var funcionarios = new List<int>();

//var locacao = new Locacao(2, 5, 100.00m, 10);
//locacaoController.AdicionarLocacao(locacao, funcionarios);
//Console.WriteLine("Locação adicionada com sucesso" + locacao);


//var locacoes = locacaoController.ListarLocacaoPorFuncionario(1);

//foreach (var locacao in locacoes)
//{
//    Console.WriteLine(locacao);
//    Console.WriteLine("-----------------------------");
//}
var funcionariosComLocacoes = locacaoController.ListarFuncionariosComLocacoes();

foreach (var item in funcionariosComLocacoes)
{
    var funcionario = item.funcionario;
    var locacao = item.locacao;

    Console.WriteLine($"Funcionario: {funcionario.Nome} - Salario: {funcionario.Salario}");

    if (locacao != null)
    {
        Console.WriteLine($"LocacaoID: {locacao.LocacaoID}");
        Console.WriteLine($"ClienteID: {locacao.ClienteID}");
        Console.WriteLine($"VeiculoID: {locacao.VeiculoID}");
        Console.WriteLine($"DataLocacao: {locacao.DataLocacao}");
        Console.WriteLine($"ValorDiaria: {locacao.ValorDiaria}");
        Console.WriteLine($"Status: {locacao.Status}");
    }
    else
    {
        Console.WriteLine("Sem locações para este funcionário.");
    }

    Console.WriteLine("-----------------------------");
}

//var veiculo = veiculoController.BuscarVeiculoId(1);
//Console.WriteLine(veiculo);



//var locacoes = locacaoController.ListarLocacaoPorFuncionario(1);

//foreach (var locacao in locacoes)
//{
//    Console.WriteLine(locacao);
//}






//var funcionario = new Funcionario("Aelin Galatynius", "78945612585", "aelin@email.com");
//funcionarioController.AdicionarFuncionario(funcionario);
//Console.WriteLine("Funcionário adicionado com sucesso." + funcionario);



//var funcionarios = funcionarioController.ListarTodosFuncionarios();
//foreach (var funcionario in funcionarios)
//{
//    Console.WriteLine(funcionario);
//    Console.WriteLine("----------------------");
//}

//Console.WriteLine(funcionarioController.BuscarFuncionarioPorID(1));

//try
//{
//    funcionarioController.AtualizarSalarioFuncionario(id: 1003, salario: 3000.00m);
//    Console.WriteLine("Salário atualizado com sucesso");
//}
//catch(Exception ex)
//{
//    Console.WriteLine(ex.Message);
//}






//try
//{
//    var veiculo = new Veiculo(1, "XYZ-7474", "Fiat", "Uno", 2025, EStatusVeiculo.Disponivel.ToString());
//    veiculoController.AdicionarVeiculo(veiculo);
//    Console.WriteLine("Veículo adicionado com sucesso.");

//    var veiculos = veiculoController.ListarTodosVeiculos();

//    foreach (var v in veiculos)
//    {
//        Console.WriteLine(v);
//        Console.WriteLine("--------------------------------");
//        Console.WriteLine();
//    }
//}
//catch (Exception ex)
//{
//    throw new Exception(ex.Message);
//}
//var veiculoEncontrado = veiculoController.BuscarVeiculoPorPlaca("MNO7890");



//var veiculo = veiculoController.BuscarVeiculoPorPlaca("XYZ-7474");
//veiculoController.DeletarVeiculo(veiculo.VeiculoID);
//Console.WriteLine(veiculo);
//try
//{
//    clienteController.AdicionarCliente(cliente, documento);
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.Message);
//}
//    try
//{
//    Console.WriteLine(categoriaController.BuscarCategoriaPorNome("Esportivo"));
//    categoriaController.ExcluirCategoria("Esportivo");
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.Message);
//}
//try
//{
//    veiculoController.AtualizarStatusVeiculo(EStatusVeiculo.Manutencao.ToString(), "ABC1234");
//    Console.WriteLine(veiculoController.BuscarVeiculoPorPlaca("ABC1234"));
//}
//catch(Exception ex)
//{
//    throw new Exception(ex.Message);
//}

//try
//{
//    var listaClientes = clienteController.ListarClientes();

//    foreach (var clienteLista in listaClientes)
//    {
//        Console.WriteLine(clienteLista);
//    }
//}

//catch (Exception ex)
//{
//    Console.WriteLine(ex.Message);
//}

//clienteController.AtualizarTelefoneCliente("121212121212", "farofinha@email.com");
//Console.WriteLine(clienteController.BuscarClientePorEmail("farofinha@email.com"));

//clienteController.DeletarCliente("farofinha@email.com");
//Console.WriteLine(documento);

//try
//{
//    clienteController.AtualizarDocumentoCliente("querie@email.com", documento);
//    Console.WriteLine(clienteController.BuscarClientePorEmail("querie@email.com"));
//}
//catch(Exception ex)
//{
//    throw new Exception("Erro ao atualizar documento");
//}





//CATEGORIAS

//adicionar categoria

//Categoria categoria = new Categoria("Carro elétrico", 200, "Veiculo compacto e confortável");

//try
//{
//    categoriaController.AdicionarCategoria(categoria);
//    Console.WriteLine("Categoria adicionada com sucesso");
//}
//catch (Exception ex)
//{
//    Console.WriteLine("Erro ao adicionar categoria");
//    Console.WriteLine(ex.Message);
//}




//ListarCategorias

//try
//{
//    List<Categoria> categorias = categoriaController.ListarCategorias();


//    foreach (Categoria cat in categorias)
//    {
//        Console.WriteLine(cat);
//        Console.WriteLine("------------------------------------------------------------------");
//    }
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.Message);
//}

//BuscarCategoria

//try
//{
//    var categoria = categoriaController.BuscaCategoriaPorId(1);
//    Console.WriteLine(categoria);
//}
//catch (Exception ex)
//{
//    Console.WriteLine("Categoria não encontrada." + ex.Message);
//}






//DeletarCategoria

//try
//{
//    categoriaController.DeletarCategoria("Carro elétrico");
//    Console.WriteLine("Categoria removida com sucesso");

//}
//catch (Exception ex)
//{
//    Console.WriteLine("Erro ao deletar categoria" + ex.Message);
//}



//AtualizarCategoria

//try
//{

//    categoriaController.AtualizarDescricaoCategoria("SUV", "Veiculo grande e espaçoso");
//    Console.WriteLine("Categoria atualizada com sucesso");
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.Message);
//}

