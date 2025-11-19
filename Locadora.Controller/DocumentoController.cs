using Locadora.Controller;
using Locadora.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Utils.DataBases;

namespace Locadora.Controller
{
    public class DocumentoController
    {
        public void AdicionarDocumento(Documento documento, SqlConnection connection, SqlTransaction transaction)
        {
            //var connection = new SqlConnection(ConnectionDB.GetConnectionString());

            //var _transaction = transaction;

            //using (_transaction)
            {
                try
                {

                    SqlCommand command = new SqlCommand(Documento.INSERTDOCUMENTO, connection, transaction);

                    command.Parameters.AddWithValue("@ClienteID", documento.ClienteID);
                    command.Parameters.AddWithValue("@TipoDocumento", documento.TipoDocumento);
                    command.Parameters.AddWithValue("@Numero", documento.Numero);
                    command.Parameters.AddWithValue("@DataEmissao", documento.DataEmissao);
                    command.Parameters.AddWithValue("@DataValidade", documento.DataValidade);

                    command.ExecuteNonQuery();

                    //transaction.Commit();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao inesperado ao adicionar documento." + ex.Message);
                }

                catch (Exception ex)
                { 
   
                    throw new Exception("Erro ao adicionar documento." + ex.Message);
                }

                //finally
                //{
                //    connection.Close();
                //}

            }
        }
        public void AtualizarDocumento(Documento documento, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {

                SqlCommand command = new SqlCommand(Documento.UPDATEDOCUMENTO, connection, transaction);

                command.Parameters.AddWithValue("@ClienteID", documento.ClienteID);
                command.Parameters.AddWithValue("@TipoDocumento", documento.TipoDocumento);
                command.Parameters.AddWithValue("@Numero", documento.Numero);
                command.Parameters.AddWithValue("@DataEmissao", documento.DataEmissao);
                command.Parameters.AddWithValue("@DataValidade", documento.DataValidade);

                command.ExecuteNonQuery();

                //transaction.Commit();
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao inesperado ao alterar documento." + ex.Message);
            }

            catch (Exception ex)
            {

                throw new Exception("Erro ao alterar documento." + ex.Message);
            }
        }
    }
}
