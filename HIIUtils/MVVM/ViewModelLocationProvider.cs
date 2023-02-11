using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace HIIUtils.MVVM
{
    /// <summary>
    /// 为附加属性 AutowireViewModelChanged 为 <c>trut</c> 的 view 定位 viewModel 并注入到 view 的 DataContext 中
    /// </summary>
    public static class ViewModelLocationProvider
    {
        // 包含所有 view 注册的，用于产生 viewModel 的工厂实例
        private static Dictionary<string, Func<object>> _factories = new Dictionary<string, Func<object>>();

        // 包含所有 view 注册的，用于产生 viewModel Type 的工厂实例
        private static Dictionary<string, Type> _typeFactories = new Dictionary<string, Type>();

        private static Func<Type, object> _defaultViewModelFactory = type => Activator.CreateInstance(type);
        private static Func<object, Type, object> _defaultViewModelFactoryWithViewParameter;

        private static Func<Type, Type> _defaultViewTypeToViewModelTypeResolver = viewType =>
        {
            // 假设 view 和 viewModel 在同一个程序集，且前缀相同，分别在 .Views 和 .ViewModels 命名空间
            string fullName = viewType.FullName;
            fullName = fullName.Replace(".Views.", ".ViewModels.");
            string fullName2 = viewType.GetTypeInfo().Assembly.FullName;
            string arg = (fullName.EndsWith("View") ? "Model" : "ViewModel");
            return Type.GetType(string.Format(CultureInfo.InvariantCulture, "{0}{1}, {2}", fullName, arg, fullName2));
        };

        /// <summary>
        /// 设置默认的 viewModel 工厂
        /// </summary>
        /// <param name="viewModelFactory"></param>
        public static void SetDefaultViewModelFactory(Func<Type, object> viewModelFactory)
        {
            _defaultViewModelFactory = viewModelFactory;
        }

        /// <summary>
        /// 设置默认的 viewModel 工厂
        /// </summary>
        /// <param name="viewModelFactory"></param>
        public static void SetDefaultViewModelFactory(Func<object, Type, object> viewModelFactory)
        {
            _defaultViewModelFactoryWithViewParameter = viewModelFactory;
        }

        /// <summary>
        /// 设置默认的 viewType 到 viewModelType 的解析器
        /// </summary>
        /// <param name="viewTypeToViewModelTypeResolver"></param>
        public static void SetDefaultViewTypeToViewModelTypeResolver(Func<Type, Type> viewTypeToViewModelTypeResolver)
        {
            _defaultViewTypeToViewModelTypeResolver = viewTypeToViewModelTypeResolver;
        }

        /// <summary>
        /// 自动查找指定 view 对应的 viewModel 并建立绑定关系
        /// </summary>
        /// <param name="view"></param>
        /// <param name="setDataContextCallback">用于建立 view 和 viewModel 绑定关系的委托方法</param>
        public static void AutowireViewModelChanged(object view, Action<object, object> setDataContextCallback)
        {
            // 获取 viewModel
            object viewModel = GetViewModelForView(view); // 1. 根据 _factories
            if (viewModel == null)                         // 2. 根据 defaultFactory
            {
                Type viewModelType = GetViewModelTypeForView(view.GetType()); // a. 根据 _typeFactories
                if (viewModelType == null)
                {
                    viewModelType = _defaultViewTypeToViewModelTypeResolver(view.GetType()); // b. 根据 defaultResolver
                    if (viewModelType == null) return;
                }

                viewModel = _defaultViewModelFactoryWithViewParameter != null ?
                            _defaultViewModelFactoryWithViewParameter(view, viewModelType) : // 1. 带参数的 view
                            _defaultViewModelFactory(viewModelType);                         // 2. 不带参数的 view
            }

            // 注入
            setDataContextCallback(view, viewModel);
        }

        /// <summary>
        /// 获取指定 view 对应的 viewModel 的类型
        /// </summary>
        /// <param name="viewType"></param>
        /// <returns></returns>
        private static Type GetViewModelTypeForView(Type viewType)
        {
            _typeFactories.TryGetValue(viewType.ToString(), out Type viewModelType);
            return viewModelType;
        }

        /// <summary>
        /// 获取指定 view 对应的 viewModel
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        private static object GetViewModelForView(object view)
        {
            if (_factories.TryGetValue(view.GetType().ToString(), out Func<object> factory))
                return factory();
            return null;
        }

        #region 注册

        /// <summary>
        /// 为指定 viewType 注册 viewModel 工厂
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        public static void Register<T>(Func<object> factory)
        {
            Register(typeof(T).ToString(), factory);
        }

        /// <summary>
        /// 为指定 viewType 注册 viewModelType
        /// </summary>
        /// <param name="viewTypeName"></param>
        /// <param name="factory"></param>
        public static void Register(string viewTypeName, Func<object> factory)
        {
            _factories[viewTypeName] = factory;
        }

        /// <summary>
        /// 为指定 viewType 注册 viewModelType
        /// </summary>
        /// <typeparam name="T">view 的类型</typeparam>
        /// <typeparam name="VM">viewModel 的类型</typeparam>
        public static void Register<T, VM>()
        {
            Type typeFromHandle = typeof(T);
            Register(viewModelType: typeof(VM), viewTypeName: typeFromHandle.ToString());
        }

        /// <summary>
        /// 为指定 viewType 注册 viewModelType
        /// </summary>
        /// <param name="viewTypeName"></param>
        /// <param name="viewModelType"></param>
        public static void Register(string viewTypeName, Type viewModelType)
        {
            _typeFactories[viewTypeName] = viewModelType;
        }

        #endregion 注册
    }
}