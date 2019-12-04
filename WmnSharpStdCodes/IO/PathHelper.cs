using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WmnSharpStdCodes.IO
{
    public static class PathHelper
    {
        public static string GetCurrentProcessPath()
        {
            DirectoryInfo directoryInfo = Directory.GetParent(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            string currentProcePath = directoryInfo.FullName;
            return currentProcePath;
        }

        public static string GetAbsolutePathByCurrentProcess(string fileName)
        {
            if (IsAbsolutePath(fileName)) return fileName;
            //DirectoryInfo directoryInfo = Directory.GetParent(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            string currentProcePath = GetCurrentProcessPath();// directoryInfo.FullName;
            string currentFileName = Path.Combine(currentProcePath, fileName);
            return currentFileName;
        }

        public static bool IsAbsolutePath(string path)
        {
            try
            {
                Uri uri = new Uri(path);
                return uri.IsAbsoluteUri;
            }
            catch (System.UriFormatException)
            {
                return false;
            }
        }
    }
}
