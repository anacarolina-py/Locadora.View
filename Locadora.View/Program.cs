
using Locadora.Controller;
using Locadora.Models;
using Locadora.Models.Enums;
using Microsoft.Data.SqlClient;
using Utils.DataBases;

//Cliente cliente = new Cliente("Querie", "querie@email.com", "887891422");
//Documento documento = new Documento("RG", "2345885888", new DateOnly(2020, 1, 1), new DateOnly(2030, 2, 20));

////Console.WriteLine(cliente);

//var clienteController = new ClienteController();

var categoriaController = new CategoriaController();

var veiculoController = new VeiculoController();

var funcionarioController = new FuncionarioController();

var locacaoController = new LocacaoController();


var locacao = new Locacao(1010, 2002, 100.00m, 10 );
locacaoController.AdicionarLocacao(locacao);
Console.WriteLine("Locação adicionada com sucesso"+ locacao);
















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

