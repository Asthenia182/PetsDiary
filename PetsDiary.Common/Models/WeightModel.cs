using System;

namespace PetsDiary.Common.Models
{
    public class WeightModel
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public DateTime Date {get;set;}
        public double Weight { get; set; }
    }
}