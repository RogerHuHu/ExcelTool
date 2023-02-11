namespace HIIUtils.Services.Dialogs
{
    public static class IDialogWindowExtensions
    {
        internal static IDialogAware GetDialogViewModel(this IDialogWindow dialogWindow)
        {
            return (IDialogAware)dialogWindow.DataContext;
        }
    }
}