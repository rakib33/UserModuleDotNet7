using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementEntityModel.ViewModel
{
  public  class vm_ApplicationUser
    {
        public string Id { get; set; }
        public string Tag { get; set; }                
        public string   Email { get; set; }
        public string   PhoneNumber { get; set; }
        public string NameTitle { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string Designation { get; set; }

        public byte[] ImageInByte { get; set; }   
        public int Age { get; set; }
    }
}
