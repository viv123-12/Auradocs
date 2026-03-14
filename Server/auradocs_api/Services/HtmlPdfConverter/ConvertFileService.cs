using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;

public interface IConvertFileService
{
    public byte[] ConvertFile(string content);
}

public class ConvertHtmlToPdf: IConvertFileService
{
    public readonly IConverter _converter;
    public ConvertHtmlToPdf(IConverter converter)
    {
        _converter = converter;
    }
    public byte[] ConvertFile(string htmlContent)
    {
        HtmlToPdfDocument doc = new HtmlToPdfDocument()
        {
            GlobalSettings = {
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4
            },
            Objects = {
                new ObjectSettings() {
                    HtmlContent = CommonHelper.GetHtmlContentWrapper(htmlContent),
                    WebSettings = {
                        DefaultEncoding = "utf-8"
                    }
                }
            }
        };
        byte[] pdf = _converter.Convert(doc);
        return pdf;
    }
}