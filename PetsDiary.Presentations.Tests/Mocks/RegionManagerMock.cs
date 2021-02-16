using Moq;
using Prism.Regions;
using System;

namespace PetsDiary.Presentations.Tests.Mocks
{
    public class RegionManagerMock : Mock<IRegionManagerMock>
    {
        public void SetupRequestNavigate(string regionName, string viewName, NavigationParameters navigationParameters)
        {
            Setup(x => x.RequestNavigate(regionName, viewName, navigationParameters))
                .Verifiable();
        }

        public void SetupRegisterViewWithRegion(string regionName, Type viewType)
        {
            Setup(x => x.RegisterViewWithRegion(regionName, viewType))
                .Verifiable();
        }
    }
}