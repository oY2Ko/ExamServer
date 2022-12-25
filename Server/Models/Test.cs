using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class Test
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public User Owner { get; set; }
        [Required]
        public List<Question> Questions { get; set; }

    }
}
