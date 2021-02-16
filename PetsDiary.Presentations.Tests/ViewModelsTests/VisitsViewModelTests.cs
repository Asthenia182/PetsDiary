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
    public class VisitsViewModelTests
    {
        private PetsDataMock petsDataMock = new PetsDataMock();
        private DialogServiceMock dialogServiceMock = new DialogServiceMock();
        private Mock<IPetDescription> petDescriptionMock = new Mock<IPetDescription>();
        private MapperMock mapperMock = new MapperMock();

        public VisitsViewModel BuildSUT()
        {
            return new VisitsViewModel(petsDataMock.Object, petDescriptionMock.Object, dialogServiceMock.Object, mapperMock.Object);
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
        public void ShouldInvokeGetVisitsOnInitialized()
        {
            //Arrange

            petDescriptionMock.SetupGet(x => x.Id).Returns(1);

            petsDataMock.SetupGetVisits(petDescriptionMock.Object.Id.Value, null);

            //Act
            var sut = BuildSUT();

            //Assert
            petsDataMock.Verify();
        }

        [TestMethod]
        public void ShouldDeleteSelectedItem()
        {
            //Arrange
            petDescriptionMock.SetupGet(x => x.Id).Returns(1);
            var sut = BuildSUT();
            var visits = GetVisitMocks().Select(x => x.Object).ToList();
            sut.Visits.AddRange(visits);
            int countBefore = visits.Count;

            petsDataMock.SetupDeleteVisitById(visits[0].Id.Value);
            dialogServiceMock.SetupShowDialog();

            //Act
            sut.DeleteCommand.Execute(visits[0].Id);

            //Assert
            Assert.IsNotNull(sut.Visits);
            Assert.AreNotEqual(countBefore, sut.Visits.Count);
            CollectionAssert.DoesNotContain(sut.Visits, visits[0]);
            CollectionAssert.Contains(sut.Visits, visits[1]);
            petsDataMock.Verify();
        }

        [TestMethod]
        public void ShouldSaveNewVisitOnDialogResultOk()
        {
            //Arrange
            petDescriptionMock.SetupGet(x => x.Id).Returns(1);
            var sut = BuildSUT();

            int countBefore = sut.Visits.Count;
            var dialogResult = new DialogResult(ButtonResult.OK);

            var newVisit = new VisitModel();
            mapperMock.SetupMap(newVisit);
            petsDataMock.SetupSaveVisit(newVisit);

            dialogServiceMock.SetupShowDialog(DialogNames.VisitDialog, dialogResult);

            //Act
            sut.AddCommand.Execute();

            //Assert
            petsDataMock.Verify(x => x.AddVisit(newVisit), Times.Once);
        }

        [TestMethod]
        public void ShouldNotSaveNewVisitOnDialogResultCancel()
        {
            //Arrange
            petDescriptionMock.SetupGet(x => x.Id).Returns(1);
            var sut = BuildSUT();

            var dialogResult = new DialogResult(ButtonResult.Cancel);

            petsDataMock.SetupSaveVisit();

            dialogServiceMock.SetupShowDialog(DialogNames.VisitDialog, dialogResult);

            //Act
            sut.AddCommand.Execute();

            //Assert

            petsDataMock.Verify(x => x.AddVisit(It.IsAny<VisitModel>()), Times.Never);
        }

        [TestMethod]
        public void ShouldUpdateVisitOnDialogResultOk()
        {
            //Arrange
            petDescriptionMock.SetupGet(x => x.Id).Returns(1);
            var sut = BuildSUT();
            var mockedVisits = GetVisitMocks();
            var mockedVisit = mockedVisits.First();
            mockedVisit.SetupUpdate();

            sut.Visits.AddRange(mockedVisits.Select(x => x.Object).ToList());

            var dialogResult = new DialogResult(ButtonResult.OK);

            dialogServiceMock.SetupShowDialog(DialogNames.VisitDialog, dialogResult);

            //Act
            sut.EditCommand.Execute(mockedVisit.Object.Id);

            //Assert
            mockedVisit.Verify(x => x.Update(), Times.Once);
        }

        [TestMethod]
        public void ShouldNotUpdateVisitOnDialogResultCancel()
        {
            //Arrange
            petDescriptionMock.SetupGet(x => x.Id).Returns(1);
            var sut = BuildSUT();
            var mockedVisits = GetVisitMocks();
            var mockedVisit = mockedVisits.First();
            mockedVisit.SetupUpdate();

            sut.Visits.AddRange(mockedVisits.Select(x => x.Object).ToList());

            var dialogResult = new DialogResult(ButtonResult.Cancel);

            dialogServiceMock.SetupShowDialog(DialogNames.VisitDialog, dialogResult);

            //Act
            sut.EditCommand.Execute(mockedVisit.Object.Id);

            //Assert
            mockedVisit.Verify(x => x.Update(), Times.Never);
        }

        [TestMethod]
        public void ShouldInvokeSetOriginValuesOnDialogResultCancel()
        {
            //Arrange
            petDescriptionMock.SetupGet(x => x.Id).Returns(1);
            var sut = BuildSUT();
            var mockedVisits = GetVisitMocks();
            var mockedVisit = mockedVisits.First();
            mockedVisit.SetupSetValuesByOrigin();

            sut.Visits.AddRange(mockedVisits.Select(x => x.Object).ToList());

            var dialogResult = new DialogResult(ButtonResult.Cancel);

            dialogServiceMock.SetupShowDialog(DialogNames.VisitDialog, dialogResult);

            //Act
            sut.EditCommand.Execute(mockedVisit.Object.Id);

            //Assert
            mockedVisit.Verify(x => x.SetValuesByOriginValues(), Times.Once);
        }

        private List<VisitViewModelMock> GetVisitMocks()
        {
            var visitViewModelMock = new VisitViewModelMock();
            visitViewModelMock.SetupGet(x => x.Id).Returns(1);

            var visitViewModelMock2 = new VisitViewModelMock();
            visitViewModelMock.SetupGet(x => x.Id).Returns(2);

            return new List<VisitViewModelMock>() {
                visitViewModelMock,
                visitViewModelMock2
            };
        }
    }
}