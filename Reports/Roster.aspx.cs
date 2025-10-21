using System;
using System.IO;
using System.Web;
using System.Web.UI;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Text;
using MSI.Web.MSINet.BusinessEntities;
using System.Collections.Generic;
using MSI.Web.MSINet.BusinessLogic;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MSI.Web.MSINet.Common;

public partial class Reports_Roster : System.Web.UI.Page
{
    Roster r = new Roster();
    List<Roster> employeeList = new List<Roster>();
    private int clientId;
    private string clientName;
    private string departmentName;
    private string shiftType;
    private int count;
    private DateTime date;

    protected void Page_Load(object sender, EventArgs e)
    {
        string str = Request.QueryString["clientId"];
        clientId = Convert.ToInt32(str);

        str = Request.QueryString["deptId"];
        int deptId = Convert.ToInt32(str);

        str = Request.QueryString["locId"];
        int locId = Convert.ToInt32(str);

        str = Request.QueryString["shiftType"];
        int shift = Convert.ToInt32(str);
        shiftType = HelperFunctions.shiftType(shift);

        str = Request.QueryString["startDate"];
        date = Convert.ToDateTime(str);

        ClientBL clientBL = new ClientBL();
        RosterInfo roster = clientBL.GetRostersWithLocation(clientId, date, locId, deptId, shift);
        if (roster.rosters.Count > 0)
        {
            clientName = roster.ClientName;
            departmentName = roster.DeptName;
            lblClient.InnerText = roster.ClientName;
            lblDate.InnerText = date.ToLongDateString();
        }

        rptrRoster.DataSource = roster.rosters;
        rptrRoster.DataBind();
        //CreatePDFDocument2(roster);
        
        // Add diagnostic logging for assembly loading
        try
        {
            // Try to explicitly load BouncyCastle to see which version is being used
            var bcAssembly = System.Reflection.Assembly.Load("BouncyCastle.Cryptography");
            string logPath = HttpContext.Current.Server.MapPath("~/App_Data/assembly_info.log");
            File.AppendAllText(logPath, DateTime.Now.ToString() + ": Loaded BouncyCastle.Cryptography: " +
                bcAssembly.FullName + Environment.NewLine);
        }
        catch (Exception ex)
        {
            string logPath = HttpContext.Current.Server.MapPath("~/App_Data/assembly_info.log");
            File.AppendAllText(logPath, DateTime.Now.ToString() + ": Error loading BouncyCastle.Cryptography: " +
                ex.Message + Environment.NewLine);
            
            // Try loading the older version
            try
            {
                var bcAssembly = System.Reflection.Assembly.Load("BouncyCastle.Crypto");
                File.AppendAllText(logPath, DateTime.Now.ToString() + ": Loaded BouncyCastle.Crypto: " +
                    bcAssembly.FullName + Environment.NewLine);
            }
            catch (Exception ex2)
            {
                File.AppendAllText(logPath, DateTime.Now.ToString() + ": Error loading BouncyCastle.Crypto: " +
                    ex2.Message + Environment.NewLine);
            }
        }
    }
    public void CreatePDFDocument2(RosterInfo roster)
    {
        string fileName = DateTime.Now.ToString("yyyyMMdd_hhmmss");
        string strFileName = HttpContext.Current.Server.MapPath(fileName);

        Document document = new Document();
        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(strFileName, FileMode.Create));

        document.Open();
        PdfPTable table = new PdfPTable(2);
        table.SetTotalWidth(new float[] { 160, 120 });
        table.LockedWidth = true;
        Rectangle small = new Rectangle(290, 100);
        Font smallfont = new Font(Font.FontFamily.HELVETICA, 10);
        PdfContentByte cb = writer.DirectContent;
        // first row
        PdfPCell cell = new PdfPCell(new Phrase("Some text here"));
        cell.FixedHeight = 30;
        cell.Border = Rectangle.BOTTOM_BORDER;
        cell.Colspan = 2;
        table.AddCell(cell);

        // second row
        cell = new PdfPCell(new Phrase("Some more text", smallfont));
        cell.FixedHeight = 30;
        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        cell.Border = Rectangle.BOTTOM_BORDER;
        table.AddCell(cell);
        Barcode128 code128 = new Barcode128();
        code128.Code = "14785236987541";
        code128.CodeType = Barcode128.CODE128;
        iTextSharp.text.Image code128Image = code128.CreateImageWithBarcode(cb, null, null);
        cell = new PdfPCell(code128Image, true);
        cell.Border = (Rectangle.BOTTOM_BORDER);
        cell.FixedHeight = (30);
        table.AddCell(cell);
        // third row
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase("and something else here", smallfont));
        cell.Border = (Rectangle.BOTTOM_BORDER);
        cell.HorizontalAlignment = (Element.ALIGN_RIGHT);
        table.AddCell(cell);
        document.Add(table);
        table = new PdfPTable(8);
        for (int aw = 0; aw < 16; aw++)
        {
            table.AddCell("hi");
        }
        document.Add(table);
        int newPage = 0;
        foreach( Roster r in roster.rosters )
        {
            document.Add(new Paragraph(r.ID + "\t" + r.LastName + "\t" + r.FirstName));
            newPage++;
            if( newPage % 16 == 0 )
            {
                document.NewPage();
            }
        }
        document.Close();
        ShowPdf(strFileName);

    }
    protected override void Render(HtmlTextWriter writer)
    {
        StringBuilder sb = new StringBuilder();
        StringWriter sw = new StringWriter(sb);
        HtmlTextWriter hWriter = new HtmlTextWriter(sw);

        RenderChildren(hWriter);
        string html = sb.ToString();

        CreatePDFDocument(html);
    }

    public void CreatePDFDocument(string strHtml)
    {
        try
        {
            string fileName = DateTime.Now.ToString("yyyyMMdd_hhmmss");
            string strFileName = HttpContext.Current.Server.MapPath(fileName);
            // step 1: creation of a document-object
            Document document = new Document();
            // step 2:
            // we create a writer that listens to the document
            PdfWriter.GetInstance(document, new FileStream(strFileName, FileMode.Create));
            StringReader se = new StringReader(strHtml);
            
            HTMLWorker obj = new HTMLWorker(document);
            document.Open();
            obj.Parse(se);
            document.Close();
            ShowPdf(strFileName);
        }
        catch (Exception ex)
        {
            // Log the full exception details including inner exceptions
            string errorMessage = "Error in CreatePDFDocument: " + ex.Message;
            if (ex.InnerException != null)
            {
                errorMessage += " Inner exception: " + ex.InnerException.Message;
                
                // Check specifically for BouncyCastle related errors
                if (ex.InnerException.Message.Contains("BouncyCastle"))
                {
                    errorMessage += " BouncyCastle error detected. Stack trace: " + ex.InnerException.StackTrace;
                }
            }
            
            // Log to a file for debugging
            string logPath = HttpContext.Current.Server.MapPath("~/App_Data/pdf_error.log");
            File.AppendAllText(logPath, DateTime.Now.ToString() + ": " + errorMessage + Environment.NewLine);
            
            // Display error to user
            Response.Write("Error generating PDF: " + errorMessage);
        }
    }

    public void ShowPdf(string strFileName)
    {
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Content-Disposition", "inline;filename=" + strFileName);
        Response.ContentType = "application/pdf";
        Response.WriteFile(strFileName);
        Response.Flush();
        Response.Clear();
    }

    protected void rptrRoster_DataBinding(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Header)
        {
            HtmlTableCell td;
            td = (HtmlTableCell)e.Item.FindControl("tdShiftType");
            td.InnerText = shiftType;
            td = (HtmlTableCell)e.Item.FindControl("tdDeptName");
            td.InnerText = departmentName;
        }
        if ( e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem )
        {
            HtmlTableCell td;
            HtmlTableRow tr;
            if( count == 0 )
            {
                tr = (HtmlTableRow)(e.Item.FindControl("trHeader"));
                tr.Visible = true;
                td = (HtmlTableCell)e.Item.FindControl("tdSuncastHeader");
                if (clientId != 8)
                    td.Visible = false;
            }
            count++;
            Roster r = (Roster)(e.Item.DataItem);
            td = (HtmlTableCell)e.Item.FindControl("tdEmpCnt");
            td.InnerText = count + ".";
            td = (HtmlTableCell)e.Item.FindControl("tdLastName");
            td.InnerText = r.LastName;
            td = (HtmlTableCell)e.Item.FindControl("tdFirstName");
            td.InnerText = r.FirstName;
            td = (HtmlTableCell)e.Item.FindControl("tdMsiId");
            td.InnerText = r.ID;
            td = (HtmlTableCell)e.Item.FindControl("tdSuncastId");
            if (clientId == 8)
                td.InnerText = r.SubID;
            else
                td.Visible = false;
        }
    }
}