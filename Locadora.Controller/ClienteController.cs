using Locadora.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using Utils.DataBases;

namespace Locadora.Controller
{
    public class ClienteController
    {
        
        public void AdicionarCliente(Cliente cliente)
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
                    var cliente = new Cliente(reader["Nome"].ToString(),
                        reader["Email"].ToString(),
                        reader["Telefone"] != DBNull.Value ? reader["Telefone"].ToString() : null);

                    cliente.SetClienteID(Convert.ToInt32(reader["ClienteID"]));

                    clientes.Add(cliente);
                }

                return clientes;
            }
            catch(SqlException ex)
            {
                throw new Exception("Erro ao listar clientes." + ex.Message);
            }
            catch(Exception ex)
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
                    var cliente = new Cliente(reader["Nome"].ToString(),
                        reader["Email"].ToString(),
                        reader["Telefone"] != DBNull.Value ? reader["Telefone"].ToString() : null);

                    cliente.SetClienteID(Convert.ToInt32(reader["ClienteID"]));
                    return cliente;  
                }
                return null;

            }
            catch(SqlException ex)
            {
                throw new Exception("Cliente não encontrado.");
            }
            catch(Exception ex)
            {
                throw new Exception("Cliente não encontrado.");
            }
            finally
            {
                connection.Close();
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
    }
}
