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
        public readonly static string SELECTALLCLIENTES = @"SELECT c.ClienteID, c.Nome, c.Email, c.Telefone,

		                                                    d.TipoDocumento, d.Numero, d.DataEmissao, d.DataValidade

                                                            FROM tblClientes c

                                                            JOIN tblDocumentos d

                                                            ON c.ClienteID = d.ClienteID";

        public readonly static string UPDATELEFONECLIENTE = "UPDATE tblClientes SET Telefone = @Telefone " +
                                                            "WHERE ClienteID = @ClienteID";

        public readonly static string SELECTCLIENTEPOREMAIL = @"SELECT c.ClienteID, c.Nome, c.Email, c.Telefone,

		                                                    d.TipoDocumento, d.Numero, d.DataEmissao, d.DataValidade

                                                            FROM tblClientes c

                                                            JOIN tblDocumentos d

                                                            ON c.ClienteID = d.ClienteID

                                                            WHERE c.Email = @Email";

        public readonly static string SELECTCLIENTEPORID = "SELECT c.ClienteID, c.Nome, c.Email, c.Telefone, " +
                                                          "d.TipoDocumento, d.Numero, d.DataEmissao, d.DataValidade " +
                                                          "FROM tblClientes c " +
                                                          "JOIN tblDocumentos d " +
                                                          "ON c.ClienteID = d.ClienteID " +
                                                           "WHERE c.ClienteID = @ClienteID";

        public readonly static string DELETECLIENTE = "DELETE FROM tblClientes WHERE ClienteID = @ClienteID";

        public int ClienteID { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string? Telefone { get; private set; } = String.Empty;
        
        public Documento Documento { get; private set; }
        public Cliente(string nome, string email)
        {
            Nome = nome;
            Email = email;
        }

        public Cliente(string nome, string email, string? telefone) : this(nome, email)
        {
            Telefone = telefone;
        }

        public Cliente()
        {
        }

        public void SetClienteID(int clienteID)
        {
            ClienteID = clienteID;
        }
      
        public void SetDocumento(Documento documento)
        {
            Documento = documento;
        }
        public void SetTelefone(string telefone)
        {
            Telefone = telefone;
        }
        public override string ToString()
        {
            return $"Nome: {Nome} \nEmail: {Email} \nTelefone: {Telefone}\n" +
                $"\nDocumento: {Documento.ToString()}"
                ;
        }
    }
}
