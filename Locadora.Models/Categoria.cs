using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Models
{
    public class Categoria
    {
        public readonly static string INSERTCATEGORIA = "INSERT INTO tblCategorias VALUES (@Nome, @Descricao, @Diaria)";
                                                      

        public readonly static string SELECTALLCATEGORIAS = "SELECT * FROM tblCategorias;";

        public readonly static string SELECTCATEGORIAPORNOME = @"SELECT * 
                                                               FROM tblCategorias 
                                                               WHERE Nome = @Nome";

        public readonly static string SELECTNOMECATEGORIAPORID = @"SELECT Nome
                                                               FROM tblCategorias 
                                                               WHERE CategoriaID = @CategoriaID";

        public readonly static string DELETECATEGORIA = @"DELETE FROM tblCategorias WHERE CategoriaID = @CategoriaID";

        public static readonly string UPDATECATEGORIA = @"UPDATE tblCategorias SET Descricao = @Descricao WHERE CategoriaID = @CategoriaID";

        public readonly static string UPDATEDIARIACATEGORIA = "UPDATE tblCategorias SET Diaria = @Diaria " +
                                                               "WHERE CategoriaId = @CategoriaId";

        public readonly static string SELECTCATEGORIAPORID = "SELECT * " +
                                                               "FROM tblCategorias " +
                                                               "WHERE CategoriaID = @CategoriaID";



        public int CategoriaID { get; private set; }
        public string Nome { get; private set; }
        public string? Descricao { get; private set; }
        public decimal Diaria { get; set; }

        public List<Veiculo> Veiculos { get; set; } = new List<Veiculo>();

        public Categoria(string nome, decimal diaria)
        {
            Nome = nome;
            Diaria = diaria;
        }

        public Categoria(string nome, decimal diaria, string? descricao) : this(nome, diaria)
        {
            Descricao = descricao;
        }

        public void setCategoriaId(int categoriaId)
        {
            this.CategoriaID = categoriaId;
        }

        public void SetDescricao(string descricao)
        {
            this.Descricao = descricao;
        }
        public void SetDiaria(decimal diaria)
        {
            Diaria = diaria;
        }
        public override string ToString()
        {
            return $"Categoria: {this.Nome}\n" +
                $"Descrição categoria: {this.Descricao}\n" +
                $"Diaria: {this.Diaria}\n";

        }
    }
}
