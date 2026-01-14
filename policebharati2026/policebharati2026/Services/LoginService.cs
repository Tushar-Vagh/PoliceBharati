//using Microsoft.Data.SqlClient;
//using PoliceBharatiLogin.DTOs;

//namespace PoliceBharatiLogin.Services
//{
//    public class LoginService
//    {
//        private readonly IConfiguration _config;

//        public LoginService(IConfiguration config)
//        {
//            _config = config;
//        }

//        public async Task<UserDto?> AuthenticateAsync(string username, string password)
//        {
//            var connStr = _config.GetConnectionString("DefaultConnection");

//            using var conn = new SqlConnection(connStr);
//            using var cmd = new SqlCommand("SELECT * FROM USERS WHERE USERNAME = @username AND PASSWORD = @password", conn);
//            cmd.Parameters.AddWithValue("@username", username);
//            cmd.Parameters.AddWithValue("@password", password);

//            await conn.OpenAsync();
//            using var reader = await cmd.ExecuteReaderAsync();

//            if (await reader.ReadAsync())
//            {
//                return new UserDto
//                {
//                    Id = Convert.ToDouble(reader["ID"]),
//                    Username = reader["USERNAME"].ToString(),
//                    PermissionNo = Convert.ToDouble(reader["PERMISSIONNO"]),
//                    UserGroup = Convert.ToDouble(reader["USERGROUP"])
//                };
//            }

//            return null;
//        }
//    }
//}
//using Microsoft.Data.SqlClient;
//using PoliceBharatiLogin.DTOs;

//namespace PoliceBharatiLogin.Services
//{
//    public class LoginService
//    {
//        private readonly IConfiguration _config;

//        public LoginService(IConfiguration config)
//        {
//            _config = config;
//        }

//        public async Task<UserDto?> AuthenticateAsync(string username, string password)
//        {
//            var connStr = _config.GetConnectionString("DefaultConnection");

//            using var conn = new SqlConnection(connStr);
//            using var cmd = new SqlCommand(
//                "SELECT ID, USERNAME, PERMISSIONNO, USERGROUP FROM USERS WHERE USERNAME = @username AND PASSWORD = @password",
//                conn);

//            cmd.Parameters.AddWithValue("@username", username);
//            cmd.Parameters.AddWithValue("@password", password);

//            await conn.OpenAsync();
//            using var reader = await cmd.ExecuteReaderAsync();

//            if (await reader.ReadAsync())
//            {
//                return new UserDto
//                {
//                    Id = Convert.ToInt32(reader["ID"]),
//                    Username = reader["USERNAME"].ToString(),
//                    PermissionNo = Convert.ToInt32(reader["PERMISSIONNO"]),
//                    UserGroup = Convert.ToInt32(reader["USERGROUP"])
//                };
//            }

//            return null;
//        }
//    }
//}

using Microsoft.Data.SqlClient;
using policebharati2026.DTOs;

namespace policebharati2026.Services
{
    public class LoginService
    {
        private readonly IConfiguration _config;

        public LoginService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<UserDto?> AuthenticateAsync(string username, string password)
        {
            var connStr = _config.GetConnectionString("DefaultConnection");

            using var conn = new SqlConnection(connStr);
            using var cmd = new SqlCommand(
                "SELECT ID, USERNAME, PERMISSIONNO, USERGROUP " +
                "FROM USERS " +
                "WHERE USERNAME = @username AND PASSWORD = @password",
                conn);

            // ✅ Trim inputs to avoid leading/trailing space issues
            cmd.Parameters.AddWithValue("@username", username.Trim());
            cmd.Parameters.AddWithValue("@password", password.Trim());

            await conn.OpenAsync();

            // ✅ Debug logging to see what’s happening
            Console.WriteLine($"Attempting login for: {username} with password: {password}");

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new UserDto
                {
                    Id = Convert.ToInt32(reader["ID"]),
                    Username = reader["USERNAME"].ToString(),
                    PermissionNo = Convert.ToInt32(reader["PERMISSIONNO"]),
                    UserGroup = Convert.ToInt32(reader["USERGROUP"])
                };
            }

            return null; // ✅ If no match, return null
        }
    }
}
