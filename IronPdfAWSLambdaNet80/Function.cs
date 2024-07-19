using Amazon.Lambda.Core;
using IronPdf.Rendering;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace IronPdfAWSLambdaNet80;

public class Function
{

    /// <summary>
    /// A simple function that takes a string and returns both the upper and lower case version of the string.
    /// </summary>
    /// <param name="input">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public Casing GetPdf(string input, ILambdaContext context)
    {
        try
        {
            context.Logger.LogLine($"START FunctionHandler RequestId: {context.AwsRequestId} Input: {input}");
            var awsTmpPath = @"/tmp/"; // AWS temporary storage
                                       //[optional] enable logging (please uncomment these code if you face any problem)
                                       //IronPdf.Logging.Logger.EnableDebugging = true;
                                       //IronPdf.Logging.Logger.LogFilePath = awsTmpPath;
                                       //IronPdf.Logging.Logger.LoggingMode = IronPdf.Logging.Logger.LoggingModes.All;
                                       //set your license key
            IronPdf.License.LicenseKey = "YOUR-LICENSE-KEY";
            //set logging
            IronPdf.Logging.Logger.LoggingMode = IronPdf.Logging.Logger.LoggingModes.All;
            IronPdf.Logging.Logger.LogFilePath = $"{awsTmpPath}Default.log";
            //set ChromeGpuMode to Disabled
            IronPdf.Installation.ChromeGpuMode = IronPdf.Engines.Chrome.ChromeGpuModes.Disabled;
            //set IronPDF Temp Path
            Environment.SetEnvironmentVariable("TEMP", awsTmpPath, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("TMP", awsTmpPath, EnvironmentVariableTarget.Process);
            IronPdf.Installation.TempFolderPath = awsTmpPath;
            IronPdf.Installation.CustomDeploymentDirectory = awsTmpPath;
            //set auto LinuxAndDockerDependenciesAutoConfig
            IronPdf.Installation.LinuxAndDockerDependenciesAutoConfig = false;
            context.Logger.LogLine($"creating IronPdf.ChromePdfRenderer");
            var _renderer = new IronPdf.ChromePdfRenderer();
            context.Logger.LogLine($"rendering Pdf");
            _renderer.RenderingOptions.CssMediaType = PdfCssMediaType.Screen;
            _renderer.RenderingOptions.MarginTop = 10;
            _renderer.RenderingOptions.MarginLeft = 10;
            _renderer.RenderingOptions.MarginRight = 10;
            _renderer.RenderingOptions.MarginBottom = 10;
            _renderer.RenderingOptions.PaperSize = PdfPaperSize.A4;
            using var pdfDoc = _renderer.RenderHtmlAsPdf("<h1>Hello, world<</h1>");
            var guid = Guid.NewGuid();
            var fileName = $"/tmp/{input}_{guid}.pdf"; //save file to /tmp
            context.Logger.LogLine($"saving pdf name : {fileName}");
            pdfDoc.SaveAs(fileName);
            //here you can upload saved pdf file to anywhere such as S3. 
            context.Logger.LogLine($"COMPLETE!");
        }
        catch (Exception e)
        {
            context.Logger.LogLine($"[ERROR] FunctionHandler : {e.Message}");
        }
        return new Casing(input?.ToLower(), input?.ToUpper());
    }
}

public record Casing(string Lower, string Upper);