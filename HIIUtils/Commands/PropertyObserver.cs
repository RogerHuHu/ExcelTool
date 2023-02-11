using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace HIIUtils.Commands
{
    internal class PropertyObserver
    {
        private readonly Action _action;

        public PropertyObserver(Expression propertyExpression, Action action)
        {
            _action = action;
            SubscribeListeners(propertyExpression);
        }

        private void SubscribeListeners(Expression propertyExpression)
        {
            var propNameStack = new Stack<PropertyInfo>();
            while (propertyExpression is MemberExpression temp)
            {
                propertyExpression = temp.Expression;
                propNameStack.Push(temp.Member as PropertyInfo);
            }

            if (!(propertyExpression is ConstantExpression constantExpression))
                throw new NotSupportedException("Operation not supported for the given expression type." +
                                                "Only MemberExpression and ConstantExpression are currently supported.");

            var propObserveNodeRoot = new PropertyObserverNode(propNameStack.Pop(), _action);
            PropertyObserverNode previousNode = propObserveNodeRoot;
            foreach (var propName in propNameStack)
            {
                var currentNode = new PropertyObserverNode(propName, _action);
                previousNode.Next = currentNode;
                previousNode = currentNode;
            }

            object propOwnerObject = constantExpression.Value;

            if (!(propOwnerObject is INotifyPropertyChanged inpcObject))
                throw new InvalidOperationException("Trying to subscribe PropertyChanged listener in object that" +
                                                    $"owns '{propObserveNodeRoot.PropertyInfo.Name}' property, but the object does not implements INotifyPropertyChanged");

            propObserveNodeRoot.SubscribeListenerFor(inpcObject);
        }

        internal static PropertyObserver Observes<T>(Expression<Func<T>> propertyExpression, Action action)
        {
            return new PropertyObserver(propertyExpression.Body, action);
        }
    }
}