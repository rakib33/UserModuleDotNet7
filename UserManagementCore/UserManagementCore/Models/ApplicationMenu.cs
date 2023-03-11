using System.ComponentModel.DataAnnotations;

namespace UserManagementCore.Models
{
    public class ApplicationMenu
    {
        [Key]
        public int Id { get; set; }

        // 0 is main root parent 
        public int ParentId { get; set; } = 0;

        /// <summary>
        /// [Visisbility]-> devAdmin and customer Admin
        /// </summary>
        public bool IsParentMenu { get; set; }

        /// <summary>
        /// [Visisbility]-> devAdmin and customer Admin
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// [Visisbility]-> devAdmin 
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// [Visisbility]-> devAdmin and customer Admin
        /// </summary>
        public bool MenuVisibility { get; set; }

        /// <summary>
        /// [Visisbility]-> devAdmin 
        /// </summary>
        [Required]
        public int Sequence { get; set; }

        /// <summary>
        /// [Visisbility]-> devAdmin and customer Admin
        /// this is is for menu generation and provide user acces Admin user or customer for Admin Panel
        /// </summary>
        [StringLength(250)]
        [Required]
        public string MenuName { get; set; }

        /// <summary>
        ///  [Visisbility]-> devAdmin and customer Admin
        ///  This name will display to end user customer for User panel
        /// </summary>
        [StringLength(100)]
        [Required]
        public string MenuDisplayName { get; set; }

        /// <summary>
        ///  [Visisbility]-> devAdmin  and customer       
        ///  Insert data from MenuUser also from UserStatus /designation table will add later
        ///  from MenuVisibility devAdmin define which menu will display for CustomerAdmin.
        ///  Customer Admin distribute all menu to child user .He can add data in UserStatus table and assign value .
        /// </summary>
        [StringLength(100)]
        public String MenuUserStatus { get; set; }

        /// <summary>
        /// [Visisbility]-> devAdmin and customer Admin
        /// </summary>
        [StringLength(500)]
        public string MenuDescription { get; set; }

        /// <summary>
        /// [Visisbility]-> devAdmin 
        /// </summary>       
        [Required]
        [StringLength(100)]
        public string MenuActionName { get; set; }

        [Required]
        [StringLength(100)]
        public virtual string ControllerName { get; set; }

        /// <summary>
        /// [Visisbility]-> devAdmin
        /// [Description]-> full url route will save hear, like [base api]/api/controller/action
        /// </summary>
        [StringLength(250)]
        [Required]
        [Url]
        public string MenuActionRoute { get; set; }

        /// <summary>
        /// One App Menu has many RoleDetails
        /// </summary>
        public virtual ICollection<ApplicationRoleDetails> RoleDetails { get; set; }
    }
}

/// <summary>
/// DevAdmin,DevUser,DevSupport for Developer company
/// CustomerAdmin,OfficeUser, OfficeStaff for Application client
/// EndUser is visitor
/// </summary>
public enum MenuUser
{
    DevAdmin,
    DevUser,
    DevSupport,
    CustomerAdmin,
    OfficeUser,
    OfficeStaff,
    EndUser
}


