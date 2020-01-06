using iText.Html2pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Itext7Example.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            string path = HttpContext.Server.MapPath("~/Docs/html/invoice.html");
            string html = System.IO.File.ReadAllText(path);
            //string html = PartialViewToString(this, "ViewInvoice", invoiceDisplayViewModel);
            html = html.Replace("<br>", "<br/>");
            string dirPath = HttpContext.Server.MapPath("~/Docs/Invoices/");
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            string[] files = Directory.GetFiles(dirPath);

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.LastAccessTime < DateTime.Now.AddMinutes(-10))
                    fi.Delete();
            }
            string pdfFileName = dirPath + DateTime.UtcNow.Ticks + ".pdf";

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(html);
            writer.Flush();
            stream.Position = 0;

            using (FileStream pdfDest = new FileStream(pdfFileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                ConverterProperties converterProperties = new ConverterProperties();
                HtmlConverter.ConvertToPdf(stream, pdfDest, converterProperties);
            }

            //return new FileStreamResult(ms, "application/pdf");
            return File(pdfFileName, "application/pdf");
        }
    }
}