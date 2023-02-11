using System;

namespace HIIUtils.Services.Dialogs
{
    public interface IDialogAware
    {
        /// <summary>
        /// 确定对话框是否可以被关闭
        /// </summary>
        /// <returns>若对话框可以被关闭，返回 <c>true</c>; 否则返回 <c>false</c></returns>
        bool CanCloseDialog();

        /// <summary>
        /// 当对话框被关闭后调用
        /// </summary>
        void OnDialogClosed();

        /// <summary>
        /// 在对话框打开时调用
        /// </summary>
        /// <param name="parameters">对话框参数</param>
        void OnDialogOpened(IDialogParameters parameters);

        string Title { get; }

        /// <summary>
        /// 指示关闭对话框
        /// </summary>
        event Action<IDialogResult> RequestClose;
    }
}