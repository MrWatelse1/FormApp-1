using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FormApplication.Models
{
    public class OnlineModel
    {
        public int ID { get; set; }
        public string Fullname { get; set; }
        public string Gender { get; set; }
        public string HouseUnit { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Profession { get; set; }
        public string StatusType { get; set; }
        public List<SpouseModel> SpouseModels { get; set; }

        public OnlineModel(int id, string fullname, string gender, string houseUnit, string email, string mobile, string profession, string statusType, string spouseName, string genderType, string emailAddress, string number)
        {
            ID = id;
            Fullname = fullname;
            Gender = gender;
            HouseUnit = houseUnit;
            Email = email;
            Mobile = mobile;
            Profession = profession;
            StatusType = statusType;
        }

        public OnlineModel()
        {
            ID = -1;
            Fullname = "Null";
            Gender = "Null";
            HouseUnit = "Null";
            Email = "Null";
            Mobile = "Null";
            Profession = "Null";
            StatusType = "Null";
        }
    }
}