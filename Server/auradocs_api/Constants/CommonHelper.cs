public class CommonHelper
{
    public static string GetDocumentState(int value)
    {
        string key = Enum.GetName(typeof(DocumentStatus), value);
        return key;
    }
    public static string GetHtmlContentWrapper(string htmlContent)
    {
        string wrappedHtmlContent = $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                    <meta charset='utf-8'>
                    </head>
                    <body>
                    {htmlContent}
                    </body>
                    </html>";
        return wrappedHtmlContent;
    }
}