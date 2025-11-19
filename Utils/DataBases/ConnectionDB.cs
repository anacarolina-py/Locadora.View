namespace Utils.DataBases
{
    public class ConnectionDB
    {
        private static readonly string _connectionString=
        "Data Source=localhost;Initial Catalog=LocadoraBD;User Id = sa;Password = SqlServer@1996; Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;";
        
        public static string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
