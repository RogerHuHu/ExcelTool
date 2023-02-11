using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HIIUtils.HIIFile
{
    public sealed class FileHelper
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);
        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);
        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;
        public static IntPtr HFILE_ERROR = new IntPtr(-1);

        #region 读文件
        /// <summary>
        /// 从指定路径读取文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static ArrayList ReadFromFile(string path, Encoding encoding)
        {
            ArrayList lines = new ArrayList();
            string line = null;
            FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
            StreamReader streamReader = new StreamReader(fileStream, encoding);

            while((line = streamReader.ReadLine()) != null)
                lines.Add(line);
            streamReader.Close();
            fileStream.Close();
            streamReader.Dispose();
            fileStream.Dispose();
            return lines;
        }
        #endregion
    }
}
