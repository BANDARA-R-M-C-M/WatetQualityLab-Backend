namespace Project_v1.Services.IdGeneratorService {
    public interface IIdGenerator {
        public string GenerateSampleId();
        public string GenerateReportId();
        public string GenerateLabId();
        public string GeneratePHIAreaId();
        public string GenerateMOHAreaId();
        public string GenerateGeneralInventoryId();
        public string GenerateGeneralCatagoryId();
        public string GenerateSurgicalInventoryId();
        public string GenerateSurgicalCatagoryId();
        public string GenerateIssuedItemId();
    }
}
