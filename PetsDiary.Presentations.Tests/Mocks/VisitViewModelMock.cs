using Moq;
using PetsDiary.Presentation.Interfaces;

namespace PetsDiary.Presentations.Tests.Mocks
{
    public class VisitViewModelMock : Mock<IVisitViewModel>
    {
        public void SetupUpdate()
        {
            Setup(x => x.Update())
                .Verifiable();
        }

        public void SetupSetValuesByOrigin()
        {
            Setup(x => x.SetValuesByOriginValues())
                .Verifiable();
        }
    }
}