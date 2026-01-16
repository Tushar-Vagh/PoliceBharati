using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;

namespace WebCamAPI.Controllers
{
    [ApiController]
    [Route("api/photo")]
    public class PhotoController : ControllerBase
    {
        private readonly IConfiguration _config;

        public PhotoController(IConfiguration config)
        {
            _config = config;
        }

        // 🔐 AES-256 key derivation (PBKDF2)
        private static byte[] DeriveKey(string password, byte[] salt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                100_000,
                HashAlgorithmName.SHA256
            );

            return pbkdf2.GetBytes(32); // 32 bytes = AES-256
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(
            IFormFile file,
            [FromForm] string applicationNo
        )
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            if (string.IsNullOrWhiteSpace(applicationNo))
                return BadRequest("ApplicationNo is required");

            // 🔑 Read encryption password
            var password = _config["Encryption:Key"];
            if (string.IsNullOrEmpty(password))
                throw new Exception("Encryption key missing in appsettings.json");

            // 📥 Read uploaded image
            byte[] plainBytes;
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                plainBytes = ms.ToArray();
            }

            // 🔐 Encrypt image
            byte[] encrypted;
            byte[] iv;
            byte[] salt;

            using (var aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                salt = RandomNumberGenerator.GetBytes(16);
                aes.Key = DeriveKey(password, salt);
                aes.GenerateIV();
                iv = aes.IV;

                using var encryptor = aes.CreateEncryptor();
                encrypted = encryptor.TransformFinalBlock(
                    plainBytes, 0, plainBytes.Length
                );
            }

            // 💾 Store encrypted photo in MASTER table
            using var conn = new SqlConnection(
                _config.GetConnectionString("DefaultConnection")
            );
            await conn.OpenAsync();

            var cmd = new SqlCommand(@"
                UPDATE dbo.Master
                SET
                    EncryptedPhoto = @Photo,
                    PhotoIV = @IV,
                    PhotoSalt = @Salt,
                    PhotoUploadedDate = GETDATE()
                WHERE ApplicationNo = @ApplicationNo
            ", conn);

            cmd.Parameters.Add("@Photo", System.Data.SqlDbType.VarBinary).Value = encrypted;
            cmd.Parameters.Add("@IV", System.Data.SqlDbType.VarBinary).Value = iv;
            cmd.Parameters.Add("@Salt", System.Data.SqlDbType.VarBinary).Value = salt;
            cmd.Parameters.AddWithValue("@ApplicationNo", applicationNo);

            int rows = await cmd.ExecuteNonQueryAsync();

            if (rows == 0)
                return NotFound("ApplicationNo not found");

            return Ok(new
            {
                message = "Photo encrypted and stored successfully",
                applicationNo
            });
        }
    }
}
