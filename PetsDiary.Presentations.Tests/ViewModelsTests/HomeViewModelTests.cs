using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PetsDiary.Common.Constants;
using PetsDiary.Common.Models;
using PetsDiary.Presentation;
using PetsDiary.Presentation.Constants;
using PetsDiary.Presentation.ViewModels;
using PetsDiary.Presentation.Views;
using PetsDiary.Presentations.Tests.Mocks;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PetsDiary.Presentations.Tests.ViewModelsTests
{
    [TestClass]
    public class HomeViewModelTests
    {
        private PetsDataMock petsDataMock = new PetsDataMock();
        private DialogServiceMock dialogServiceMock = new DialogServiceMock();
        private RegionManagerMock regionManagerMock = new RegionManagerMock();
        private Mock<IPetDescription> petDescriptionMock = new Mock<IPetDescription>();

        private string name = "name";

        public HomeViewModel BuildSUT()
        {
            return new HomeViewModel(regionManagerMock.Object, petsDataMock.Object, petDescriptionMock.Object, dialogServiceMock.Object);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            petsDataMock.Reset();
            dialogServiceMock.Reset();
            regionManagerMock.Reset();
            petDescriptionMock.Reset();
        }

        [TestMethod]
        public void ShouldInvokeGetPetsOnInitialized()
        {
            //Arrange
            petsDataMock
                .Setup(x => x.GetPets())
                .Verifiable();

            //Act
            var sut = BuildSUT();

            //Assert
            petsDataMock.Verify();
        }

        [TestMethod]
        public void ShouldCreatePetDescriptionsByPetModelsOnInitialized()
        {
            //Arrange
            var petModels = new List<PetModel>() { new PetModel() { Id = 1, Name = name } };
            petsDataMock.SetupGetPets(petModels);

            //Act
            var sut = BuildSUT();

            //Assert
            Assert.IsNotNull(sut.Pets);
            Assert.AreEqual(petModels.Count, sut.Pets.Count);
            Assert.AreEqual(petModels[0].Id, sut.Pets[0].Id);
            Assert.AreEqual(petModels[0].Name, sut.Pets[0].Name);
        }

        [TestMethod]
        public void ShouldDeleteSelectedItem()
        {
            //Arrange
            var sut = BuildSUT();
            var pets = GetPets();
            sut.Pets.AddRange(pets);
            int countBefore = pets.Count;

            petsDataMock.SetupDeletePetById(pets[0].Id.Value);
            dialogServiceMock.SetupShowDialog();

            //Act
            sut.SelectedItem = sut.Pets[0];
            sut.DeleteCommand.Execute();

            //Assert
            Assert.IsNotNull(sut.Pets);
            Assert.AreNotEqual(countBefore, sut.Pets.Count);
            CollectionAssert.DoesNotContain(sut.Pets, pets[0]);
            CollectionAssert.Contains(sut.Pets, pets[1]);
            petsDataMock.Verify();
        }

        [TestMethod]
        public void ShouldNavigateToPetViewOnAdd()
        {
            //Arrange
            var sut = BuildSUT();

            var navigationParams = new NavigationParameters
            {
                { ParametersKeys.IsNew, true}
            };

            regionManagerMock.SetupRequestNavigate(RegionNames.Content, ViewNames.Pet, navigationParams);

            //Act
            sut.AddCommand.Execute();

            //Assert
            regionManagerMock.Verify();
        }

        [TestMethod]
        public void ShouldRegisterNavigationBarViewOnNavigateFrom()
        {
            //Arrange
            var sut = BuildSUT();

            regionManagerMock.SetupRegisterViewWithRegion(RegionNames.Navigation, typeof(NavigationBarView));

            //Act
            sut.OnNavigatedFrom(null);

            //Assert
            regionManagerMock.Verify();
        }

        [TestMethod]
        public void ShouldSetPetDescriptionBySelectedItemOnOpen()
        {
            //Arrange
            var sut = BuildSUT();

            sut.Pets.AddRange(GetPets());
            sut.SelectedItem = sut.Pets[0];
            petDescriptionMock.SetupProperty(x => x.Id);
            petDescriptionMock.SetupProperty(x => x.Name);

            //Act
            sut.OpenCommand.Execute();

            //Assert
            Assert.AreEqual(sut.SelectedItem.Name, petDescriptionMock.Object.Name);
            Assert.AreEqual(sut.SelectedItem.Id, petDescriptionMock.Object.Id);
        }

        [TestMethod]
        public void ShouldClearPetDescriptionOnNavigatedTo()
        {
            //Arrange
            var sut = BuildSUT();

            petDescriptionMock.SetupProperty(x => x.Id);
            petDescriptionMock.SetupProperty(x => x.Name);
            int id = 1;
            petDescriptionMock.Object.Id = id;
            petDescriptionMock.Object.Name = name;

            regionManagerMock.SetupGet(x => x.Regions).Returns(new Mock<IRegionCollection>().Object);

            //Act
            sut.OnNavigatedTo(null);

            //Assert
            Assert.AreNotEqual(id, petDescriptionMock.Object.Id);
            Assert.AreNotEqual(name, petDescriptionMock.Object.Name);
        }

        private List<IPetDescription> GetPets()
        {
            return new List<IPetDescription>() {
                new PetDescription() { Id = 1, Name= name },
                new PetDescription() { Id = 2, Name= name }
            };
        }
    }
}