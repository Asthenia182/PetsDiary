using System;

namespace PetsDiary.Common.Models
{
    public class VisitModel
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}