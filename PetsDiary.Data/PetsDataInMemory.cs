using PetsDiary.Common.Interfaces;
using PetsDiary.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PetsDiary.Data
{
    public class PetsDataInMemory : IPetsData
    {
        private readonly List<NoteModel> notes;
        private readonly List<PetModel> pets;
        private readonly List<VaccinationModel> vaccinations;
        private readonly List<VisitModel> visits;
        private readonly List<WeightModel> weights;

        public PetsDataInMemory()
        {
            pets = new List<PetModel>();

            vaccinations = new List<VaccinationModel>();

            visits = new List<VisitModel>();

            weights = new List<WeightModel>();

            notes = new List<NoteModel>();
        }

        public NoteModel AddNote(NoteModel model)
        {
            notes.Add(model);
            model.Id = notes.Max(r => r.Id) + 1;
            return model;
        }

        public PetModel AddPet(PetModel model)
        {
            model.Id = pets.Max(r => r.Id) + 1;
            model.LastModified = DateTime.Now;
            pets.Add(model);

            return model;
        }

        public VaccinationModel AddVacination(VaccinationModel model)
        {
            vaccinations.Add(model);
            model.Id = vaccinations.Max(r => r.Id) + 1;
            return model;
        }

        public VisitModel AddVisit(VisitModel model)
        {
            visits.Add(model);
            model.Id = visits.Max(r => r.Id) + 1;
            return model;
        }

        public WeightModel AddWeight(WeightModel model)
        {
            weights.Add(model);
            model.Id = weights.Max(r => r.Id) + 1;
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

        public void DeleteNoteByPetId(int petId)
        {
            var note = this.notes.FirstOrDefault(r => r.PetId == petId);
            if (note != null)
            {
                notes.Remove(note);
            }
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

        public void DeleteVisitById(int id)
        {
            var visit = visits.FirstOrDefault(r => r.Id == id);
            if (visit != null)
            {
                visits.Remove(visit);
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

        public void DeleteWeightById(int id)
        {
            var weight = weights.FirstOrDefault(r => r.Id == id);
            if (weight != null)
            {
                weights.Remove(weight);
            }
        }

        public void DeleteWeightByPetId(int petId)
        {
            var weight = this.weights.FirstOrDefault(r => r.PetId == petId);
            if (weight != null)
            {
                weights.Remove(weight);
            }
        }

        public IEnumerable<NoteModel> GetNotes(int petId)
        {
            return notes
                .Where(x => x.PetId == petId);
        }

        public PetModel GetPetById(int id)
        {
            return pets.SingleOrDefault(r => r.Id == id);
        }

        public IEnumerable<PetModel> GetPets()
        {
            return pets.OrderBy(x => x.Name);
        }

        public IEnumerable<VaccinationModel> GetVaccinations(int petId)
        {
            return vaccinations
                .Where(x => x.PetId == petId)
                .OrderBy(x => x.ShotDate);
        }

        public IEnumerable<VisitModel> GetVisits(int petId)
        {
            return visits
                .Where(x => x.PetId == petId)
                .OrderByDescending(x => x.Date);
        }

        public IEnumerable<WeightModel> GetWeights(int petId)
        {
            return weights
                .Where(x => x.PetId == petId)
                .OrderByDescending(x => x.Date);
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
    }
}