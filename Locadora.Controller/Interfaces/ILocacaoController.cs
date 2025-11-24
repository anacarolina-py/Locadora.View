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
        public void AdicionarLocacao(Locacao locacao, List<int> funcionarios);
        public List<Locacao> ListarLocacoes();
        public void AtualizarDataDevolucaoRealLocacao(Locacao locacao, DateTime? devolucao);
        public void AtualizarStatusLocacao(Locacao locacao, string status);
        public Locacao BuscarLocacaoPorId(Guid id);
        public List<Locacao> ListarLocacaoPorCliente(int clienteID);
        public List<Locacao> ListarLocacaoPorFuncionario(int funcionarioID);
        
    }
}
