using Locadora.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

                    command.ExecuteNonQuery();
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
                finally
                {
                    connection.Close();
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

        public string BuscarNomeCategoriaPorId(int id)
        {
            var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            try
            {
                SqlCommand command = new SqlCommand(Categoria.SELECTNOMECATEGORIAPORID, connection);
                command.Parameters.AddWithValue("@CategoriaID", id);

                string nomecategoria = String.Empty;

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    nomecategoria = reader["Nome"].ToString() ?? string.Empty;
                }
                return nomecategoria;
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao buscar categoria." + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao buscar categoria." + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public Categoria? BuscaCategoriaPorNome(string nome)
        {
            using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            using var command = new SqlCommand(Categoria.SELECTCATEGORIAPORNOME, connection);
            command.Parameters.AddWithValue("@Nome", nome);
            

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                var categoria = new Categoria(
                    reader["Nome"].ToString() ?? string.Empty,
                    reader["Diaria"] != DBNull.Value ? Convert.ToDecimal(reader["Diaria"]) : 0m,
                    reader["Descricao"] != DBNull.Value ? reader["Descricao"].ToString() : string.Empty
                );

                categoria.setCategoriaId(reader["CategoriaID"] != DBNull.Value ? Convert.ToInt32(reader["CategoriaID"]) : 0);

                return categoria;
            }

            return null; 
        }

        public void AtualizarDiariaCategoria(decimal diaria, string nome)
        {
            var categoriaEncontrado = this.BuscaCategoriaPorNome(nome);

            if (categoriaEncontrado is null)
                throw new Exception("Não existe categoria com esse nome cadastrado!");

            categoriaEncontrado.SetDiaria(diaria);

            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    SqlCommand command = new SqlCommand(Categoria.UPDATEDIARIACATEGORIA, connection, transaction);
                    var p = command.Parameters.Add("@Diaria", SqlDbType.Decimal);
                    p.Precision = 10;
                    p.Scale = 2;
                    p.Value = categoriaEncontrado.Diaria;
                    command.Parameters.AddWithValue("@CategoriaId", categoriaEncontrado.CategoriaID);

                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao atualizar diaria da categoria: " + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro inesperado ao atualizar diaria da categoria: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public void AtualizarDescricaoCategoria(string nome, string descricao)
        {
            Categoria categoriaEncontrada = BuscaCategoriaPorNome(nome);
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

        public Categoria? BuscaCategoriaPorId(int id)
        {
            using var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            using var command = new SqlCommand(Categoria.SELECTCATEGORIAPORID, connection);
            command.Parameters.AddWithValue("@CategoriaID", id);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                var categoria = new Categoria(
                    reader["Nome"].ToString() ?? string.Empty,
                    reader["Diaria"] != DBNull.Value ? Convert.ToDecimal(reader["Diaria"]) : 0m,
                    reader["Descricao"] != DBNull.Value ? reader["Descricao"].ToString() : string.Empty
                );

                categoria.setCategoriaId(reader["CategoriaID"] != DBNull.Value ? Convert.ToInt32(reader["CategoriaID"]) : 0);

                return categoria;
            }

            return null;
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
