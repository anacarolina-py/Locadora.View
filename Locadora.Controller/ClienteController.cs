using Locadora.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using Utils.DataBases;

namespace Locadora.Controller
{
    public class ClienteController
    {

        public void AdicionarCliente(Cliente cliente, Documento documento)
        {
            var connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {

                    SqlCommand command = new SqlCommand(Cliente.INSERTCLIENTE, connection, transaction);

                    command.Parameters.AddWithValue("@Nome", cliente.Nome);
                    command.Parameters.AddWithValue("@Email", cliente.Email);
                    command.Parameters.AddWithValue("@Telefone", cliente.Telefone ?? (object)DBNull.Value);

                    int clienteId = Convert.ToInt32(command.ExecuteScalar());
                    cliente.SetClienteID(clienteId);

                    var documentoController = new DocumentoController();

                    documento.setClienteID(clienteId);

                    documentoController.AdicionarDocumento(documento, connection, transaction);

                    transaction.Commit();

                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao inesperado ao adicionar cliente." + ex.Message);
                }

                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao adicionar cliente." + ex.Message);
                }

                finally
                {
                    connection.Close();
                }
            }

        }

        public List<Cliente> ListarClientes()
        {
            var connection = new SqlConnection(ConnectionDB.GetConnectionString());


            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(Cliente.SELECTALLCLIENTES, connection);

                SqlDataReader reader = command.ExecuteReader();

                List<Cliente> clientes = new List<Cliente>();

                while (reader.Read())
                {
                    var cliente = new Cliente(
                        reader["Nome"].ToString(),
                        reader["Email"].ToString(),
                        reader["Telefone"] != DBNull.Value ? reader["Telefone"].ToString() : null);

                    //cliente.SetClienteID(Convert.ToInt32(reader["ClienteID"]));

                    var documento = new Documento(
                        reader["TipoDocumento"].ToString(),
                        reader["Numero"].ToString(),
                        DateOnly.FromDateTime(reader.GetDateTime(5)),
                        DateOnly.FromDateTime(reader.GetDateTime(6))

                        );

                    cliente.SetDocumento(documento);
                    clientes.Add(cliente);
                }

                return clientes;
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao listar clientes." + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao listar clientes." + ex.Message);
            }
            finally
            {
                connection.Close();
            }

        }

        public Cliente BuscarClientePorEmail(string email)

        {
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();
            try
            {
                SqlCommand command = new SqlCommand(Cliente.SELECTCLIENTEPOREMAIL, connection);

                command.Parameters.AddWithValue("@Email", email);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    var cliente = new Cliente(
                        reader["Nome"].ToString(),
                        reader["Email"].ToString(),
                        reader["Telefone"] != DBNull.Value ? reader["Telefone"].ToString() : null);

                    var documento = new Documento(
                        reader["TipoDocumento"].ToString(),
                        reader["Numero"].ToString(),
                        DateOnly.FromDateTime(reader.GetDateTime(6)),
                        DateOnly.FromDateTime(reader.GetDateTime(7))

                        );
                    cliente.SetClienteID(Convert.ToInt32(reader["ClienteID"]));
                    return cliente;
                }
                return null;

            }
            catch (SqlException ex)
            {
                throw new Exception("Cliente não encontrado.");
            }
            catch (Exception ex)
            {
                throw new Exception("Cliente não encontrado.");
            }
            finally
            {
                connection.Close();
            }
        }

        public Cliente BuscaClientePorId(int id)
        {
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();

            using (connection)
            {
                try
                {
                    SqlCommand command = new SqlCommand(Cliente.SELECTCLIENTEPORID, connection);
                    command.Parameters.AddWithValue("@ClienteID", id);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        var cliente = new Cliente(
                            reader["Nome"].ToString(),
                            reader["Email"].ToString(),
                            reader["Telefone"] != DBNull.Value ? reader["Telefone"].ToString() : null
                        );
                        cliente.SetClienteID(Convert.ToInt32(reader["ClienteID"]));

                        var documento = new Documento(
                            reader["TipoDocumento"].ToString(),
                            reader["Numero"].ToString(),
                            DateOnly.FromDateTime(reader.GetDateTime(6)),
                            DateOnly.FromDateTime(reader.GetDateTime(7))
                        );

                        cliente.SetDocumento(documento);
                        return cliente;
                    }
                    return null;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao buscar cliente por email: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro inesperado ao buscar cliente por email: " + ex.Message);
                }

            }


        }
        public void AtualizarTelefoneCliente(string telefone, string email)
        {
            var clienteEncontrado = this.BuscarClientePorEmail(email);

            if (clienteEncontrado == null)
            {
                throw new Exception("Cliente não encontrado.");
            }
            clienteEncontrado.SetTelefone(telefone);

            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();

            try
            {
                SqlCommand command = new SqlCommand(Cliente.UPDATELEFONECLIENTE, connection);
                command.Parameters.AddWithValue("@Telefone", clienteEncontrado.Telefone);
                command.Parameters.AddWithValue("@ClienteID", clienteEncontrado.ClienteID);

                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao atualizar telefone do cliente." + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar telefone do cliente." + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        public void AtualizarDocumentoCliente(string email, Documento documento)
        {
            var clienteEncontrado = this.BuscarClientePorEmail(email);

            if (clienteEncontrado == null)
            {
                throw new Exception("Cliente não encontrado.");
            }
            clienteEncontrado.SetDocumento(documento);

            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    clienteEncontrado.SetClienteID(clienteEncontrado.ClienteID);
                    DocumentoController documentoController = new DocumentoController();
                    documentoController.AtualizarDocumento(documento, connection, transaction);

                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao atualizar documento do cliente." + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao atualizar documento do cliente." + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        
        public void DeletarCliente(string email)
        {
            var clienteEncontrado = BuscarClientePorEmail(email);
            if (clienteEncontrado == null)
            {
                throw new Exception("Não existe cliente com esse email cadastrado");
            }
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            using (SqlTransaction transaction = connection.BeginTransaction())
            {

                try
                {
                    SqlCommand command = new SqlCommand(Cliente.DELETECLIENTE, connection, transaction);
                    command.Parameters.AddWithValue("@ClienteId", clienteEncontrado.ClienteID);
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao deletar cliente." + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro inesperado ao deletar cliente." + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
