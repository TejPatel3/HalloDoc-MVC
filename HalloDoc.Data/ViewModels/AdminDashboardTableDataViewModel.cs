﻿using System.ComponentModel;

namespace Services.ViewModels
{
    public class AdminDashboardTableDataViewModel
    {
        public string PatientName { get; set; }
        public DateOnly PatientDOB { get; set; }
        public string RequestorName { get; set; }
        public DateTime RequestedDate { get; set; }
        public string PatientPhone { get; set; }
        public string RequestorPhone { get; set; }
        public string RequestorEmail { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public string ProviderEmail { get; set; }
        public string PatientEmail { get; set; }
        public int RequestType { get; set; }
        public string? PhysicianName { get; set; }
        public int? PhysicianId { get; set; }

        public int requestid { get; set; }
        public int? regionid { get; set; }

        public int regionidtoclose { get; set; }


        public enum RegionName
        {
            India,
            [Description("New York")]
            NewYork,
            Virginia,
            [Description("District Of Columbia")]
            DistrictOfColumbia,
            Maryland
        }

        public static string GetEnumDescription(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public string RegionNameById(int regionid)
        {
            string regionname = (GetEnumDescription((RegionName)regionid)).ToString();
            return regionname;
        }
        public enum Requestby
        {
            first,
            Patient,
            Friend_Family,
            Concierge,
            Business_Partner
        }
        public string RequestTypeName(int by)
        {
            string By = ((Requestby)by).ToString();
            return By;
        }
    }
}
