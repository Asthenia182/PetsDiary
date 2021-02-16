using Moq;
using PetsDiary.Common.Interfaces;
using PetsDiary.Common.Models;
using System.Collections.Generic;

namespace PetsDiary.Presentations.Tests.Mocks
{
    public class PetsDataMock : Mock<IPetsData>
    {
        public void SetupDeleteNoteById(int id)
        {
            Setup(x => x.DeleteNoteById(id))
                .Verifiable();
        }

        public void SetupDeletePetById(int petId)
        {
            Setup(x => x.DeletePetById(petId))
                .Verifiable();
        }

        public void SetupDeleteVaccinationByPetId(int id)
        {
            Setup(x => x.DeleteVaccinationById(id))
                .Verifiable();
        }

        public void SetupDeleteVisitById(int id)
        {
            Setup(x => x.DeleteVisitById(id))
                .Verifiable();
        }

        public void SetupDeleteWeightById(int id)
        {
            Setup(x => x.DeleteWeightById(id))
                .Verifiable();
        }

        public void SetupGetNotes(int petId, List<NoteModel> models)
        {
            Setup(x => x.GetNotes(petId))
                  .Returns(models);
        }

        public void SetupGetPetById(int petId, PetModel returnModel)
        {
            Setup(x => x.GetPetById(petId))
                .Returns(returnModel);
        }

        public void SetupGetPets(List<PetModel> models)
        {
            Setup(x => x.GetPets())
                  .Returns(models);
        }
        public void SetupGetVaccinations(int petId, List<VaccinationModel> models)
        {
            Setup(x => x.GetVaccinations(petId))
                  .Returns(models);
        }

        public void SetupGetVisits(int petId, List<VisitModel> models)
        {
            Setup(x => x.GetVisits(petId))
                  .Returns(models);
        }
        public void SetupGetWeights(int petId, List<WeightModel> models)
        {
            Setup(x => x.GetWeights(petId))
                  .Returns(models);
        }

        public void SetupSaveNote(NoteModel model = null)
        {
            if (model != null)
            {
                Setup(x => x.AddNote(model))
                    .Returns(model)
                    .Verifiable();
            }
            else
            {
                Setup(x => x.AddNote(It.IsAny<NoteModel>()))
                   .Returns(new NoteModel())
                   .Verifiable();
            }
        }

        public void SetupSavePet(PetModel model = null)
        {
            if (model != null)
            {
                Setup(x => x.AddPet(model))
                    .Returns(model)
                    .Verifiable();
            }
            else
            {
                Setup(x => x.AddPet(It.IsAny<PetModel>()))
                    .Returns(new PetModel())
                   .Verifiable();
            }
        }

        public void SetupSaveVaccination(VaccinationModel model = null)
        {
            if (model != null)
            {
                Setup(x => x.AddVacination(model))
                    .Returns(model)
                    .Verifiable();
            }
            else
            {
                Setup(x => x.AddVacination(It.IsAny<VaccinationModel>()))
                   .Returns(new VaccinationModel())
                   .Verifiable();
            }
        }

        public void SetupSaveVisit(VisitModel model = null)
        {
            if (model != null)
            {
                Setup(x => x.AddVisit(model))
                     .Returns(model)
                    .Verifiable();
            }
            else
            {
                Setup(x => x.AddVisit(It.IsAny<VisitModel>()))
                     .Returns(new VisitModel())
                   .Verifiable();
            }
        }

        public void SetupSaveWeight(WeightModel model = null)
        {
            if (model != null)
            {
                Setup(x => x.AddWeight(model))
                     .Returns(model)
                    .Verifiable();
            }
            else
            {
                Setup(x => x.AddWeight(It.IsAny<WeightModel>()))
                     .Returns(new WeightModel())
                   .Verifiable();
            }
        }

        public void SetupUpdateNote(NoteModel model = null)
        {
            if (model != null)
            {
                Setup(x => x.UpdateNote(model))
                 .Returns(model)
                 .Verifiable();
            }
            else
            {
                Setup(x => x.UpdateNote(It.IsAny<NoteModel>()))
                    .Returns(new NoteModel())
                    .Verifiable();
            }
        }

        public void SetupUpdatePet(PetModel model = null)
        {
            if (model != null)
            {
                Setup(x => x.UpdatePet(model))
                 .Returns(model)
                 .Verifiable();
            }
            else
            {
                Setup(x => x.UpdatePet(It.IsAny<PetModel>()))
                    .Returns(new PetModel())
                    .Verifiable();
            }
        }
        public void SetupUpdateVaccination(VaccinationModel model = null)
        {
            if (model != null)
            {
                Setup(x => x.UpdateVaccination(model))
                 .Returns(model)
                 .Verifiable();
            }
            else
            {
                Setup(x => x.UpdateVaccination(It.IsAny<VaccinationModel>()))
                     .Returns(new VaccinationModel())
                    .Verifiable();
            }
        }

        public void SetupUpdateVisit(VisitModel model = null)
        {
            if (model != null)
            {
                Setup(x => x.UpdateVisit(model))
                 .Returns(model)
                 .Verifiable();
            }
            else
            {
                Setup(x => x.UpdateVisit(It.IsAny<VisitModel>()))
                    .Returns(new VisitModel())
                    .Verifiable();
            }
        }
        public void SetupUpdateWeight(WeightModel model = null)
        {
            if (model != null)
            {
                Setup(x => x.UpdateWeight(model))
                     .Returns(model)
                 .Verifiable();
            }
            else
            {
                Setup(x => x.UpdateWeight(It.IsAny<WeightModel>()))
                     .Returns(new WeightModel())
                    .Verifiable();
            }
        }
    }
}