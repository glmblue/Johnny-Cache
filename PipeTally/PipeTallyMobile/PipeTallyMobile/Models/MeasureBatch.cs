﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipeTallyMobile.Models
{
    public class MeasureBatch
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public float OuterDiameter { get; set; }
        public int Weight { get; set; }
        public string Grade { get; set; }
        public float InnerDiameter { get; set; }
        public string TopThread { get; set; }
        public bool Uploaded { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string SiteName { get; set; }

        //[Ignore] //have SqlLite ignore this. It's not EF 
        //public List<Measurement> Measurements {get;set;}
    }
}
