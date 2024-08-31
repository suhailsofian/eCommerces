using Dapper;
using ECommerce.API.Data;
using ECommerce.API.Models;
using ECommerce.API.Models.EmailSettings;
using Microsoft.IdentityModel.Tokens;
using System.Data.Common;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace ECommerce.API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration configuration;
        private readonly string dbconnection;
         private readonly DapperContext _dapperContext;
          private readonly IEmailService _emailService;
        private readonly string dateformat;
        public UserRepository(IConfiguration configuration, DapperContext dapperContext,IEmailService emailService)
        {
            this.configuration = configuration;
            _dapperContext=dapperContext;
             _emailService=emailService;
            dbconnection = this.configuration["ConnectionStrings:DB"];
            dateformat = this.configuration["Constants:DateFormat"];
        }

        public async Task sendEmailAsync(User user){

              var emailBody = $@"
                    <html>
                    <body style='text-align: center;'>
                        <div style='background-color: #f4f4f4; padding: 20px; display: inline-block;'>
                            <h2 style='color: #333; font-size: 18px; margin-bottom: 10px;'>Reset Password</h2>
                            <p style='color: #555; font-size: 16px;'>Hello {user.FirstName}:{user.Mobile},</p>
                            <p style='color: #555; font-size: 16px;'>Please click the button below to reset your password:</p>
                            <div style='text-align: center; margin: 20px 0;'>
                                // <a href='' style='background-color: #007bff; color: #fff; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block;'>
                                //     Reset Password
                                // </a>
                            </div>
                            <p style='color: #555; font-size: 16px;'>If you didn't request a password reset, please ignore this email.</p>
                            <p style='color: #555; font-size: 16px;'>Thank you,</p>
                            <p style='color: #555; font-size: 16px;'>Market Team</p>
                        </div>
                    </body>
                    </html>
                ";

                var subject = "Reset Password";

                var email = new Email
                {
                    To = user.Email,
                    Subject = subject,
                    Body = emailBody,
                    IsHtml = true // Set IsHtml to true to indicate that the email body contains HTML
                };

                await _emailService.SendEmailAsync(email);

        }

    

       

       

       
      

        public User GetUser(int id)
        {
            var user = new User();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Users WHERE Id=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    user.Id = (int)reader["Id"];
                    user.FirstName = (string)reader["FirstName"];
                    user.LastName = (string)reader["LastName"];
                    user.Email = (string)reader["Email"];
                    user.Address = (string)reader["Address"];
                    user.Mobile = (string)reader["Mobile"];
                    user.Password = (string)reader["Password"];
                    user.CreatedAt = (string)reader["CreatedAt"];
                    user.ModifiedAt = (string)reader["ModifiedAt"];
                }
            }
            return user;
        }

       


        

       
    

        public async Task<bool> InsertUserAsync(UserDto user)
        {
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                connection.Open();

                string query = "SELECT COUNT(*) FROM Users WHERE Email='" + user.Email + "';";
                command.CommandText = query;
                int count = (int)command.ExecuteScalar();
                if (count > 0)
                {
                    connection.Close();
                    return false;
                }

                query = "INSERT INTO Users (FirstName, LastName, Address, Mobile, Email, Password, CreatedAt, ModifiedAt) values (@fn, @ln, @add, @mb, @em, @pwd, @cat, @mat);";

                command.CommandText = query;
                command.Parameters.Add("@fn", System.Data.SqlDbType.NVarChar).Value = user.FirstName;
                command.Parameters.Add("@ln", System.Data.SqlDbType.NVarChar).Value = user.LastName;
                command.Parameters.Add("@add", System.Data.SqlDbType.NVarChar).Value = user.Address;
                command.Parameters.Add("@mb", System.Data.SqlDbType.NVarChar).Value = user.Mobile;
                command.Parameters.Add("@em", System.Data.SqlDbType.NVarChar).Value = user.Email;
                command.Parameters.Add("@pwd", System.Data.SqlDbType.NVarChar).Value = user.Password;
                command.Parameters.Add("@cat", System.Data.SqlDbType.NVarChar).Value = user.CreatedAt;
                command.Parameters.Add("@mat", System.Data.SqlDbType.NVarChar).Value = user.ModifiedAt;

                command.ExecuteNonQuery();
            }
            // await sendEmailAsync(user);
            return true;
        }

        public string IsUserPresent(string email, string password)
        {
            User user = new();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                connection.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE Email='" + email + "' AND Password='" + password + "';";
                command.CommandText = query;
                int count = (int)command.ExecuteScalar();
                if (count == 0)
                {
                    connection.Close();
                    return "";
                }

                query = "SELECT * FROM Users WHERE Email='" + email + "' AND Password='" + password + "';";
                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    user.Id = (int)reader["Id"];
                    user.FirstName = (string)reader["FirstName"];
                    user.LastName = (string)reader["LastName"];
                    user.Email = (string)reader["Email"];
                    user.Address = (string)reader["Address"];
                    user.Mobile = (string)reader["Mobile"];
                    user.Password = (string)reader["Password"];
                    user.CreatedAt = (string)reader["CreatedAt"];
                    user.ModifiedAt = (string)reader["ModifiedAt"];
                }

                string key = "MNU66iBl3T5rh6H52i69";
                string duration = "60";
                var symmetrickey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(symmetrickey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim("firstName", user.FirstName),
                    new Claim("lastName", user.LastName),
                    new Claim("address", user.Address),
                    new Claim("mobile", user.Mobile),
                    new Claim("email", user.Email),
                    new Claim("createdAt", user.CreatedAt),
                    new Claim("modifiedAt", user.ModifiedAt)
                };

                var jwtToken = new JwtSecurityToken(
                    issuer: "localhost",
                    audience: "localhost",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(Int32.Parse(duration)),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(jwtToken);
            }
            // return "";
        }
    }
}
