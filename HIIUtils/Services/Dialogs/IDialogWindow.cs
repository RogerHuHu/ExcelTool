using System;
using System.ComponentModel;
using System.Windows;

namespace HIIUtils.Services.Dialogs
{
    public interface IDialogWindow
    {
        object Content { get; set; }

        void Close();

        Window Owner { get; set; }

        bool? ShowDialog();

        void Show();

        object DataContext { get; set; }

        event RoutedEventHandler Loaded;

        event EventHandler Closed;

        event CancelEventHandler Closing;

        IDialogResult Result { get; set; }
        Style Style { get; set; }
    }
}