using Project_v1.Data;

namespace Project_v1.Services.IdGeneratorService {
    public class IdGenerator : IIdGenerator {

        public readonly ApplicationDBContext _context;

        public IdGenerator(ApplicationDBContext context) {
            _context = context;
        }
        public string GenerateSampleId() {
            /*string newSampleId;
            do {
                newSampleId = "SMP-" + Guid.NewGuid().ToString();
            } while (!(_context.Samples.Any(s => s.SampleId == newSampleId)));*/

            return ("SAMPLE-" + Guid.NewGuid().ToString());
        }     

        public string GenerateReportId() {
            /*string newReportId;
             *            do {
             *                           newReportId = "RPT-" + Guid.NewGuid().ToString();
             *                                      } while (!(_context.Reports.Any(r => r.ReportId == newReportId)));*/

            return ("REPORT-" + Guid.NewGuid().ToString());
        }

        public string GenerateLabId() {
            return ("LAB-" + Guid.NewGuid().ToString());
        }

        public string GeneratePHIAreaId() {
            return ("PHIAREA-" + Guid.NewGuid().ToString());
        }

        public string GenerateMOHAreaId() {
            return ("MOHAREA-" + Guid.NewGuid().ToString());
        }

        public string GenerateGeneralInventoryId() {
            return ("GENERALINVENTORY-" + Guid.NewGuid().ToString());
        }

        public string GenerateGeneralCatagoryId() {
            return ("GENERALCATEGORY-" + Guid.NewGuid().ToString());
        }

        public string GenerateSurgicalInventoryId() {
            return ("SURGICALINVENTORY-" + Guid.NewGuid().ToString());
        }

        public string GenerateSurgicalCatagoryId() {
            return ("SURGICALCATEGORY-" + Guid.NewGuid().ToString());
        }

        public string GenerateIssuedItemId() {
            return ("ISSUEDITEM-" + Guid.NewGuid().ToString());
        }
    }
}
