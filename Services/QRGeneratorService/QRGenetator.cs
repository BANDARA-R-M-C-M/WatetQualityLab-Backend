using Project_v1.Models;
using Project_v1.Models.DTOs.GeneralInventoryItems;
using QRCoder;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Kernel.Geom;


namespace Project_v1.Services.QRGeneratorService {
    public class QRGenetator : IQRGenerator {

        private readonly IConfiguration _configuration;

        public QRGenetator(IConfiguration configuration) {
            _configuration = configuration;
        }
        public byte[] GenerateGeneralInventoryQRCode(String categoryId, String itemId) {

            var url = $"{_configuration["BaseURL:Frontend"]}/mlt/inventory-general/{categoryId}/{itemId}";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.H);
            BitmapByteQRCode bitMap = new BitmapByteQRCode(qrCodeData);
            var QRCodeBytes = bitMap.GetGraphic(5);

            using var memoryStream = new MemoryStream();

            var image = Image.FromStream(new MemoryStream(QRCodeBytes));

            var width = image.Width;
            var height = image.Height;

            var pageSize = new PageSize(width, height);

            var pdfWriter = new PdfWriter(memoryStream);
            var pdfDocument = new PdfDocument(pdfWriter);
            var document = new Document(pdfDocument, pageSize);

            var imageData = new iText.Layout.Element.Image(iText.IO.Image.ImageDataFactory.Create(QRCodeBytes));

            document.Add(imageData);

            document.Close();

            return memoryStream.ToArray();
        }

        public byte[] GenerateSurgicalInventoryQRCode(string categoryId, string itemId) {

            var url = $"{_configuration["BaseURL:Frontend"]}/mlt/inventory-surgical/{categoryId}/{itemId}";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.H);
            BitmapByteQRCode bitMap = new BitmapByteQRCode(qrCodeData);
            var QRCodeBytes = bitMap.GetGraphic(5);

            using var memoryStream = new MemoryStream();

            var image = Image.FromStream(new MemoryStream(QRCodeBytes));

            var width = image.Width;
            var height = image.Height;

            var pageSize = new PageSize(width, height);

            var pdfWriter = new PdfWriter(memoryStream);
            var pdfDocument = new PdfDocument(pdfWriter);
            var document = new Document(pdfDocument, pageSize);

            var imageData = new iText.Layout.Element.Image(iText.IO.Image.ImageDataFactory.Create(QRCodeBytes));

            document.Add(imageData);

            document.Close();

            return memoryStream.ToArray();
        }
    }
}
