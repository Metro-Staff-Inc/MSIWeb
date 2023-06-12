using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using System.IO;
using MSI.Web.Controls;

[ServiceContract]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class WebService
{
    private string filePath;
    

    public WebService()
    {
        filePath = HttpContext.Current.Server.MapPath("Images");
    }
	[OperationContract]
	public DateTime GetServerTime() /* test */
	{
		// Add your operation implementation here
		return DateTime.Now;
	}

	[OperationContract]
	public string SaveImage(string fileName, byte[] data)
	{
        string file = Path.Combine(filePath, Path.GetFileName(fileName));
        try
        {
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                fs.Write(data, 0, (int)data.Length);
            }
        }
        catch( Exception e)
        {
        }
        return data.Length + " bytes written to file - " + file.ToString(); 
        //return filePath + ", " + file + ", " + data.Length + ", " + DateTime.Now.ToString();
	}

	// Add more operations here and mark them with [OperationContract]
}
