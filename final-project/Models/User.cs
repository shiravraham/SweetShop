using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace final_project.Models
{
    public class User
{
        public int Id;
        public string FullName;
        public string Address;
        public string Email;
        public string Gender;
        public UserType UserType;
}
}
