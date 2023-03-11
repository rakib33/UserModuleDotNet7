using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using UserManagementEntityModel.Models;

namespace UserManagementCore.Models
{
    public class ApplicationRoleDetails : BaseEntity
    {

        [Key]
        public virtual int Id { get; set; }

        [StringLength(100)]
        public virtual string Url { get; set; }
        public virtual string CustomQueryString { get; set; }

        [Required]
        public virtual bool IsActive { get; set; }

        public virtual string RoleId { get; set; }
        public virtual ApplicationRole Role { get; set; }

        [ForeignKey("MenuId")]
        public virtual ApplicationMenu ApplicationMenus { get; set; }
        public virtual int MenuId { get; set; }

    }
}
