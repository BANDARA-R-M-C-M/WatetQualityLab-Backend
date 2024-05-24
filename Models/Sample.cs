﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_v1.Models {
    public class Sample {
        [Key]
        public string SampleId { get; set; }
        public string YourRefNo { get; set; }
        public string StateOfChlorination { get; set; }
        public DateOnly DateOfCollection { get; set; }
        public string CatagoryOfSource { get; set; }
        public string CollectingSource { get; set; }
        public DateOnly AnalyzedDate { get; set; }
        public string phiAreaName { get; set; }
        public string Acceptance { get; set; }
        public string Comments { get; set; }
        public string PhiId { get; set; }
        public string PHIAreaId { get; set; }
        public virtual PHIArea PHIArea { get; set; }
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}