using HIIUtils.Properties;
using System;
using System.Linq.Expressions;

namespace HIIUtils.Commands
{
    public class DelegateCommand : DelegateCommandBase
    {
        private readonly Action _executeMethod;
        private Func<bool> _canExecuteMethod;

        public DelegateCommand(Action executeMethod) : this(executeMethod, () => true)
        {
        }

        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod) : base()
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException(nameof(executeMethod), Resources.DelegateCommandDelegatesCannotBeNull);

            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }

        public void Execute()
        {
            _executeMethod();
        }

        public bool CanExecute()
        {
            return _canExecuteMethod();
        }

        protected override bool CanExecute(object parameter)
        {
            return CanExecute();
        }

        protected override void Execute(object parameter)
        {
            Execute();
        }

        /// <summary>
        /// 观察一个实现 INotifyPropertyChanged 的实体的属性，并且当该属性改变时自动调用 DelegateCommandBase.RaiseCanExecuteChanged
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public DelegateCommand ObservesProperty<T>(Expression<Func<T>> propertyExpression)
        {
            ObservesPropertyInternal(propertyExpression);
            return this;
        }

        /// <summary>
        /// 观察一个用于确定当前命令是否能执行的属性，并且如果该属性的实体实现了 INotifyPropertyChanged，当该属性改变时自动调用 DelegateCommandBase.RaiseCanExecuteChanged
        /// </summary>
        /// <param name="canExecuteExpression"></param>
        /// <returns></returns>
        public DelegateCommand ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
        {
            _canExecuteMethod = canExecuteExpression.Compile();
            ObservesPropertyInternal(canExecuteExpression);
            return this;
        }
    }
}