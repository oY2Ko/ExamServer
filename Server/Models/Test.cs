using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class Test
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public User Owner { get; set; }
        
        public List<Question> Questions { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
