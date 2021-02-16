using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PetsDiary.Presentation;
using PetsDiary.Presentation.ViewModels;
using PetsDiary.Presentations.Tests.Mocks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PetsDiary.Presentations.Tests.ViewModelsTests
{
    [TestClass]
    public class NotesViewModelTests
    {
        private PetsDataMock petsDataMock = new PetsDataMock();
        private DialogServiceMock dialogServiceMock = new DialogServiceMock();
        private Mock<IPetDescription> petDescriptionMock = new Mock<IPetDescription>();
        private MapperMock mapperMock = new MapperMock();

        public NotesViewModel BuildSUT()
        {
            return new NotesViewModel(petsDataMock.Object, petDescriptionMock.Object, dialogServiceMock.Object, mapperMock.Object);
        }

        public NotesViewModelTests()
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
            petsDataMock.SetupGetNotes(petDescriptionMock.Object.Id.Value, null);

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
            var notes = GetNotesMocks().Select(x => x.Object).ToList();
            sut.Notes.AddRange(notes);
            int countBefore = notes.Count;

            petsDataMock.SetupDeleteNoteById(notes[0].Id.Value);
            dialogServiceMock.SetupShowDialog();

            //Act
            sut.DeleteCommand.Execute(notes[0].Id);

            //Assert
            Assert.IsNotNull(sut.Notes);
            Assert.AreNotEqual(countBefore, sut.Notes.Count);
            CollectionAssert.DoesNotContain(sut.Notes, notes[0]);
            CollectionAssert.Contains(sut.Notes, notes[1]);
            petsDataMock.Verify();
        }

        [TestMethod]
        public void ShouldAddNewVaccination()
        {
            //Arrange
            var sut = BuildSUT();

            int countBefore = sut.Notes.Count;

            //Act
            sut.AddCommand.Execute();

            //Assert
            Assert.AreNotEqual(countBefore, sut.Notes.Count);
        }

        [TestMethod]
        public void ShouldSaveNewVaccination()
        {
            //Arrange
            var sut = BuildSUT();

            var noteViewModelMock = new NoteViewModelMock();
            noteViewModelMock.SetupSave();

            sut.Notes.Add(noteViewModelMock.Object);

            //Act
            sut.SaveCommand.Execute(noteViewModelMock.Object.Id);

            //Assert
            noteViewModelMock.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        public void ShouldUpdateVaccination()
        {
            //Arrange
            var sut = BuildSUT();

            var noteViewModelMock = new NoteViewModelMock();
            noteViewModelMock.SetupGet(x => x.Id).Returns(1);
            noteViewModelMock.SetupUpdate();

            sut.Notes.Add(noteViewModelMock.Object);

            //Act
            sut.SaveCommand.Execute(noteViewModelMock.Object.Id);

            //Assert
            noteViewModelMock.Verify(x => x.Update(), Times.Once);
        }

        [TestMethod]
        public void ShouldInvokeCancelWhenNoteHasId()
        {
            //Arrange
            var sut = BuildSUT();
            var mockedVNote = new NoteViewModelMock();
            mockedVNote.SetupGet(x => x.Id).Returns(1);
            mockedVNote.SetupCancel();

            sut.Notes.Add(mockedVNote.Object);

            //Act
            sut.CancelCommand.Execute(mockedVNote.Object.Id);

            //Assert
            mockedVNote.Verify(x => x.Cancel(), Times.Once);
        }

        [TestMethod]
        public void ShouldRemoveFromNotesWhenNoteHasNotIdOnCancel()
        {
            //Arrange
            var sut = BuildSUT();
            var mockedNote = new NoteViewModelMock();

            sut.Notes.Add(mockedNote.Object);
            var countBefore = sut.Notes.Count;

            //Act
            sut.CancelCommand.Execute(mockedNote.Object.Id);

            //Assert
            Assert.AreNotEqual(countBefore, sut.Notes.Count);
        }

        private List<NoteViewModelMock> GetNotesMocks()
        {
            var viewmodelMock = new NoteViewModelMock();
            viewmodelMock.SetupGet(x => x.Id).Returns(1);

            var viewModelMock2 = new NoteViewModelMock();
            viewModelMock2.SetupGet(x => x.Id).Returns(2);

            return new List<NoteViewModelMock>() {
                viewmodelMock,
                viewModelMock2
            };
        }
    }
}