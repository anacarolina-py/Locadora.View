using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Models
{
    public class LocacaoFuncionario
    {
        public static readonly string ASSOCIARFUNCIONARIO = @"INSERT INTO tblLocacaoFuncionarios (LocacaoID, FuncionarioID) 
                                                              VALUES (@LocacaoID, @FuncionarioID)";

        public static readonly string DESASSOCIARFUNCIONARIO = @"DELETE FROM tblLocacaoFuncionarios WHERE LocacaoID = @LocacaoID
                                                             AND FuncionarioID = @FuncionarioID";
        public int LocacaoFuncionarioID { get; set; }
        public int LocacaoID { get; set; }
        public int FuncionarioID { get; set; }
    }
}
