using System.Data;
using System.Data.SqlClient;

namespace realtimefraudsystem.utility
{
    public class ConnectToDB
    {
        private string _dbconnectionstring;
        private SqlDataReader _reader;
        private string _query;
        public ConnectToDB(string dataSource, string query)
        {
            _dbconnectionstring = dataSource;
            _query = query;
        }

        public DataTable GetTable()
        {
            DataTable table = new DataTable();
            try
            {
                using (SqlConnection connect = new SqlConnection(_dbconnectionstring))
                {
                    connect.Open();
                    using(SqlCommand cmd = new SqlCommand(_query, connect))
                    {
                        _reader=cmd.ExecuteReader();
                        table.Load( _reader );
                    }
                }

            }catch (SqlException sqlException)
            {
                //handle sql-specific exceptions
                Console.WriteLine($"SQL Exception: {sqlException.Message}");
            }
            catch(InvalidOperationException invOpEx)
            {
                // Handle invalid operations like trying to open an already opened connection
                Console.WriteLine($"Invalid Operation: {invOpEx.Message}");
            }
            catch(Exception ex)
            {
                // Handle any other exceptions that weren't specifically caught above
                Console.WriteLine($"General Exception: {ex.Message}");
            }
            finally
            {
                _reader.Close();
            }

            return table;
        }
    }
}
