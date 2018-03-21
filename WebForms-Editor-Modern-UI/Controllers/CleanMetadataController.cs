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
    public class CleanMetadataController : ApiController
    {
        public HttpResponseMessage Get(string file)
        {
            try
            {
                File.Copy(Utils._storagePath + "\\" + file, Utils._storagePath + "\\Cleaned_" + file, true);
                FileStream original = File.Open(Utils._storagePath + "\\Cleaned_" + file, FileMode.Open, FileAccess.ReadWrite);
                FileFormatChecker fileFormatChecker = new FileFormatChecker(original);
                DocumentType documentType = fileFormatChecker.GetDocumentType();


                if (fileFormatChecker.VerifyFormat(documentType))
                {
                    switch (documentType)
                    {
                        case DocumentType.Doc:

                            DocFormat docFormat = new DocFormat(original);
                            docFormat.CleanMetadata();
                            docFormat.ClearBuiltInProperties();
                            docFormat.ClearComments();
                            docFormat.ClearCustomProperties();
                            docFormat.RemoveHiddenData(new DocInspectionOptions(DocInspectorOptionsEnum.All));

                            docFormat.Save(Utils._storagePath + "\\Cleaned_" + file);
                            break;

                        case DocumentType.Xls:

                            XlsFormat xlsFormat = new XlsFormat(original);
                            xlsFormat.CleanMetadata();
                            xlsFormat.ClearBuiltInProperties();
                            xlsFormat.ClearContentTypeProperties();
                            xlsFormat.ClearCustomProperties();
                            xlsFormat.RemoveHiddenData(new XlsInspectionOptions(XlsInspectorOptionsEnum.All));

                            xlsFormat.Save(Utils._storagePath + "\\Cleaned_" + file);

                            break;

                        case DocumentType.Pdf:

                            PdfFormat pdfFormat = new PdfFormat(original);
                            pdfFormat.CleanMetadata();
                            pdfFormat.ClearBuiltInProperties();
                            pdfFormat.ClearCustomProperties();
                            pdfFormat.RemoveHiddenData(new PdfInspectionOptions(PdfInspectorOptionsEnum.All));
                            pdfFormat.RemoveXmpData();

                            pdfFormat.Save(Utils._storagePath + "\\Cleaned_" + file);

                            break;

                        case DocumentType.Png:

                            PngFormat pngFormat = new PngFormat(original);
                            pngFormat.CleanMetadata();
                            pngFormat.RemoveXmpData();

                            pngFormat.Save(Utils._storagePath + "\\Cleaned_" + file);

                            break;

                        case DocumentType.Jpeg:

                            JpegFormat jpegFormat = new JpegFormat(original);
                            jpegFormat.CleanMetadata();
                            jpegFormat.RemoveExifInfo();
                            jpegFormat.RemoveGpsLocation();
                            jpegFormat.RemoveIptc();
                            jpegFormat.RemovePhotoshopData();
                            jpegFormat.RemoveXmpData();

                            jpegFormat.Save(original);

                            break;

                        case DocumentType.Bmp:

                            BmpFormat bmpFormat = new BmpFormat(original);
                            bmpFormat.CleanMetadata();

                            bmpFormat.Save(Utils._storagePath + "\\Cleaned_" + file);

                            break;

                        case DocumentType.Gif:

                            GifFormat gifFormat = new GifFormat(original);
                            gifFormat.CleanMetadata();
                            gifFormat.RemoveXmpData();

                            gifFormat.Save(Utils._storagePath + "\\Cleaned_" + file);

                            break;

                        case DocumentType.Msg:

                            OutlookMessage outlookMessage = new OutlookMessage(original);
                            outlookMessage.CleanMetadata();
                            outlookMessage.RemoveAttachments();

                            outlookMessage.Save(Utils._storagePath + "\\Cleaned_" + file);

                            break;

                        case DocumentType.Eml:

                            EmlFormat emlFormat = new EmlFormat(original);
                            emlFormat.CleanMetadata();
                            emlFormat.RemoveAttachments();

                            emlFormat.Save(Utils._storagePath + "\\Cleaned_" + file);

                            break;

                        case DocumentType.Dwg:

                            DwgFormat dwgFormat = new DwgFormat(original);
                            dwgFormat.CleanMetadata();

                            dwgFormat.Save(Utils._storagePath + "\\Cleaned_" + file);

                            break;

                        case DocumentType.Dxf:

                            DxfFormat dxfFormat = new DxfFormat(original);
                            dxfFormat.CleanMetadata();

                            dxfFormat.Save(Utils._storagePath + "\\Cleaned_" + file);

                            break;

                        default:

                            DocFormat defaultDocFormat = new DocFormat(original);
                            defaultDocFormat.CleanMetadata();
                            defaultDocFormat.ClearBuiltInProperties();
                            defaultDocFormat.ClearComments();
                            defaultDocFormat.ClearCustomProperties();
                            defaultDocFormat.RemoveHiddenData(new DocInspectionOptions(DocInspectorOptionsEnum.All));

                            defaultDocFormat.Save(Utils._storagePath + "\\Cleaned_" + file);

                            break;
                    }
                }
                else
                {
                    throw new Exception("File format not supported.");
                }

                using (var ms = new MemoryStream())
                {
                    original = File.OpenRead(Utils._storagePath + "\\Cleaned_" + file);
                    original.CopyTo(ms);
                    var result = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(ms.ToArray())
                    };
                    result.Content.Headers.ContentDisposition =
                    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = "Cleaned_" + file
                    };
                    result.Content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

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