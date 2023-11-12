using System.ComponentModel.DataAnnotations;

namespace GadgetIPhoneStore.Models
{
    public class Register
    {
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "User password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "User email is required")]
        public string Email { get; set; }
    }
}
