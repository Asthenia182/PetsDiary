using System;

namespace PetsDiary.Common.Models
{
    public class VaccinationModel
    {
        public int Id { get; set; }

        public int PetId { get; set; }

        public DateTime ShotDate { get; set; }

        public DateTime NextShotDate { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string ShotInformation { get; set; }
    }
}