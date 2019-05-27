using System.ComponentModel.DataAnnotations;


namespace final_project.Models
{
    public class OrderStatus
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
