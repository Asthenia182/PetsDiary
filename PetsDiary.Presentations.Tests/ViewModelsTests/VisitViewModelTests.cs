using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PetsDiary.Common.Models;
using PetsDiary.Presentation.ViewModels;
using PetsDiary.Presentations.Tests.Mocks;
using System;

namespace PetsDiary.Presentations.Tests.ViewModelsTests
{
    [TestClass]
    public class VisitViewModelTests
    {
        private PetsDataMock petsDataMock = new PetsDataMock();

        private MapperMock mapperMock = new MapperMock();

        public VisitViewModel BuildSUT()
        {
            return new VisitViewModel(petsDataMock.Object, mapperMock.Object);
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

            petsDataMock.SetupSaveVisit();

            mapperMock.SetupMap(sut, new VisitModel());

            //Act
            sut.Save();

            //Assert
            petsDataMock.Verify(m => m.AddVisit(It.IsAny<VisitModel>()), Times.Once);
        }

        [TestMethod]
        public void ShouldUpdate()
        {
            //Arrange
            var sut = BuildSUT();

            petsDataMock.SetupUpdateVisit();
            mapperMock.SetupMap(sut, new VisitModel());

            sut.Id = 1;

            //Act
            sut.Update();

            //Assert
            petsDataMock.Verify(m => m.UpdateVisit(It.IsAny<VisitModel>()), Times.Once);
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

            var originDescription = "description";
            sut.Description = originDescription;
            var originDate = DateTime.Now.AddDays(5);
            sut.Date = originDate;

            sut.Id = 1;
            //saves originvalues based on this property's value
            sut.IsDirty = false;

            //Act
            sut.Description = "newDescription";
            sut.Date = DateTime.Now.AddDays(-5);

            sut.Cancel();

            //Assert
            Assert.AreEqual(sut.Description, originDescription);
            Assert.AreEqual(sut.Date.ToString(), originDate.ToString());
        }
    }
}