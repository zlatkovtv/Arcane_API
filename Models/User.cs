using System;
using System.ComponentModel.DataAnnotations;

public class User {
    [Required]
    public int Id { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }

    public string PasswordSalt { get; set; }

    public bool IsPasswordValid()
    {
        throw new NotImplementedException();
    }
}