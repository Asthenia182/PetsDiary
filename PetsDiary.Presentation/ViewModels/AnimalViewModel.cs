using PetsDiary.Presentation.Interfaces;
using System;
using System.Collections.Generic;

namespace PetsDiary.Presentation.ViewModels
{
    public class AnimalViewModel : BaseViewModel, IAnimalViewModel
    {
        public int? Id { get; set; } = null;

        public string Name { get; set; }

        public string Breed { get; set; }

        public int AnimalType { get; set; }

        public decimal ActualWeight { get; set; }

        /// <summary>
        /// Picture's file name
        /// </summary>
        public string FileName { get; set; }

        public Dictionary<DateTime, decimal> WeightByDate { get; set; }
    }
}