using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DepartmentMapping
/// </summary>
/// 
public class MapInfo
{
    public int ShiftID;
    public int ShiftType;
    public int MSIDeptID;
    public int TWMapID;
    public int ShiftMappingDBIdx;
    public string Desc;
}

public class DepartmentMapping
{
    public double OTRate;
    public double RegRate;
    public double OTRate2;
    public double RegRate2;
    public double BonusRate;
    public double VacRate;
    public double OtherRate;
    public double PassThruRate;

    public int ShiftMappingID;
    public int ClientID;
    public List<MapInfo> MapList;

	public DepartmentMapping()
	{
		//
		// TODO: Add constructor logic here
		//
        MapList = new List<MapInfo>();
	}
}