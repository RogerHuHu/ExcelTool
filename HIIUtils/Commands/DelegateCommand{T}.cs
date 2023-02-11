using HIIUtils.Properties;
using System;
using System.Linq.Expressions;

namespace HIIUtils.Commands
{
    public class DelegateCommand<T> : DelegateCommandBase
    {
        private readonly Action<T> _executeMethod;
        private Func<T, bool> _canExecuteMethod;

        public DelegateCommand(Action<T> executeMethod) : this(executeMethod, o => true)
        {
        }

        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod) : base()
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException(nameof(executeMethod), Resources.DelegateCommandDelegatesCannotBeNull);

            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }

        public bool CanExecute(T parameter)
        {
            return _canExecuteMethod(parameter);
        }

        public void Execute(T parameter)
        {
            _executeMethod(parameter);
        }

        /// <summary>
        /// 观察一个用于确定当前命令是否能执行的属性，并且如果该属性的实体实现了 INotifyPropertyChanged, 当该属性改变时自动调用
        /// DelegateCommandBase.RaiseCanExecuteChanged
        /// </summary>
        /// <param name="canExecuteExpression"></param>
        /// <returns></returns>
        public DelegateCommand<T> ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
        {
            Expression<Func<T, bool>> expression = Expression.Lambda<Func<T, bool>>(canExecuteExpression.Body,
                                                                                    Expression.Parameter(typeof(T), "o"));

            _canExecuteMethod = expression.Compile();
            ObservesPropertyInternal(canExecuteExpression);
            return this;
        }

        public DelegateCommand<T> ObservesProperty<PType>(Expression<Func<PType>> propertyExpression)
        {
            ObservesPropertyInternal(propertyExpression);
            return this;
        }

        protected override bool CanExecute(object parameter)
        {
            if (parameter == null) return true;
            return CanExecute((T)parameter);
        }

        protected override void Execute(object parameter)
        {
            if (parameter != null)
                Execute((T)parameter);
        }
    }
}