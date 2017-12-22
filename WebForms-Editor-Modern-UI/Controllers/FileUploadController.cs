using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using Metadata_Editor_Modren_UI.Helpers;
using System.Web;
using System.Net.Http;

namespace Metadata_Editor_Modren_UI
{
    public class FileUploadController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Get()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = Utils._storagePath + "\\" + postedFile.FileName;
                    postedFile.SaveAs(filePath);
                }
            }
            return response;
        }
    }
}