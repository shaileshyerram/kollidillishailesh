using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestMVC001.Models
{
    public class Student
    {

        public int StudentId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(100,MinimumLength =2 )]
        public string StudentFirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string StudentMiddleName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(100, MinimumLength = 2)]
        public string StudentLastName { get; set; }

        public string Class { get; set; }
        public string Section { get; set; }

        [Required]
        [StringLength(1, MinimumLength = 1)]
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "RFID #" ) ]        
        [RegularExpression(@"\d{6,10}")]
        public string Rfid { get; set; }

        [Required]
        [Display(Name = "Parent First Name")]
        [StringLength(100, MinimumLength = 2)]
        public string ParentFirstName { get; set; }

        [Display(Name = "Parent Middle Name")]
        [StringLength(100, MinimumLength = 2)]
        public string ParentMiddleName { get; set; }

        [Required]
        [Display(Name = "Parent Last Name")]
        [StringLength(100, MinimumLength = 2)]
        public string ParentLastName { get; set; }

        [Required]
        [Display(Name = "Parent Phone Number")]
        [RegularExpression(@"\d")]
        public string ParentPrimaryPhoneNumber { get; set; }

        [Display(Name = "Home Phone Number")]
        [RegularExpression(@"\d")]
        public string ParentHomePhoneNumber { get; set; }

        [Required]
        [Display(Name = "Parent Email Address")]
        [DataType(DataType.EmailAddress)]
        public string ParentEmailId { get; set; }


        public string Orgid { get; set; }

        //public string StudentName
        //{
        //    get
        //    {
        //        return string.Format("{0} {1} {2}", StudentFirstName, StudentMiddleName, StudentLastName);

        //    }
        //}
        public string StudentName => $"{StudentFirstName} {StudentMiddleName} {StudentLastName}";
        public string ParentName => $"{ParentFirstName} {ParentMiddleName} {ParentLastName}";
     }


}

//bigint StudentId
//varchar StudentFirstName
//varchar StudentMiddleName
//varchar StudentLastName
//varchar Class
//varchar Section
//varchar Gender
//datetime DateOfBirth
//varchar Rfid
//varchar ParentFirstName
//varchar ParentMiddleName
//varchar ParentLastName
//float ParentPrimaryPhoneNumber
//float ParentHomePhoneNumber
//varchar ParentEmailId
//varchar Orgid