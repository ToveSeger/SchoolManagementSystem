//namespace SchoolManagementSystem.Models
//{
//    using System;
//    using System.Collections.Generic;
//    using System.ComponentModel.DataAnnotations;
//    using System.Linq;
//    using System.Web;

//    public class ParticipantMetaData
//    {
//        [StringLength(50)]
//        [Display(Name = "Last Name")]
//        public string LastName { get; set; }

//        [StringLength(50)]
//        [Display(Name = "First Name")]
//        public string FirstName { get; set; }

//        [Required]
//        [DataType(DataType.DateTime)]
//        [Display(Name = "Date of enrollment")]
//        public DateTime EntrollmentDate { get; set; }

//        [Required]
//        [DataType(DataType.DateTime)]
//        [Display(Name = "Date of birth")]
//        public DateTime DateOfBirth { get; set; }

//        [StringLength(50)]
//        [Display(Name = "Middle Name")]
//        public string MiddleName { get; set; }
//    }

//    [MetadataType(typeof(ParticipantMetaData))]
//    public partial class Participant
//    {

//    }
//}