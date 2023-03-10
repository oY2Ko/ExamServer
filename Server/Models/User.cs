using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
        public List<Test> Tests { get; set; }
        public User( string name, string password, string role)
        {
            Name = name;
            Password = password;
            Role = role;
        }
    }
}
