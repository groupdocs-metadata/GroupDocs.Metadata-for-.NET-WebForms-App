﻿using System;
using System.Web.Http;
using System.Web.Routing;
using GroupDocs.Metadata;

namespace Metadata_Editor_Modren_UI
{
    public class Global : System.Web.HttpApplication
    {
        private static string _licensePath = "D:\\License\\GroupDocs.Total.lic";
        protected void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = System.Web.Http.RouteParameter.Optional }
                );

            License l = new License();
            if (System.IO.File.Exists(_licensePath))
            {
                try
                {
                    l.SetLicense(_licensePath);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}