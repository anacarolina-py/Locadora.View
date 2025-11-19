
using Locadora.Controller;
using Locadora.Models;
using Microsoft.Data.SqlClient;
using Utils.DataBases;

Cliente cliente = new Cliente("Querie", "querie@email.com", "887891422");
Documento documento = new Documento("RG", "2345885888", new DateOnly(2020, 1, 1), new DateOnly(2030, 2, 20));

//Console.WriteLine(cliente);

var clienteController = new ClienteController();

var categoriaController = new CategoriaController();

//try
//{
//    clienteController.AdicionarCliente(cliente, documento);
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.Message);
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
//   categoriaController.AdicionarCategoria(categoria);
//   Console.WriteLine("Categoria adicionada com sucesso");}
//catch(Exception ex)
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
//    var categoria = categoriaController.BuscaCategoriaPorNome("SUV");
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

try
{
    
    categoriaController.AtualizarDescricaoCategoria("SUV", "Veiculo grande e espaçoso");
    Console.WriteLine("Categoria atualizada com sucesso");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

