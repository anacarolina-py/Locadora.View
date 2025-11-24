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
        public readonly static string INSERTLOCACAO = @"INSERT INTO tblLocacoes (ClienteID, VeiculoID, DataLocacao, DataDevolucaoPrevista, ValorDiaria, ValorTotal, Multa, Status)
                                                        OUTPUT INSERTED.LocacaoID VALUES (@ClienteID, @VeiculoID, @DataLocacao, @DataDevolucaoPrevista, @ValorDiaria, @ValorTotal, @Multa, @Status)";


        public readonly static string SELECTALLLOCACOES = @"SELECT LocacaoID, ClienteID, VeiculoID, DataLocacao, DataDevolucaoPrevista, DataDevolucaoReal, 
                                                          ValorDiaria, ValorTotal, Multa, Status
                                                          FROM tblLocacoes;";
       

        public readonly static string SELECTLOCACAOPORID = @"SELECT l.LocacaoID, l.ClienteID, l.VeiculoID, l.DataLocacao, l.DataDevolucaoPrevista, l.DataDevolucaoReal,
                                                            l.ValorDiaria, l.ValorTotal, l.Multa, l.Status, (SELECT Placa FROM tblVeiculos v WHERE v.VeiculoID = l.VeiculoID) 
                                                            AS VeiculoPlaca FROM tblLocacoes l WHERE l.LocacaoID = @LocacaoID";

        public readonly static string SELECTLOCACAOPORCLIENTE = "SELECT * FROM tblLocacoes WHERE ClienteID = @ClienteID";

        public readonly static string SELECTLOCACAOPORFUNCIONARIO = @"SELECT l.LocacaoID, l.ClienteID, l.VeiculoID, l.DataLocacao,
                                                                        l.DataDevolucaoPrevista, l.ValorDiaria, 
                                                                        l.ValorTotal, l.Status
                                                                        FROM tblLocacoes l JOIN tblLocacaoFuncionarios lf
                                                                        ON lf.LocacaoID = l.LocacaoID WHERE lf.FuncionarioID = @FuncionarioID";

        public readonly static string SELECTALLLOCACOESFUNCIONARIOS = @"SELECT f.FuncionarioID, f.Nome, f.CPF, f.Email, f.Salario, l.LocacaoID,
                                                                        l.ClienteID, l.VeiculoID, l.DataLocacao, l.DataDevolucaoPrevista,
                                                                        l.ValorDiaria,  l.ValorTotal, l.Multa, l.Status
                                                                            FROM tblFuncionarios f
                                                                     LEFT JOIN tblLocacaoFuncionarios lf ON f.FuncionarioID = lf.FuncionarioID
                                                                        LEFT JOIN tblLocacoes l ON lf.LocacaoID = l.LocacaoID";

        public readonly static string UPDATELOCACAODEVOLUCAOREAL = @"UPDATE tblLocacoes SET DataDevolucaoReal = @DataDevolucaoReal
                                                                        WHERE LocacaoID = @LocacaoID";

        public readonly static string UPDATELOCACAOSTATUS = @"UPDATE tblLocacoes SET Status = @Status 
                                                                WHERE LocacaoID = @LocacaoID";

        public readonly static string UPDATELOCACAOVALORTOTAL = "UPDATE tblLocacoes SET ValorTotal = @ValorTotal WHERE LocacaoID = @LocacaoID";

        public Guid LocacaoID { get; private set; }
        public int ClienteID { get; private set; }
        public int VeiculoID { get; private set; }
        public DateTime DataLocacao { get; private set; }
        public DateTime DataDevolucaoPrevista { get; private set; }
        public DateTime? DataDevolucaoReal { get; private set; }
        public decimal ValorDiaria { get; private set; }
        public decimal ValorTotal { get; set; }
        public decimal Multa { get; private set; }
        public string Status { get; private set; }
        public Cliente Cliente { get; private set; } = new Cliente();
        public Veiculo Veiculo { get; private set; } = new Veiculo();
        //public List<Funcionario> FuncionariosEnvolvidos { get; set; } = new List<Funcionario>(); -- não está sendo usada
        public Locacao(Cliente cliente, Veiculo veiculo, decimal valorDiaria, int diasLocacao, string status)
        {
            this.Cliente = cliente;
            this.Veiculo = veiculo;
            this.DataLocacao = DateTime.Now;
            this.DataDevolucaoPrevista = DateTime.Now.AddDays(diasLocacao);
            this.DataDevolucaoReal = null;
            this.ValorDiaria = valorDiaria;
            this.ValorTotal = valorDiaria * diasLocacao;
            this.Multa = 0.5m * (decimal)this.Veiculo.Categoria.Diaria;
            this.Status = status;
        }

        public Locacao(Guid locacaoID, Cliente cliente, Veiculo veiculo, DateTime dataLocacao,
        DateTime dataDevolucaoPrevista, DateTime? dataDevolucaoReal, decimal valorDiaria,
        decimal valorTotal, decimal multa, string status)
        {
            LocacaoID = locacaoID;
            this.Cliente = cliente;
            this.Veiculo = veiculo;
            DataLocacao = dataLocacao;
            DataDevolucaoPrevista = dataDevolucaoPrevista;
            DataDevolucaoReal = dataDevolucaoReal;
            ValorDiaria = valorDiaria;
            ValorTotal = valorTotal;
            Multa = multa;
            Status = status; 
        }

        public Locacao(int clienteID, int veiculoID, DateTime dataLocacao, DateTime dataDevolucaoPrevista, decimal valorDiaria,string status)
        {
            ClienteID = clienteID;
            VeiculoID = veiculoID;

            
            DataLocacao = dataLocacao < (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue
                ? (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue
                : dataLocacao;

            DataDevolucaoPrevista = dataDevolucaoPrevista > (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue
                ? (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue
                : dataDevolucaoPrevista;

            ValorDiaria = valorDiaria;
            ValorTotal = valorDiaria * (int)(DataDevolucaoPrevista - DataLocacao).TotalDays;
            Multa = 0;
            Status = status;
        }

        public Locacao(int clienteID, int veiculoID, decimal valorDiaria, int diasLocacao)
        {
            ClienteID = clienteID;
            VeiculoID = veiculoID;
            DataLocacao = DateTime.Now;
            ValorDiaria = valorDiaria;
            ValorTotal = ValorDiaria * diasLocacao;
            DataDevolucaoPrevista = DateTime.Now.AddDays(diasLocacao);
            Status = "Ativa";
        }

       
     
        public void SetLocacaoId(Guid locacaoId)
        {
            LocacaoID = locacaoId;
        }
        public void SetValorDiaria(decimal valorDiaria)
        {
            ValorDiaria = valorDiaria;
        }
        public void SetMulta(decimal multa)
        {
            Multa = multa;
        }
        public void SetDataDevolucaoReal(DateTime? devolucaoReal)
        {
            DataDevolucaoReal = devolucaoReal;
        }
        public void SetStatus(string status)
        {
            Status = status;
        }
        public void SetValorTotal(decimal valorTotal)
        {
            ValorTotal = valorTotal;
        }

        public decimal CalcularValorFinal()
        {
            if (DataDevolucaoReal == null)
                throw new Exception("A data de devolução real não foi definida.");


            int diasTotais = (DataDevolucaoReal.Value - DataLocacao).Days;

            if (diasTotais < 1)
                diasTotais = 1;

            decimal valorBase = diasTotais * ValorDiaria;


            int diasAtraso = (DataDevolucaoReal.Value - DataDevolucaoPrevista).Days;

            decimal valorMulta = 0;

            if (diasAtraso > 0)
            {

                valorMulta = Multa + (diasAtraso * ValorDiaria);
            }

            this.ValorTotal = valorBase + valorMulta;
            return valorBase + valorMulta;
        }

        public override string ToString()
        {
            
            return 
                    $"\nCliente ID: {ClienteID}\n" +
                    $"Nome Cliente: {Cliente.Nome}\n" +
                    $"Veículo ID: {VeiculoID}\n" +
                    $"Modelo Veículo: {Veiculo.Modelo}\n" +
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


