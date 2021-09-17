using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FormApplication.Models
{
    public class OnlineModel
    {
        public string Fullname { get; set; }
        public string Gender { get; set; }
        public string HouseUnit { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Profession { get; set; }
        public string StatusType { get; set; }
        public string SpouseName { get; set; }
        public string GenderType { get; set; }
        public string EmailAddress { get; set; }
        public string Number { get; set; }

        public OnlineModel(string fullname, string gender, string houseUnit, string email, string mobile, string profession, string statusType, string spouseName, string genderType, string emailAddress, string number)
        {
            Fullname = fullname;
            Gender = gender;
            HouseUnit = houseUnit;
            Email = email;
            Mobile = mobile;
            Profession = profession;
            StatusType = statusType;
            SpouseName = spouseName;
            GenderType = genderType;
            EmailAddress = emailAddress;
            Number = number;
        }

        public OnlineModel()
        {
            Fullname = "Null";
            Gender = "Null";
            HouseUnit = "Null";
            Email = "Null";
            Mobile = "Null";
            Profession = "Null";
            StatusType = "Null";
            SpouseName = "Null";
            GenderType = "Null";
            EmailAddress = "Null";
            Number = "Null";
        }
    }
}