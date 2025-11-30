using Sharp_231.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MySql.Data.MySqlClient;
using Sharp_231.Attributes;

namespace Sharp_231.Data
{
    internal class DataAccessor
    {
        public readonly MySqlConnection connection;

        public DataAccessor()
        {
            string connectionString = "Server=127.0.0.1;Port=3306;Database=demo;User Id=root;Password=veravira;";
            this.connection = new MySqlConnection(connectionString);
            try
            {
                this.connection.Open();
                EnsureDatabaseSetup();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Connection failed: " + ex.Message);
            }
        }

        private void EnsureDatabaseSetup()
        {
            string sqlProducts = @"CREATE TABLE IF NOT EXISTS Products(
                                        Id CHAR(36) PRIMARY KEY,
                                        Name VARCHAR(64) NOT NULL,
                                        Price DECIMAL(14,2) NOT NULL)";
            ExecuteNonQuery(sqlProducts);

            string sqlDepartments = @"CREATE TABLE IF NOT EXISTS Departments(
                                        Id CHAR(36) PRIMARY KEY,
                                        Name VARCHAR(64) NOT NULL)";
            ExecuteNonQuery(sqlDepartments);

            string sqlManagers = @"CREATE TABLE IF NOT EXISTS Managers(
                                        Id CHAR(36) PRIMARY KEY,
                                        DepartmentId CHAR(36) NOT NULL,
                                        Name VARCHAR(64) NOT NULL,
                                        WorksFrom DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP)";
            ExecuteNonQuery(sqlManagers);

            string sqlSales = @"CREATE TABLE IF NOT EXISTS Sales(
                                    Id CHAR(36) PRIMARY KEY,
                                    ManagerId CHAR(36) NOT NULL,
                                    ProductId CHAR(36) NOT NULL,
                                    Quantity INT NOT NULL DEFAULT 1,
                                    Moment DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP)";
            ExecuteNonQuery(sqlSales);
            string sqlNews = @"
CREATE TABLE IF NOT EXISTS News (
    Id CHAR(36) PRIMARY KEY,
    AuthorId CHAR(36) NOT NULL,
    Title VARCHAR(256) NOT NULL,
    Content TEXT NOT NULL,
    Moment DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
)";
            ExecuteNonQuery(sqlNews);

            
        }
        

        private void ExecuteNonQuery(string sql, Dictionary<string, object>? parameters = null)
        {
            using var cmd = new MySqlCommand(sql, connection);
            if (parameters != null)
                foreach (var p in parameters)
                    cmd.Parameters.AddWithValue(p.Key, p.Value);
            cmd.ExecuteNonQuery();
        }

        private T FromReader<T>(MySqlDataReader reader)
        {
            var t = typeof(T);
            var ctr = t.GetConstructor(Array.Empty<Type>());
            T res = (T)ctr!.Invoke(null);

            foreach (var prop in t.GetProperties())
            {
                try
                {
                    object val = reader[prop.Name];
                    if (val is decimal)
                        prop.SetValue(res, Convert.ToDouble(val));
                    else
                        prop.SetValue(res, val);
                }
                catch { }
            }
            return res;
        }

        private List<T> ExecuteList<T>(string sql, Dictionary<string, object>? parameters = null)
        {
            List<T> res = new();
            using var cmd = new MySqlCommand(sql, connection);
            if (parameters != null)
                foreach (var p in parameters)
                    cmd.Parameters.AddWithValue(p.Key, p.Value);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                res.Add(FromReader<T>(reader));

            return res;
        }

        private int ExecuteScalarInt(string sql, Dictionary<string, object>? parameters = null)
        {
            using var cmd = new MySqlCommand(sql, connection);
            if (parameters != null)
                foreach (var p in parameters)
                    cmd.Parameters.AddWithValue(p.Key, p.Value);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // -------------------- PRODUCTS --------------------
        public List<Product> GetProducts()
        {
            return ExecuteList<Product>("SELECT * FROM Products");
        }

        public List<Department> GetDepartment()
        {
            return ExecuteList<Department>("SELECT * FROM Departments");
        }
        public List<Manager> GetManagers()
        {
            return ExecuteList<Manager>("SELECT * FROM Managers");
        }
        public List<News> GetNews()
        {
            return ExecuteList<News>("SELECT * FROM News");
        }

        public List<T> GetAll<T>()
        {
            var t = typeof(T);
            var attr = t.GetCustomAttribute<TableNameAttribute>();
            String tableName = attr?.Value ?? t.Name + "s";
            return ExecuteList<T>(
                $"SELECT * FROM {tableName}");
        }
        
        // варіант Генератори
        // генератори - спосіб одержання результатів "по одному"
        public IEnumerable<Department> EnumDepartments()
        {
            String sql = "SELECT * FROM Departments";
            using MySqlCommand cmd = new MySqlCommand(sql, connection);
            MySqlDataReader? reader;
            try { reader = cmd.ExecuteReader(); }
            catch (Exception ex)
            {
            Console.WriteLine("Failed: {0}\n{1}", ex.Message, sql);
            throw;
            }

            while (reader.Read())
            {
                yield return FromReader<Department>(reader);
            }
            reader.Dispose();
        }
        public IEnumerable<Manager> EnumManagers()
        {
            String sql = "SELECT * FROM Managers";
            using MySqlCommand cmd = new MySqlCommand(sql, connection);
            MySqlDataReader? reader;
            try { reader = cmd.ExecuteReader(); }
            catch (Exception ex)
            {
                Console.WriteLine("Failed: {0}\n{1}", ex.Message, sql);
                throw;
            }

            while (reader.Read())
            {
                yield return FromReader<Manager>(reader);
            }
            reader.Dispose();
        }

        public IEnumerable<Product> EnumProducts()
        {
            String sql = "SELECT * FROM Products";
            using MySqlCommand cmd = new MySqlCommand(sql, connection);
            MySqlDataReader? reader;
            try { reader = cmd.ExecuteReader(); }
            catch (Exception ex)
            {
                Console.WriteLine("Failed: {0}\n{1}", ex.Message, sql);
                throw;
            }

            while (reader.Read())
            {
                yield return FromReader<Product>(reader);
            }
            reader.Dispose();
        }
        public IEnumerable<News> EnumNews()
        {
            String sql = "SELECT * FROM News";
            using MySqlCommand cmd = new MySqlCommand(sql, connection);
            MySqlDataReader? reader;
            try { reader = cmd.ExecuteReader(); }
            catch (Exception ex)
            {
                Console.WriteLine("Failed: {0}\n{1}", ex.Message, sql);
                yield break;
            }

            while (reader.Read())
                yield return FromReader<News>(reader);

            reader.Dispose();
        }
        public IEnumerable<T> EnumAll<T>()
        {
            var t = typeof(T);
            var attr = t.GetCustomAttribute<TableNameAttribute>();
            string table = attr?.Value ?? t.Name + "s";

            string sql = $"SELECT * FROM {table}";
            using var cmd = new MySqlCommand(sql, connection);

            MySqlDataReader? reader;
            try { reader = cmd.ExecuteReader(); }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed: {ex.Message}\n{sql}");
                yield break;
            }

            while (reader.Read())
                yield return FromReader<T>(reader);

            reader.Dispose();
        }

        public void Install()
        {
            InstallProducts();
            InstallDepartments();
            InstallSales();
            InstallNews();
            InstallAccess();
        }
        private void InstallAccess()
        {
            String sql = "CREATE TABLE Accesses(" +
                         "Id CHAR(36) PRIMARY KEY," +
                         "ManagerId CHAR(36) NOT NULL," +
                         "Login VARCHAR(64) NOT NULL," +
                         "Salt VARCHAR(64) NOT NULL," +
                         "Dk VARCHAR(128) NOT NULL)";
            using MySqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();   
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
            }
        }

        private void InstallSales()
        {
            String sql = "CREATE TABLE Sales(" +
                "Id           UNIQUEIDENTIFIER PRIMARY KEY," +
                "ManagerId UNIQUEIDENTIFIER NOT NULL," +
                "ProductId UNIQUEIDENTIFIER NOT NULL," +
                "Quantity INT NOT NULL DEFAULT 1," +
                "Moment    DATETIME2        NOT NULL  DEFAULT CURRENT_TIMESTAMP)";
            using MySqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();   // без зворотнього результату
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
            }
        }
        private void InstallDepartments()
        {
            String sql = "CREATE TABLE Departments(" +
                "Id    UNIQUEIDENTIFIER PRIMARY KEY," +
                "Name  NVARCHAR(64)     NOT NULL)";
            using MySqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();   // без зворотнього результату
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
            }
        }
        private void InstallProducts()
        {
            String sql = "CREATE TABLE Products(" +
                "Id    UNIQUEIDENTIFIER PRIMARY KEY," +
                "Name  NVARCHAR(64)     NOT NULL," +
                "Price DECIMAL(14,2)    NOT NULL)";
            using MySqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();   // без зворотнього результату
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
            }
        }
        private void InstallNews()
        {
            String sql = "CREATE TABLE News(" +
                         "Id       CHAR(36) PRIMARY KEY," +
                         "AuthorId CHAR(36) NOT NULL," +
                         "Title    VARCHAR(256)    NOT NULL," +
                         "Content  TEXT   NOT NULL," +
                         "Moment   DATETIME        NOT NULL  DEFAULT CURRENT_TIMESTAMP)";
            using MySqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();   // без зворотнього результату
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
            }
        }
        public void Seed()
        {
            SeedProducts();
            SeedDepartments();
            SeedManagers();
            SeedNews();
            SeedSales();
            SeedAccess();
        }
        public void SeedProducts()
        {
            String sql = "Insert into Products values" +
                "('1E7F21A1-237B-427B-BDED-1B1B32639E48',N'Samsung galaxy s24 Ultra',20000.00)," +
                "('ADE91C53-C314-4CFC-8B4A-D24F19E35646',N'iPhone 15 pro',30000.00)," +
                "('BC1FA982-51DD-41E9-8130-D7F0C08E07B7',N'Samsung s26',45000.00)," +
                "('E3AD30F6-5FD7-4F58-8EAD-1B5E964750DA',N'OpePlus Pro',12000.00)," +
                "('80AE1DB7-097B-4DE1-BB23-36A08E49A825',N'Google Pixel 8 Pro',8000.00)," +
                "('D4277097-7E80-4C56-9F75-202F014930A0',N'iPhone 17 Air',33000.00)," +
                "('DB3B716C-2AFA-4990-9D3A-D87540E8C574',N'Poco Plus Max',25000.00)," +
                "('1BF31902-7C21-4612-A91F-9FBAC75A1B27',N'Asus Rog Phone 3',17000.00)," +
                "('F16E0815-F613-44E3-8B52-297E1F6F9780',N'Motorola 2020',27000.00)," +
                "('E2146763-0B08-435F-93B5-D204123AC93C',N'Honor Magic 7 Pro',19000.00)," +
                "('B4B2B7BD-FD5A-4AC9-9DB6-4297212DA89F',N'Xiaomi Mi 14 Ultra',28000.00)," +
                "('C090762F-E6C4-4EBC-8D43-2962944131C8',N'Nokia X90 Max',9000.00)," +
                "('B10F5B9C-0025-4C34-B910-833EABC65D9B',N'Sony Xperia Z9 Premium',22000.00)," +
                "('FB8F4579-73B5-45BB-A3A1-7D60740BCE4E',N'Huawei P70 Pro',26000.00)," +
                "('609E794E-2913-4377-A325-EB996B5FDEB3',N'Realme GT 7 Neo',15000.00)";
                        using MySqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();//без зворотнього результату
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed {0}\n{1}: ", ex.Message, sql);
            }
        }
        public void SeedDepartments()
        {
            String sql = "Insert into Departments values" +
                "('25F34D1F-23DD-4FDE-BC7B-AA5823F373DD',N'Marketing')," +
                "('674DBCB8-4F90-4ED5-AA2A-898D35A05650',N'Add')," +
                "('89E33CC6-92F3-4ED5-B2B4-2B41F395F47A',N'Sales')," +
                "('80A345E3-BB65-4282-A564-619A1F627D45',N'IT')," +
                "('2D4995BC-0C44-4A0F-BB65-5DD0EDC3680B',N'Security')";
            using MySqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();//без зворотнього результату
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed {0}\n{1}: ", ex.Message, sql);
            }
        }
        public void SeedManagers()
        {
            String sql = "Insert into Managers(DepartmentID, ID,Name,WorksFrom) values" +
                "('25F34D1F-23DD-4FDE-BC7B-AA5823F373DD','57D1DD41-5B5E-47D8-BB2C-3433D4CD9BE8',N'Daniel Harris','2001-01-01')," +
                "('25F34D1F-23DD-4FDE-BC7B-AA5823F373DD','FE2D64B3-C943-43B3-90BD-9806C09756C9',N'Sofia Martinez','2002-01-01')," +
                "('25F34D1F-23DD-4FDE-BC7B-AA5823F373DD','EAE8E1B0-5D14-4542-B7E0-9473ABAE29F0',N'Lucas Bennett','2004-01-01')," +
                "('25F34D1F-23DD-4FDE-BC7B-AA5823F373DD','2D2B009E-66BA-4802-AEA3-52BAEAB4D459',N'Emma Novak','2005-01-01')," +
                "('25F34D1F-23DD-4FDE-BC7B-AA5823F373DD','03A876E6-B5F8-46EF-A3D8-07EC5A28C31A',N'Matteo Ricci','2006-01-01')," +
                "('674DBCB8-4F90-4ED5-AA2A-898D35A05650','46392AD2-86FF-4AD2-BD8E-9D4395C623D4',N'Aisha Khan','2003-01-01')," +
                "('674DBCB8-4F90-4ED5-AA2A-898D35A05650','2D47C786-04EC-4069-B9C5-FF4DFB8A6994',N'Oliver Stein','2001-01-01')," +
                "('674DBCB8-4F90-4ED5-AA2A-898D35A05650','0117882E-CFF2-4B22-A515-DE49EC3C60DE',N'Mila Petrova','2013-01-01')," +
                "('674DBCB8-4F90-4ED5-AA2A-898D35A05650','C210A5EE-B065-41B4-BBF1-64B27AF55733',N'Ethan Clarke','2011-01-01')," +
                "('89E33CC6-92F3-4ED5-B2B4-2B41F395F47A','307B9B90-7BFF-4D0D-9E38-424C70281C8A',N'Isabella Rossi','2007-01-01')," +
                "('89E33CC6-92F3-4ED5-B2B4-2B41F395F47A','E8200526-27ED-40FA-8E56-19B2C54F076B',N'Noah Williams','2019-01-01')," +
                "('89E33CC6-92F3-4ED5-B2B4-2B41F395F47A','3D2160E8-DC3C-46DE-B883-2CE19FB1E138',N'Chloe Lefevre','2018-01-01')," +
                "('89E33CC6-92F3-4ED5-B2B4-2B41F395F47A','31AD41B5-17D5-4248-8F09-6890C90D705D',N'Liam O’Connor','2017-01-01')," +
                "('89E33CC6-92F3-4ED5-B2B4-2B41F395F47A','860921D6-284E-46AF-ABA6-AD5539AE7FBC',N'Hana Suzuki','2016-01-01')," +
                "('80A345E3-BB65-4282-A564-619A1F627D45','985DEF3B-2F40-4A06-B8DD-FE1C46DD4266',N'Gabriel Costa','2015-01-01')," +
                "('80A345E3-BB65-4282-A564-619A1F627D45','9B88E497-88E0-44DC-B805-9A0A901BD3AC',N'Amira Hassan','2014-01-01')," +
                "('80A345E3-BB65-4282-A564-619A1F627D45','F0D5D6D6-FC59-4DF4-AE11-CE4F2324499A',N'Viktor Kovalenko','2013-01-01')," +
                "('80A345E3-BB65-4282-A564-619A1F627D45','58B557D6-56E1-4FCF-B34A-A48B2AC096F2',N'Sara Johansson','2020-01-01')," +
                "('80A345E3-BB65-4282-A564-619A1F627D45','99A36B74-04F2-4F3C-9CCD-E6431E8AE413',N'Adam Chen','2005-01-01')," +
                "('80A345E3-BB65-4282-A564-619A1F627D45','0CD7F24D-CAC6-4A82-8CBC-40D9C062385A',N'Layla Carter','2008-01-01')," +
                "('80A345E3-BB65-4282-A564-619A1F627D45','8C62C335-C744-4110-B1E5-71BEDAA554CD',N'Julian Weber','2009-01-01')," +
                "('2D4995BC-0C44-4A0F-BB65-5DD0EDC3680B','3D91306F-2706-4C90-B61B-BC4827DA1677',N'Daria Sokolova','2011-01-01')," +
                "('2D4995BC-0C44-4A0F-BB65-5DD0EDC3680B','3282A168-0FF9-4EEC-8A77-1589AF76F8A2',N'Marco Almeida','2010-01-01')," +
                "('2D4995BC-0C44-4A0F-BB65-5DD0EDC3680B','2C28822B-C9EF-48E1-B53D-F0E1A0D2DA8E',N'Nora Schultz','2021-01-01')";
            using MySqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();//без зворотнього результату
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed {0}\n{1}: ", ex.Message, sql);
            }
        }
        


        public void SeedNews()
        {
            String sql = "INSERT INTO News VALUES" +
                         "('488CD481-E5FF-4594-8F67-27AF6B723834', '36726ECC-120C-491B-AD53-2AC3C0FFD752', 'What happens to your body if you only eat fruit?', '...', '2025-11-22')," +
                         "('7D1AB47C-BDFD-49E9-B8FF-6E982E10A32F', '4B388C74-87F8-4F54-85B3-41E13035F0B1', 'Wife wanted!', '...', '2025-11-23')," +
                         "('0E7B6B39-84A3-4B54-A90B-B7EFBE7DA44C', '93FE7CA7-A98D-4A0A-89C2-F2C78B1B6C5B', 'The most beautiful cars ever made', '...', '2025-11-24')";
            using MySqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
            }
        }
        public void SeedSales()
        {
            string sql = "INSERT INTO Sales VALUES" +
                         "('1E7F21A1-237B-427B-BDED-1B1B32639E48', '57D1DD41-5B5E-47D8-BB2C-3433D4CD9BE8', '1E7F21A1-237B-427B-BDED-1B1B32639E48', 5, '2025-11-22 10:00:00')," +
                         "('ADE91C53-C314-4CFC-8B4A-D24F19E35646', 'FE2D64B3-C943-43B3-90BD-9806C09756C9', 'ADE91C53-C314-4CFC-8B4A-D24F19E35646', 3, '2025-11-23 14:30:00')," +
                         "('BC1FA982-51DD-41E9-8130-D7F0C08E07B7', 'EAE8E1B0-5D14-4542-B7E0-9473ABAE29F0', 'BC1FA982-51DD-41E9-8130-D7F0C08E07B7', 7, '2025-11-24 09:15:00')," +
                         "('E3AD30F6-5FD7-4F58-8EAD-1B5E964750DA', '2D2B009E-66BA-4802-AEA3-52BAEAB4D459', 'E3AD30F6-5FD7-4F58-8EAD-1B5E964750DA', 4, '2025-11-25 16:45:00');";

            using MySqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
            }
        }
        
        private void SeedAccess()
        {
            string sql = "INSERT INTO Accesses (Id, ManagerId, Login, Salt, Dk) VALUES" +
                         "('1A2B3C4D-0001-0000-0000-000000000001', '57D1DD41-5B5E-47D8-BB2C-3433D4CD9BE8', 'daniel.h', 'salt123', 'dk1')," +
                         "('1A2B3C4D-0002-0000-0000-000000000002', 'FE2D64B3-C943-43B3-90BD-9806C09756C9', 'sofia.m', 'salt234', 'dk2')," +
                         "('1A2B3C4D-0003-0000-0000-000000000003', 'EAE8E1B0-5D14-4542-B7E0-9473ABAE29F0', 'lucas.b', 'salt345', 'dk3')," +
                         "('1A2B3C4D-0004-0000-0000-000000000004', '2D2B009E-66BA-4802-AEA3-52BAEAB4D459', 'emma.n', 'salt456', 'dk4');";

            using MySqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery(); // без зворотнього результату
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed {0}\n{1}: ", ex.Message, sql);
            }
        }

        public void SeedSalesToday()
        {
            var products = GetAll<Product>();
            var managers = GetManagers();

            if (!products.Any() || !managers.Any())
                return;

            var random = new Random();
            var salesToday = new List<string>();

            // Додаємо випадкові продажі на сьогодні
            for (int i = 0; i < 10; i++)
            {
                var product = products[random.Next(products.Count)];
                var manager = managers[random.Next(managers.Count)];
                int quantity = random.Next(1, 10); // випадкова кількість 1-9

                string id = Guid.NewGuid().ToString();
                string sql = $"INSERT INTO Sales (Id, ManagerId, ProductId, Quantity, Moment) VALUES " +
                             $"('{id}', '{manager.Id}', '{product.Id}', {quantity}, '{DateTime.Today:yyyy-MM-dd} {random.Next(0,24)}:{random.Next(0,60)}:00')";
        
                salesToday.Add(sql);
            }

            // Виконуємо всі запити
            foreach (var sql in salesToday)
            {
                using var cmd = new MySqlCommand(sql, connection);
                try { cmd.ExecuteNonQuery(); }
                catch (Exception ex) { Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql); }
            }

            Console.WriteLine("Додано тестові продажі на сьогодні.");
        }


        
        public void PrintNP() => GetProducts().OrderBy(p => p.Price).ToList().ForEach(p => Console.WriteLine($"{p.Name} -- {p.Price:F2}"));
        public void PrintDesc() => GetProducts().OrderByDescending(p => p.Price).ToList().ForEach(p => Console.WriteLine($"{p.Name} -- {p.Price:F2}"));
        public void PrintAlphabet() => GetProducts().OrderBy(p => p.Name).ToList().ForEach(p => Console.WriteLine($"{p.Name} -- {p.Price:F2}"));
        public void PrintLoser() => GetProducts().OrderBy(p => p.Price).Take(3).ToList().ForEach(p => Console.WriteLine($"{p.Name} -- {p.Price:F2}"));
        public void PrintTop() => GetProducts().OrderByDescending(p => p.Price).Take(3).ToList().ForEach(p => Console.WriteLine($"{p.Name} -- {p.Price:F2}"));

        // -------------------- SALES --------------------
        public int GetSalesCountByMonth(int month, int year)
        {
            string sql = @"SELECT COUNT(*) FROM Sales 
                           WHERE MONTH(Moment) = @month AND YEAR(Moment) = @year";
            return ExecuteScalarInt(sql, new() { ["@month"] = month, ["@year"] = year });
        }
        
        public (int, int) GetSalesInfoByMonth(int month)
        {
            int currentYear = DateTime.Now.Year;
            int prevYear = currentYear - 1;

            string sqlCurrent = @"SELECT IFNULL(SUM(Quantity),0)
                          FROM Sales
                          WHERE MONTH(Moment) = @month 
                          AND YEAR(Moment) = @year";

            string sqlPrev = @"SELECT IFNULL(SUM(Quantity),0)
                       FROM Sales
                       WHERE MONTH(Moment) = @month 
                       AND YEAR(Moment) = @year";

            int current = ExecuteScalarInt(sqlCurrent, new Dictionary<string, object>
            {
                {"@month", month},
                {"@year", currentYear}
            });

            int prev = ExecuteScalarInt(sqlPrev, new Dictionary<string, object>
            {
                {"@month", month},
                {"@year", prevYear}
            });

            return (current, prev);
        }

        public List<SaleStat> GetSalesStatsByMonthSQL(int month, int year)
        {
            string sql = @"
        SELECT p.Name AS ProductName, IFNULL(SUM(s.Quantity), 0) AS Quantity
        FROM Products p
        LEFT JOIN Sales s ON p.Id = s.ProductId 
            AND MONTH(s.Moment) = @month 
            AND YEAR(s.Moment) = @year
        GROUP BY p.Name
        ORDER BY Quantity DESC";

            return ExecuteList<SaleStat>(sql, new Dictionary<string, object>
            {
                ["@month"] = month,
                ["@year"] = year
            });
        }

        public List<SaleStat> GetSalesStatsByMonth(int month, int year)
        {
            // Отримуємо дані продажів та продуктів
            var sales = ExecuteList<Sale>("SELECT * FROM Sales WHERE MONTH(Moment) = @month AND YEAR(Moment) = @year", 
                new Dictionary<string, object> { ["@month"] = month, ["@year"] = year });

            var products = GetAll<Product>();

            var stats = sales
                .GroupBy(s => s.ProductId)
                .Select(g => new SaleStat
                {
                    ProductName = products.First(p => p.Id == g.Key).Name,
                    Quantity = g.Sum(s => s.Quantity)
                })
                .ToList();

            return stats;
        }

        public class SaleLeader
        {
            public string ProductName { get; set; }
            public int CountChecks { get; set; }
            public int TotalQuantity { get; set; }
            public double TotalSum { get; set; }
        }
        public List<SaleLeader> GetTopSalesToday()
        {
            DateTime today = DateTime.Today;

            var salesToday = ExecuteList<Sale>("SELECT * FROM Sales WHERE DATE(Moment) = @today", 
                new Dictionary<string, object> { ["@today"] = today });

            var products = GetAll<Product>();

            var grouped = salesToday
                .GroupBy(s => s.ProductId)
                .Select(g =>
                {
                    var product = products.FirstOrDefault(p => p.Id == g.Key);
                    if (product == null) return null;

                    int countChecks = g.Count();
                    int totalQuantity = g.Sum(s => s.Quantity);
                    double totalSum = g.Sum(s => s.Quantity * product.Price);

                    return new SaleLeader
                    {
                        ProductName = product.Name,
                        CountChecks = countChecks,
                        TotalQuantity = totalQuantity,
                        TotalSum = totalSum
                    };
                })
                .Where(x => x != null)
                .ToList();

            Console.WriteLine("=== Top 3 за кількістю чеків ===");
            if (grouped.Any())
                foreach (var item in grouped.OrderByDescending(x => x.CountChecks).Take(3))
                    Console.WriteLine($"{item.ProductName} -- {item.CountChecks} чеків");
            else
                Console.WriteLine("Немає продажів сьогодні");

            Console.WriteLine("=== Top 3 за кількістю штук ===");
            if (grouped.Any())
                foreach (var item in grouped.OrderByDescending(x => x.TotalQuantity).Take(3))
                    Console.WriteLine($"{item.ProductName} -- {item.TotalQuantity} шт.");
            else
                Console.WriteLine("Немає продажів сьогодні");

            Console.WriteLine("=== Top 3 за сумою продажів ===");
            if (grouped.Any())
                foreach (var item in grouped.OrderByDescending(x => x.TotalSum).Take(3))
                    Console.WriteLine($"{item.ProductName} -- {item.TotalSum:F2} грн");
            else
                Console.WriteLine("Немає продажів сьогодні");

            return grouped;
        }

        public (List<SaleLeader> byReceipts, List<SaleLeader> byQuantity, List<SaleLeader> byAmount) GetTopSalesTodayTuple()
        {
            var grouped = GetTopSalesToday(); // вже існуючий метод
            var byReceipts = grouped.OrderByDescending(x => x.CountChecks).Take(3).ToList();
            var byQuantity = grouped.OrderByDescending(x => x.TotalQuantity).Take(3).ToList();
            var byAmount = grouped.OrderByDescending(x => x.TotalSum).Take(3).ToList();
            return (byReceipts, byQuantity, byAmount);
        }

        

        
    }
}
