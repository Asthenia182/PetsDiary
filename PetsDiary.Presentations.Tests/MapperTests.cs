using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetsDiary.Presentation.Utilities;

namespace PetsDiary.Presentations.Tests
{
    [TestClass]
    public class MapperTests
    {
        [TestMethod]
        public void ShouldConfigurationBeValid()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<MappingProfile>();
                cfg.DisableConstructorMapping();
            });
            config.AssertConfigurationIsValid();
        }
    }
}