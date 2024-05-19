﻿using Microsoft.AspNetCore.Mvc;
using Project_v1.Models.DTOs.GeneralInventoryItems;

namespace Project_v1.Services.QRGeneratorService {
    public interface IQRGenerator {
        byte[] GenerateGeneralInventoryQRCode(String CategoryId, String ItemId);
        byte[] GenerateSurgicalInventoryQRCode(String CategoryId, String ItemId);
    }
}
