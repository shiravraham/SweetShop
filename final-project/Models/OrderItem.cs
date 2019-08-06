using System.ComponentModel.DataAnnotations;

namespace final_project.Models
{
    public class OrderItem
    {
        [Key]
        public int ID { get; set; }
        public Order Order { get; set; }
        public Product Product{ get; set; }
        public int Quantity{ get; set; }
     }
}
