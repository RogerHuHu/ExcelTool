using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HIIUtils.MVVM
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region UI更新接口

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }

        /// <summary>
        /// 更新 UI 中对应属性的值，当在对应属性的 set 方法中调用时，可以省略参数
        /// </summary>
        /// <param name="propertyName">属性名</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 更新 UI 中对应属性的值，当在对应属性的 set 方法中调用时，可以省略参数
        /// </summary>
        /// <param name="propertyName">属性名</param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 更新 UI 中对应属性的值，更新前会判断新值与旧值是否相等，若相等则不更新
        /// 当在对应属性的 set 方法中调用时，可以省略 propertyName 参数
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="lastValue">属性旧值</param>
        /// <param name="value">新值</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        protected virtual bool SetProperty<T>(ref T lastValue, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(lastValue, value))
                return false;
            lastValue = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// 更新 UI 中对应属性的值，当 overwrite 为 true 时，无论新值和旧值是否相等，都更新，否则当新值和旧值不相等时才更新
        /// 当在对应属性的 set 方法中调用时，可以省略 propertyName 参数
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="lastValue">属性旧值</param>
        /// <param name="value">新值</param>
        /// <param name="overwrite">决定是否需要判断新值与旧值是否相等</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        protected virtual bool SetProperty<T>(ref T lastValue, T value, bool overwrite, [CallerMemberName] string propertyName = null)
        {
            if (!overwrite && EqualityComparer<T>.Default.Equals(lastValue, value))
                return false;
            lastValue = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion UI更新接口
    }
}