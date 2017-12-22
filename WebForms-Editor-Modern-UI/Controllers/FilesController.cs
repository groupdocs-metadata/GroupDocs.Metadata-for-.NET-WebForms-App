using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using Metadata_Editor_Modren_UI.Helpers;
using System.IO;

namespace Metadata_Editor_Modren_UI
{
    public class FilesController : ApiController
    {
        public JsonResult<List<string>> Get()
        {
            List<string> tree = new List<string>();
            try
            {
                tree = Directory.GetFiles(Utils._storagePath).Select(s => new FileInfo(s).Name).OrderBy(f => f).ToList<string>();

                if (tree.Contains("README.txt"))
                {
                    tree.Remove("README.txt");
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Json(tree);
        }
    }
}