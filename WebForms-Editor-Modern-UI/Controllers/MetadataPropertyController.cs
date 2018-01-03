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
                FileStream original = File.OpenRead(Utils._storagePath + "\\" + file);
                FileFormatChecker fileFormatChecker = new FileFormatChecker(original);

                DocumentType documentType = fileFormatChecker.GetDocumentType();
                List<PropertyItem> values = new List<PropertyItem>();

                if (fileFormatChecker.VerifyFormat(documentType))
                {
                    switch (documentType)
                    {
                        case DocumentType.Doc:

                            DocFormat docFormat = new DocFormat(original);
                            values = AppendMetadata(docFormat.GetMetadata(), values);

                            break;

                        case DocumentType.Xls:

                            XlsFormat xlsFormat = new XlsFormat(original);
                            values = AppendMetadata(xlsFormat.GetMetadata(), values);

                            break;

                        case DocumentType.Pdf:

                            PdfFormat pdfFormat = new PdfFormat(original);
                            values = AppendMetadata(pdfFormat.GetMetadata(), values);
                            values = AppendXMPData(pdfFormat.GetXmpData(), values);

                            break;

                        case DocumentType.Png:

                            PngFormat pngFormat = new PngFormat(original);
                            values = AppendMetadata(pngFormat.GetMetadata(), values);
                            values = AppendXMPData(pngFormat.GetXmpData(), values);

                            break;

                        case DocumentType.Jpeg:

                            JpegFormat jpegFormat = new JpegFormat(original);
                            values = AppendMetadata(jpegFormat.GetMetadata(), values);
                            values = AppendXMPData(jpegFormat.GetXmpData(), values);

                            break;

                        case DocumentType.Gif:

                            GifFormat gifFormat = new GifFormat(original);
                            values = AppendMetadata(gifFormat.GetMetadata(), values);
                            values = AppendXMPData(gifFormat.GetXmpData(), values);

                            break;

                        case DocumentType.Bmp:

                            BmpFormat bmpFormat = new BmpFormat(original);
                            values = AppendMetadata(bmpFormat.GetMetadata(), values);

                            break;

                        default:

                            DocFormat defaultDocFormat = new DocFormat(original);
                            values = AppendMetadata(defaultDocFormat.GetMetadata(), values);

                            break;
                    }

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
                PropertyItem propertyItem;

                foreach (XmpPackage package in xmpMetadata.Packages)
                {
                    foreach (KeyValuePair<string, XmpValueBase> pair in package)
                    {
                        XmpArray xmpArray = pair.Value as XmpArray;
                        LangAlt langAlt = pair.Value as LangAlt;
                        XmpComplexType xmpComplex = pair.Value as XmpComplexType;

                        if (xmpArray != null)
                        {
                            propertyItem = new PropertyItem(pair.Key, pair.Key, false);
                            if (!values.Contains(propertyItem))
                            {
                                values.Add(propertyItem);
                            }

                            foreach (string arrayItem in xmpArray.Values)
                            {
                                propertyItem = new PropertyItem(arrayItem, arrayItem, false);
                                if (!values.Contains(propertyItem))
                                {
                                    values.Add(propertyItem);
                                }
                            }
                        }
                        else if (langAlt != null)
                        {
                            propertyItem = new PropertyItem(pair.Key + langAlt.ToString(), langAlt.ToString(), false);
                            if (!values.Contains(propertyItem))
                            {
                                values.Add(propertyItem);
                            }
                        }
                        else
                        {
                            propertyItem = new PropertyItem(pair.Key, pair.Value.ToString(), false);
                            if (!values.Contains(propertyItem))
                            {
                                values.Add(propertyItem);
                            }
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