using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AquaparkApplication.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AquaparkApplication.Controllers
{
    public class ReportsController : Controller
    {
        
        private AquaparkDbContext _dbContext;

        public ReportsController(AquaparkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AcceptVerbs("GET")]
        [ActionName("GetCurrentOccupancy")]
        public PdfReportDto GetCurrentOccupancy()
        {
            try
            {
                IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();
                Renderer.RenderHtmlAsPdf("<h1>Hello World</h1>").SaveAs("html-string.pdf");
                //var PDF = Renderer.RenderHtmlAsPdf("<h1>Korwin król</h1>");
                 var PDF = Renderer.RenderHtmlAsPdf("<img src='theonlyking.png'>", Environment.CurrentDirectory + "\\wwwroot\\");
                PDF.SaveAs("currentOccupancy.pdf");
                string pdfInBase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes("currentOccupancy.pdf"));

                return new PdfReportDto()
                {
                    Success = true,
                    Status = "",
                    PdfData = pdfInBase64
                };
            }
            catch (Exception e)
            {

                return new PdfReportDto()
                {
                    Success = false,
                    Status = e.Message,
                    PdfData = null
                };
            }
        }
    }
}