﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Metadata_Editor_Modren_UI.Helpers;

namespace Metadata_Editor_Modren_UI.Controllers
{
    public class DownloadOriginalController : ApiController
    {
        public HttpResponseMessage Get(string file)
        {
            FileStream original = null;
            try
            {
                original = File.Open(Utils._storagePath + "\\"+file, FileMode.Open);
            }
            catch (Exception x)
            {
                throw x;
            }

            using (var ms = new MemoryStream())
            {
                original.CopyTo(ms);
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(ms.ToArray())
                };
                result.Content.Headers.ContentDisposition =
                    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = file
                    };
                result.Content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");
                original.Close();
                return result;
            }
        }
    }
}