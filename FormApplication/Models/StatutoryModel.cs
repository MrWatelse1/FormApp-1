using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FormApplication.Models
{
    public class StatutoryModel
    {
        public int StatusId { get; set; }
        public string StatusType { get; set; }

        public StatutoryModel()
        {
            StatusId = -1;
            StatusType = "Status";
        }

        public StatutoryModel(int statusId, string statusType)
        {
            StatusId = statusId;
            StatusType = statusType;
        }
    }
}