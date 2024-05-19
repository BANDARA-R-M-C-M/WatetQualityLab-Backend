namespace Project_v1.Services.IdGeneratorService {
    public interface IIdGenerator {
        string GenerateSampleId();
        string GenerateReportId();
        string GenerateLabId();
        string GeneratePHIAreaId();
        string GenerateMOHAreaId();
        string GenerateGeneralInventoryId();
        string GenerateGeneralCatagoryId();
        string GenerateSurgicalInventoryId();
        string GenerateSurgicalCatagoryId();
        string GenerateIssuedItemId();
        string GenerateInstrumentalQualityControlId();
        string GenerateMediaQualityControlId();
    }
}
