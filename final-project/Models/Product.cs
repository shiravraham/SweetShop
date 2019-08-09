using System;
using System.ComponentModel.DataAnnotations;


namespace final_project.Models
{
    public class Product : IComparable<Product>
    {
        [Key]
        public int ID { get; set; }
        public Category Category { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string ImgPath { get; set; }
        public bool IsDeleted { get; set; }

        public int CompareTo(Product other)
        {
            return this.ID.CompareTo(other.ID);
        }
    }
}
