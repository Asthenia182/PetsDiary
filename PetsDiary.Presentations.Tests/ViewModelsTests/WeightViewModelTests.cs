using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PetsDiary.Common.Models;
using PetsDiary.Presentation.Resources;
using PetsDiary.Presentation.ViewModels;
using PetsDiary.Presentations.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PetsDiary.Presentations.Tests.ViewModelsTests
{
    [TestClass]
    public class WeightViewModelTests
    {
        private PetsDataMock petsDataMock = new PetsDataMock();

        private MapperMock mapperMock = new MapperMock();

        public WeightViewModel BuildSUT()
        {
            return new WeightViewModel(petsDataMock.Object, mapperMock.Object);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            petsDataMock.Reset();
            mapperMock.Reset();
        }

        /// <summary>
        /// WeightText is binded to View, user sets its value
        /// </summary>
        [TestMethod]
        public void ShouldAddErrorWhenWeightIsZero()
        {
            //Arrange
            var sut = BuildSUT();

            sut.WeightText = "6";

            //Act
            sut.WeightText = "0";

            //Assert
            var errors = (List<string>)sut.GetErrors(nameof(sut.WeightText)); 
            Assert.AreEqual(ErrorMessages.ValidationWeightZero, errors.First());
        }

        /// <summary>
        /// WeightText is binded to View, user sets its value
        /// </summary>
        [TestMethod]
        public void ShouldNotBeValidOnErrorAdded()
        {
            //Arrange
            var sut = BuildSUT();

            sut.Weight = 6;
            var hadError = sut.HasErrors;
            var wasValid = sut.IsValid();

            //Act
            sut.WeightText = "error";

            //Assert
            Assert.IsTrue(!sut.IsValid());
            Assert.AreNotEqual(hadError, sut.HasErrors);
            Assert.AreNotEqual(wasValid, sut.IsValid());
        }

        /// <summary>
        /// WeightText is binded to View, user sets its value
        /// </summary>
        [TestMethod]
        public void ShouldAddErrorWhenCannotParseWeightToDouble()
        {
            //Arrange
            var sut = BuildSUT();

            sut.Weight = 6;

            //Act
            sut.WeightText = "error";

            //Assert
            var errors = (List<string>)sut.GetErrors(nameof(sut.WeightText));
            Assert.AreEqual(ErrorMessages.ValidationErrorWeightValueType, errors.First());
        }
    
        [TestMethod]
        public void ShouldAddErrorWhenWeightWithSameDateExists()
        {
            //Arrange
            var sut = BuildSUT();

            var savedWeights = new List<WeightModel>() { new WeightModel() { Date = DateTime.Now } };
            sut.PetId = 1;

            petsDataMock.SetupGetWeights(sut.PetId, savedWeights);

            //Act
            sut.Date = DateTime.Now;

            //Assert
            var errors = (List<string>)sut.GetErrors(nameof(sut.Date));
            Assert.AreEqual(ErrorMessages.ValidationErrorDateWeight, errors.First());
        }

        [TestMethod]
        public void ShouldNotSaveWhenIsntValid()
        {
            //Arrange
            var sut = BuildSUT();

            petsDataMock.SetupSaveWeight();

            //Act
            sut.Weight = 0;
            sut.Save();

            //Assert
            Assert.IsFalse(sut.IsValid());
            petsDataMock.Verify(m => m.AddWeight(It.IsAny<WeightModel>()), Times.Never);
        }

        [TestMethod]
        public void ShouldSaveWhenIsValid()
        {
            //Arrange
            var sut = BuildSUT();

            petsDataMock.SetupSaveWeight();

            mapperMock.SetupMap(sut, new WeightModel());

            //Act
            sut.Weight = 1;
            sut.Save();

            //Assert
            Assert.IsTrue(sut.IsValid());
            petsDataMock.Verify(m => m.AddWeight(It.IsAny<WeightModel>()), Times.Once);
        }

        [TestMethod]
        public void ShouldNotUpdateWhenIsntValid()
        {
            //Arrange
            var sut = BuildSUT();

            petsDataMock.SetupUpdateWeight();
            mapperMock.SetupMap(sut, new WeightModel());
            sut.Id = 1;

            //Act
            sut.Weight = 0;
            sut.Update();

            //Assert
            Assert.IsFalse(sut.IsValid());
            petsDataMock.Verify(m => m.UpdateWeight(It.IsAny<WeightModel>()), Times.Never);
        }

        [TestMethod]
        public void ShouldUpdateWhenIsValid()
        {
            //Arrange
            var sut = BuildSUT();

            petsDataMock.SetupUpdateWeight();
            mapperMock.SetupMap(sut, new WeightModel());

            sut.Id = 1;

            //Act
            sut.Weight = 8;
            sut.Update();

            //Assert
            Assert.IsTrue(sut.IsValid());
            petsDataMock.Verify(m => m.UpdateWeight(It.IsAny<WeightModel>()), Times.Once);
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
    }
}