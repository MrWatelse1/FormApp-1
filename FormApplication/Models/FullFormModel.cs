using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FormApplication.Models
{
    public class FullFormModel
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

        [Display(Name ="Spouse(s)")]
        public List<SpouseModel> SpouseModels { get; set; }
       
        public FullFormModel()
        {
            ID = -1;
            Fullname = "What's your name";
            Gender = "Enter gender";
            HouseNumber = "Where are you living?";
            Email = "What is your email";
            Mobile = "Phone";
            Profession = "Occupation";
            Status = "What's your position?";
        }

        public FullFormModel(int iD, string fullname, string gender, string houseNumber, string email, string mobile, string profession, string status)
        {
            ID = iD;
            Fullname = fullname;
            Gender = gender;
            HouseNumber = houseNumber;
            Email = email;
            Mobile = mobile;
            Profession = profession;
            Status = status;
        }
    }
}