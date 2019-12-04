using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WmnSharpStdCodes.Reflection
{
    public class AssemblyHelper
    {
        public static Assembly LoadAssemblyFile(AppDomain domain, string file)
        {
            byte[] rawAssembly = LoadFile(file);
            Assembly assembly = domain.Load(rawAssembly);
            return assembly;
        }

        public static byte[] LoadFile(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            byte[] buffer = new byte[(int)fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();
            return buffer;
        }
    }
}
