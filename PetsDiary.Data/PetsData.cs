﻿using PetsDiary.Common.Interfaces;
using PetsDiary.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PetsDiary.Data
{
    public class PetsData : IPetsData
    {
        private readonly List<AnimalModel> animals;
        private readonly List<VaccinationModel> vaccinations;

        public PetsData()
        {
            animals = new List<AnimalModel>()
            {
                new AnimalModel { Id = 1, Name = "Daisy", AnimalType=1, Breed="Pug", Gender=1, BirthDate= DateTime.Now, LastModified = DateTime.Now },
                new AnimalModel { Id = 2, Name = "Kicia", AnimalType=2, Breed="Mix", Gender=1, BirthDate= DateTime.Now, LastModified = DateTime.Now },
                new AnimalModel { Id = 3, Name = "Kotalke", AnimalType=2, Breed="Mix", Gender=2, BirthDate= DateTime.Now, LastModified = DateTime.Now },
            };

            vaccinations = new List<VaccinationModel>()
            {
                new VaccinationModel {Id =1, PetId = 1, Address="Warszawa, Marszalkowska 134", Name = "Przeciw wsciekliznie", NextShotDate = DateTime.Now, ShotDate = DateTime.Now, ShotInformation = "Firma szczepionki taka i numer szczepionki taki" },
                new VaccinationModel {Id =2, PetId = 1, Address="Warszawa, Marszalkowska 134", Name = "Przeciw wsciekliznie !!", NextShotDate = DateTime.Now, ShotDate = DateTime.Now, ShotInformation = "fdshr tewt sfd ewr takiefsfdsfsdg h" },
            };
        }

        public IEnumerable<VaccinationModel> GetVaccinations(int petId)
        {
            return vaccinations
                .Where(x => x.PetId == petId)
                .OrderBy(x => x.ShotDate);
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

        public void DeleteAnimalById(int id)
        {
            var animal = animals.FirstOrDefault(r => r.Id == id);
            if (animal != null)
            {
                animals.Remove(animal);
            }
        }

        public VaccinationModel AddVacination(VaccinationModel model)
        {
            vaccinations.Add(model);
            model.Id = vaccinations.Max(r => r.Id) + 1;
            return model;
        }

        public void DeleteVaccinationById(int id)
        {
            var vaccination = vaccinations.FirstOrDefault(r => r.Id == id);
            if (vaccination != null)
            {
                vaccinations.Remove(vaccination);
            }
        }

        public VaccinationModel UpdateVaccination(VaccinationModel updatedVaccination)
        {
            var vaccination = vaccinations.SingleOrDefault(r => r.Id == updatedVaccination.Id);
            if (vaccination != null)
            {
                vaccination.Id = updatedVaccination.Id;
                vaccination.Name = updatedVaccination.Name;
                vaccination.NextShotDate = updatedVaccination.NextShotDate;
                vaccination.ShotDate = updatedVaccination.ShotDate;
                vaccination.ShotInformation = updatedVaccination.ShotInformation;
                vaccination.Address = updatedVaccination.Address;
            }
            return vaccination;
        }
    }
}