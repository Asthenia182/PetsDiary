namespace PetsDiary.Common.Models
{
    public class AnimalModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Breed { get; set; }

        public int AnimalType { get; set; }

        public double ActualWeight { get; set; }

        /// <summary>
        /// Picture's file name
        /// </summary>
        public string FileName { get; set; }
    }
}