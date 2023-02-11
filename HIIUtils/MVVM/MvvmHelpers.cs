using System;
using System.Windows;

namespace HIIUtils.MVVM
{
    public static class MvvmHelpers
    {
        internal static void AutowireViewModel(object viewOrViewModel)
        {
            if (viewOrViewModel is FrameworkElement frameworkElement &&
               frameworkElement.DataContext == null &&                            // 未绑定 viewModel
               !ViewModelLocator.GetAutowireViewModel(frameworkElement).HasValue) // 未设置过依赖属性 AutowireViewModel
            {
                ViewModelLocator.SetAutowireViewModel(frameworkElement, true);
            }
        }

        /// <summary>
        /// 在 view 和 viewModel 上执行<see cref="Action{T}"/>
        /// </summary>
        /// <typeparam name="T">action 的参数类型</typeparam>
        /// <param name="view"></param>
        /// <param name="action"></param>
        /// <remarks>view 和 viewModel 分别判断是否实现 T，如果实现则执行 action</remarks>
        public static void ViewAndViewModelAction<T>(object view, Action<T> action) where T : class
        {
            if (view is T val)
                action(val);

            if (view is FrameworkElement frameworkElement)
            {
                if (frameworkElement.DataContext is T val2)
                    action(val2);
            }
        }

        /// <summary>
        /// 从 view 和 viewModel 获取指定类型的实现
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="view"></param>
        /// <returns></returns>
        /// <remarks>先判断 view 再判断 viewModel</remarks>
        public static T GetImplementerFromViewOrViewModel<T>(object view) where T : class
        {
            if (view is T val)
                return val;

            if (view is FrameworkElement frameworkElement)
            {
                if (frameworkElement.DataContext is T val2)
                    return val2;
            }

            return null;
        }
    }
}