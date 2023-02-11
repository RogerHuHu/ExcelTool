using System;

namespace HIIUtils.IoC
{
    /// <summary>
    /// 定义容器范围
    /// </summary>
    public interface IScopedProvider : IContainerProvider, IDisposable
    {
        /// <summary>
        /// 设置或获取是否正在跟踪此范围
        /// </summary>
        bool IsAttached { get; set; }
    }
}