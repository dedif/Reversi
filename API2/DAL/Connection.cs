namespace API.DAL
{
    public class Connection
    {
        public static string ConnectionString { get; set; } =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Reversi;Integrated Security=True;";
    }
}
