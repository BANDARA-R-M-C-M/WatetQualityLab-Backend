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

            return ("SMP-" + Guid.NewGuid().ToString());
        }     

        public string GenerateReportId() {
            /*string newReportId;
             *            do {
             *                           newReportId = "RPT-" + Guid.NewGuid().ToString();
             *                                      } while (!(_context.Reports.Any(r => r.ReportId == newReportId)));*/

            return ("RPT-" + Guid.NewGuid().ToString());
        }
    }
}
