using Moq;
using PetsDiary.Presentation.Interfaces;

namespace PetsDiary.Presentations.Tests.Mocks
{
    public class WeightViewModelMock : Mock<IWeightViewModel>
    {
        public void SetupUpdate()
        {
            Setup(x => x.Update())
                .Verifiable();
        }

        public void SetupSave()
        {
            Setup(x => x.Save())
                .Verifiable();
        }

        public void SetupSetValuesByOrigin()
        {
            Setup(x => x.SetValuesByOriginValues())
                .Verifiable();
        }
    }
}