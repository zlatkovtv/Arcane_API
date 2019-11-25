using System.Data;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Collections.Generic;
using System;
using System.Text;

public sealed class UserRepository: CrudRepository<User>, IUserRepository
{
    public UserRepository(IDatabaseRepository dbContext) : base(dbContext)
    {
    }
    
    public override User Get(int id)
    {
        using (IDbConnection connection = this.dbContext.GetConnection()) {
            connection.Open();
            return connection.ExecuteScalar<User>("SELECT * FROM USER WHERE ID = @Id", new { Id = id });
        }
    }

    public override int Add(User user)
    {
        if(!user.IsPasswordValid()) 
        {
            throw new ArgumentException("Password is not valid");
        }

        if(this.Get(user.Id) != null) {
            throw new ArgumentException("User already exists.");
        }

        byte[] passwordHash, passwordSalt;
        CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);

        user.Password = passwordHash.ToString();
        user.PasswordSalt = passwordSalt.ToString();

        using (IDbConnection connection = this.dbContext.GetConnection()) {
            connection.Open();
            return connection.Execute("INSERT USER values(@Username, @FirstName, @LastName, @Email, @Password, @PasswordSalt)", 
            new { 
                Username = user.Username, 
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                PasswordSalt = user.PasswordSalt
            });
        }
    }

    public override int Delete(int id)
    {
        using (IDbConnection connection = this.dbContext.GetConnection()) {
            connection.Open();
            return connection.Execute("DELETE FROM USER WHERE ID = @Id", new { Id = id });
        }
    }

    public override int Update(User user)
    {
        byte[] passwordHash, passwordSalt;
        CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);

        user.Password = Encoding.ASCII.GetString(passwordHash);
        user.PasswordSalt = Encoding.ASCII.GetString(passwordSalt);

        using (IDbConnection connection = this.dbContext.GetConnection()) {
            connection.Open();
            return connection.Execute("UPDATE USER SET FIRSTNAME=@FirstName, LASTNAME=@LastName, EMAIL=@Email, PASSWORD=@Password, PASSWORDSALT=@PasswordSalt", 
            new { 
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                PasswordSalt = user.PasswordSalt
            });
        }
    }

    public User Authenticate(User user)
    {
        var foundUser = Get(user.Id);
        if(foundUser == null) {
            return null;
        }

        if (!VerifyPasswordHash(user.Password, Encoding.ASCII.GetBytes(foundUser.Password), Encoding.ASCII.GetBytes(foundUser.PasswordSalt))) {
            return null;
        }

        return foundUser;
    }
    
    private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        if (password == null) throw new ArgumentNullException("password");
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
        if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
        if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

        using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != storedHash[i]) return false;
            }
        }

        return true;
    }

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        if (password == null) throw new ArgumentNullException("password");
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}