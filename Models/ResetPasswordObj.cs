using System.ComponentModel.DataAnnotations;

namespace Market.Models
{
    public class ResetPasswordObj {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string Token { get; set; }
    }
}