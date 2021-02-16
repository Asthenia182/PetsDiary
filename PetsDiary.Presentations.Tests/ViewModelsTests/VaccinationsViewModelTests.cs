using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PetsDiary.Common.Constants;
using PetsDiary.Common.Models;
using PetsDiary.Presentation;
using PetsDiary.Presentation.ViewModels;
using PetsDiary.Presentations.Tests.Mocks;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PetsDiary.Presentations.Tests.ViewModelsTests
{
    [TestClass]
    public class VaccinationsViewModelTests
    {
        private PetsDataMock petsDataMock = new PetsDataMock();
        private DialogServiceMock dialogServiceMock = new DialogServiceMock();
        private Mock<IPetDescription> petDescriptionMock = new Mock<IPetDescription>();
        private MapperMock mapperMock = new MapperMock();

        public VaccinationsViewModel BuildSUT()
        {            
            return new VaccinationsViewModel(petsDataMock.Object, petDescriptionMock.Object, dialogServiceMock.Object, mapperMock.Object);
        }

        public VaccinationsViewModelTests()
        {
            petDescriptionMock.SetupGet(x => x.Id).Returns(1);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            petsDataMock.Reset();
            dialogServiceMock.Reset();
            mapperMock.Reset();
            petDescriptionMock.Reset();
        }

        [TestMethod]
        public void ShouldInvokeGetVacinationsOnInitialized()
        {
            //Arrange
            petsDataMock.SetupGetVaccinations(petDescriptionMock.Object.Id.Value, null);

            //Act
            var sut = BuildSUT();

            //Assert
            petsDataMock.Verify();
        }

        [TestMethod]
        public void ShouldDeleteSelectedItem()
        {
            //Arrange
            var sut = BuildSUT();
            var vaccinations = GetVaccinationsMocks().Select(x => x.Object).ToList();
            sut.Vaccinations.AddRange(vaccinations);
            int countBefore = vaccinations.Count;

            petsDataMock.SetupDeleteVaccinationByPetId(vaccinations[0].Id.Value);
            dialogServiceMock.SetupShowDialog();

            //Act
            sut.DeleteCommand.Execute(vaccinations[0].Id);

            //Assert
            Assert.IsNotNull(sut.Vaccinations);
            Assert.AreNotEqual(countBefore, sut.Vaccinations.Count);
            CollectionAssert.DoesNotContain(sut.Vaccinations, vaccinations[0]);
            CollectionAssert.Contains(sut.Vaccinations, vaccinations[1]);
            petsDataMock.Verify();
        }

        [TestMethod]
        public void ShouldAddNewVaccination()
        {
            //Arrange
            var sut = BuildSUT();

            int countBefore = sut.Vaccinations.Count;

            //Act
            sut.AddCommand.Execute();

            //Assert
            Assert.AreNotEqual(countBefore, sut.Vaccinations.Count);
        }

        [TestMethod]
        public void ShouldSaveNewVaccination()
        {
            //Arrange
            var sut = BuildSUT();

            var vaccinationViewModelMock = new VaccinationViewModelMock();
            vaccinationViewModelMock.SetupSave();

            sut.Vaccinations.Add(vaccinationViewModelMock.Object);

            //Act
            sut.SaveCommand.Execute(vaccinationViewModelMock.Object.Id);

            //Assert
            vaccinationViewModelMock.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        public void ShouldUpdateVaccination()
        {
            //Arrange
            var sut = BuildSUT();

            var vaccinationViewModelMock = new VaccinationViewModelMock();
            vaccinationViewModelMock.SetupGet(x => x.Id).Returns(1);
            vaccinationViewModelMock.SetupUpdate();

            sut.Vaccinations.Add(vaccinationViewModelMock.Object);

            //Act
            sut.SaveCommand.Execute(vaccinationViewModelMock.Object.Id);

            //Assert
            vaccinationViewModelMock.Verify(x => x.Update(), Times.Once);
        }


        [TestMethod]
        public void ShouldInvokeCancelWhenVaccinationHasId()
        {
            //Arrange
            var sut = BuildSUT();
            var mockedVaccination = new VaccinationViewModelMock();
            mockedVaccination.SetupGet(x => x.Id).Returns(1);
            mockedVaccination.SetupCancel();

            sut.Vaccinations.Add(mockedVaccination.Object);

            //Act
            sut.CancelCommand.Execute(mockedVaccination.Object.Id);

            //Assert
            mockedVaccination.Verify(x => x.Cancel(), Times.Once);
        }

        [TestMethod]
        public void ShouldRemoveFromVaccinationWhenVaccinationHasNotIdOnCancel()
        {
            //Arrange
            var sut = BuildSUT();
            var mockedVaccination = new VaccinationViewModelMock();
                        
            sut.Vaccinations.Add(mockedVaccination.Object);
            var countBefore = sut.Vaccinations.Count;

            //Act
            sut.CancelCommand.Execute(mockedVaccination.Object.Id);

            //Assert
            Assert.AreNotEqual(countBefore, sut.Vaccinations.Count);
        }

        private List<VaccinationViewModelMock> GetVaccinationsMocks()
        {
            var viewmodelMock = new VaccinationViewModelMock();
            viewmodelMock.SetupGet(x => x.Id).Returns(1);

            var viewModelMock2 = new VaccinationViewModelMock();
            viewModelMock2.SetupGet(x => x.Id).Returns(2);

            return new List<VaccinationViewModelMock>() {
                viewmodelMock,
                viewModelMock2
            };
        }
    }
}