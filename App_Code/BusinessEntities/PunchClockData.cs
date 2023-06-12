using MSI.Web.MSINet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

/// <summary>
/// Data Definitions for punches recieved from FA05 bio clocks
/// </summary>
/// 

namespace PunchClock
{
    public class MobileDataIn
    {
        public string PhoneNum { get; set; }
        public string Id { get; set; }
        public string Image { get; set; }
        public string PhoneLatitude { get; set; }
        public string PhoneLongitude { get; set; }
        public DateTime _phoneDateTime { get; set; }
        public string PhoneDateTime { get; set; }
        public string ClientId { get; set; }
        public string LocationId { get; set; }
        public string DepartmentId { get; set; }
        public string PunchClockId { get; set; }
        public DateTime _clientDateTime { get; set; }
        public string ClientDateTime { get; set; }
    }
    public class MobileDataOut
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string ClientName { get; set; }
        public string DepartmentName { get; set; }
        public string Message { get; set; }
        public int Success { get; set; }
        public string Day { get; set; }
    }
    public class PunchClockData
    {
        static long tickCount = new DateTime(1970, 1, 1, 0, 0, 0).Ticks;
        public static DateTime ClockTicksToTime(long ticks)
        {
            DateTime dt = new DateTime((ticks * 10000) + tickCount);
            return dt;
        }
        public string DeviceKey { get; set; }
        public string deviceKey
        {
            get { return DeviceKey; }
            set { DeviceKey = value; }
        }
        public Data Data { get; set; }
        public Data data
        {
            get { return Data; }
            set { Data = value; }
        }
        public int Result { get; set; }
        public int result
        {
            get { return Result; }
            set { Result = value; }
        }
        public bool Success { get; set; }
        public bool success
        {
            get { return Success; }
            set { Success = value; }
        }
    }
    public class Data
    {
        public PageInfo PageInfo { get; set; }
        public PageInfo pageInfo
        {
            get { return PageInfo; }
            set { PageInfo = value; }
        }

        public List<Record> Records { get; set; }
        public List<Record> records
        {
            get { return Records; }
            set { Records = value; }
        }
    }
    public class PageInfo
    {
        public int Index { get; set; }
        public int index
        {
            get { return Index; }
            set { Index = value; }
        }
        public int Length { get; set; }
        public int length
        {
            get { return Length; }
            set { Length = value; }
        }
        public int Size { get; set; }
        public int size
        {
            get { return Size; }
            set { Size = value; }
        }
        public int Total { get; set; }
        public int total
        {
            get { return Total; }
            set { Total = value; }
        }
    }
    public class Record
    {
        public int Id { get; set; }
        public int id
        {
            get { return Id; }
            set { Id = value; }
        }
        public bool IsImgDeleted { get; set; }
        public bool isImgDeleted
        {
            get { return IsImgDeleted; }
            set { IsImgDeleted = value; }
        }
        public bool IsImgUpload { get; set; }
        public bool isImgUpload
        {
            get { return IsImgUpload; }
            set { IsImgUpload = value; }
        }
        public string Path { get; set; }
        public string path
        {
            get { return Path; }
            set { Path = value; }
        }
        public string PersonId { get; set; }
        public string personId
        {
            get { return PersonId; }
            set { PersonId = value; }
        }
        public int State { get; set; }
        public int state
        {
            get { return State; }
            set { State = value; }
        }
        public long Time { get; set; }
        public long time
        {
            get { return Time; }
            set { Time = value; }
        }
        public int Type { get; set; }
        public int type
        {
            get { return Type; }
            set { Type = value; }
        }
        public DateTime PunchDt { get; set; }
    }
    public class PunchClockResponse : Response
    {
        public long LastPunchDt { get; set; }
    }
    public class Response
    {
        public string Msg { get; set; }
        public string ErrorMsg { get; set; }
        public string DeviceKey { get; set; }
        public bool Success { get; set; }
    }
}