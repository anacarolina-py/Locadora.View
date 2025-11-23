using Locadora.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Controller.Interfaces
{
    public interface ILocacaoController
    {
        public void AdicionarLocacao(Locacao locacao);
        public List<Locacao> ListarLocacoes();
        public void AtualizarDataDevolucaoRealLocacao(Guid id, DateTime devolucao);
        public void AtualizarStatusLocacao(Guid id, string status);
        public Locacao BuscarLocacaoPorId(Guid id);
        public List<Locacao> ListarLocacaoPorCliente(int clienteID);
        public List<Locacao> ListarLocacaoPorFuncionario(int funcionarioID);
        public List<Locacao> ListarTodasLocacoesEFuncionarios();
        

    }
}
