using Locadora.Controller.Interfaces;
using Locadora.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.DataBases;

namespace Locadora.Controller
{
    public class VeiculoController : IVeiculoController
    {
        public void AdicionarVeiculo(Veiculo veiculo)
        {
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();


            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    SqlCommand command = new SqlCommand(Veiculo.INSERTVEICULO, connection, transaction);
                    command.Parameters.AddWithValue("@CategoriaID", veiculo.CategoriaID);
                    command.Parameters.AddWithValue("@Placa", veiculo.Placa);
                    command.Parameters.AddWithValue("@Marca", veiculo.Marca);
                    command.Parameters.AddWithValue("@Modelo", veiculo.Modelo);
                    command.Parameters.AddWithValue("@Ano", veiculo.Ano);
                    command.Parameters.AddWithValue("@StatusVeiculo", veiculo.StatusVeiculo);
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao adicionar veículo." + ex.Message);

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro inesperado ao adicionar veículo." + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void AtualizarStatusVeiculo(string statusVeiculo, string placa)
        {
            Veiculo veiculoEncontrado = BuscarVeiculoPorPlaca(placa);
            if (veiculoEncontrado is null)
            {
                throw new Exception("Veículo não encontrado.");
            }

            veiculoEncontrado.SetStatusVeiculo(statusVeiculo);

            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();
            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                SqlCommand command = new SqlCommand(Veiculo.UPDATESTATUSVEICULO, connection, transaction);

                try
                {
                    command.Parameters.AddWithValue("@StatusVeiculo", statusVeiculo);
                    command.Parameters.AddWithValue("@VeiculoID", veiculoEncontrado.VeiculoID);

                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao atualizar o veículo: " + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro inesperado ao atualizar o veículo: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public Veiculo BuscarVeiculoPorPlaca(string placa)
        {
  
            var categoriaController = new CategoriaController();

            Veiculo veiculo = null;

            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();

            using (SqlCommand command = new SqlCommand(Veiculo.SELECTVEICULOPELAPLACA, connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@Placa", placa);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                       
                        while (reader.Read())
                        {
                            veiculo = new Veiculo(
                               reader.GetInt32(1),
                               reader.GetString(2),
                               reader.GetString(3),
                               reader.GetString(4),
                               reader.GetInt32(5),
                               reader.GetString(6)
                               );

                            veiculo.SetVeiculoID(reader.GetInt32(0));
                            veiculo.SetNomeCategoria(categoriaController.BuscarNomeCategoriaPorId(veiculo.CategoriaID));

                        }

                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro inesperado ao encontrar veículo." + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao encontrar veículo." + ex.Message);
                }
                finally
                {

                    connection.Close();
                }
                return veiculo ?? throw new Exception("Veículo não encontrado.");

            }
        }

        public void DeletarVeiculo(int idVeiculo)
        {
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                SqlCommand command = new SqlCommand(Veiculo.DELETEVEICULO, connection, transaction);

                try
                {
                    command.Parameters.AddWithValue("VeiculoID", idVeiculo);
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao deletar veículo." + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro inesperado ao deletar veículo." + ex.Message);
                }
                finally
                {

                    connection.Close();
                }
            
            }

        }

        public List<Veiculo> ListarTodosVeiculos()

        {
            var veiculos = new List<Veiculo>();

            var categoriaController = new CategoriaController();

            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();

            using (SqlCommand command = new SqlCommand(Veiculo.SELECTALLVEICULOS, connection))
            {
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Veiculo veiculo = new Veiculo(
                               reader.GetInt32(0),
                               reader.GetString(1),
                               reader.GetString(2),
                               reader.GetString(3),
                               reader.GetInt32(4),
                               reader.GetString(5)
                               );

                            veiculo.SetNomeCategoria(categoriaController.BuscarNomeCategoriaPorId(veiculo.CategoriaID));
                            veiculos.Add(veiculo);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro no banco de dados ao listar veículos." + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao listar veículos." + ex.Message);
                }
                finally
                {

                    connection.Close();
                }
                return veiculos;
            }

        }

    }
}
