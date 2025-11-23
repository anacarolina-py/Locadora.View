using Locadora.Controller.Interfaces;
using Locadora.Models;
using Locadora.Models.Enums;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using Utils.DataBases;

namespace Locadora.Controller
{
    public class VeiculoController : IVeiculoController
    {
        public void AdicionarVeiculo(Veiculo veiculo)
        {
            using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                SqlCommand command = new SqlCommand(Veiculo.INSERTVEICULO, connection, transaction);
                command.Parameters.AddWithValue("@CategoriaID", veiculo.CategoriaID);
                command.Parameters.AddWithValue("@Placa", veiculo.Placa);
                command.Parameters.AddWithValue("@Marca", veiculo.Marca);
                command.Parameters.AddWithValue("@Modelo", veiculo.Modelo);
                command.Parameters.AddWithValue("@Ano", veiculo.Ano);
                command.Parameters.AddWithValue("@StatusVeiculo", "Disponível");
                command.ExecuteNonQuery();

                // Setar objeto Categoria completo
                var categoriaController = new CategoriaController();
                var categoria = categoriaController.BuscaCategoriaPorId(veiculo.CategoriaID);
                veiculo.SetCategoria(categoria);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Erro ao adicionar veículo: " + ex.Message);
            }
        }

        public void AtualizarStatusVeiculo(string statusVeiculo, string placa)
        {
            var veiculo = BuscarVeiculoPorPlaca(placa);
            veiculo.SetStatusVeiculo(statusVeiculo);

            using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();
            using var transaction = connection.BeginTransaction();
            using var command = new SqlCommand(Veiculo.UPDATESTATUSVEICULO, connection, transaction);

            try
            {
                command.Parameters.AddWithValue("@StatusVeiculo", statusVeiculo);
                command.Parameters.AddWithValue("@VeiculoID", veiculo.VeiculoID);
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Erro ao atualizar veículo: " + ex.Message);
            }
        }

        public Veiculo BuscarVeiculoPorPlaca(string placa)
        {
            Veiculo veiculo = null;
            var categoriaController = new CategoriaController();

            using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();
            using var command = new SqlCommand(Veiculo.SELECTVEICULOPELAPLACA, connection);
            command.Parameters.AddWithValue("@Placa", placa);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                veiculo = new Veiculo(
                    reader.GetInt32(1), // CategoriaID
                    reader.GetString(2), // Placa
                    reader.GetString(3), // Marca
                    reader.GetString(4), // Modelo
                    reader.GetInt32(5), // Ano
                    reader.GetString(6) // Status
                );
                veiculo.SetVeiculoID(reader.GetInt32(0));

                var categoria = categoriaController.BuscaCategoriaPorId(veiculo.CategoriaID);
                veiculo.SetCategoria(categoria);
            }

            return veiculo ?? throw new Exception("Veículo não encontrado.");
        }

        public Veiculo BuscarVeiculoId(int id)
        {
            Veiculo veiculo = null;
            var categoriaController = new CategoriaController();

            using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();
            using var command = new SqlCommand(Veiculo.SELECTVEICULOBYID, connection);
            command.Parameters.AddWithValue("@VeiculoID", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
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

                var categoria = categoriaController.BuscaCategoriaPorId(veiculo.CategoriaID);
                veiculo.SetCategoria(categoria);
            }

            return veiculo ?? throw new Exception("Veículo não encontrado.");
        }

        public void DeletarVeiculo(int idVeiculo)
        {
            using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();
            using var transaction = connection.BeginTransaction();
            using var command = new SqlCommand(Veiculo.DELETEVEICULO, connection, transaction);

            try
            {
                command.Parameters.AddWithValue("@VeiculoID", idVeiculo);
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Erro ao deletar veículo: " + ex.Message);
            }
        }

        public List<Veiculo> ListarTodosVeiculos()
        {
            var veiculos = new List<Veiculo>();
            var categoriaController = new CategoriaController();

            using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();
            using var command = new SqlCommand(Veiculo.SELECTALLVEICULOS, connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var veiculo = new Veiculo(
                    reader.GetInt32(0), 
                    reader.GetString(1), 
                    reader.GetString(2), 
                    reader.GetString(3), 
                    reader.GetInt32(4), 
                    reader.GetString(5) 
                );

                veiculo.SetVeiculoID(reader.GetInt32(0));

                var categoria = categoriaController.BuscaCategoriaPorId(veiculo.CategoriaID);
                veiculo.SetCategoria(categoria);

                veiculos.Add(veiculo);
            }

            return veiculos;
        }
    }
}
