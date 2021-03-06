﻿using PetsDiary.Common.Models;
using System.Collections.Generic;

namespace PetsDiary.Common.Interfaces
{
    public interface IPetsData
    {
        PetModel AddPet(PetModel model);

        void DeletePetById(int id);

        IEnumerable<PetModel> GetPets();

        PetModel GetPetById(int id);

        PetModel UpdatePet(PetModel updatedmodel);

        IEnumerable<VaccinationModel> GetVaccinations(int petId);

        VaccinationModel AddVacination(VaccinationModel model);

        void DeleteVaccinationById(int id);

        VaccinationModel UpdateVaccination(VaccinationModel updatedVaccination);

        VisitModel UpdateVisit(VisitModel updatedVisit);

        void DeleteVisitById(int id);

        VisitModel AddVisit(VisitModel model);

        IEnumerable<VisitModel> GetVisits(int petId);

        IEnumerable<WeightModel> GetWeights(int petId);

        WeightModel AddWeight(WeightModel model);

        void DeleteWeightById(int id);

        WeightModel UpdateWeight(WeightModel updatedWeight);

        IEnumerable<NoteModel> GetNotes(int petId);

        NoteModel AddNote(NoteModel model);

        void DeleteNoteById(int id);

        NoteModel UpdateNote(NoteModel updatedWeight);
    }
}