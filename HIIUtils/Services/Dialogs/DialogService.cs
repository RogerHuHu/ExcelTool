using HIIUtils.IoC;
using HIIUtils.MVVM;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace HIIUtils.Services.Dialogs
{
    public class DialogService : IDialogService
    {
        protected readonly IContainerExtension _containerExtension;

        public DialogService(IContainerExtension containerExtension)
        {
            _containerExtension = containerExtension;
        }

        public void Show(string name, IDialogParameters parameters, Action<IDialogResult> callback)
        {
            ShowDialogInternal(name, parameters, callback, false);
        }

        public void Show(string name, IDialogParameters parameters, Action<IDialogResult> callback, string windowName)
        {
            ShowDialogInternal(name, parameters, callback, false, windowName);
        }

        public void ShowDialog(string name, IDialogParameters parameters, Action<IDialogResult> callback)
        {
            ShowDialogInternal(name, parameters, callback, true);
        }

        public void ShowDialog(string name, IDialogParameters parameters, Action<IDialogResult> callback, string windowName)
        {
            ShowDialogInternal(name, parameters, callback, true, windowName);
        }

        /// <summary>
        /// 配置 Window 的 Content
        /// </summary>
        /// <param name="dialogName"></param>
        /// <param name="dialogWindow"></param>
        /// <param name="parameters"></param>
        protected virtual void ConfigureDialogWindowContent(string dialogName, IDialogWindow dialogWindow, IDialogParameters parameters)
        {
            if (!(_containerExtension.Resolve<object>(dialogName) is FrameworkElement frameworkElement))
                throw new NullReferenceException("A dialog's content must be a FrameworkElement");

            MvvmHelpers.AutowireViewModel(frameworkElement);
            if (!(frameworkElement.DataContext is IDialogAware dialogAware))
                throw new NullReferenceException("A dialog's ViewModel must implement the IDialogAware interface");

            ConfigureDialogWindowProperties(dialogWindow, frameworkElement, dialogAware);
            MvvmHelpers.ViewAndViewModelAction(dialogAware, delegate (IDialogAware d)
            {
                d.OnDialogOpened(parameters);
            });
        }

        /// <summary>
        /// 配置 Window 事件
        /// </summary>
        /// <param name="dialogWindow"></param>
        /// <param name="callback"></param>
        protected virtual void ConfigureDialogWindowEvents(IDialogWindow dialogWindow, Action<IDialogResult> callback)
        {
            // ViewModel 请求关闭事件的处理方法
            void RequestClose(IDialogResult result)
            {
                dialogWindow.Result = result;
                dialogWindow.Close();
            }

            // Window 加载事件的处理方法
            void Loaded(object sender, RoutedEventArgs e)
            {
                dialogWindow.Loaded -= Loaded; // 调用一次就移除
                // 监听 ViewModel 请求关闭事件
                dialogWindow.GetDialogViewModel().RequestClose += RequestClose;
            }

            // Window 关闭前事件的处理方法
            void Closing(object sender, CancelEventArgs e)
            {
                if (!dialogWindow.GetDialogViewModel().CanCloseDialog())
                    e.Cancel = true;
            }

            // Window 关闭后事件的处理方法
            void Closed(object sender, EventArgs e)
            {
                // 移除事件监听
                dialogWindow.Closed -= Closed;
                dialogWindow.Closing -= Closing;
                dialogWindow.GetDialogViewModel().RequestClose -= RequestClose;
                // 通知 ViewModel，对话框关闭
                dialogWindow.GetDialogViewModel().OnDialogClosed();
                // 调用回调函数
                if (dialogWindow.Result == null) dialogWindow.Result = new DialogResult();
                callback?.Invoke(dialogWindow.Result);
                // 清除 Window 的相关引用
                dialogWindow.DataContext = null;
                dialogWindow.Content = null;
            }

            dialogWindow.Loaded += Loaded;
            dialogWindow.Closing += Closing;
            dialogWindow.Closed += Closed;
        }

        protected virtual void ConfigureDialogWindowProperties(IDialogWindow dialogWindow, FrameworkElement dialogView, IDialogAware dialogViewModel)
        {
            Style windowStyle = Dialog.GetWindowStyle(dialogView);
            if (windowStyle != null)
                dialogWindow.Style = windowStyle;

            dialogWindow.Content = dialogView;
            dialogWindow.DataContext = dialogViewModel;
            if (dialogWindow.Owner == null)
                dialogWindow.Owner = Application.Current?.Windows.OfType<Window>().FirstOrDefault((Window w) => w.IsActive);
        }

        /// <summary>
        /// 获取对话框所在的 Window
        /// </summary>
        /// <param name="windowName"></param>
        /// <returns></returns>
        protected virtual IDialogWindow CreateDialogWindow(string windowName)
        {
            if (string.IsNullOrWhiteSpace(windowName))
                return _containerExtension.Resolve<IDialogWindow>();

            return _containerExtension.Resolve<IDialogWindow>(windowName);
        }

        protected virtual void ShowDialogWindow(IDialogWindow dialogWindow, bool isModal)
        {
            if (isModal)
                dialogWindow.ShowDialog();
            else
                dialogWindow.Show();
        }

        private void ShowDialogInternal(string name, IDialogParameters parameters, Action<IDialogResult> callback, bool isModal,
                                        string windowName = null)
        {
            if (parameters == null)
                parameters = new DialogParameters();

            // 1. 获取对话框所在的 Window
            IDialogWindow dialogWindow = CreateDialogWindow(windowName);
            // 2. 配置 Window 事件
            ConfigureDialogWindowEvents(dialogWindow, callback);
            // 3. 配置 Window 的 Content
            ConfigureDialogWindowContent(name, dialogWindow, parameters);
            // 4. 显示窗口
            ShowDialogWindow(dialogWindow, isModal);
        }
    }
}