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
    public class CategoriaController
    {
        public void AdicionarCategoria(Categoria categoria)
        {
            var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();
            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    SqlCommand command = new SqlCommand(Categoria.INSERTCATEGORIA, connection, transaction);
                    command.Parameters.AddWithValue("@Nome", categoria.Nome);
                    command.Parameters.AddWithValue("@Descricao", categoria.Descricao ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Diaria", categoria.Diaria);

                    int categoriaId = Convert.ToInt32(command.ExecuteScalar());
                    categoria.setCategoriaId(categoriaId);
                    transaction.Commit();

                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao adicionar categoria" + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro inesperado ao adicionar categoria" + ex.Message);
                }
            }
        }

        public List<Categoria> ListarCategorias()
        {

            var connection = new SqlConnection(ConnectionDB.GetConnectionString());

            using (connection)
            {
                try
                {
                    connection.Open();

                    List<Categoria> categorias = new List<Categoria>();

                    SqlCommand command = new SqlCommand(Categoria.SELECTALLCATEGORIAS, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var categoria = new Categoria(
                            reader["Nome"].ToString(),
                            reader.GetDecimal(reader.GetOrdinal("Diaria")),
                            reader["Descricao"] != DBNull.Value ? reader["Descricao"].ToString() : null
                         );

                        categorias.Add(categoria);
                    }

                    return categorias;

                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao listar categorias: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro inesperado ao listar categorias: " + ex.Message);
                }

            }
        }

        public Categoria BuscaCategoriaPorNome(string nome)
        {
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();

            using (connection)
            {
                try
                {
                    SqlCommand command = new SqlCommand(Categoria.SELECTCATEGORIAPORNOME, connection);
                    command.Parameters.AddWithValue("@Nome", nome);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        var categoria = new Categoria(
                           reader["Nome"].ToString(),
                           reader.GetDecimal(reader.GetOrdinal("Diaria")),
                           reader["Descricao"] != DBNull.Value ? reader["Descricao"].ToString() : null
                        );
                        categoria.setCategoriaId(Convert.ToInt32(reader["CategoriaID"]));


                        return categoria;
                    }
                    return null;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao buscar categoria por nome: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro inesperado ao buscar categoria por nome: " + ex.Message);
                }

            }


        }


        public void AtualizarDescricaoCategoria(string nome, string descricao)
        {
            var categoriaEncontrada = BuscaCategoriaPorNome(nome);
            if (categoriaEncontrada is null)
            {
                throw new Exception("Categoria não encontrada");
            }

            categoriaEncontrada.SetDescricao(descricao);

            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();
            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    SqlCommand command = new SqlCommand(Categoria.UPDATECATEGORIA, connection, transaction);
                   
                    command.Parameters.AddWithValue("@Descricao", categoriaEncontrada.Descricao ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CategoriaID", categoriaEncontrada.CategoriaID);

                   

                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao atualizar a categoria: " + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro inesperado ao atualizar a categoria: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        public void DeletarCategoria(string nome)
        {
            var categoriaEncontrada = BuscaCategoriaPorNome(nome);
            if (categoriaEncontrada is null)
            {
                throw new Exception("Categoria não encontrada");
            }

            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();
            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    SqlCommand command = new SqlCommand(Categoria.DELETECATEGORIA, connection, transaction);
                    command.Parameters.AddWithValue("@CategoriaID", categoriaEncontrada.CategoriaID);
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao deletar a categoria: " + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro inesperado ao deletar a categoria: " + ex.Message);
                }

            }
        }
    }
}
