using System;

namespace HIIUtils.Services.Dialogs
{
    public interface IDialogService
    {
        void ShowDialog(string name, IDialogParameters parameters, Action<IDialogResult> callback);

        void ShowDialog(string name, IDialogParameters parameters, Action<IDialogResult> callback, string windowName);

        void Show(string name, IDialogParameters parameters, Action<IDialogResult> callBack);

        void Show(string name, IDialogParameters parameters, Action<IDialogResult> callBack, string windowName);
    }
}