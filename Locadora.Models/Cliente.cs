using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Models
{
    public class Cliente
    {
        public readonly static string INSERTCLIENTE = "INSERT INTO tblClientes VALUES(@Nome, @Email, @Telefone)"+
            "SELECT SCOPE_IDENTITY();";
        public readonly static string SELECTALLCLIENTES = "SELECT * FROM tblClientes;";

        public readonly static string UPDATELEFONECLIENTE = "UPDATE tblClientes SET Telefone = @Telefone" +
            "WHERE ClienteID = @CliendID";
        public readonly static string SELECTCLIENTEPOREMAIL = "SELECT * FROM tblClientes WHERE Email = @Email";

        public int ClienteID { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string? Telefone { get; private set; } = String.Empty;
        public Cliente(string nome, string email)
        {
            Nome = nome;
            Email = email;
        }

        public Cliente(string nome, string email, string? telefone) : this(nome, email)
        {
            Telefone = telefone;
        }

        public void SetClienteID(int clienteID)
        {
            ClienteID = clienteID;
        }

        public void SetTelefone(string telefone)
        {
            Telefone = telefone;
        }
        public override string ToString()
        {
            return $"Nome: {Nome} \nEmail: {Email} \nTelefone: {Telefone}\n";
        }
    }
}
