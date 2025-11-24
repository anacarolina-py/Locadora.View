using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Models
{
    public class Funcionario
    {
        public readonly static string INSERTFUNCIONARIO = "INSERT INTO tblFuncionarios VALUES (@Nome, @CPF, @Email, @Salario)";


        public readonly static string SELECTALLFUNCIONARIOS = "SELECT FuncionarioID, Nome, CPF, Email, Salario FROM tblFuncionarios;";


        public readonly static string SELECTFUNCIONARIOPORID = @"SELECT Nome, CPF, Email, Salario
                                                               FROM tblFuncionarios 
                                                               WHERE FuncionarioID = @FuncionarioID";

        public readonly static string SELECTFUNCIONARIOSPOREMAIL = @"SELECT * FROM tblFuncionarios 
                                                                    WHERE Email = @Email";

        public readonly static string DELETEFUNCIONARIO = @"DELETE FROM tblFuncionarios WHERE Email = @Email";

        public static readonly string UPDATESALARIOFUNCIONARIO = @"UPDATE tblFuncionarios SET Salario = @Salario WHERE Email = @Email";


        public int FuncionarioID { get; private set; }
        public string Nome { get; private set; }
        public string CPF { get; private set; }
        public string Email { get; private set; }
        public decimal? Salario { get; private set; }
        public List<Locacao> LocacoesGerenciadas { get; set; } = new List<Locacao>();
        public Funcionario(string nome, string cPF, string email)
        {
            Nome = nome;
            CPF = cPF;
            Email = email;
        }

        public Funcionario(string nome, string cPF, string email, decimal? salario)
        {
            Nome = nome;
            CPF = cPF;
            Email = email;
            Salario = salario;
        }

        public void SetFuncionarioID(int id)
        {
            FuncionarioID = id;
        }

        public void SetSalario(decimal salario)
        {
            Salario = salario;
        }

        public override string ToString()
        {
            return $"Nome: {Nome} \nCPF: {CPF} \nEmail: {Email} \nSalário: {Salario}.";
        }
    }
}
