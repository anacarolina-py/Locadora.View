using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Models
{
    public class LocacaoFuncionario
    {
        public int LocacaoFuncionarioID { get; set; }
        public int LocacaoID { get; set; }
        public int FuncionarioID { get; set; }
    }
}
