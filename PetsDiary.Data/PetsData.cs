using PetsDiary.Common.Interfaces;
using PetsDiary.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PetsDiary.Data
{
    public class PetsData : IPetsData
    {
        private readonly List<AnimalModel> animals;

        public PetsData()
        {
            animals = new List<AnimalModel>()
            {
                new AnimalModel { Id = 1, Name = "Daisy", AnimalType=1, Breed="Pug", Gender=1, BirthDate= DateTime.Now, LastModified = DateTime.Now },
                new AnimalModel { Id = 2, Name = "Kicia", AnimalType=2, Breed="Mix", Gender=1, BirthDate= DateTime.Now, LastModified = DateTime.Now },
                new AnimalModel { Id = 3, Name = "Kotalke", AnimalType=2, Breed="Mix", Gender=2, BirthDate= DateTime.Now, LastModified = DateTime.Now },
            };
        }

        public IEnumerable<AnimalModel> GetPets()
        {
            return animals.OrderBy(x => x.Name);
        }

        public AnimalModel GetAnimalById(int id)
        {
            return animals.SingleOrDefault(r => r.Id == id);
        }

        public AnimalModel AddAnimal(AnimalModel model)
        {
            animals.Add(model);
            model.Id = animals.Max(r => r.Id) + 1;
            return model;
        }

        public AnimalModel UpdateAnimal(AnimalModel updatedAnimal)
        {
            var animal = animals.SingleOrDefault(r => r.Id == updatedAnimal.Id);
            if (animal != null)
            {
                animal.Id = updatedAnimal.Id;
                animal.Name = updatedAnimal.Name;
                animal.Breed = updatedAnimal.Breed;
                animal.AnimalType = updatedAnimal.AnimalType;
                animal.LastModified = DateTime.Now;
                animal.BirthDate = updatedAnimal.BirthDate;
                animal.Gender = updatedAnimal.Gender;
            }
            return animal;
        }

        public AnimalModel DeleteAnimalById(int id)
        {
            var animal = animals.FirstOrDefault(r => r.Id == id);
            if (animal != null)
            {
                animals.Remove(animal);
            }
            return null;
        }
    }
}