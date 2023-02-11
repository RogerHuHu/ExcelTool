using System;
using System.IO;

namespace HIIUtils.HIIFile
{
    /// <summary>
    /// 文件夹操作
    /// </summary>
    public static class DirectoryHelper
    {
        /// <summary>
        /// 检测指定目录是否存在
        /// </summary>
        /// <param name="path">目录的绝对路径</param>
        /// <returns></returns>
        public static bool Exists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// 创建一个目录
        /// </summary>
        /// <param name="path">目录的绝对路径</param>
        public static void Create(string path)
        {
            if (!Exists(path))
                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <returns>删除成功返回 true，否则返回 false</returns>
        public static bool Delete(string path)
        {
            try
            {
                if (RecursiveDelete(path))
                    Directory.Delete(path, true);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 递归删除目录
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns>删除成功返回 true，否则返回 false</returns>
        private static bool RecursiveDelete(string path)
        {
            try
            {
                foreach (string subDir in Directory.GetDirectories(path))
                    RecursiveDelete(subDir);

                foreach (string files in Directory.GetFiles(path))
                    File.Delete(files);

                Directory.Delete(path, true);
                return true;
            }
            catch (Exception ex)
            {
                Directory.Delete(path, true);
                return false;
            }
        }

        /// <summary>
        /// 获取指定目录中的所有文件
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static string[] GetFileNames(string path)
        {
            // 如果目录不存在，则抛出异常
            if (!Exists(path))
                throw new FileNotFoundException();

            // 获取文件列表
            return Directory.GetFiles(path);
        }

        /// <summary>
        /// 获取指定目录及子目录中的所有文件
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <param name="searchPattern">模式字符串，“*” 代表 0 或 N 个字符，“?” 代表 1 个字符
        ///                             范例：“Log*.xml” 表示搜索所有以 Log 开头的 xml 文件</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static string[] GetFileNames(string path, string searchPattern, bool isSearchChild)
        {
            // 如果目录不存在，则抛出异常
            if (!Exists(path))
                throw new FileNotFoundException();

            try
            {
                if (isSearchChild)
                    return Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories);
                else
                    return Directory.GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取指定目录中所有子目录的列表
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns></returns>
        public static string[] GetDirectories(string path)
        {
            try
            {
                return Directory.GetDirectories(path);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取指定目录中所有子目录的列表
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <param name="searchPattern">模式字符串，“*” 代表 0 或 N 个字符，“?” 代表 1 个字符
        ///                             范例：“Log*” 表示搜索所有以 Log 开头的目录</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        /// <returns></returns>
        public static string[] GetDirectories(string path, string searchPattern, bool isSearchChild)
        {
            try
            {
                if (isSearchChild)
                    return Directory.GetDirectories(path, searchPattern, SearchOption.AllDirectories);
                else
                    return Directory.GetDirectories(path, searchPattern, SearchOption.TopDirectoryOnly);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 检测目录是否为空
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns></returns>
        public static bool IsEmptyDirectory(string path)
        {
            try
            {
                // 判断是否存在文件
                string[] fileNames = GetFileNames(path);
                if (fileNames.Length > 0)
                    return false;

                // 判断是否存在文件夹
                string[] directoryNames = GetDirectories(path);
                if (directoryNames.Length > 0)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        /// <summary>
        /// 检测目录中是否存在指定的文件
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <param name="searchPattern">模式字符串，“*” 代表 0 或 N 个字符，“?” 代表 1 个字符
        ///                             范例：“Log*.xml” 表示搜索所有以 Log 开头的 xml 文件</param>
        /// <returns></returns>
        public static bool Contains(string path, string searchPattern)
        {
            try
            {
                // 获取指定的文件列表
                string[] fileNames = GetFileNames(path, searchPattern, false);

                // 判断指定文件是否存在
                if (fileNames.Length == 0)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 检测目录中是否存在指定的文件
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <param name="searchPattern">模式字符串，“*” 代表 0 或 N 个字符，“?” 代表 1 个字符
        ///                             范例：“Log*.xml” 表示搜索所有以 Log 开头的 xml 文件</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        /// <returns></returns>
        public static bool Contains(string path, string searchPattern, bool isSearchChild)
        {
            try
            {
                // 获取指定的文件列表
                string[] fileNames = GetFileNames(path, searchPattern, true);

                // 判断指定文件是否存在
                if (fileNames.Length == 0)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 返回路径中的目录信息
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static string GetDirectoryName(string path)
        {
            path = path.Replace("/", "\\");
            return Path.GetDirectoryName(path);
        }

        /// <summary>
        /// 根据时间获得目录名
        /// </summary>
        /// <param name="format">时间格式，比如 yyyyMMdd</param>
        /// <returns></returns>
        public static string GetDateDirectoryName(string format)
        {
            return DateTime.Now.ToString(format);
        }

        /// <summary>
        /// 复制目录（递归）
        /// </summary>
        /// <param name="from">源路径</param>
        /// <param name="to">目标路径</param>
        public static void Copy(string from, string to)
        {
            Directory.CreateDirectory(to);
            if (!Directory.Exists(from)) return;
            string[] directories = Directory.GetDirectories(from);

            if (directories.Length > 0)
            {
                foreach (string d in directories)
                    Copy(d, to + d.Substring(d.LastIndexOf("\\")));
            }

            string[] files = Directory.GetFiles(from);
            if (files.Length > 0)
            {
                foreach (string s in files)
                    File.Copy(s, to + s.Substring(s.LastIndexOf("\\")), true);
            }
        }

        /// <summary>
        /// 清空目录下的所有文件及子目录，但该目录依然保存
        /// </summary>
        /// <param name="path">目录路径</param>
        public static void Clear(string path)
        {
            if (Exists(path))
            {
                // 删除目录中的所有文件
                string[] fileNames = GetFileNames(path);
                for (int i = 0; i < fileNames.Length; i++)
                    File.Delete(fileNames[i]);

                // 删除目录中的子目录
                string[] directoryNames = GetDirectories(path);
                for (int i = 0; i < directoryNames.Length; i++)
                    Delete(directoryNames[i]);
            }
        }
    }
}