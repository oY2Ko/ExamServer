using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public string Answer { get; set; }
        [Required]
        public double Mark { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }

    }
}
