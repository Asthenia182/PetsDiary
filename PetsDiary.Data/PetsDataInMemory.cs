using PetsDiary.Common.Interfaces;
using PetsDiary.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PetsDiary.Data
{
    public class PetsDataInMemory : IPetsData
    {
        private readonly List<PetModel> pets;
        private readonly List<VaccinationModel> vaccinations;
        private readonly List<VisitModel> visits;
        private readonly List<WeightModel> weights;
        private readonly List<NoteModel> notes;

        public PetsDataInMemory()
        {
            pets = new List<PetModel>()
            {
                new PetModel { Id = 1, Name = "Daisy", AnimalType=1, Breed="Pug", Gender=1, BirthDate= DateTime.Now, LastModified = DateTime.Now },
                new PetModel { Id = 2, Name = "Kicia", AnimalType=2, Breed="Mix", Gender=1, BirthDate= DateTime.Now, LastModified = DateTime.Now },
                new PetModel { Id = 3, Name = "Kotalke", AnimalType=2, Breed="Mix", Gender=2, BirthDate= DateTime.Now, LastModified = DateTime.Now },
            };

            vaccinations = new List<VaccinationModel>()
            {
                new VaccinationModel {Id =1, PetId = 1, Address="Warszawa, Marszalkowska 134", Name = "Przeciw wsciekliznie", NextShotDate = DateTime.Now, ShotDate = DateTime.Now, ShotInformation = "Firma szczepionki taka i numer szczepionki taki" },
                new VaccinationModel {Id =2, PetId = 1, Address="Warszawa, Marszalkowska 134", Name = "Przeciw wsciekliznie !!", NextShotDate = DateTime.Now, ShotDate = DateTime.Now, ShotInformation = "fdshr tewt sfd ewr takiefsfdsfsdg h" },
            };

            visits = new List<VisitModel>()
            {
                new VisitModel(){Id=1, PetId=1, Description = "Lorem ipsum lorem ipsum lorem ipsum", Date = DateTime.Today.AddDays(-1)},
                new VisitModel(){Id=2, PetId = 1, Description = "Przykladowy tekst", Date = DateTime.Today.AddDays(-10)},
                new VisitModel(){Id=3, PetId = 1, Description = "dsablalba", Date = DateTime.Today.AddDays(-7)},
                new VisitModel(){Id=4, PetId = 1, Description = "ggeelablalba", Date = DateTime.Today.AddDays(-7)},
                new VisitModel(){Id=5, PetId = 1, Description = "ttttblalba", Date = DateTime.Today.AddDays(-7)},
            };

            weights = new List<WeightModel>()
            {
                new WeightModel() {Id = 1, PetId = 1, Date = DateTime.Today, Weight = 10},
                new WeightModel() {Id = 2, PetId = 1, Date = DateTime.Today.AddDays(-10), Weight = 5},
                new WeightModel() {Id = 3, PetId = 1, Date = DateTime.Today.AddDays(-5), Weight = 7},
            };

            notes = new List<NoteModel>()
            {
            new NoteModel(){Id=1,PetId=1,Note=string.Empty},
            new NoteModel(){Id=2,PetId=1,Note=string.Empty},
            new NoteModel(){Id=3,PetId=1,Note=string.Empty},
            };
        }

        public IEnumerable<NoteModel> GetNotes(int petId)
        {
            return notes
                .Where(x => x.PetId == petId);
        }

        public NoteModel AddNote(NoteModel model)
        {
            notes.Add(model);
            model.Id = notes.Max(r => r.Id) + 1;
            return model;
        }

        public void DeleteNoteById(int id)
        {
            var note = notes.FirstOrDefault(r => r.Id == id);
            if (note != null)
            {
                notes.Remove(note);
            }
        }

        public NoteModel UpdateNote(NoteModel updatedWeight)
        {
            var note = notes.SingleOrDefault(r => r.Id == updatedWeight.Id);

            if (note != null)
            {
                note.Id = updatedWeight.Id;
                note.PetId = updatedWeight.PetId;
                note.Note = updatedWeight.Note;
            }

            return note;
        }

        public IEnumerable<WeightModel> GetWeights(int petId)
        {
            return weights
                .Where(x => x.PetId == petId)
                .OrderByDescending(x => x.Date);
        }

        public WeightModel AddWeight(WeightModel model)
        {
            weights.Add(model);
            model.Id = weights.Max(r => r.Id) + 1;
            return model;
        }

        public void DeleteWeightById(int id)
        {
            var weight = weights.FirstOrDefault(r => r.Id == id);
            if (weight != null)
            {
                weights.Remove(weight);
            }
        }

        public WeightModel UpdateWeight(WeightModel updatedWeight)
        {
            var weight = weights.SingleOrDefault(r => r.Id == updatedWeight.Id);

            if (weight != null)
            {
                weight.Id = updatedWeight.Id;
                weight.PetId = updatedWeight.PetId;
                weight.Weight = updatedWeight.Weight;
                weight.Date = updatedWeight.Date;
            }

            return weight;
        }

        public IEnumerable<VisitModel> GetVisits(int petId)
        {
            return visits
                .Where(x => x.PetId == petId)
                .OrderByDescending(x => x.Date);
        }

        public VisitModel AddVisit(VisitModel model)
        {
            visits.Add(model);
            model.Id = visits.Max(r => r.Id) + 1;
            return model;
        }

        public void DeleteVisitById(int id)
        {
            var visit = visits.FirstOrDefault(r => r.Id == id);
            if (visit != null)
            {
                visits.Remove(visit);
            }
        }

        public VisitModel UpdateVisit(VisitModel updatedVisit)
        {
            var visit = visits.SingleOrDefault(r => r.Id == updatedVisit.Id);

            if (visit != null)
            {
                visit.Id = updatedVisit.Id;
                visit.PetId = updatedVisit.PetId;
                visit.Description = updatedVisit.Description;
                visit.Date = updatedVisit.Date;
            }

            return visit;
        }

        public IEnumerable<VaccinationModel> GetVaccinations(int petId)
        {
            return vaccinations
                .Where(x => x.PetId == petId)
                .OrderBy(x => x.ShotDate);
        }

        public IEnumerable<PetModel> GetPets()
        {
            return pets.OrderBy(x => x.Name);
        }

        public PetModel GetPetById(int id)
        {
            return pets.SingleOrDefault(r => r.Id == id);
        }

        public PetModel AddPet(PetModel model)
        {
            model.Id = pets.Max(r => r.Id) + 1;
            model.LastModified = DateTime.Now;
            pets.Add(model);

            return model;
        }

        public PetModel UpdatePet(PetModel updatedmodel)
        {
            var pet = pets.SingleOrDefault(r => r.Id == updatedmodel.Id);
            if (pet != null)
            {
                pet.Id = updatedmodel.Id;
                pet.Name = updatedmodel.Name;
                pet.Breed = updatedmodel.Breed;
                pet.AnimalType = updatedmodel.AnimalType;
                pet.LastModified = DateTime.Now;
                pet.BirthDate = updatedmodel.BirthDate;
                pet.Gender = updatedmodel.Gender;
                pet.Image = updatedmodel.Image;
            }
            return pet;
        }

        public void DeletePetById(int id)
        {
            var pet = pets.FirstOrDefault(r => r.Id == id);
            if (pet != null)
            {
                pets.Remove(pet);
                DeleteNoteByPetId(id);
                DeleteVaccinationByPetId(id);
                DeleteVisitByPetId(id);
                DeleteVaccinationByPetId(id);
                DeleteWeightByPetId(id);
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

        public void DeleteVaccinationByPetId(int petId)
        {
            var vaccination = vaccinations.FirstOrDefault(r => r.PetId == petId);
            if (vaccination != null)
            {
                vaccinations.Remove(vaccination);
            }
        }

        public void DeleteVisitByPetId(int petId)
        {
            var visit = this.visits.FirstOrDefault(r => r.PetId == petId);
            if (visit != null)
            {
                visits.Remove(visit);
            }
        }

        public void DeleteNoteByPetId(int petId)
        {
            var note = this.notes.FirstOrDefault(r => r.PetId == petId);
            if (note != null)
            {
                notes.Remove(note);
            }
        }

        public void DeleteWeightByPetId(int petId)
        {
            var weight = this.weights.FirstOrDefault(r => r.PetId == petId);
            if (weight != null)
            {
                notes.Remove(weight);
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