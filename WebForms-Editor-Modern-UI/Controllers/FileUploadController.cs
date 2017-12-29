using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using Metadata_Editor_Modren_UI.Helpers;
using System.Web;
using System.Net.Http;
using GroupDocs.Metadata.Tools;
using GroupDocs.Metadata;

namespace Metadata_Editor_Modren_UI
{
    public class FileUploadController : ApiController
    {
        [HttpPost]
        public void Get()
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        var filePath = Utils._storagePath + "\\" + postedFile.FileName;
                        FileFormatChecker fileFormatChecker = new FileFormatChecker(postedFile.InputStream);
                        DocumentType documentType = fileFormatChecker.GetDocumentType();

                        if (fileFormatChecker.VerifyFormat(documentType))
                        {
                            postedFile.SaveAs(filePath);
                        }
                        else
                        {
                            throw new Exception("File format not supported.");
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}