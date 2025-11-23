using Locadora.Controller.Interfaces;
using Locadora.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Utils.DataBases;

namespace Locadora.Controller
{
    public class LocacaoController : ILocacaoController
    {
        public void AdicionarLocacao(Locacao locacao)
        {
            using (var connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        //  verificar disponibilidade do veículo
                        
                        using (var command = new SqlCommand(Veiculo.CHECKVEICULO, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@VeiculoID", locacao.VeiculoID);
                            using (var reader = command.ExecuteReader())
                            {
                                if (!reader.Read())
                                    throw new Exception("Veículo não encontrado.");

                                string statusVeiculo = reader.GetString(reader.GetOrdinal("StatusVeiculo"));
                               

                                if (!string.Equals(statusVeiculo, "Disponível", StringComparison.OrdinalIgnoreCase))
                                    throw new Exception("Veículo não está disponível para locação.");

                            }
                        }


                        // Inserir locação
         
                        Guid locacaoId;
                        using (var command = new SqlCommand(Locacao.INSERTLOCACAO, connection, transaction))
                        {

                            command.Parameters.AddWithValue("@ClienteID", locacao.ClienteID);
                            command.Parameters.AddWithValue("@VeiculoID", locacao.VeiculoID);
                            command.Parameters.AddWithValue("@DataLocacao", locacao.DataLocacao);
                            command.Parameters.AddWithValue("@DataDevolucaoPrevista", locacao.DataDevolucaoPrevista);
                            command.Parameters.AddWithValue("@ValorDiaria", locacao.ValorDiaria);
                            command.Parameters.AddWithValue("@ValorTotal", locacao.ValorTotal);
                            command.Parameters.AddWithValue("@Multa", 0m);
                            command.Parameters.AddWithValue("@Status", "Ativa");

                            locacaoId = command.ExecuteScalar() as Guid? ?? Guid.Empty;

                        }

                        // associar funcionários 
                        //if (funcionariosId != null && funcionariosId.Count > 0)
                        //{
                          
                        //    foreach (var funcId in funcionariosId)
                        //    {
                        //        using (var command = new SqlCommand(LocacaoFuncionario.ASSOCIARFUNCIONARIO, connection, transaction))
                        //        {
                        //            command.Parameters.AddWithValue("@LocacaoID", locacaoId);
                        //            command.Parameters.AddWithValue("@FuncionarioID", funcId);
                        //            command.ExecuteNonQuery();
                        //        }
                        //    }
                        //}

                        // atualizar status do veículo para alugad
                        
                        using (var command = new SqlCommand(Veiculo.UPDATESTATUSVEICULO, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@StatusVeiculo", "Alugado");
                            command.Parameters.AddWithValue("@VeiculoID", locacao.VeiculoID);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro de banco ao adicionar locação: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro ao adicionar locação: " + ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
            

        public List<Locacao> ListarLocacoes()
        {
            var locacoes = new List<Locacao>();

            using (var connection = new SqlConnection(ConnectionDB.GetConnectionString()))

            {
                connection.Open();

                SqlCommand command = new SqlCommand(Locacao.SELECTALLLOCACOES, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            var locacao = new Locacao(
                             
                                    Convert.ToInt32(reader["ClienteID"]),
                                    Convert.ToInt32(reader["VeiculoID"]),
                                    Convert.ToDateTime(reader["DataLocacao"]),
                                    Convert.ToDateTime(reader["DataDevolucaoPrevista"]),
                                    Convert.ToDecimal(reader["ValorDiaria"]),
                                    reader["Status"].ToString()
                                    );
                            locacoes.Add(locacao);
                        }
                    }
                                   
                    catch (SqlException ex)
                    {
                        throw new Exception("Erro no banco de dados ao listar locações." + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Erro ao listar locações." + ex.Message);
                    }
                    finally
                    {
                        connection?.Close();
                    }
                    return locacoes;
                }
                }
            }

        public void AtualizarDataDevolucaoRealLocacao(Guid id, DateTime devolucao)
        {
            var locacaoEncontrada = BuscarLocacaoPorId(id);
            if (locacaoEncontrada is null)
            {
                throw new Exception("Locação não encontrada!");
            }

            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();
            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    SqlCommand command = new SqlCommand(Locacao.UPDATELOCACAODEVOLUCAOREAL, connection, transaction);
                    command.Parameters.AddWithValue("@DataDevolucaoReal", devolucao);
                    command.Parameters.AddWithValue("@LocacaoID", id);
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao atualizar a data de devolução do veículo: " + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao atualizar a data de devolução do veículo: " + ex.Message);
                }
            }
        }

        public Locacao BuscarLocacaoPorId(Guid id)
        {

            var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();


            using (SqlCommand command = new SqlCommand(Locacao.SELECTLOCACAOPORID, connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@LocacaoID", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {


                            var locacao = new Locacao(
                                    Convert.ToInt32(reader["ClienteID"]),
                                    Convert.ToInt32(reader["VeiculoID"]),
                                    Convert.ToDateTime(reader["DataLocacao"]),
                                    Convert.ToDateTime(reader["DataDevolucaoPrevista"]),
                                    Convert.ToDecimal(reader["ValorDiaria"]),
                                    reader["Status"].ToString()
                                    );

                            return locacao;
                        }

                    }
                    return null;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Locação não encontrada: erro no acesso ao banco" + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Locação não encontrada" + ex.Message);
                }
            }
        }
        public void AtualizarStatusLocacao(Guid id, string status)
        {
            var locacaoEncontrada = BuscarLocacaoPorId(id);
            if (locacaoEncontrada is null)
            {
                throw new Exception("Locação não encontrada!");
            }

            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();
            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    SqlCommand command = new SqlCommand(Locacao.UPDATELOCACAOSTATUS, connection, transaction);
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@LocacaoID", id);
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao atualizar o status da locação: erro ao conectar com o banco: " + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao atualizar o status da locação: " + ex.Message);
                }
            }
        }

        public List<Locacao> ListarLocacaoPorCliente(int clienteId)
        {
            var locacoes = new List<Locacao>();

            using (var connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(Locacao.SELECTLOCACAOPORCLIENTE, connection);
                command.Parameters.AddWithValue("@ClienteID", clienteId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            var locacao = new Locacao(
                                    Convert.ToInt32(reader["ClienteID"]),
                                    Convert.ToInt32(reader["VeiculoID"]),
                                    Convert.ToDateTime(reader["DataLocacao"]),
                                    Convert.ToDateTime(reader["DataDevolucaoPrevista"]),
                                    Convert.ToDecimal(reader["ValorDiaria"]),
                                    reader["Status"].ToString()
                                    );

                            locacoes.Add(locacao);
                        }
                    }

                    catch (SqlException ex)
                    {
                        throw new Exception("Erro no banco de dados ao listar locações." + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Erro ao listar locações." + ex.Message);
                    }
                    finally
                    {
                        connection?.Close();
                    }
                    return locacoes;
                }
            }
        }


        public List<Locacao> ListarLocacaoPorFuncionario(int funcionarioID)
        {
            var locacoes = new List<Locacao>();

            using (var connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(Locacao.SELECTLOCACAOPORFUNCIONARIO, connection);
                command.Parameters.AddWithValue("@FuncionarioID", funcionarioID);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            var locacao = new Locacao(
                                    Convert.ToInt32(reader["ClienteID"]),
                                    Convert.ToInt32(reader["VeiculoID"]),
                                    Convert.ToDateTime(reader["DataLocacao"]),
                                    Convert.ToDateTime(reader["DataDevolucaoPrevista"]),
                                    Convert.ToDecimal(reader["ValorDiaria"]),
                                    reader["Status"].ToString()
                                    );

                            locacoes.Add(locacao);
                        }
                    }

                    catch (SqlException ex)
                    {
                        throw new Exception("Erro no banco de dados ao listar locações." + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Erro ao listar locações." + ex.Message);
                    }
                    finally
                    {
                        connection?.Close();
                    }
                    return locacoes;
                }
            }
        }


       
            public List<(Funcionario funcionario, Locacao locacao)> ListarFuncionariosComLocacoes()
        {
            var resultados = new List<(Funcionario, Locacao)>();

            using (var connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                using (var command = new SqlCommand(Locacao.SELECTALLLOCACOESFUNCIONARIOS, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        
                        var funcionario = new Funcionario(
                        reader["Nome"] != DBNull.Value ? reader["Nome"].ToString() : string.Empty,
                        reader["CPF"] != DBNull.Value ? reader["CPF"].ToString() : string.Empty,
                        reader["Email"] != DBNull.Value ? reader["Email"].ToString() : string.Empty,
                        reader["Salario"] != DBNull.Value ? Convert.ToDecimal(reader["Salario"]) : 0m
);

                        funcionario.SetFuncionarioID(reader["FuncionarioID"] != DBNull.Value ? Convert.ToInt32(reader["FuncionarioID"]) : 0);

                         Locacao locacao = null;
                        if (reader["LocacaoID"] != DBNull.Value)
                        {
                            locacao = new Locacao(
                                reader["ClienteID"] != DBNull.Value ? Convert.ToInt32(reader["ClienteID"]) : 0,
                                reader["VeiculoID"] != DBNull.Value ? Convert.ToInt32(reader["VeiculoID"]) : 0,
                                reader["DataLocacao"] != DBNull.Value ? Convert.ToDateTime(reader["DataLocacao"]) : DateTime.MinValue,
                                reader["DataDevolucaoPrevista"] != DBNull.Value ? Convert.ToDateTime(reader["DataDevolucaoPrevista"]) : DateTime.MinValue,
                                reader["ValorDiaria"] != DBNull.Value ? Convert.ToDecimal(reader["ValorDiaria"]) : 0m,
                                reader["Status"] != DBNull.Value ? reader["Status"].ToString() : string.Empty
                            );

                            locacao.SetLocacaoId(reader["LocacaoID"] != DBNull.Value ? (Guid)reader["LocacaoID"] : Guid.Empty);
                          
                        }

                        resultados.Add((funcionario, locacao));
                    }
                }
            }

            return resultados;
        }
    }
    }

