
using System.ComponentModel.DataAnnotations;

namespace final_project.Models
{
    public class UserType
    {
        [Key]
        public int Id { get; set; }
        public string TypeName { get; set; }
    }
}
