using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RoleInfo
/// </summary>
public class RoleInfo
{
	public RoleInfo()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public String DepartmentName { get; set; }
    public int ShiftType { get; set; }
    public String Id { get; set; }
    public String Role { get; set; }
    public String FirstName { get; set; }
    public String LastName { get; set; }
    public int Rating { get; set; }
    public Boolean Vip { get; set; }
    public Boolean Dnr { get; set; }
    public String DnrReason { get; set; }
    public int DayOff { get; set; }
    public String Notes { get; set; }
    public String CrossTrained { get; set; }
}