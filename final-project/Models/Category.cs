using System.ComponentModel.DataAnnotations;

namespace final_project.Models
{
    public class Category
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
