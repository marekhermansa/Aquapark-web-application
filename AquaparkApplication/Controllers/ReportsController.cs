using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AquaparkApplication.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using AquaparkApplication.Models;


namespace AquaparkApplication.Controllers
{
    public class ReportsController : Controller
    {
        private AquaparkDbContext _dbContext;

        public ReportsController(AquaparkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        static string UppercaseWords(string value)
        {
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }

        [AcceptVerbs("GET")]
        [ActionName("GetCurrentOccupancy")]
        public PdfReportDto GetCurrentOccupancy()
        {
            //https://documentation.image-charts.com/

            try
            {
                // get info on occupancy per zone

                var zoneNames = _dbContext.Zones.ToList();

                var occupiedZones = _dbContext.ZoneHistories.Where(z => z.FinishTime == null);
                var t = occupiedZones
                    .GroupBy(o => o.Zone.Id)
                    .Select(group => new
                    {
                        Zone = group.FirstOrDefault().Zone.Id,
                        Count = group.Count()
                    }).ToList();

                var zones = t.Select(x => UppercaseWords(zoneNames.SingleOrDefault(z => z.Id == x.Zone).Name)).ToList();
                var counts = t.Select(x => x.Count);
                var sum = counts.Sum();
                var parts = counts.Select(x => (100.0 * x / sum).ToString("0.0")).ToList();

                var zoneStr = String.Format("{0}", string.Join("|", zones));
                var countStr = String.Format("{0}", string.Join(",", parts));
                var url = String.Format("https://chart.googleapis.com/chart?cht=p3&chs=450x150&chd=t:{0}&chl={1}", countStr, zoneStr);

                var detailList = t.Select(x => String.Format("{0}: {3}% ({1} {2})",
                    UppercaseWords(zoneNames.SingleOrDefault(z => z.Id == x.Zone).Name),
                    x.Count,
                    x.Count == 1 ? "osoba" : (x.Count > 4 ? "osób" : "osoby"),
                    (100.0 * x.Count / sum).ToString("0.0")));

                // render image

                byte[] fileBytes = null;

                WebClient client = new WebClient();
                fileBytes = client.DownloadData(url);
                MemoryStream theMemStream = new MemoryStream();

                theMemStream.Write(fileBytes, 0, fileBytes.Length);
                System.Drawing.Image img1 = System.Drawing.Image.FromStream(theMemStream);
                var filename = "test.jpg";
                img1.Save("wwwroot\\" + filename);

                // save to PDF

                IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();

                var template1 = String.Format("<!DOCTYPE html><html><head><title>{0}</title></head><body>", "Raport z obłożenia stref");
                var template2 = "</body></html>";
                var title = "<h1>Raport z obłożenia stref</h1>";
                var subtitle = String.Format("<p>Czas wygenerowania: {0}</p>", DateTime.Now.ToString());
                var header1 = "<h2>Obłożenie Stref</h2>";
                var imgStr1 = String.Format("<center><img src='{0}' align=\"middle\" width=\"90%\"></center>", filename);
                var details = String.Format("<p style=\"font-size:150%;\">{0}</p>", string.Join("<br>", detailList));

                var htmls = new List<String>() { template1, title, subtitle, header1, imgStr1, details, template2 };
                var html = String.Format("{0}", string.Join("<br>", htmls));

                var PDF = Renderer.RenderHtmlAsPdf(html, Environment.CurrentDirectory + "\\wwwroot\\");
                PDF.SaveAs("currentOccupancy.pdf");
                string pdfInBase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes("currentOccupancy.pdf"));


                //var PDF = Renderer.RenderHtmlAsPdf(String.Format("<img src='{0}'>",filename), Environment.CurrentDirectory + "\\wwwroot\\");
                //PDF.SaveAs("currentOccupancy.pdf");
                //string pdfInBase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes("currentOccupancy.pdf"));

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
                    Status = e.ToString(),
                    PdfData = null
                };
            }
        }

        [AcceptVerbs("GET")]
        [ActionName("GetSelectedOccupancy")]
        public PdfReportDto GetSelectedOccupancy(TimespanDto timespanDto)
        {
            //https://quickchart.io/

            try
            {
                // Orders
                var dbOrders = _dbContext.Orders.Where(o => o.DateOfOrder > timespanDto.StartTimeDate
                    && o.DateOfOrder < timespanDto.FinishTimeDate).Include(o => o.Positions).ToList();

                // Positions
                var positionLists = dbOrders.Select(o => o.Positions);
                var positionList = new List<Position>();
                positionLists.ToList().ForEach(p=>p.ToList().ForEach(pp=> positionList.Add(pp)));
                positionList.GroupBy(p => p.Id).Select(p => p.First()); // remove duplicated positions
                var positions = _dbContext.Positions
                    .Where(p=>positionList.Any(pl=>pl.Id==p.Id))
                    .Include(p=>p.PeriodicDiscount)
                    .Include(p=>p.SocialClassDiscount)
                    .Include(p => p.Ticket)
                    .Include(p => p.ZoneHistories).ToList();

                var ticketList = positions.Select(p=>p.Ticket).ToList();
                ticketList.RemoveAll(x => x == null);
                var dbTickets = _dbContext.Tickets
                    .Where(x => ticketList.Any(tl => tl.Id == x.Id))
                    .Include(x => x.Zone).ToList();

                var ticketCountRef = ticketList
                    .GroupBy(o => o.Id)
                    .Select(group => new
                    {
                        Ticket = group.FirstOrDefault().Id,
                        Count = group.Count()
                    }).ToList();

                // 1st chart

                var ticketCount = dbTickets
                    .GroupBy(o => o.Zone)
                    .Select(group => new
                    {
                        Zone = group.FirstOrDefault().Zone.Id,
                        Count = ticketCountRef.SingleOrDefault(x=>x.Ticket==group.FirstOrDefault().Id).Count
                    }).ToList();

                var zoneNames = _dbContext.Zones.ToList();
                var zones = ticketCount
                    .Select(x => UppercaseWords(zoneNames
                    .SingleOrDefault(z => z.Id == x.Zone).Name)).ToList();
                var counts = ticketCount.Select(x => x.Count).ToList();

                var sum = counts.Sum();
                var parts = counts.Select(x => (100.0 * x / sum).ToString("0.0")).ToList();

                var zoneStr = String.Format("'{0}'", string.Join("','", zones));
                var countStr = String.Format("{0}", string.Join(",", counts));
                var url1 = "https://quickchart.io/chart?width=500&height=300&c={type:'bar',data:{labels:["+ zoneStr + "],datasets:[{label:'Zones',data:["+ countStr + "]}]}}";

                var detailList = ticketCount.Select(x => String.Format("{0}: {1} {2} ({3}%)",
                    UppercaseWords(zoneNames.SingleOrDefault(z => z.Id == x.Zone).Name),
                    x.Count,
                    x.Count == 1 ? "bilet" : (x.Count > 4 ? "biletów" : "bilety"),
                    (100.0 * x.Count / sum).ToString("0.0")));

                // 2nd chart

                var ticketTypeCount = dbTickets
                    .GroupBy(o => o.Name)
                    .Select(group => new
                    {
                        Name = group.FirstOrDefault().Name,
                        Count = ticketCountRef.SingleOrDefault(x => x.Ticket == group.FirstOrDefault().Id).Count
                    }).ToList();
                var names = ticketTypeCount.Select(x => x.Name).ToList();
                counts = ticketTypeCount.Select(x => x.Count).ToList();

                sum = counts.Sum();
                parts = counts.Select(x => (100.0 * x / sum).ToString("0.0")).ToList();

                var nameStr = String.Format("'{0}'", string.Join("','", names));
                countStr = String.Format("{0}", string.Join(",", counts));
                //url = "https://quickchart.io/chart?width=500&height=300&c={type:'bar',data:{labels:[" + nameStr + "],datasets:[{label:'Tickets',data:[" + countStr + "]}]}}";
                var url2 = "https://quickchart.io/chart?c={type:'pie',data:{labels:[" + nameStr + "], datasets:[{data:[" + countStr + "]}]}}";

                var detailList2 = ticketTypeCount.Select(x => String.Format("{0}: {1} {2} ({3}%)",
                    x.Name,
                    x.Count,
                    x.Count == 1 ? "bilet" : (x.Count > 4 ? "biletów" : "bilety"),
                    (100.0 * x.Count / sum).ToString("0.0")));


                // render 1st image

                byte[] fileBytes = null;

                WebClient client = new WebClient();
                fileBytes = client.DownloadData(url1);
                MemoryStream theMemStream = new MemoryStream();

                theMemStream.Write(fileBytes, 0, fileBytes.Length);
                System.Drawing.Image img1 = System.Drawing.Image.FromStream(theMemStream);
                var filename1 = "chart1.jpg";
                img1.Save("wwwroot\\" + filename1);


                // render 2nd image

                fileBytes = null;

                client = new WebClient();
                fileBytes = client.DownloadData(url2);
                theMemStream = new MemoryStream();

                theMemStream.Write(fileBytes, 0, fileBytes.Length);
                System.Drawing.Image img2 = System.Drawing.Image.FromStream(theMemStream);
                var filename2 = "chart2.jpg";
                img2.Save("wwwroot\\" + filename2);


                // save to PDF

                IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();

                var templateHeader = String.Format("<!DOCTYPE html><html><head><title>{0}</title></head><body>", "Raport z liczby sprzedanych biletów");
                var templateFooter = "</body></html>";
                var title = String.Format("<header><h1>Raport z liczby sprzedanych biletów</h1><p>Dane z okresu od {0} do {1}</p></header>", timespanDto.StartTime, timespanDto.FinishTime);

                var header1 = "<h2>Sprzedane bilety w poszczególnych strefach</h2>";
                var imgStr1 = String.Format("<center><img src='{0}' align=\"middle\" width=\"500\" width=\"500\"></center>", filename1);
                var details = String.Format("<p style=\"font-size:120%;\">{0}</p>", string.Join("<br>", detailList));

                var header2 = "<h2>Rodzaje sprzedanych biletów</h2>";
                var imgStr2 = String.Format("<center><img src='{0}' align=\"middle\" width=\"500\" width=\"500\"></center>", filename2);
                var details2 = String.Format("<p style=\"font-size:120%;\">{0}</p>", string.Join("<br>", detailList2));

                var htmls = new List<String>()
                {
                    templateHeader,
                    title,
                    header1,
                    imgStr1,
                    details,
                    header2,
                    imgStr2,
                    details2,
                    templateFooter };
                var html = String.Format("{0}", string.Join("<br>", htmls));

                var PDF = Renderer.RenderHtmlAsPdf(html, Environment.CurrentDirectory + "\\wwwroot\\");
                PDF.SaveAs("currentOccupancy.pdf");
                string pdfInBase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes("currentOccupancy.pdf"));


                //var PDF = Renderer.RenderHtmlAsPdf(String.Format("<img src='{0}'>",filename), Environment.CurrentDirectory + "\\wwwroot\\");
                //PDF.SaveAs("currentOccupancy.pdf");
                //string pdfInBase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes("currentOccupancy.pdf"));

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
                    Status = e.ToString(),
                    PdfData = null
                };
            }
        }
    }
}