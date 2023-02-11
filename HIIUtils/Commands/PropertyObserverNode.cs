using System;
using System.ComponentModel;
using System.Reflection;

namespace HIIUtils.Commands
{
    internal class PropertyObserverNode
    {
        #region 变量

        private readonly Action _action;
        private INotifyPropertyChanged _inpcObject;

        #endregion 变量

        #region 构造函数

        public PropertyObserverNode(PropertyInfo propertyInfo, Action action)
        {
            PropertyInfo = propertyInfo ?? throw new ArgumentException(nameof(propertyInfo));
            _action = () =>
            {
                action?.Invoke();
                if (Next == null) return;
                Next.UnSubscribeListener();
                GenerateNextNode();
            };
        }

        #endregion 构造函数

        #region 属性

        public PropertyInfo PropertyInfo { get; }

        public PropertyObserverNode Next { get; set; }

        #endregion 属性

        #region 方法

        public void SubscribeListenerFor(INotifyPropertyChanged inpcObject)
        {
            _inpcObject = inpcObject;
            _inpcObject.PropertyChanged += OnPropertyChanged;

            if (Next != null)
                GenerateNextNode();
        }

        private void GenerateNextNode()
        {
            var nextProperty = PropertyInfo.GetValue(_inpcObject);
            if (nextProperty == null) return;
            if (!(nextProperty is INotifyPropertyChanged nextInpcObject))
                throw new InvalidOperationException("Trying to subscribe PropertyChanged listener in object that" +
                                                    $"owns '{Next.PropertyInfo.Name}' property, but the object does not implements INotifyPropertyChanged");

            Next.SubscribeListenerFor(nextInpcObject);
        }

        private void UnSubscribeListener()
        {
            if (_inpcObject != null)
                _inpcObject.PropertyChanged -= OnPropertyChanged;

            Next?.UnSubscribeListener();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e?.PropertyName == PropertyInfo.Name || string.IsNullOrEmpty(e?.PropertyName))
            {
                _action?.Invoke();
            }
        }

        #endregion 方法
    }
}