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
    }
}
