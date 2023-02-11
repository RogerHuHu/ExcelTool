using System.Collections.Generic;

namespace HIIUtils.Services.Dialogs
{
    /// <summary>
    /// 对话框的参数集合接口
    /// </summary>
    /// <remarks>用于在显示和关闭对话框期间传递参数</remarks>
    public interface IDialogParameters
    {
        /// <summary>
        /// 参数的数量
        /// </summary>
        int Count { get; }

        IEnumerable<string> Keys { get; }

        void Add(string key, object value);

        bool ContainsKey(string key);

        T GetValue<T>(string key);

        /// <summary>
        /// 获取 key 指向的全部参数值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        IEnumerable<T> GetValues<T>(string key);

        /// <summary>
        /// 尝试获取 key 指向的参数值，若参数存在则返回 true，若参数不存在则返回 false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>参数是否存在</returns>
        bool TryGetValue<T>(string key, out T value);
    }
}