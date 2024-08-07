using System.ComponentModel.DataAnnotations;

namespace Solvefy.Task.Models.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // Better way is to using hashing techniques to protect password
        public int Role { get; set; } // 1 for admin , 0 for simple user
    }
}
