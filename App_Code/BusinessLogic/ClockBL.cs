using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using MSI.Web.MSINet.Common;

/// <summary>
/// Summary description for ClockBL
/// </summary>
namespace ClockWebServices
{
    public class ClockBL
    {
        public ClockBL()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public ClientRosterLastUpdate ClientRosterLastUpdate(int clientId, int locationId)
        {
            ClockDB clockDB = new ClockDB();
            return clockDB.ClientRosterLastUpdate(clientId, locationId);
        }
        public ClientRosterLastUpdate EmployeeList(int clientId, DateTime createdDate)
        {
            ClockDB clockDB = new ClockDB();
            return clockDB.EmployeeList(clientId, createdDate);
        }
        public string SetICCardAident(string aident, string icCardNum)
        {
            ClockDB clockDB = new ClockDB();
            string msg = clockDB.SetICCardAident(aident, icCardNum);
            return msg;
        }

        internal void SaveB64AsJpg(TextFile tf)
        {
            byte[] bytes = Convert.FromBase64String(tf.Content);

            Image image = null;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
                image.Save(tf.Path + "\\" + tf.FileName + "." + tf.Extension, ImageFormat.Jpeg);
            }
        }

        internal TextFile SaveTextFile(string aident, string content)
        {
            string suffix = "";
            if( aident.Contains("_"))
            {
                suffix = aident.Substring(aident.IndexOf("_"));
                aident = aident.Substring(0, aident.IndexOf("_"));
            }

            TextFile tf = new TextFile();
            tf.Path = GetEmployeeIDPath(aident);
            tf.Content = content;

            if (!Directory.Exists(tf.Path))
            {
                Directory.CreateDirectory(tf.Path);
            }

            tf.FileName = aident + suffix;
            tf.Extension = "b64";
            try
            {
                File.WriteAllText(tf.Path + "\\" + tf.FileName + "." + tf.Extension, content);
                tf.Message = "File: " + tf.FileName + " saved as type " + tf.Extension;
            }
            catch(Exception ex)
            {
                tf.Message = ex.ToString();
            }
            return tf;
        }


        internal TextFile GetTextFile(string aident)
        {
            TextFile tf = new TextFile();
            tf.Path = GetEmployeeIDPath(aident);

            try
            {
                tf.FileName = aident;
                tf.Extension = "b64";
                tf.Content = File.ReadAllText(tf.Path + "\\" + tf.FileName + "." + tf.Extension);
                tf.Message = "File Read Successfully";
            }
            catch (Exception ex)
            {
                tf.Message = ex.ToString();
            }
            return tf;
        }
        internal string GetEmployeeIDPath(string aident)
        {
            int idVal = Convert.ToInt32(aident);
            idVal /= 100;
            string dirPath = "/";
            for (int i = 0; i < 4; i++)
            {
                dirPath = "/" + (idVal % 10) + dirPath;
                idVal /= 10;
            }
            return HttpContext.Current.Server.MapPath("..\\" + "EmployeeImages" + dirPath);
        }
    }
}