namespace Project_v1.Services.IdGeneratorService {
    public interface IIdGenerator {
        public string GenerateSampleId();
        public string GenerateReportId();
        public string GenerateLabId();
        public string GeneratePHIAreaId();
        public string GenerateMOHAreaId();
    }
}
