using System.Data;
using Npgsql;

namespace EventlyServerTest;

public static class InitDatabase
{
    public static async Task RunScript()
    {
        var pgHost = Environment.GetEnvironmentVariable("DATABASE_HOST");
        var pgPort = Environment.GetEnvironmentVariable("DATABASE_PORT");
        var pgUser = Environment.GetEnvironmentVariable("DATABASE_USER");
        var pgPass = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
        var pgDb = Environment.GetEnvironmentVariable("DATABASE_NAME");

        if (pgHost == null || pgPort == null || pgUser == null || pgPass == null || pgDb == null)
            throw new ArgumentNullException(nameof(pgHost), "One of db config params is null");
            
        string connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};";

        if (!TableExists("landing_invitations", connStr))
        {
            string script = await File.ReadAllTextAsync(@"../../../../../init.sql");

            await using var dataSource = NpgsqlDataSource.Create(connStr);
            await using var command = dataSource.CreateCommand(script);

            await command.ExecuteNonQueryAsync();
        }
    }
    
    public static bool TableExists(string tableName, string connectionString)
    {
        string sql = "SELECT * FROM information_schema.tables WHERE table_name = '" + tableName + "'";
        using var con = new NpgsqlConnection(connectionString);
        using var cmd = new NpgsqlCommand(sql);
        if (cmd.Connection == null)
            cmd.Connection = con;
        if (cmd.Connection.State != ConnectionState.Open)
            cmd.Connection.Open();

        lock (cmd)
        {
            using NpgsqlDataReader rdr = cmd.ExecuteReader();
            try
            {
                if (rdr != null && rdr.HasRows)
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}