using MySql.Data.MySqlClient;
namespace sampleapp.Models;
public class UserRepository
{
    private readonly string _connectionString;
    private readonly int addTablist = 0;
    private readonly int deleteTablist = 0;
    
    public  List<string> tabList = new List<string>{};
    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    private bool UpdateTableList(string tabname, int updateaction)
    {
        using(var connection = new MySqlConnection(_connectionString))
        {
            var query = new MySqlCommand("SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = tablist");
            int count = Convert.ToInt16(query.ExecuteScalar());
            if(count > 0)
            {
                if(updateaction==addTablist)
                {
                    var command = new MySqlCommand("INSERT INTO tablist (tabname) VALUES (@tabname)", connection);
                    command.Parameters.AddWithValue("@tabname", tabname);
                    int result = command.ExecuteNonQuery();
                    if(result == -1)
                    {
                        return false;
                    }
                    tabList.Add(tabname);
                    return true;
                }
                else if(updateaction == deleteTablist)
                {
                    var command= new MySqlCommand("DELETE FROM tablist WHERE tabname = @tabname", connection);
                    int result  = command.ExecuteNonQuery();
                    if(result == -1)
                    {
                        return false;
                    }
                    tabList.Remove(tabname);
                    return true;
                }
                return false;
            }
            else
            {
                var createCommand = new MySqlCommand("CREATE TABLE tablist (tabname VARCHAR(255))", connection);
                int createResult = createCommand.ExecuteNonQuery();
                if(createResult == -1)
                {
                    return false;
                }
                if(updateaction==addTablist)
                {
                    var command = new MySqlCommand("INSERT INTO tablist (tabname) VALUES (@tabname)", connection);
                    command.Parameters.AddWithValue("@tabname", tabname);
                    int result = command.ExecuteNonQuery();
                    if(result == -1)
                    {
                        return false;
                    }
                    tabList.Add(tabname);
                    return true;
                }
                else if(updateaction==deleteTablist)
                {
                    var command= new MySqlCommand("DELETE FROM tablist WHERE tabname = @tabname", connection);
                    int result  = command.ExecuteNonQuery();
                    if(result == -1)
                    {
                        return false;
                    }
                    tabList.Remove(tabname);
                    return true;
                }
                return false;
            }
        }
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
                UpdateTableList(tablename, addTablist);
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
                UpdateTableList(tablename, deleteTablist);
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
    
    public (bool result, int id) InsertTabData(string tablename, DateTime date, string imageUrl, string description)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            try
            {
                // 创建命令
                var command = new MySqlCommand($"INSERT INTO {tablename} (event_time, event_imgurl, event_description) VALUES (@event_time, @event_imgurl, @event_description);SELECT LAST_INSERT_ID();", connection);

                // 绑定参数
                command.Parameters.AddWithValue("@event_time", date);
                command.Parameters.AddWithValue("@event_imgurl", imageUrl);
                command.Parameters.AddWithValue("@event_description",description);
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
    
    public bool DeleteTabData(string tablename,int id)
    {
        
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            try
            {
                // 创建命令
                var command = new MySqlCommand($"DELETE FROM {tablename} WHERE id = @id", connection);

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
    
    public List<string>  getTabList()
    {
        using(var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("SELECT * FROM tablist", connection);
            using(var reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    tabList.Add(reader.GetString(0));
                }
                return tabList;
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
