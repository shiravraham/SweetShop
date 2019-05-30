using System.ComponentModel.DataAnnotations;


namespace final_project.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public Category Category { get; set; }
        public string ImgPath { get; set; }
    }
}
