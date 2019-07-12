using System.ComponentModel.DataAnnotations;

namespace final_project.Models
{
    public class Branch
    {
        [Key]
        public int ID { get; set; }
        public string branchName { get; set; }
        public string addressInfo { get; set; }
        public float locationX { get; set; }
        public float locationY { get; set; }
    }
}
