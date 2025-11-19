
using Locadora.Controller;
using Locadora.Models;
using Microsoft.Data.SqlClient;
using Utils.DataBases;

//Cliente cliente = new Cliente("Farofinha", "farofinha@email.com", "99789696");
////Documento documento = new Documento(1, "RG", "12345678", new DateOnly(2020, 1, 1), new DateOnly(2030, 2, 20));

//Console.WriteLine(cliente);

var clienteController = new ClienteController();

//try
//{
//    clienteController.AdicionarCliente(cliente);
//}
//catch(Exception ex)
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

clienteController.AtualizarTelefoneCliente("989898989898", "farofinha@email.com");
clienteController.BuscarClientePorEmail("farofinha@email.com");

//Console.WriteLine(documento);
