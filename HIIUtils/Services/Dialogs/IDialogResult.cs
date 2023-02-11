namespace HIIUtils.Services.Dialogs
{
    /// <summary>
    /// 对话框返回的结果接口
    /// </summary>m
    public interface IDialogResult
    {
        /// <summary>
        /// 对话框返回的参数
        /// </summary>
        IDialogParameters Parameters { get; }

        /// <summary>
        /// 对话结果
        /// </summary>
        ButtonResult Result { get; }
    }
}