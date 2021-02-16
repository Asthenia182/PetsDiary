using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PetsDiary.Common.Models;
using PetsDiary.Presentation.ViewModels;
using PetsDiary.Presentations.Tests.Mocks;
using System;

namespace PetsDiary.Presentations.Tests.ViewModelsTests
{
    [TestClass]
    public class NoteViewModelTests
    {
        private PetsDataMock petsDataMock = new PetsDataMock();

        private MapperMock mapperMock = new MapperMock();

        public NoteViewModel BuildSUT()
        {
            return new NoteViewModel(petsDataMock.Object, mapperMock.Object);
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

            petsDataMock.SetupSaveNote();

            mapperMock.SetupMap(sut, new NoteModel());

            //Act
            sut.Save();

            //Assert
            petsDataMock.Verify(m => m.AddNote(It.IsAny<NoteModel>()), Times.Once);
        }

        [TestMethod]
        public void ShouldUpdate()
        {
            //Arrange
            var sut = BuildSUT();

            petsDataMock.SetupUpdateNote();
            mapperMock.SetupMap(sut, new NoteModel());

            sut.Id = 1;

            //Act
            sut.Update();

            //Assert
            petsDataMock.Verify(m => m.UpdateNote(It.IsAny<NoteModel>()), Times.Once);
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

            var originNote = "note";
            sut.Note = originNote;

            sut.Id = 1;
            //saves originvalues based on this property's value
            sut.IsDirty = false;

            //Act
            sut.Note = "newNote";

            sut.Cancel();

            //Assert
            Assert.AreEqual(sut.Note, originNote);
        }
    }
}