namespace HIIUtils.IoC
{
    /// <summary>
    /// 期望从容器中获得的内容的一般抽象
    /// </summary>
    /// <remarks>服务注册 + 服务解析</remarks>
    public interface IContainerExtension : IContainerRegistry, IContainerProvider
    {
        /// <summary>
        /// 用于执行容器任何可能需要的，用于拓展配置的最后步骤
        /// </summary>
        void FinalizeExtension();
    }

    public interface IContainerExtension<TContainer> : IContainerExtension
    {
        /// <summary>
        /// 封装容器的实例
        /// </summary>
        TContainer Instance { get; }
    }
}