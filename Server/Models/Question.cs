using System.ComponentModel.DataAnnotations;

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
        [Required]
        public Test Test { get; set; }
    }
}
