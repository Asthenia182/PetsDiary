using Moq;
using Prism.Services.Dialogs;
using System;

namespace PetsDiary.Presentations.Tests.Mocks
{
    public class DialogServiceMock : Mock<IDialogServiceMock>
    {
        public void SetupShowDialog()
        {
           Setup(x => x.ShowDialog(It.IsAny<string>(), It.IsAny<IDialogParameters>(), It.IsAny<Action<IDialogResult>>()))
                       .Callback<string, IDialogParameters, Action<IDialogResult>>((name, dp, cb) =>
                           cb.Invoke(new DialogResult(ButtonResult.OK)));
        }

        public void SetupShowDialog(string name, DialogResult dialogResult)
        {
            Setup(x => x.ShowDialog(name, It.IsAny<IDialogParameters>(), It.IsAny<Action<IDialogResult>>()))
                        .Callback<string, IDialogParameters, Action<IDialogResult>>((name, dp, cb) =>
                            cb.Invoke(dialogResult));
        }
    }
}