using System;
using System.Collections.Generic;

/// <summary>
/// Summary description for Transportation
/// </summary>
/// 
namespace MSI.Web.MSINet.BusinessEntities
{
    public class TransportationPunch
    {
        public TransportationPunch() { }
        public int transportationID { get; set; }
        public string aident { get; set; }
        public DateTime rideDate { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string client { get; set; }
        public string vehicleId { get; set; }
        public int swipeCount { get; set; }
        public string fleetMaticsId { get; set; }
        public string versionId { get; set; }
        public string dispatch { get; set; }
        public string rosterClient { get; set; }
        public string driverName { get; set; }
    }
    public class Vehicle
    {
        //SELECT vehicle_id, office_id, license_plate, description, num_passengers, make, model, year, vehicle_num, out_of_service
        public Vehicle() { }
        public string vehicleId { get; set; } //vehicle_num
        public string fleetMaticsId { get; set; } //
        public string office { get; set; }
        public string vin { get; set; }
        public string makeModel { get; set; }
        public int numPassengers { get; set; }
        public bool outOfService { get; set; }
        public List<TransportationPunch> transportList { get; set; }
        public Boolean inFleet { get; set; }
    }
    public class DriverData
    {
        public DriverData() { }
        public string driverId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int officeId { get; set; }
        public string officeName { get; set; }
        public DateTime rideDate { get; set; }
        public int vehicleId { get; set; }
        public int passengerCount { get; set; }
    }
}