using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PetsDiary.Common.Constants;
using PetsDiary.Presentation;
using PetsDiary.Presentation.Interfaces;
using PetsDiary.Presentation.ViewModels;
using PetsDiary.Presentations.Tests.Mocks;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PetsDiary.Presentations.Tests.ViewModelsTests
{
    [TestClass]
    public class WeightsViewModelTests
    {
        private PetsDataMock petsDataMock = new PetsDataMock();
        private DialogServiceMock dialogServiceMock = new DialogServiceMock();
        private Mock<IPetDescription> petDescriptionMock = new Mock<IPetDescription>();
        private MapperMock mapperMock = new MapperMock();

        public WeightsViewModel BuildSUT()
        {
            return new WeightsViewModel(petsDataMock.Object, petDescriptionMock.Object, dialogServiceMock.Object, mapperMock.Object);
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
        public void ShouldInvokeGetWeightsOnInitialized()
        {
            //Arrange

            petDescriptionMock.SetupGet(x => x.Id).Returns(1);

            petsDataMock.SetupGetWeights(petDescriptionMock.Object.Id.Value, null);

            //Act
            var sut = BuildSUT();

            //Assert
            petsDataMock.Verify();
        }

        /// <summary>
        /// User must edit weights on dialog, if he deletes weight on dialog,
        /// sut must invoke delete method on viewmodel and delete from its list
        /// </summary>
        [TestMethod]
        public void ShouldInvokeDeleteWhenWeightInNotOnDialogResultList()
        {
            //Arrange
            petDescriptionMock.SetupGet(x => x.Id).Returns(1);
            var sut = BuildSUT();
            var weightsMocks = GetWeightMocks();

            //weights at the start
            sut.Weights.AddRange(weightsMocks.Select(x => x.Object).ToList());
            int countBefore = weightsMocks.Count;

            var deletedId = sut.Weights[1].Id.Value;
            petsDataMock.SetupDeleteWeightById(deletedId);

            var dialogResult = new DialogResult(ButtonResult.OK);
            //modified weights in dialog, only 1 weight is on the list, which means that another one must be deleted
            var modifiedWeights = new ObservableCollection<IWeightViewModel>() { sut.Weights[0] };
            dialogResult.Parameters.Add(nameof(sut.Weights), modifiedWeights);

            dialogServiceMock.SetupShowDialog(DialogNames.EditWeightsDialog, dialogResult);

            //Act
            sut.EditCommand.Execute();

            //Assert
            petsDataMock.Verify(x => x.DeleteWeightById(deletedId), Times.Once);
        }

        [TestMethod]
        public void ShouldSaveNewWeightWhenCanSaveIsTrue()
        {
            //Arrange
            petDescriptionMock.SetupGet(x => x.Id).Returns(1);
            var sut = BuildSUT();
            var weightMock = new WeightViewModelMock();
            weightMock.SetupSave();
            sut.CanSave = true;
            sut.InitializedWeight = weightMock.Object;

            //Act
            sut.AddCommand.Execute();

            //Assert
            weightMock.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        public void ShouldNotSaveNewWeightWhenCanSaveIsFalse()
        {
            //Arrange
            petDescriptionMock.SetupGet(x => x.Id).Returns(1);
            var sut = BuildSUT();
            var weightMock = new WeightViewModelMock();
            weightMock.SetupSave();
            sut.CanSave = false;
            sut.InitializedWeight = weightMock.Object;

            //Act
            sut.AddCommand.Execute();

            //Assert
            weightMock.Verify(x => x.Save(), Times.Never);
        }

        private List<WeightViewModelMock> GetWeightMocks()
        {
            var viewModelMock = new WeightViewModelMock();
            viewModelMock.SetupGet(x => x.Id).Returns(1);

            var viewModelMock1 = new WeightViewModelMock();
            viewModelMock1.SetupGet(x => x.Id).Returns(2);

            return new List<WeightViewModelMock>() {
                viewModelMock,
                viewModelMock1
            };
        }
    }
}