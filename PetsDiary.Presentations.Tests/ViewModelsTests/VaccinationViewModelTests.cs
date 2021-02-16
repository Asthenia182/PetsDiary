using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PetsDiary.Common.Models;
using PetsDiary.Presentation.ViewModels;
using PetsDiary.Presentations.Tests.Mocks;
using System;

namespace PetsDiary.Presentations.Tests.ViewModelsTests
{
    [TestClass]
    public class VaccinationViewModelTests
    {
        private PetsDataMock petsDataMock = new PetsDataMock();

        private MapperMock mapperMock = new MapperMock();

        public VaccinationViewModel BuildSUT()
        {
            return new VaccinationViewModel(petsDataMock.Object, mapperMock.Object);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            petsDataMock.Reset();
            mapperMock.Reset();
        }

        [TestMethod]
        public void ShouldSave()
        {
            //Arrange
            var sut = BuildSUT();

            petsDataMock.SetupSaveVaccination();

            mapperMock.SetupMap(sut, new VaccinationModel());

            //Act
            sut.Save();

            //Assert
            petsDataMock.Verify(m => m.AddVacination(It.IsAny<VaccinationModel>()), Times.Once);
        }

        [TestMethod]
        public void ShouldUpdate()
        {
            //Arrange
            var sut = BuildSUT();

            petsDataMock.SetupUpdateVaccination();
            mapperMock.SetupMap(sut, new VaccinationModel());

            sut.Id = 1;

            //Act
            sut.Update();

            //Assert
            petsDataMock.Verify(m => m.UpdateVaccination(It.IsAny<VaccinationModel>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowExceptionWhenIdIsNullOnUpdate()
        {
            //Arrange
            var sut = BuildSUT();

            //Act
            sut.Update();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowExceptionWhenIdHasValueOnSave()
        {
            //Arrange
            var sut = BuildSUT();
            sut.Id = 1;

            //Act
            sut.Save();
        }

        [TestMethod]
        public void ShouldSetOriginValuesWhenIsCancelled()
        {
            //Arrange
            var sut = BuildSUT();

            var originName = "name";
            sut.Name = originName;
            var originShotDate = DateTime.Now.AddDays(5);
            sut.ShotDate = originShotDate;
            var originShotInformation = "information";
            sut.ShotInformation = originShotInformation;
            var originNextShotDate = DateTime.Now.AddDays(10);
            sut.NextShotDate = originNextShotDate;
            var originAdress = "address";
            sut.Address = originAdress;

            sut.Id = 1;
            //saves originvalues based on this property's value
            sut.IsDirty = false;

            //Act
            sut.Name = "newName";
            sut.ShotDate = DateTime.Now.AddDays(-5);
            sut.ShotInformation = "newInformation";
            sut.NextShotDate = DateTime.Now.AddDays(1);
            sut.Address = "newAdress";

            sut.Cancel();

            //Assert
            Assert.AreEqual(sut.Name, originName);
            Assert.AreEqual(sut.ShotDate.Value.ToString(), originShotDate.ToString());
            Assert.AreEqual(sut.ShotInformation, originShotInformation);
            Assert.AreEqual(sut.NextShotDate.Value.ToString(), originNextShotDate.ToString());
            Assert.AreEqual(sut.Address, originAdress);
        }
    }
}