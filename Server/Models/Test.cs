using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Server.Models
{
    public class Test
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [AllowNull]
        public List<Question> Questions { get; set; }
        [AllowNull]
        public string Description { get; set; }

        public bool IsActive { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
    }
}
