﻿using Humanizer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_v1.Models {
    public class GeneralInventory {
        [Key]
        public string GeneralInventoryID { get; set; }
        public string ItemName { get; set; }
        public DateOnly IssuedDate { get; set; }
        public string IssuedBy { get; set;}
        public string Remarks { get; set; }
        public string ItemQR { get; set; }
        public string GeneralCategoryID { get; set; }
        public virtual GeneralCategory GeneralCategory { get; set; }
    }
}
