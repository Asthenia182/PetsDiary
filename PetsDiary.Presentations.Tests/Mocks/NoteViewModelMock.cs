using Moq;
using PetsDiary.Presentation.Interfaces;

namespace PetsDiary.Presentations.Tests.Mocks
{
    public class NoteViewModelMock : Mock<INoteViewModel>
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

        public void SetupCancel()
        {
            Setup(x => x.Cancel())
                .Verifiable();
        }
    }
}