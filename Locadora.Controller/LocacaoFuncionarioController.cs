using Locadora.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using Utils.DataBases;

namespace Locadora.Controller
{
    public class LocacaoFuncionarioController
    {
        public void AssociarFuncionario(Guid locacaoId, int funcionarioId)
        {
            var connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();
            using (SqlCommand command = new SqlCommand(LocacaoFuncionario.ASSOCIARFUNCIONARIO, connection))
            {
                command.Parameters.AddWithValue("LocacaoID", locacaoId);
                command.Parameters.AddWithValue("FuncionarioID", funcionarioId);

                int linhas = command.ExecuteNonQuery();
                if (linhas == 0)
                {
                    throw new Exception("Falha ao associar locação.");
                }
                connection.Close();
            }
        }
        public void DesassociarFuncionario(Guid locacaoId, int funcionarioId)
        {
            var connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();
            using (SqlCommand command = new SqlCommand(LocacaoFuncionario.DESASSOCIARFUNCIONARIO, connection))
            {
                command.Parameters.AddWithValue("LocacaoID", locacaoId);
                command.Parameters.AddWithValue("FuncionarioID", funcionarioId);

                int linhas = command.ExecuteNonQuery();
                if (linhas == 0)
                {
                    throw new Exception("Falha ao desassociar locação.");
                }
                connection.Close();
            }
        }
        public List<Funcionario> ListarFuncionariosComLocacoes()
        {
            var funcionarios = new List<Funcionario>();

            using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            using var command = new SqlCommand(Locacao.SELECTALLLOCACOESFUNCIONARIOS, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                int funcionarioId = reader["FuncionarioID"] != DBNull.Value ? Convert.ToInt32(reader["FuncionarioID"]) : 0;

                // Verifica se o funcionário já está na lista
                var funcionario = funcionarios.FirstOrDefault(f => f.FuncionarioID == funcionarioId);
                if (funcionario == null)
                {
                    funcionario = new Funcionario(
                        reader["Nome"] != DBNull.Value ? reader["Nome"].ToString() : string.Empty,
                        reader["CPF"] != DBNull.Value ? reader["CPF"].ToString() : string.Empty,
                        reader["Email"] != DBNull.Value ? reader["Email"].ToString() : string.Empty,
                        reader["Salario"] != DBNull.Value ? Convert.ToDecimal(reader["Salario"]) : 0m
                    );
                    funcionario.SetFuncionarioID(funcionarioId);
                    funcionarios.Add(funcionario);
                }

                // Adiciona a locação se existir
                if (reader["LocacaoID"] != DBNull.Value)
                {
                    var locacao = new Locacao(
                        reader["ClienteID"] != DBNull.Value ? Convert.ToInt32(reader["ClienteID"]) : 0,
                        reader["VeiculoID"] != DBNull.Value ? Convert.ToInt32(reader["VeiculoID"]) : 0,
                        reader["DataLocacao"] != DBNull.Value ? Convert.ToDateTime(reader["DataLocacao"]) : DateTime.MinValue,
                        reader["DataDevolucaoPrevista"] != DBNull.Value ? Convert.ToDateTime(reader["DataDevolucaoPrevista"]) : DateTime.MinValue,
                        reader["ValorDiaria"] != DBNull.Value ? Convert.ToDecimal(reader["ValorDiaria"]) : 0m,
                        reader["Status"] != DBNull.Value ? reader["Status"].ToString() : string.Empty
                    );

                    locacao.SetLocacaoId(reader["LocacaoID"] != DBNull.Value ? (Guid)reader["LocacaoID"] : Guid.Empty);
                    funcionario.LocacoesGerenciadas.Add(locacao);
                }
            }

            return funcionarios;
        }

    }
}
