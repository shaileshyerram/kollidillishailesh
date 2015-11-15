using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestMVC001.Models
{
    public class OldStudent
    {

        public int StudentId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(255,MinimumLength =2 )]
        public string StudentFirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string StudentMiddleName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(255, MinimumLength = 2)]
        public string StudentLastName { get; set; }

        //TODO do a dropdown list
        public string Class { get; set; }

        //TODO do a dropdown list
        public string Section { get; set; }

        [Required]
        [StringLength(1)]
        //TODO do a dropdown list
        public string Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "RFID #" ) ]        
        [RegularExpression(@"\d{6,20}")]
        public string Rfid { get; set; }

        [Required]
        [Display(Name = "Parent First Name")]
        [StringLength(255, MinimumLength = 2)]
        public string ParentFirstName { get; set; }

        [Display(Name = "Parent Middle Name")]
        [StringLength(255, MinimumLength = 2)]
        public string ParentMiddleName { get; set; }

        [Required]
        [Display(Name = "Parent Last Name")]
        [StringLength(255, MinimumLength = 2)]
        public string ParentLastName { get; set; }

        [Required]
        [Display(Name = "Parent Phone Number")]
        //[RegularExpression(@"\d")]
        [DataType(DataType.PhoneNumber)]
        public string ParentPrimaryPhoneNumber { get; set; }

        [Display(Name = "Home Phone Number")]
        //[RegularExpression(@"\d")]
        [DataType(DataType.PhoneNumber)]
        public string ParentHomePhoneNumber { get; set; }

        [Required]
        [Display(Name = "Parent Email Address")]
        [DataType(DataType.EmailAddress)]
        public string ParentEmailId { get; set; }

        [Required]
        [RegularExpression(@"\d{4,20}")]
        public string Orgid { get; set; }

        public string StudentName
        {
            get { return string.Format("{0} {1} {2}", StudentFirstName, StudentMiddleName, StudentLastName); }
        }
        public string ParentName
        {
            get { return string.Format("{0} {1} {2}", ParentFirstName, ParentMiddleName, ParentLastName); }
        }
     }


}