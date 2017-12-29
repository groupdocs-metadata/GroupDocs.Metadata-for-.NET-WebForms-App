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
using GroupDocs.Metadata.Formats.Image;
using GroupDocs.Metadata.Xmp;
using GroupDocs.Metadata.Formats.Document;

namespace Metadata_Editor_Modren_UI
{
    public class MetadataPropertyController : ApiController
    {
        public JsonResult<List<PropertyItem>> Get(string file)
        {
            try
            {
                FileFormatChecker fileFormatChecker = new FileFormatChecker(Utils._storagePath + "\\" + file);
                DocumentType documentType = fileFormatChecker.GetDocumentType();
                List<PropertyItem> values = new List<PropertyItem>();

                if (fileFormatChecker.VerifyFormat(documentType))
                {
                    switch (documentType)
                    {
                        case DocumentType.Doc:

                            DocFormat docFormat = new DocFormat(Utils._storagePath + "\\" + file);
                            values = AppendMetadata(docFormat.GetMetadata(), values);

                            break;

                        case DocumentType.Xls:

                            XlsFormat xlsFormat = new XlsFormat(Utils._storagePath + "\\" + file);
                            values = AppendMetadata(xlsFormat.GetMetadata(), values);

                            break;

                        case DocumentType.Pdf:

                            PdfFormat pdfFormat = new PdfFormat(Utils._storagePath + "\\" + file);
                            values = AppendMetadata(pdfFormat.GetMetadata(), values);
                            values = AppendXMPData(pdfFormat.GetXmpData(), values);

                            break;

                        case DocumentType.Png:

                            PngFormat pngFormat = new PngFormat(Utils._storagePath + "\\" + file);
                            values = AppendMetadata(pngFormat.GetMetadata(), values);
                            values = AppendXMPData(pngFormat.GetXmpData(), values);

                            break;

                        case DocumentType.Jpeg:

                            JpegFormat jpegFormat = new JpegFormat(Utils._storagePath + "\\" + file);
                            values = AppendMetadata(jpegFormat.GetMetadata(), values);
                            values = AppendXMPData(jpegFormat.GetXmpData(), values);

                            break;

                        case DocumentType.Gif:

                            GifFormat gifFormat = new GifFormat(Utils._storagePath + "\\" + file);
                            values = AppendMetadata(gifFormat.GetMetadata(), values);
                            values = AppendXMPData(gifFormat.GetXmpData(), values);

                            break;

                        case DocumentType.Bmp:

                            BmpFormat bmpFormat = new BmpFormat(Utils._storagePath + "\\" + file);
                            values = AppendMetadata(bmpFormat.GetMetadata(), values);

                            break;

                        default:

                            DocFormat defaultDocFormat = new DocFormat(Utils._storagePath + "\\" + file);
                            values = AppendMetadata(defaultDocFormat.GetMetadata(), values);

                            break;
                    }

                    //DataSet ds = null;
                    //ds = ExportFacade.ExportToDataSet(Utils._storagePath + "\\" + file);

                    //if (ds.Tables.Count > 0)
                    //{
                    //    for (int i = 0; i < ds.Tables.Count; i++)
                    //    {
                    //        DataTable table = ds.Tables[i];

                    //        foreach (DataRow dr in table.Rows)
                    //        {
                    //            values.Add(new PropertyItem(dr[0].ToString(), dr[1].ToString(), false));
                    //        }
                    //    }
                    //}

                    return Json(values);
                }
                else
                {
                    throw new Exception("File format not supported.");
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        private List<PropertyItem> AppendMetadata(Metadata[] metadataArray, List<PropertyItem> values)
        {
            foreach (Metadata metadata in metadataArray)
            {
                foreach (MetadataProperty metadataPropert in metadata)
                {
                    if (metadataPropert.Value != null)
                    {
                        values.Add(new PropertyItem(metadataPropert.Name, metadataPropert.Value.ToString(), metadataPropert.IsBuiltInProperty));
                    }
                }
            }

            return values;
        }

        private List<PropertyItem> AppendXMPData(XmpPacketWrapper xmpMetadata, List<PropertyItem> values)
        {
            if (xmpMetadata != null)
            {
                foreach (XmpPackage package in xmpMetadata.Packages)
                {
                    foreach (KeyValuePair<string, XmpValueBase> pair in package)
                    {
                        XmpArray xmpArray = pair.Value as XmpArray;
                        LangAlt langAlt = pair.Value as LangAlt;
                        XmpComplexType xmpComplex = pair.Value as XmpComplexType;

                        if (xmpArray != null)
                        {
                            values.Add(new PropertyItem(pair.Key, pair.Key, false));
                            foreach (string arrayItem in xmpArray.Values)
                            {
                                values.Add(new PropertyItem(arrayItem, arrayItem, false));
                            }
                        }
                        else if (langAlt != null)
                        {
                            values.Add(new PropertyItem(pair.Key + langAlt.ToString(), langAlt.ToString(), false));
                        }
                        else
                        {
                            string xmpProperty = string.Format("{0} - {1}", pair.Key, pair.Value.ToString());
                            values.Add(new PropertyItem(pair.Key, xmpProperty, false));
                        }
                    }
                }
            }

            return values;
        }

        public class PropertyItem
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public bool IsBuiltIn { get; set; }
            //public bool IsChanged { get; set; }

            public PropertyItem(string name, string value, bool isBuiltIn)
            {
                this.Name = name;
                this.Value = value;
                this.IsBuiltIn = isBuiltIn;
            }
        }
    }
}