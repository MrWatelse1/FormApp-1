using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FormApplication.Models
{
    public class HousingModel
    {
        public int HouseId { get; set; }
        public string HouseType { get; set; }
        public string HouseUnit { get; set; }

        public HousingModel()
        {
            HouseId = -1;
            HouseType = "Letter";
            HouseUnit = "Unit";
        }

        public HousingModel(int houseId, string houseType, string houseUnit)
        {
            HouseId = houseId;
            HouseType = houseType;
            HouseUnit = houseUnit;
        }
    }
}