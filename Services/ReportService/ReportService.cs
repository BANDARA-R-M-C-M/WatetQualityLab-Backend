using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Project_v1.Models.DTOs.WCReport;

namespace Project_v1.Services.ReportService
{
    public class ReportService : IReportService {
        public byte[] GenerateWaterQualityReport(FullReport wcreport) {
            using MemoryStream stream = new MemoryStream();
            // Create a new PDF document
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(stream));

            // Create a document
            Document document = new Document(pdfDoc);

            // Header
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

            // Comments
            Paragraph comment = new Paragraph($"Comment :-           {wcreport.Results}")
                .SetPaddingTop(25);
            document.Add(comment);

            document.Close();

            return stream.ToArray();
        }
    }
}