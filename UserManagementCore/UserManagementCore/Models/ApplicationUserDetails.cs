using System.ComponentModel.DataAnnotations;

namespace UserManagementCore.Models
{
    public class ApplicationUserDetails
    {
        [Key]
        // [StringLength(128)]
        public virtual string Id { get; set; }
        public byte[] ImageInByte { get; set; }

        [Range(18, 120)]
        public int Age { get; set; }

        [CreditCard]
        public string CreditCard { get; set; }

        public string NameTitle { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string FatherName { get; set; }
        public string MotherName { get; set; }

        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }

        public string Designation { get; set; }
        public string Occupation { get; set; }
        public string Education { get; set; }

        public virtual string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
