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

namespace Metadata_Editor_Modren_UI
{
    public class CleanMetadataController : ApiController
    {
        public void Get(string file)
        {
            try
            {
                FileFormatChecker fileFormatChecker = new FileFormatChecker(Utils._storagePath + "\\" + file);
                DocumentType documentType = fileFormatChecker.GetDocumentType();

                if (fileFormatChecker.VerifyFormat(documentType))
                {
                    switch (documentType)
                    {
                        case DocumentType.Doc:

                            DocFormat docFormat = new DocFormat(Utils._storagePath + "\\" + file);
                            docFormat.CleanMetadata();
                            docFormat.ClearBuiltInProperties();
                            docFormat.ClearComments();
                            docFormat.ClearCustomProperties();
                            docFormat.RemoveHiddenData(new DocInspectionOptions(DocInspectorOptionsEnum.All));

                            break;

                        case DocumentType.Xls:

                            XlsFormat xlsFormat = new XlsFormat(Utils._storagePath + "\\" + file);
                            xlsFormat.CleanMetadata();
                            xlsFormat.ClearBuiltInProperties();
                            xlsFormat.ClearContentTypeProperties();
                            xlsFormat.ClearCustomProperties();
                            xlsFormat.RemoveHiddenData(new XlsInspectionOptions(XlsInspectorOptionsEnum.All));

                            break;

                        case DocumentType.Pdf:

                            PdfFormat pdfFormat = new PdfFormat(Utils._storagePath + "\\" + file);
                            pdfFormat.CleanMetadata();
                            pdfFormat.ClearBuiltInProperties();
                            pdfFormat.ClearCustomProperties();
                            pdfFormat.RemoveHiddenData(new PdfInspectionOptions(PdfInspectorOptionsEnum.All));
                            pdfFormat.RemoveXmpData();

                            break;

                        case DocumentType.Png:

                            PngFormat pngFormat = new PngFormat(Utils._storagePath + "\\" + file);
                            pngFormat.CleanMetadata();
                            pngFormat.RemoveXmpData();

                            break;

                        case DocumentType.Jpeg:

                            JpegFormat jpegFormat = new JpegFormat(Utils._storagePath + "\\" + file);
                            jpegFormat.CleanMetadata();
                            jpegFormat.RemoveExifInfo();
                            jpegFormat.RemoveGpsLocation();
                            jpegFormat.RemoveIptc();
                            jpegFormat.RemovePhotoshopData();
                            jpegFormat.RemoveXmpData();

                            break;

                        case DocumentType.Bmp:

                            BmpFormat bmpFormat = new BmpFormat(Utils._storagePath + "\\" + file);
                            bmpFormat.CleanMetadata();

                            break;

                        case DocumentType.Gif:

                            GifFormat gifFormat = new GifFormat(Utils._storagePath + "\\" + file);
                            gifFormat.CleanMetadata();
                            gifFormat.RemoveXmpData();

                            break;

                        default:

                            DocFormat defaultDocFormat = new DocFormat(Utils._storagePath + "\\" + file);
                            defaultDocFormat.CleanMetadata();
                            defaultDocFormat.ClearBuiltInProperties();
                            defaultDocFormat.ClearComments();
                            defaultDocFormat.ClearCustomProperties();
                            defaultDocFormat.RemoveHiddenData(new DocInspectionOptions(DocInspectorOptionsEnum.All));

                            break;
                    }
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
    }
}