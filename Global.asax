<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        RegisterRoutes();
        log4net.Config.XmlConfigurator.Configure();

        MSI.Web.MSINet.Common.EventItem[] evList = new MSI.Web.MSINet.Common.EventItem[2];
        evList[0] = new MSI.Web.MSINet.Common.EventItem(7,30,0,MSI.Web.MSINet.Common.HelperFunctions.sendDailyDriverData);
        evList[1] = new MSI.Web.MSINet.Common.EventItem(9,50,0,MSI.Web.MSINet.Common.HelperFunctions.sendDailyDriverData);
        //evList[2] = new MSI.Web.MSINet.Common.EventItem(22,24,0,MSI.Web.MSINet.Common.HelperFunctions.sendDailyDriverData);
        MSI.Web.MSINet.Common.HelperFunctions.initDailyTimer(evList);
    }

    private void RegisterRoutes()
    {
        // Edit the base address of Service1 by replacing the "Service1" string below
        System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Roster",
            new System.ServiceModel.Activation.WebServiceHostFactory(), typeof(RosterWebServices.RosterWS)));
        System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Client",
            new System.ServiceModel.Activation.WebServiceHostFactory(), typeof(ClientWebServices.ClientWS)));
        System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Open",
            new System.ServiceModel.Activation.WebServiceHostFactory(), typeof(OpenWebServices.OpenWS)));
        System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Clock",
            new System.ServiceModel.Activation.WebServiceHostFactory(), typeof(ClockWebServices.ClockWS)));
        System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Api",
            new System.ServiceModel.Activation.WebServiceHostFactory(), typeof(ApiWebServices.ApiWS)));
    }

    private DateTime m_StartTime;

    protected void Application_BeginRequest(object sender, EventArgs e)
    {
        HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
        HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
        HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Authorization");
        HttpContext.Current.Response.AddHeader("Access-Control-Allow-Credentials", "true");
        if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
        {
            HttpContext.Current.Response.End();
        }
        try
        {
            m_StartTime = DateTime.Now;
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();

            EnableCrossDomainAjaxCall();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
    protected void Application_EndRequest(object sender, EventArgs e)
    {
        MSIToolkit.Logging.ActivityLogger.LogWebRequest(Request, m_StartTime, DateTime.Now);
    }

    private void EnableCrossDomainAjaxCall()
    {
        HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin",
                      "http://localhost:55248");
        //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin",
        //              "http://msiwebtrax.com");

        if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods",
                          "GET, POST");
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers",
                          "Content-Type, Accept");
            HttpContext.Current.Response.AddHeader("Access-Control-Max-Age",
                          "1000");
            HttpContext.Current.Response.End();
        }
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown
    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started
    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
    }

</script>
