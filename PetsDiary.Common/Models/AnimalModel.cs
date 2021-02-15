using System;

namespace PetsDiary.Common.Models
{
    public class AnimalModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Breed { get; set; }

        public int AnimalType { get; set; }

        public int Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime LastModified { get; set; }

        public byte[] Image { get; set; }
    }
}