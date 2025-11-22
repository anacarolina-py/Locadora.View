using Locadora.Controller.Interfaces;
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
    public class LocacaoController : ILocacaoController
    {
        public void AdicionarLocacao(Locacao locacao)
        {
            var connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {

                    SqlCommand command = new SqlCommand(Locacao.INSERTLOCACAO, connection, transaction);
                    
                    command.Parameters.AddWithValue("@ClienteID", locacao.ClienteID);
                    command.Parameters.AddWithValue("@VeiculoID", locacao.VeiculoID);
                    command.Parameters.AddWithValue("@DataLocacao", locacao.DataLocacao);
                    command.Parameters.AddWithValue("@DataDevolucaoPrevista", locacao.DataDevolucaoPrevista);
                    command.Parameters.AddWithValue("@ValorDiaria", locacao.ValorDiaria);
                    command.Parameters.AddWithValue("@ValorTotal", locacao.ValorTotal);
                    command.Parameters.AddWithValue("@Status", locacao.Status);

                    command.ExecuteNonQuery();

                    transaction.Commit();

                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao inesperado ao adicionar locação." + ex.Message);
                }

                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao adicionar locação." + ex.Message);
                }

                finally
                {
                    connection.Close();
                }
            }
        }

        public void ConsultarHistoricoLocacaoes()
        {
            throw new NotImplementedException();
        }

        public void FinalizarLocacao()
        {
            throw new NotImplementedException();
        }

        public List<Locacao> ListarLocacaoPorCliente()
        {
            throw new NotImplementedException();
        }

        public List<Locacao> ListarLocacaoPorFuncionario()
        {
            throw new NotImplementedException();
        }

        public List<Locacao> ListarLocacoes()
        {
            throw new NotImplementedException();
        }

        public List<Locacao> ListarTodasLocacoesEFuncionarios()
        {
            throw new NotImplementedException();
        }
    }
}
