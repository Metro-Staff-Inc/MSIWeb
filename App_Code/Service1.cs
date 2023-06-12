using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

/// <summary>
/// Summary description for Service1
/// </summary>

[ServiceContract]
[AspNetCompatibilityRequirements(RequirementsMode=AspNetCompatibilityRequirementsMode.Allowed)]
[ServiceBehavior(InstanceContextMode=InstanceContextMode.PerCall)]
public class TimeService
{
	public TimeService()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    [WebGet(UriTemplate = "CurrentTime")]
    public string CurrentTime()
    {
        return DateTime.Now.ToString();
    }
}