﻿using HalloDoc.DataContext;
using HalloDoc.DataModels;
using Services.Contracts;
using System.Collections;

namespace Services.Implementation
{
    public class TableData : ITableData
    {

        private readonly ApplicationDbContext _context;
        public TableData()
        {
            _context = new ApplicationDbContext();
        }
        public List<Region> GetRegionList()
        {
            List<Region> regions = _context.Regions.ToList();
            return regions;
        }
        public List<Physician> GetPhysicianList()
        {
            List<Physician> physicians = _context.Physicians.Where(m => m.IsDeleted == new BitArray(new[] { false })).ToList();
            return physicians;
        }
        public List<PhysicianNotification> GetPhysicianNotificationList()
        {
            List<PhysicianNotification> physiciansnoti = _context.PhysicianNotifications.ToList();
            return physiciansnoti;
        }
        public List<PhysicianLocation> GetPhysicianLocationList()
        {
            List<PhysicianLocation> list = _context.PhysicianLocations.ToList();
            return list;
        }
    }
}
