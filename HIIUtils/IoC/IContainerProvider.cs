using System;
using System.Collections.Generic;

namespace HIIUtils.IoC
{
    /// <summary>
    /// 从容器中解析服务的接口
    /// </summary>
    public interface IContainerProvider
    {
        /// <summary>
        /// 获取当前所在的限定范围
        /// </summary>
        IScopedProvider CurrentScope { get; }

        /// <summary>
        /// 新建一个限定范围
        /// </summary>
        /// <returns></returns>
        IScopedProvider CreateScope();

        /// <summary>
        /// 解析指定类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object Resolve(Type type);

        /// <summary>
        /// 解析指定类型
        /// </summary>
        /// <param name="type">服务类型</param>
        /// <param name="parameters">解析时使用的类型化参数</param>
        /// <returns></returns>
        object Resolve(Type type, params NewStruct[] parameters);

        /// <summary>
        /// 解析指定类型
        /// </summary>
        /// <param name="type">服务类型</param>
        /// <param name="name">服务注册时使用的 name 或 key</param>
        /// <returns></returns>
        object Resolve(Type type, string name);

        /// <summary>
        /// 解析指定类型
        /// </summary>
        /// <param name="type">服务类型</param>
        /// <param name="name">服务注册时使用的 name 或 key</param>
        /// <param name="parameters">解析时使用的类型化参数</param>
        /// <returns></returns>
        object Resolve(Type type, string name, params NewStruct[] parameters);
    }

    public struct NewStruct
    {
        public object Instance;
        public Type type;

        public NewStruct(Type type, object instance)
        {
            this.type = type;
            Instance = instance;
        }

        public void Deconstruct(out Type type, out object instance)
        {
            type = this.type;
            instance = Instance;
        }

        public override bool Equals(object obj)
        {
            return obj is NewStruct other &&
                   EqualityComparer<Type>.Default.Equals(type, other.type) &&
                   EqualityComparer<object>.Default.Equals(Instance, other.Instance);
        }

        public override int GetHashCode()
        {
            int hashCode = 124610567;
            hashCode = hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(type);
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(Instance);
            return hashCode;
        }
    }
}