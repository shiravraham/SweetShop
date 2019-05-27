using System;
using System.ComponentModel.DataAnnotations;


namespace final_project.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public User user { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }

    }
}
