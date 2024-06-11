using Project_v1.Data;

namespace Project_v1.Services.IdGeneratorService {
    public class IdGenerator : IIdGenerator {

        public string GenerateSampleId() {
            return ("SMP" + Guid.NewGuid().ToString());
        }     

        public string GenerateReportId() {
            return ("REP" + Guid.NewGuid().ToString());
        }

        public string GenerateLabId() {
            return ("LAB" + Guid.NewGuid().ToString());
        }

        public string GeneratePHIAreaId() {
            return ("PHIA" + Guid.NewGuid().ToString());
        }

        public string GenerateMOHAreaId() {
            return ("MOHA" + Guid.NewGuid().ToString());
        }

        public string GenerateGeneralInventoryId() {
            return ("GI" + Guid.NewGuid().ToString());
        }

        public string GenerateGeneralCatagoryId() {
            return ("GC" + Guid.NewGuid().ToString());
        }

        public string GenerateSurgicalInventoryId() {
            return ("SI" + Guid.NewGuid().ToString());
        }

        public string GenerateSurgicalCatagoryId() {
            return ("SC" + Guid.NewGuid().ToString());
        }

        public string GenerateIssuedItemId() {
            return ("II" + Guid.NewGuid().ToString());
        }

        public string GenerateInstrumentalQualityControlId() {
            return ("IQC" + Guid.NewGuid().ToString());
        }

        public string GenerateMediaQualityControlId() {
            return ("MQC" + Guid.NewGuid().ToString());
        }
    }
}
