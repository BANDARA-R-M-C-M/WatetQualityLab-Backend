using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Project_v1.Models.DTOs.GeneralInventoryItems;
using Project_v1.Models.DTOs.SurgicalInventoryItems;
using Project_v1.Models.DTOs.WCReport;
using System.Globalization;

namespace Project_v1.Services.ReportService {
    public class ReportService : IReportService {

        public byte[] GenerateWaterQualityReport(FullReport wcreport) {
            using MemoryStream stream = new MemoryStream();
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(stream));
            Document document = new Document(pdfDoc);

            Paragraph labHeader = new Paragraph($"{wcreport.LabName}")
              .SetTextAlignment(TextAlignment.CENTER)
              .SetFontSize(20)
              .SetBold();
            document.Add(labHeader);

            Paragraph address = new Paragraph($"{wcreport.LabLocation}")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(18)
                .SetBold();
            document.Add(address);

            Paragraph telephone = new Paragraph("T:P " + $"{wcreport.LabTelephone}")
              .SetTextAlignment(TextAlignment.CENTER)
              .SetFontSize(15);
            document.Add(telephone);

            Paragraph header = new Paragraph(
                "Date                :   " + $"{wcreport.DateOfCollection:dd/MM/yyyy}\n" +
                "My Ref No       :   " + $"{wcreport.YourRefNo}\n" +
                "Your Ref No    :   " + $"{wcreport.MyRefNo}"
               )
              .SetFontSize(10)
              .SetRelativePosition(320, 8, 0, 0);
            document.Add(header);

            Paragraph senderAddress = new Paragraph("Phi,\nNavy Camp,\nBlank")
                .SetFontSize(10)
                .SetBold()
                .SetRelativePosition(0, 0, 0, 0)
                .SetPaddingTop(35)
                .SetPaddingBottom(10);
            document.Add(senderAddress);

            Paragraph infoTitle = new Paragraph("BACTERIOLOGICAL EXAMINATION OF WATER")
              .SetTextAlignment(TextAlignment.CENTER)
              .SetFontSize(12)
              .SetBold()
              .SetUnderline()
              .SetPaddingBottom(25);
            document.Add(infoTitle);

            Paragraph details = new Paragraph(
                "Source of Sample                   :  " + $"{wcreport.CollectingSource}\n" +
                "Date of Collection                   :  " + $"{wcreport.DateOfCollection:dd/MM/yyyy}\n" +
                "Date of Processing                 :  " + $"{wcreport.IssuedDate:dd/MM/yyyy}\n" +
                "Appearance of Sample           :  " + $"{wcreport.AppearanceOfSample}\n" +
                "Chlorinated                             :  " + $"{wcreport.StateOfChlorination}"
               )
              .SetFontSize(12)
              .SetPaddingBottom(25);
            document.Add(details);

            // Main Table
            Table table = new Table(3);
            table.SetWidth(UnitValue.CreatePercentValue(100));

            Cell parameterHeader = new Cell().Add(new Paragraph("Parameter")
                .SetBold())
                .SetTextAlignment(TextAlignment.CENTER);
            table.AddHeaderCell(parameterHeader);

            Cell limitHeader = new Cell().Add(new Paragraph("Limit")
                .SetBold())
                .SetTextAlignment(TextAlignment.CENTER);
            table.AddHeaderCell(limitHeader);

            Cell resultsHeader = new Cell().Add(new Paragraph("Results")
                .SetBold())
                .SetTextAlignment(TextAlignment.CENTER);
            table.AddHeaderCell(resultsHeader);


            Cell presumptiveColiformCell = new Cell().Add(new Paragraph("Presumptive Coliform Count")
                .SetTextAlignment(TextAlignment.LEFT));
            table.AddCell(presumptiveColiformCell);

            Cell presumptiveColiformLimitCell = new Cell().Add(new Paragraph("Less Than 10/100ml"))
                .SetTextAlignment(TextAlignment.CENTER);
            table.AddCell(presumptiveColiformLimitCell);

            Cell presumptiveColiformResultsCell = new Cell().Add(new Paragraph(wcreport.PresumptiveColiformCount.ToString() + "/100ml"))
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(11);
            table.AddCell(presumptiveColiformResultsCell);

            Cell eColiCell = new Cell().Add(new Paragraph("Escherichia Coli Count"))
                .SetTextAlignment(TextAlignment.LEFT);
            table.AddCell(eColiCell);

            Cell eColiLimitCell = new Cell().Add(new Paragraph("Should not be detected/100ml"))
                .SetTextAlignment(TextAlignment.CENTER);
            table.AddCell(eColiLimitCell);

            Cell eColiResultsCell = new Cell().Add(new Paragraph(wcreport.EcoliCount.ToString() + "/100ml"))
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(11);
            table.AddCell(eColiResultsCell);

            document.Add(table);

            Paragraph comment = new Paragraph($"Comment :-           {wcreport.Results}")
                .SetPaddingTop(25);
            document.Add(comment);

            document.Close();

            return stream.ToArray();
        }

        public byte[] GenerateSampleCountReport(List<SampleCount> sampleCounts, int year) {
            using MemoryStream stream = new MemoryStream();
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(stream));
            Document document = new Document(pdfDoc);

            pdfDoc.SetDefaultPageSize(PageSize.A4.Rotate());

            Paragraph header = new Paragraph($"Water Quality Testing Laboratory - {year}")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(20)
                .SetBold();
            document.Add(header);

            document.Add(new Paragraph("\n"));

            Table table = new Table(13);
            table.SetWidth(UnitValue.CreatePercentValue(100));

            table.AddHeaderCell(new Cell().Add(new Paragraph("MOH Area").SetBold()));
            for (int month = 1; month <= 12; month++) {
                table.AddHeaderCell(new Cell().Add(new Paragraph(new DateTime(year, month, 1).ToString("MMMM")).SetBold()));
            }

            table.SetSkipFirstHeader(false);
            table.SetSkipLastFooter(false);

            var groupedSamples = sampleCounts.GroupBy(s => s.MOHAreaName).OrderBy(g => g.Key);

            foreach (var group in groupedSamples) {
                table.AddCell(new Cell().Add(new Paragraph(group.Key)));

                for (int month = 1; month <= 12; month++) {
                    var sampleCount = group.FirstOrDefault(s => s.Month == month)?.TotalCount ?? 0;
                    table.AddCell(new Cell().Add(new Paragraph(sampleCount.ToString())));
                }
            }

            document.Add(table);

            document.Close();

            return stream.ToArray();
        }

        public byte[] GenerateInventoryDurationReport(List<GeneralInventoryReport> inventories, int year) {
            using MemoryStream stream = new MemoryStream();
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(stream));
            Document document = new Document(pdfDoc);

            pdfDoc.SetDefaultPageSize(PageSize.A4.Rotate());

            Paragraph header = new Paragraph($"Inventory Duration Report - {year}")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(20)
                .SetBold();
            document.Add(header);

            document.Add(new Paragraph("\n"));

            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 10, 10, 15, 20, 25, 20 }));
            table.SetWidth(UnitValue.CreatePercentValue(100));

            table.AddHeaderCell(new Cell().Add(new Paragraph("Category").SetBold()));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Item Name").SetBold()));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Issued Date").SetBold()));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Issued By").SetBold()));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Remarks").SetBold()));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Duration (days)").SetBold()));

            foreach (var inventory in inventories) {
                table.AddCell(new Cell().Add(new Paragraph(inventory.GeneralCategoryName)));
                table.AddCell(new Cell().Add(new Paragraph(inventory.ItemName)));
                table.AddCell(new Cell().Add(new Paragraph(inventory.IssuedDate.ToString("yyyy-MM-dd"))));
                table.AddCell(new Cell().Add(new Paragraph(inventory.IssuedBy)));
                table.AddCell(new Cell().Add(new Paragraph(inventory.Remarks)));
                table.AddCell(new Cell().Add(new Paragraph(GetDuration(inventory.Duration))));
            }

            document.Add(table);

            Paragraph footer = new Paragraph($"Generated on {DateTime.Today:yyyy-MM-dd}")
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetFontSize(12)
                .SetBold();
            document.Add(footer);

            document.Close();

            return stream.ToArray();

            static String GetDuration(int duration) {
                var years = duration / 365;
                var months = (duration - years * 365) / 30;
                var days = (duration - years * 365 - months * 30) % 30;
                return (years + " years " + months + " months " + days + " days").ToString();
            }
        }

        public byte[] GenerateItemIssuingReport(List<ItemIssuingReport> issuedItems, int year, int month) {
            using MemoryStream stream = new MemoryStream();
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(stream));
            Document document = new Document(pdfDoc);

            pdfDoc.SetDefaultPageSize(PageSize.A4.Rotate());

            Paragraph header = new Paragraph($"Item Issuing Report - {month}/{year}")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(20)
                .SetBold();
            document.Add(header);

            document.Add(new Paragraph("\n"));

            Table table = new Table(new float[] { 1, 2, 1, 1, 1, 1 });
            table.SetWidth(UnitValue.CreatePercentValue(100));

            table.AddHeaderCell(new Cell().Add(new Paragraph("Category").SetBold()));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Item Name").SetBold()));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Initial Quantity").SetBold()));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Issued Quantity").SetBold()));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Added Quantity").SetBold()));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Remaining Quantity").SetBold()));

            foreach (var item in issuedItems) {
                table.AddCell(new Cell().Add(new Paragraph(item.SurgicalCategory)));
                table.AddCell(new Cell().Add(new Paragraph(item.ItemName)));
                table.AddCell(new Cell().Add(new Paragraph(item.InitialQuantity.ToString())));
                table.AddCell(new Cell().Add(new Paragraph(item.IssuedInMonth.ToString())));
                table.AddCell(new Cell().Add(new Paragraph(item.AddedInMonth.ToString())));
                table.AddCell(new Cell().Add(new Paragraph(item.RemainingQuantity.ToString())));
            }

            document.Add(table);

            document.Close();

            return stream.ToArray();
        }
    }
}