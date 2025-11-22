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
        public void FinalizarLocacao();
        public List<Locacao> ListarLocacaoPorCliente();
        public List<Locacao> ListarLocacaoPorFuncionario();
        public List<Locacao> ListarTodasLocacoesEFuncionarios();
        

    }
}
