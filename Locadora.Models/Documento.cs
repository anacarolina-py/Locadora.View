namespace Locadora.Models
{
    public class Documento
    {
        public static readonly string INSERTDOCUMENTO = "INSERT INTO tblDocumentos (ClienteID, TipoDocumento, Numero, DataEmissao, DataValidade)" +
            "VALUES (@ClienteID, @TipoDocumento, @Numero, @DataEmissao, @DataValidade)";

        public static readonly string UPDATEDOCUMENTO = @"UPDATE tblDocumentos
                                                        SET TipoDocumento = @TipoDocumento,
                                                        Numero = @Numero,
                                                        DataEmissao = @DataEmissao,
                                                        DataValidade = @DataValidade
                                                        WHERE ClienteID = @ClienteID";

        public int DocumentoID { get; private set; }
        public int ClienteID { get; private set; }
        public string TipoDocumento { get; private set; }
        public string Numero {  get; private set; }
        public DateOnly DataEmissao { get; private set; }
        public DateOnly DataValidade { get; private set; }

        public Documento(string tipoDocumento, string numero, DateOnly dataEmissao, DateOnly dataValidade)
        {
            TipoDocumento = tipoDocumento;
            Numero = numero;
            DataEmissao = dataEmissao;
            DataValidade = dataValidade;
        }

        public void setClienteID(int clienteID)
        {
            ClienteID = clienteID;
        }

       
        public override string ToString()
        {
            return $"Tipo: {TipoDocumento} \nNúmero: {Numero} \nData Emissão: {DataEmissao} \nData Validade: {DataValidade}\n";
        }
    }
}
