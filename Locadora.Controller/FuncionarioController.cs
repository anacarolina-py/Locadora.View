using Locadora.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.DataBases;

namespace Locadora.Controller
{
    public class FuncionarioController
    {
        public void AdicionarFuncionario(Funcionario funcionario)
        {
            var connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {

                    SqlCommand command = new SqlCommand(Funcionario.INSERTFUNCIONARIO, connection, transaction);

                    command.Parameters.AddWithValue("@Nome", funcionario.Nome);
                    command.Parameters.AddWithValue("@CPF", funcionario.CPF);
                    command.Parameters.AddWithValue("@Email", funcionario.Email);
                    command.Parameters.AddWithValue("@Salario", funcionario.Salario is decimal s ? (object)s : DBNull.Value);

                    command.ExecuteNonQuery();

                    transaction.Commit();

                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao inesperado ao adicionar funcionário." + ex.Message);
                }

                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao adicionar funcionário." + ex.Message);
                }

                finally
                {
                    connection.Close();
                }
            }

        }

        public List<Funcionario> ListarTodosFuncionarios()
        {
            var connection = new SqlConnection(ConnectionDB.GetConnectionString());


            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(Funcionario.SELECTALLFUNCIONARIOS, connection);

                SqlDataReader reader = command.ExecuteReader();

                List<Funcionario> funcionarios = new List<Funcionario>();

                while (reader.Read())
                {
                    var funcionario = new Funcionario(
                        reader["Nome"].ToString(),
                        reader["CPF"].ToString(),
                        reader["Email"].ToString(),
                        reader["Salario"] is decimal s ? s : reader["Salario"] != DBNull.Value ? Convert.ToDecimal(reader["Salario"]) : 0m);

                    funcionario.SetFuncionarioID(Convert.ToInt32(reader["FuncionarioID"]));

                    funcionarios.Add(funcionario);
                }

                return funcionarios;
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao listar funcionários." + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao listar funcionários." + ex.Message);
            }
            finally
            {
                connection.Close();
            }

        }
           
        public Funcionario BuscarFuncionarioPorID(int id)
        {
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();

            using (connection)
            {
                try
                {
                    SqlCommand command = new SqlCommand(Funcionario.SELECTFUNCIONARIOPORID, connection);
                    command.Parameters.AddWithValue("@FuncionarioID", id);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        var funcionario = new Funcionario(
                        reader["Nome"].ToString(),
                        reader["CPF"].ToString(),
                        reader["Email"].ToString(),
                        reader["Salario"] is decimal s ? s : reader["Salario"] != DBNull.Value ? Convert.ToDecimal(reader["Salario"]) : 0m);


                        return funcionario;
                    }
                    return null;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao buscar funcionário por id: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro inesperado ao buscar funcionário por id: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void AtualizarSalarioFuncionario(string email, decimal salario)
        {
            var funcionarioEncontrado = BuscarFuncionarioEmail(email);

            if (funcionarioEncontrado == null)
            {
                throw new Exception("Funcionário não encontrado.");
            }


            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();

            funcionarioEncontrado.SetSalario(salario);

            try
            {
                SqlCommand command = new SqlCommand(Funcionario.UPDATESALARIOFUNCIONARIO, connection);

                command.Parameters.AddWithValue("@Salario", salario);
                command.Parameters.AddWithValue("@Email", email);

                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro inesperado ao atualizar salário do funcionário" + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar salário do funcionário." + ex.Message);
            }
            finally
            {
                connection.Close();
            }

        }

        public void DeletarFuncionario(string email)
        {
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                SqlCommand command = new SqlCommand(Funcionario.DELETEFUNCIONARIO, connection, transaction);

                try
                {
                    command.Parameters.AddWithValue("Email", email);
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao deletar funcionário." + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro inesperado ao deletar funcionário." + ex.Message);
                }
                finally
                {

                    connection.Close();
                }

            }

        }

        public Funcionario BuscarFuncionarioEmail(string email)
        {
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();
            try
            {
                SqlCommand command = new SqlCommand(Funcionario.SELECTFUNCIONARIOSPOREMAIL, connection);

                command.Parameters.AddWithValue("@Email", email);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    var funcionario = new Funcionario(reader["Nome"].ToString()!,
                                                reader["CPF"].ToString()!,
                                                reader["Email"].ToString()!,
                                                reader["Salario"] != DBNull.Value ?
                                                reader.GetDecimal(4) : null
                                                );

                    funcionario.SetFuncionarioID(Convert.ToInt32(reader["FuncionarioID"]));

                    return funcionario;
                }
                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao buscar funcionario por email: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao buscar funcionario por email: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

    }

    
}
