using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ResourceGroup
/// </summary>
namespace MSI.Web.MSINet.BusinessEntities
{
    public class ResourceGroupHours
    {
        	//SELECT cw.aident_number, cw.cw_aident_number, cw.cw_resource_group_employee_id, cw.start_dt, 
			//	cw.end_dt, cw.cw_resource_group_id, cwg.name, cwg.description
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String CWAidentNumber { get; set; }
        public String AidentNumber { get; set; }
        public DateTime StartDt { get; set; }
        public DateTime EndDt { get; set; }
        public int ResourceGroupId { get; set; }
        public Double Hours { get; set; }

        public ResourceGroupHours()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
    public class ResourceGroupInfo
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public int ResourceGroupId { get; set; }

        public ResourceGroupInfo()
        {

        }
    }
}
