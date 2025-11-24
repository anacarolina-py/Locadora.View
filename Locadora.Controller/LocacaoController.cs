using Locadora.Controller.Interfaces;
using Locadora.Models;
using Locadora.Models.Enums;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Utils.DataBases;

namespace Locadora.Controller
{
    public class LocacaoController : ILocacaoController
    {
        public void AdicionarLocacao(Locacao locacao, List<int>? funcionarios)
        {
            using (var connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1️⃣ Inserir locação e obter o ID gerado
                        using (var command = new SqlCommand(Locacao.INSERTLOCACAO, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@ClienteID", locacao.ClienteID);
                            command.Parameters.AddWithValue("@VeiculoID", locacao.VeiculoID);
                            command.Parameters.AddWithValue("@DataLocacao", locacao.DataLocacao);
                            command.Parameters.AddWithValue("@DataDevolucaoPrevista", locacao.DataDevolucaoPrevista);
                            command.Parameters.AddWithValue("@ValorDiaria", locacao.ValorDiaria);
                            command.Parameters.AddWithValue("@ValorTotal", locacao.ValorTotal);
                            command.Parameters.AddWithValue("@Multa", locacao.Multa);
                            command.Parameters.AddWithValue("@Status", locacao.Status.ToString());

                            // Executa e retorna o ID gerado
                            locacao.SetLocacaoId((Guid)command.ExecuteScalar());
                        }

                        // 2️⃣ Associar funcionários (se houver)
                        if (funcionarios != null && funcionarios.Count > 0)
                        {
                            foreach (var funcId in funcionarios)
                            {
                                using (var cmdAssoc = new SqlCommand(LocacaoFuncionario.ASSOCIARFUNCIONARIO, connection, transaction))
                                {
                                    cmdAssoc.Parameters.AddWithValue("@LocacaoID", locacao.LocacaoID);
                                    cmdAssoc.Parameters.AddWithValue("@FuncionarioID", funcId);
                                    cmdAssoc.ExecuteNonQuery();
                                }
                            }
                        }

                        // 3️⃣ Atualizar status do veículo para "Alugado" usando a placa
                        VeiculoController controllerVeiculo = new VeiculoController();
                        string placaVeiculo = controllerVeiculo.BuscarPlacaPorId(locacao.VeiculoID);

                        using (var commandVeiculo = new SqlCommand(Veiculo.UPDATESTATUSVEICULO, connection, transaction))
                        {
                            commandVeiculo.Parameters.AddWithValue("@StatusVeiculo", "Alugado");
                            commandVeiculo.Parameters.AddWithValue("@Placa", placaVeiculo);
                            commandVeiculo.ExecuteNonQuery();
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
                }
            }
        }

        public void AtualizarValorTotalLocacao(Locacao locacao)
        {
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();
            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    SqlCommand command = new SqlCommand(Locacao.UPDATELOCACAOVALORTOTAL, connection, transaction);
                    command.Parameters.AddWithValue("@ValorTotal", locacao.ValorTotal);
                    command.Parameters.AddWithValue("@LocacaoID", locacao.LocacaoID);
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao atualizar o valor total da locação: erro ao conectar com o banco: " + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao atualizar o valor total da locação: " + ex.Message);
                }
            }
        }
        public List<Locacao> ListarLocacoes()
        {
            var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            var clienteController = new ClienteController();
            var veiculoController = new VeiculoController();
            CategoriaController categoriaController = new CategoriaController();

            List<Locacao> locacoes = new List<Locacao>();

            using (SqlCommand command = new SqlCommand(Locacao.SELECTALLLOCACOES, connection))
            {
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var cliente = clienteController.BuscaClientePorId(reader.GetInt32(1));
                            var veiculo = veiculoController.BuscarVeiculoId(reader.GetInt32(2));


                            EStatusLocacao status;
                            string statusStr = reader["Status"] != DBNull.Value ? reader.GetString(9) : "Ativa";

                            if (!Enum.TryParse(statusStr, true, out status))
                            {
                                status = EStatusLocacao.Ativa;
                            }

                            var locacao = new Locacao(
                                reader.GetGuid(0),
                                cliente,
                                veiculo,
                                reader.GetDateTime(3),
                                reader.GetDateTime(4),
                                reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                                reader.GetDecimal(6),
                                reader.GetDecimal(7),
                                reader.GetDecimal(8),
                                statusStr
                            );

                            locacoes.Add(locacao);
                        }
                    }

                    return locacoes;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao listar locações: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro inesperado ao listar locações: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void AtualizarDataDevolucaoRealLocacao(Locacao locacao, DateTime? dataDevolucao)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlCommand command = new SqlCommand(Locacao.UPDATELOCACAODEVOLUCAOREAL, connection, transaction);

                        
                        command.Parameters.AddWithValue("@DataDevolucaoReal", (object?)dataDevolucao ?? DBNull.Value);
                        command.Parameters.AddWithValue("@LocacaoID", locacao.LocacaoID);

                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro ao atualizar a data de devolução da locação: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro ao atualizar a data de devolução da locação: " + ex.Message);
                    }
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
                            locacao.SetLocacaoId((Guid)reader["LocacaoID"]);

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
        public void AtualizarStatusLocacao(Locacao locacao, string status)
        {

            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();
            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    SqlCommand command = new SqlCommand(Locacao.UPDATELOCACAOSTATUS, connection, transaction);
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@LocacaoID", locacao.LocacaoID);
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

            using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            using var command = new SqlCommand(Locacao.SELECTLOCACAOPORCLIENTE, connection);
            command.Parameters.AddWithValue("@ClienteID", clienteId);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var locacao = new Locacao(
                    reader["ClienteID"] != DBNull.Value ? Convert.ToInt32(reader["ClienteID"]) : 0,
                    reader["VeiculoID"] != DBNull.Value ? Convert.ToInt32(reader["VeiculoID"]) : 0,
                    reader["DataLocacao"] != DBNull.Value ? Convert.ToDateTime(reader["DataLocacao"]) : DateTime.MinValue,
                    reader["DataDevolucaoPrevista"] != DBNull.Value ? Convert.ToDateTime(reader["DataDevolucaoPrevista"]) : DateTime.MinValue,
                    reader["ValorDiaria"] != DBNull.Value ? Convert.ToDecimal(reader["ValorDiaria"]) : 0m,
                    reader["Status"] != DBNull.Value ? reader["Status"].ToString() : string.Empty
                );

                locacoes.Add(locacao);
            }

            return locacoes;
        }

        public List<Locacao> ListarLocacaoPorFuncionario(int funcionarioID)
        {
            var locacoes = new List<Locacao>();

            using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            using var command = new SqlCommand(Locacao.SELECTLOCACAOPORFUNCIONARIO, connection);
            command.Parameters.AddWithValue("@FuncionarioID", funcionarioID);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var locacao = new Locacao(
                    reader["ClienteID"] != DBNull.Value ? Convert.ToInt32(reader["ClienteID"]) : 0,
                    reader["VeiculoID"] != DBNull.Value ? Convert.ToInt32(reader["VeiculoID"]) : 0,
                    reader["DataLocacao"] != DBNull.Value ? Convert.ToDateTime(reader["DataLocacao"]) : DateTime.MinValue,
                    reader["DataDevolucaoPrevista"] != DBNull.Value ? Convert.ToDateTime(reader["DataDevolucaoPrevista"]) : DateTime.MinValue,
                    reader["ValorDiaria"] != DBNull.Value ? Convert.ToDecimal(reader["ValorDiaria"]) : 0m,
                    reader["Status"] != DBNull.Value ? reader["Status"].ToString() : string.Empty
                );

                locacoes.Add(locacao);
            }

            return locacoes;
        }

       


    }

}
