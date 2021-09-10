using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FormApplication.Models
{
    public class FormModel
    {
        public int ID { get; set; }
        [Required]
        public string Fullname { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string HouseNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string Profession { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        //spouse
        public int SpouseId { get; set; }
        [Required]
        public int  FormId { get; set; }
        [Required]
        public string Names { get; set; }
        [Required]
        public string GenderType { get; set; }
        [Required][EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        public string Number { get; set; }

        public FormModel()
        {
            ID = -1;
            Fullname = "What is your name";
            Gender = "Enter Gender";
            HouseNumber = "Enter House Unit";
            Email = "What's your email?";
            Mobile = "What's your number?";
            Profession = "What's your profession?";
            Status = "What's your status";
            SpouseId = -1 ;
            FormId = -1;
            Names = "What's your name?";
            GenderType = "Gender";
            EmailAddress = "what is your email";
            Number = "Enter your number";
        }

        public FormModel(int iD, string fullname, string gender, string houseNumber, string email, string mobile, string profession, string status, int spouseId, int formId, string names, string genderType, string emailAddress, string number)
        {
            ID = iD;
            Fullname = fullname;
            Gender = gender;
            HouseNumber = houseNumber;
            Email = email;
            Mobile = mobile;
            Profession = profession;
            Status = status;
            SpouseId = spouseId;
            FormId = formId;
            Names = names;
            GenderType = genderType;
            EmailAddress = emailAddress;
            Number = number;
        }
    }

}