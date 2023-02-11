using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HIIUtils.MVVM
{
    /// <summary>
    /// 定义附加属性
    /// </summary>
    public static class ViewModelLocator
    {
        public static bool? GetAutowireViewModel(DependencyObject obj)
        {
            return (bool?)obj.GetValue(AutowireViewModelProperty);
        }

        public static void SetAutowireViewModel(DependencyObject obj, bool? value)
        {
            obj.SetValue(AutowireViewModelProperty, value);
        }

        public static readonly DependencyProperty AutowireViewModelProperty =
            DependencyProperty.RegisterAttached("AutowireViewModel", typeof(bool?), typeof(ViewModelLocator),
                                                new PropertyMetadata(null, AutowireViewModelChanged));

        /// <summary>
        /// 依赖属性 AutowireViewModel 改变事件的处理
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void AutowireViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(!DesignerProperties.GetIsInDesignMode(d))
            {
                bool? flag = (bool?)e.NewValue;
                if (flag.HasValue && flag.Value) // 置为 true 时，自动绑定
                    ViewModelLocationProvider.AutowireViewModelChanged(d, Bind);
            }
        }

        /// <summary>
        /// 将 viewModel 注入到 view 的 DataContext 中
        /// </summary>
        /// <param name="view"></param>
        /// <param name="viewModel"></param>
        private static void Bind(object view, object viewModel)
        {
            if (view is FrameworkElement frameworkElement)
                frameworkElement.DataContext = viewModel;
        }
    }
}
