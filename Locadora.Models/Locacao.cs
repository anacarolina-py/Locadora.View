using Locadora.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Models
{
    public class Locacao
    {
        public readonly static string INSERTLOCACAO = @"INSERT INTO tblLocacoes (ClienteID, VeiculoID, DataLocacao, DataDevolucaoPrevista, ValorDiaria, ValorTotal, Status) 
                                                            VALUES (@ClienteID, @VeiculoID, @DataLocacao, @DataDevolucaoPrevista, @ValorDiaria, @ValorTotal, @Status)";


        public readonly static string SELECTALLLOCACOES = "SELECT FuncionarioID, Nome, CPF, Email, Salario FROM tblFuncionarios;";


        public readonly static string SELECTLOCACAOPORID = @"SELECT Nome, CPF, Email, Salario
                                                               FROM tblFuncionarios 
                                                               WHERE FuncionarioID = @FuncionarioID";

        public readonly static string FINALIZARLOCACAO = @"DELETE FROM tblFuncionarios WHERE FuncionarioID = @FuncionarioID";

        public static readonly string UPDATELOCACAO = @"UPDATE tblFuncionarios SET Salario = @Salario WHERE FuncionarioID = @FuncionarioID";
        public Locacao(int clienteID, int veiculoID, decimal valorDiaria, int diasLocacao)
        {
            ClienteID = clienteID;
            VeiculoID = veiculoID;
            DataLocacao = DateTime.Now;
            ValorDiaria = valorDiaria;
            ValorTotal = valorDiaria * diasLocacao;
            DataDevolucaoPrevista = DateTime.Now.AddDays(diasLocacao);
            Status = EStatusLocacao.Ativa;
        }

        public Guid LocacaoID { get; private set; }
        public int ClienteID { get; private set; }
        public int VeiculoID { get; private set; }
        public DateTime DataLocacao { get; private set; }
        public DateTime DataDevolucaoPrevista { get; private set; }
        public DateTime? DataDevolucaoReal { get; private set; }
        public decimal ValorDiaria { get; private set; }
        public decimal ValorTotal { get; set; }
        public decimal Multa { get; private set; }
        public EStatusLocacao Status { get; private set; }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            //TODO: definir os valor de cliente e veiculo como nome e modelo
            return
                    $"\nCliente ID: {ClienteID}\n" +
                    $"Veículo ID: {VeiculoID}\n" +
                    $"Data de Locação: {DataLocacao}\n" +
                    $"Data de Devolução Prevista: {DataDevolucaoPrevista}\n" +
                    $"Data de Devolução Real: {DataDevolucaoReal}\n" +
                    $"Valor da Diária: {ValorDiaria:C}\n" +
                    $"Valor Total: {ValorTotal:C}\n" +
                    $"Multa: {Multa:C}\n" +
                    $"Status: {Status}\n";
        }
    }
}
