using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using Metadata_Editor_Modren_UI.Helpers;
using System.IO;
using GroupDocs.Metadata;
using GroupDocs.Metadata.Tools;
using System.Data;
using GroupDocs.Metadata.Formats.Document;
using GroupDocs.Metadata.Formats.Image;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using GroupDocs.Metadata.Formats.Email;
using GroupDocs.Metadata.Formats.Cad;

namespace Metadata_Editor_Modren_UI
{
    public class ExportMetadataController : ApiController
    {
        public HttpResponseMessage Get(bool isExcel, string file)
        {
            try
            {
                FileStream original = File.Open(Utils._storagePath + "\\" + file, FileMode.OpenOrCreate);
                string outputPath = Utils._storagePath + "\\Metadata_" + Path.GetFileNameWithoutExtension(file) + (isExcel ? ".xlsx" : ".csv");

                if (isExcel)
                {
                    byte[] content = ExportFacade.ExportToExcel(original);
                    // write data to the file
                    File.WriteAllBytes(outputPath, content);
                }
                else
                {
                    byte[] content = ExportFacade.ExportToCsv(original);
                    // write data to the file
                    File.WriteAllBytes(outputPath, content);
                }

                using (var ms = new MemoryStream())
                {
                    original = File.OpenRead(outputPath);
                    original.CopyTo(ms);
                    var result = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(ms.ToArray())
                    };
                    result.Content.Headers.ContentDisposition =
                    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = "Metadata_" + Path.GetFileNameWithoutExtension(file) + (isExcel ? ".xlsx" : ".csv")
                    };
                    result.Content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    original.Close();
                    File.Delete(outputPath);
                    return result;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}