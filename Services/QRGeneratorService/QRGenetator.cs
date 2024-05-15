using Project_v1.Models;
using Project_v1.Models.DTOs.GeneralInventoryItems;
using QRCoder;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;


namespace Project_v1.Services.QRGeneratorService {
    public class QRGenetator : IQRGenerator {

        private readonly IConfiguration _configuration;

        public QRGenetator(IConfiguration configuration) {
            _configuration = configuration;
        }
        public byte[] GenerateGeneralInventoryQRCode(String categoryId, String itemId) {

            var url = $"{_configuration["BaseURL:Frontend"]}/mlt/inventory-general/{categoryId}/{itemId}";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.L);
            BitmapByteQRCode bitMap = new BitmapByteQRCode(qrCodeData);
            var QRCodeBytes = bitMap.GetGraphic(5);

            MemoryStream memoryStream = new MemoryStream(QRCodeBytes);

            using (var bitmap = new Bitmap(memoryStream))
            using (var pngStream = new MemoryStream()) {
                bitmap.Save(pngStream, ImageFormat.Png);
                return pngStream.ToArray();
            }
        }

        public byte[] GenerateSurgicalInventoryQRCode(string categoryId, string itemId) {

            var url = $"{_configuration["BaseURL:Frontend"]}/mlt/inventory-surgical/{categoryId}/{itemId}";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.L);
            BitmapByteQRCode bitMap = new BitmapByteQRCode(qrCodeData);
            var QRCodeBytes = bitMap.GetGraphic(5);

            MemoryStream memoryStream = new MemoryStream(QRCodeBytes);

            using (var bitmap = new Bitmap(memoryStream))
            using (var pngStream = new MemoryStream()) {
                bitmap.Save(pngStream, ImageFormat.Png);
                return pngStream.ToArray();
            }
        }
    }
}
