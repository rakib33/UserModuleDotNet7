using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementEntityModel.ViewModel
{
    public class UserSignInModel
    {
        [EmailAddress]
        [Required]
        public String  Email { get; set; }

        [Required]
        [StringLength(16)]
        public String Password { get; set; }
        [Required]
        public bool IsRememberMe { get; set; }
    }
}
