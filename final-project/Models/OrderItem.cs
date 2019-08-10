using System.ComponentModel.DataAnnotations;

namespace final_project.Models
{
    public class OrderItem
    {
        //[Key]
        public int ID { get; set; }

        [Key]
        public Order Order { get; set; }

        [Key]
        public Product Product{ get; set; }

        public int OrderID { get; set; }

        public int Quantity{ get; set; }
     }
}
