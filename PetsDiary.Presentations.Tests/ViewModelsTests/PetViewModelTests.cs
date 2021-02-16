using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PetsDiary.Common.Models;
using PetsDiary.Presentation;
using PetsDiary.Presentation.Enums;
using PetsDiary.Presentation.ViewModels;
using PetsDiary.Presentations.Tests.Mocks;
using System;

namespace PetsDiary.Presentations.Tests.ViewModelsTests
{
    [TestClass]
    public class PetViewModelTests
    {
        private PetsDataMock petsDataMock = new PetsDataMock();

        private Mock<IPetDescription> petDescriptionMock = new Mock<IPetDescription>();

        private MapperMock mapperMock = new MapperMock();

        public PetViewModel BuildSUT()
        {
            return new PetViewModel(petsDataMock.Object, petDescriptionMock.Object, mapperMock.Object);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            petsDataMock.Reset();
            petDescriptionMock.Reset();
            mapperMock.Reset();
        }

        [TestMethod]
        public void ShouldAddErrorWhenNameIsNull()
        {
            //Arrange
            var sut = BuildSUT();

            sut.Name = "name";
            var hadError = sut.HasErrors;
            var wasValid = sut.IsValid();

            //Act
            sut.Name = null;

            //Assert
            Assert.IsTrue(!sut.IsValid());
            Assert.AreNotEqual(hadError, sut.HasErrors);
            Assert.AreNotEqual(wasValid, sut.IsValid());
        }

        [TestMethod]
        public void ShouldNotSaveWhenIsntValid()
        {
            //Arrange
            var sut = BuildSUT();

            petsDataMock.SetupSavePet();

            //Act
            sut.Name = null;
            sut.Save();

            //Assert
            Assert.IsFalse(sut.IsValid());
            petsDataMock.Verify(m => m.AddPet(It.IsAny<PetModel>()), Times.Never);
        }

        [TestMethod]
        public void ShouldSaveWhenIsValid()
        {
            //Arrange
            var sut = BuildSUT();

            petsDataMock.SetupSavePet();

            mapperMock.SetupMap(sut, new PetModel());

            //Act
            sut.Name = "name";
            sut.Save();

            //Assert
            Assert.IsTrue(sut.IsValid());
            petsDataMock.Verify(m => m.AddPet(It.IsAny<PetModel>()), Times.Once);
        }

        [TestMethod]
        public void ShouldNotUpdateWhenIsntValid()
        {
            //Arrange
            var sut = BuildSUT();

            petsDataMock.SetupUpdatePet();
            mapperMock.SetupMap(sut, new PetModel());

            //Act
            sut.Name = null;
            sut.Id = 1;
            sut.Update();

            //Assert
            Assert.IsFalse(sut.IsValid());
            petsDataMock.Verify(m => m.UpdatePet(It.IsAny<PetModel>()), Times.Never);
        }

        [TestMethod]
        public void ShouldUpdateWhenIsValid()
        {
            //Arrange
            var sut = BuildSUT();

            petsDataMock.SetupUpdatePet();
            mapperMock.SetupMap(sut, new PetModel());

            sut.Id = 1;

            //Act
            sut.Name = "name";
            sut.Update();

            //Assert
            Assert.IsTrue(sut.IsValid());
            petsDataMock.Verify(m => m.UpdatePet(It.IsAny<PetModel>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowExceptionWhenIdIsNullOnUpdate()
        {
            //Arrange
            var sut = BuildSUT();
            sut.Name = "name";

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
            sut.Name = "name";

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
            var originLastModified = DateTime.Now.AddDays(5);
            sut.LastModified = DateTime.Now.AddDays(5);
            var originAnimalType = AnimalType.Cat;
            sut.AnimalType = originAnimalType;
            var originBirthDate = DateTime.Now.AddDays(-10);
            sut.BirthDate = originBirthDate;
            var originBreed = "breed";
            sut.Breed = originBreed;
            var originGender = Gender.Male;
            sut.Gender = originGender;

            sut.Id = 1;
            //saves originvalues based on this property's value
            sut.IsDirty = false;

            //Act
            sut.Name = "newName";
            sut.LastModified = DateTime.Now.AddDays(-5);
            sut.AnimalType = AnimalType.Dog;
            sut.Breed = "newBreed";
            sut.BirthDate = DateTime.Now;
            sut.Gender = Gender.Female;

            sut.CancelCommand.Execute();

            //Assert
            Assert.AreEqual(sut.Name, originName);
            Assert.AreEqual(sut.LastModified.Value.ToString(),originLastModified.ToString());
            Assert.AreEqual(sut.AnimalType, originAnimalType);
            Assert.AreEqual(sut.BirthDate.Value.ToString(), originBirthDate.ToString());
            Assert.AreEqual(sut.Breed, originBreed);
            Assert.AreEqual(sut.Gender, originGender);
        }
    }
}