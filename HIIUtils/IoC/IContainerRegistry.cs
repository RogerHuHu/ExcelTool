using System;

namespace HIIUtils.IoC
{
    /// <summary>
    /// 用于注册服务的容器的接口
    /// </summary>
    public interface IContainerRegistry
    {
        /// <summary>
        /// 确定给定服务是否已注册
        /// </summary>
        /// <param name="type">服务类型</param>
        /// <returns></returns>
        bool IsRegistered(Type type);

        /// <summary>
        /// 确定给定服务是否已注册
        /// </summary>
        /// <param name="type">服务类型</param>
        /// <param name="name">注册服务时指定的 name 或 key</param>
        /// <returns></returns>
        bool IsRegistered(Type type, string name);

        /// <summary>
        /// 用给定服务类型注册一个非持久服务，并映射到指定的实现类型
        /// </summary>
        /// <param name="from">服务类型</param>
        /// <param name="to">实现类型</param>
        /// <returns></returns>
        IContainerRegistry Register(Type from, Type to);

        /// <summary>
        /// 给定服务类型注册一个非持久服务，并映射到指定的实现类型
        /// </summary>
        /// <param name="from">服务类型</param>
        /// <param name="to">实现类型</param>
        /// <param name="name">注册服务时指定的 name 或 key</param>
        /// <returns></returns>
        IContainerRegistry Register(Type from, Type to, string name);

        /// <summary>
        /// 用给定的服务类型和工厂生产服务实例的委托方法注册一个非持久服务
        /// </summary>
        /// <param name="type">服务类型</param>
        /// <param name="factoryMethod">生产服务实例的委托方法</param>
        /// <returns></returns>
        IContainerRegistry Register(Type type, Func<object> factoryMethod);

        /// <summary>
        /// 注册服务实例
        /// </summary>
        /// <param name="type">服务类型</param>
        /// <param name="instance">服务实例</param>
        /// <returns></returns>
        IContainerRegistry RegisterInstance(Type type, object instance);

        /// <summary>
        /// 注册服务实例
        /// </summary>
        /// <param name="type">服务类型</param>
        /// <param name="instance">服务实例</param>
        /// <param name="name">注册服务时指定的 name 或 key</param>
        /// <returns></returns>
        IContainerRegistry RegisterInstance(Type type, object instance, string name);

        /// <summary>
        /// 注册一个实现了所有服务接口的非持久服务
        /// </summary>
        /// <param name="type">服务类型</param>
        /// <param name="serviceTypes">服务接口数组</param>
        /// <returns></returns>
        IContainerRegistry RegisterMany(Type type, params Type[] serviceTypes);

        /// <summary>
        /// 注册一个实现了所有服务接口的单例服务
        /// </summary>
        /// <param name="type">服务类型</param>
        /// <param name="serviceTypes">服务接口数组</param>
        /// <returns></returns>
        IContainerRegistry RegisterManySingletion(Type type, params Type[] serviceTypes);

        /// <summary>
        /// 注册一个限定范围的服务
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        IContainerRegistry RegisterScoped(Type from, Type to);

        /// <summary>
        /// 用给定的服务类型和工厂委托方法注册一个限定范围的服务
        /// </summary>
        /// <param name="from">服务类型</param>
        /// <param name="factoryMethod">生产服务实例的委托方法</param>
        /// <returns></returns>
        IContainerRegistry RegisterScoped(Type from, Func<object> factoryMethod);

        /// <summary>
        /// 用给定的服务类型和工厂委托方法注册一个限定范围的服务
        /// </summary>
        /// <param name="from">服务类型</param>
        /// <param name="factoryMethod">使用<see cref="IContainerProvider"/>的委托方法</param>
        /// <returns></returns>
        IContainerRegistry RegisterScoped(Type from, Func<IContainerProvider, object> factoryMethod);

        /// <summary>
        /// 用给定的服务类型注册一个单例服务，并映射到指定的实现类型
        /// </summary>
        /// <param name="from">服务类型</param>
        /// <param name="to">实现类型</param>
        /// <returns></returns>
        IContainerRegistry RegisterSingletion(Type from, Type to);

        /// <summary>
        /// 用给定的服务类型注册一个单例服务，并映射到指定的实现类型
        /// </summary>
        /// <param name="from">服务类型</param>
        /// <param name="to">实现类型</param>
        /// <param name="name">注册服务时指定的 name 或 key</param>
        /// <returns></returns>
        IContainerRegistry RegisterSingletion(Type from, Type to, string name);

        /// <summary>
        /// 用给定的服务类型和工厂委托方法注册一个单例服务
        /// </summary>
        /// <param name="type">服务类型</param>
        /// <param name="factoryMethod">生产服务实例的委托方法</param>
        /// <returns></returns>
        IContainerRegistry RegisterSingletion(Type type, Func<object> factoryMethod);

        /// <summary>
        /// 用给定的服务类型和工厂委托方法注册一个单例服务
        /// </summary>
        /// <param name="type">服务类型</param>
        /// <param name="factoryMethod">使用<see cref="IContainerProvider"/>的委托方法</param>
        /// <returns></returns>
        IContainerRegistry RegisterSingletion(Type type, Func<IContainerProvider, object> factoryMethod);
    }
}