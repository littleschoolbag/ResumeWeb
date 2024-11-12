using MySql.Data.MySqlClient;
namespace sampleapp.Models;
public class UserRepository
{
    private readonly string _connectionString;
    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public bool ValidateUser(string username, string password)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("SELECT id, user_name, user_password FROM user WHERE user_name = @Username AND user_password = @Password", connection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password); // 注意：密码应加密

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    public bool CreateTabTable  (string tablename)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            try
            {
                // 创建命令
                var command = new MySqlCommand($"CREATE TABLE {tablename} " +
                    "(id INT AUTO_INCREMENT PRIMARY KEY, event_time DATETIME, event_imgurl VARCHAR(255), event_description VARCHAR(255))", connection);

                // 执行命令
                command.ExecuteNonQuery();
                return true; // 如果没有异常，返回 true
            }
            catch (MySqlException ex)
            {
                // 记录异常信息，可以使用日志记录工具
                Console.WriteLine($"Error creating table: {ex.Message}");
                return false; // 返回 false 表示失败
            }
        }
    }
    
    public bool DeleteTabTable(string tablename)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            try
            {
                // 创建命令
                var command = new MySqlCommand($"DROP TABLE {tablename}", connection);

                // 执行命令
                command.ExecuteNonQuery();
                return true; // 如果没有异常，返回 true
            }
            catch (MySqlException ex)
            {
                // 记录异常信息，可以使用日志记录工具
                Console.WriteLine($"Error deleting table: {ex.Message}");
                return false;
            }
        }
    }
    
    public (bool result, int id) InsertTabData(string tablename, TabData data)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            try
            {
                // 创建命令
                var command = new MySqlCommand($"INSERT INTO {tablename} (event_time, event_imgurl, event_description) VALUES (@event_time, @event_imgurl, @event_description);SELECT LAST_INSERT_ID();", connection);

                // 绑定参数
                command.Parameters.AddWithValue("@event_time", data.event_time);
                command.Parameters.AddWithValue("@event_imgurl", data.event_imgurl);
                command.Parameters.AddWithValue("@event_description",data.event_description);
                int insertId = Convert.ToInt32(command.ExecuteScalar());

                return (true, insertId);
            }
            catch (MySqlException ex)
            {
                // 记录异常信息，可以使用日志记录工具
                Console.WriteLine($"Error inserting data: {ex.Message}");
                return (false, -1);
            }
        }
    }
    
    public bool DeleteTabData(int id)
    {
        
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            try
            {
                // 创建命令
                var command = new MySqlCommand($"DELETE FROM tab_data WHERE id = @id", connection);

                // 绑定参数
                command.Parameters.AddWithValue("@id", id);

                // 执行命令
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException ex)
            {
                // 记录异常信息，可以使用日志记录工具
                Console.WriteLine($"Error deleting data: {ex.Message}");
                return false;
            }
        }
    }
    public List<TabData> GetTabData(string tablename)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var command = new MySqlCommand($"SELECT * FROM {tablename} ORDER BY event_date DESC", connection);

            using (var reader = command.ExecuteReader())
            {
                var tabDataList = new List<TabData>();

                while (reader.Read())
                {
                    var tabData = new TabData();
                    tabData.event_time = reader.GetDateTime(0);
                    tabData.event_imgurl = reader.GetString(1);
                    tabData.event_description = reader.GetString(2);
                    tabDataList.Add(tabData);
                }
                return tabDataList;
            }
        }
    }
}


public class TabData
{
    public int id { get; set; }
    public DateTime event_time { get; set; }
    public string event_imgurl { get; set; }
    public string event_description { get; set; }
}
