using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SwiftUserManagement.Application.Contracts.Persistence;
using SwiftUserManagement.Domain.Entities;

namespace SwiftUserManagement.Infrastructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        // Dependency injection for the configuration to get the postgresql connection string
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        // Creating a new user and adding them into the database
        public async Task<bool> CreateUser(string email, string userName, string password, string role)
        {
            using var connection = new NpgsqlConnection
                (_configuration["DatabaseSettings:ConnectionString"]);

            var affected = await connection.ExecuteAsync
                ("INSERT INTO Users(Email, UserName, Password, Role) VALUES(@Email, @Username, @Password, @Role);",
                new { Email = email, Username = userName, Password = BCrypt.Net.BCrypt.HashPassword(password), Role = role});

            if (affected == 0)
            {
                return false;
            }

            return true;
        }

        // Retreiving a user from the database
        public async Task<User> GetUser(string userName)
        {
            using var connection = new NpgsqlConnection
                (_configuration["DatabaseSettings:ConnectionString"]);

            var user = await connection.QueryFirstOrDefaultAsync<User>
                ("SELECT * FROM Users WHERE UserName = @UserName", new { UserName = userName });

            if (user == null)
            {
                return null;
            }

            return user;
        }

        // Getting a user by email
        public async Task<User> GetUserByEmail(string email)
        {
            using var connection = new NpgsqlConnection
                (_configuration["DatabaseSettings:ConnectionString"]);

            var user = await connection.QueryFirstOrDefaultAsync<User>
                ("SELECT * FROM Users WHERE Email = @Email", new { Email = email });

            if (user == null)
            {
                return new User
                {
                    Id = -1,
                };
            }

            return user;
        }

        // Updating user details
        public async Task<bool> UpdateUser(User user)
        {
            using var connection = new NpgsqlConnection
                (_configuration["DatabaseSettings:ConnectionString"]);

            var affected = await connection.ExecuteAsync
                ("UPDATE Users SET Email=@Email, UserName=@UserName, Password=@Password, Role=@Role WHERE Id = @Id",
                 new { Email = user.Email, UserName = user.UserName, Password = user.Password, Role = user.Role, Id = user.Id });

            if (affected == 0)
            {
                return false;
            }

            return true;
        }

        // Adding video analysis data to the database for a user
        public async Task<bool> AddVideoAnalysisData(string videoName, int userId, string weaknessPrediction)
        {
            using var connection = new NpgsqlConnection
                (_configuration["DatabaseSettings:ConnectionString"]);

            var affected = await connection.ExecuteAsync
                ("INSERT INTO Videos(User_Id, Video_Name, Weakness_Prediction) VALUES(@User_Id, @Video_Name, @Weakness_Prediction)",
                new { User_Id = userId, Video_Name = videoName, Weakness_Prediction =  weaknessPrediction });


            if (affected == 0)
            {
                return false;
            }

            return true;
        }

        // Adding game analysis data to the database for a user
        public async Task<bool> AddGameAnalysisData(int result1, int result2, int userId, int level, string explanation)
        {
            using var connection = new NpgsqlConnection
                (_configuration["DatabaseSettings:ConnectionString"]);

            var affected = await connection.ExecuteAsync
                ("INSERT INTO GameScores(User_Id, Level, Score, Explanation) VALUES(@User_Id, @level, @result_1, @Explanation)",
                new { User_Id = userId, level = level, result_1 = result1 + result2, Explanation = explanation });

            if (affected == 0)
            {
                return false;
            }

            return true;
        }
    }
}
