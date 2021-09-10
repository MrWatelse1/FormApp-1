using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FormApplication.Models
{
    public class GenderModel
    {
        public int GenderId { get; set; }
        public string GenderType { get; set; }
        public GenderModel()
        {
            GenderId = -1;
            GenderType = "Gender";
        }

        public GenderModel(int genderId, string genderType)
        {
            GenderId = genderId;
            GenderType = genderType;
        }
    }
}