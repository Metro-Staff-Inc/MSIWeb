using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Employee
/// </summary>
/// 
namespace ApiWebServices_EmployeeInfo
{
     /*
    emp.lastName = "Murfey";
    emp.firstName = "Jonathan";
    emp.middleInitial = "H";
    emp.ssn = 5789;
    emp.street = "3637 N. Springfield Ave.";
    emp.city = "Chicago";
    emp.state = "IL";
    emp.email = "jmurfey@msistaff.com";
    emp.phone = "773 354-2056";
    emp.zip = "60618";
    emp.dateOfBirth = "09/19/1965";
    emp.success = true;
    return emp;
    */
    public class EmployeeInfo
    {
        public string Aident { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string SSN { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Zip { get; set; }
        public string Birthdate { get; set; }
        public EmployeeInfo()
        {
            //Console.WriteLine("Hello World!");
        }
    }
    public class EmployeeInfoResponse
    {
        public EmployeeInfoResponse()
        {
        }
        public EmployeeInfo Data { get; set; }
        public string Msg { get; set; }
        public bool Success { get; set; }
    }
}
 