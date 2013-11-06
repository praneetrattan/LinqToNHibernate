namespace DatabaseGenerator
{
    using System.Data.SqlClient;

    public class SqlHelper
    {
        public const string ConnectionString = "Data Source=localhost;Initial Catalog="+SampleDataGenerator.DatabaseName+";Integrated Security=true";

        private SqlConnection connection;

        public void StartConnection(string connectionString)
        {
            connectionString = connectionString ?? ConnectionString;
            this.connection = new SqlConnection(connectionString);

            var success = false;
            try
            {
                this.connection.Open();
                success = true;
            }
            finally
            {
                if (!success)
                {
                    this.connection.Dispose();
                }
            }
        }

        public T ExecuteScalar<T>( string query, SqlTransaction txn = null)
        {
            using (var cmd = this.connection.CreateCommand())
            {
                cmd.Transaction = txn;
                cmd.CommandText = query;
                return (T)cmd.ExecuteScalar();
            }
        }

        public void ExecuteNonQuery( string query, SqlTransaction txn = null)
        {
            using (var cmd = this.connection.CreateCommand())
            {
                cmd.Transaction = txn;
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
