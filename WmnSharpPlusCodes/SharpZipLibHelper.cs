using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmnSharpPlusCodes
{
    public static class SharpZipLibHelper
    {
        private static void AppendAct(string zipfile, Action<ZipOutputStream> act)
        { 
            Debug.WriteLine(" ---------------------------------------------- " );  
            using (System.IO.MemoryStream memoryStream = new MemoryStream())
            {
                Debug.WriteLine(" memoryStream 长度 1:" + memoryStream.Length);
                using (ZipOutputStream zipOutStream = new ZipOutputStream(memoryStream))
                {
                    using (var zipInputStream = new ZipInputStream(File.OpenRead(zipfile)))
                    {
                        CopyZipEntry(zipOutStream, zipInputStream);
                    }
                    Debug.WriteLine(" memoryStream 长度 2:" + memoryStream.Length);
                    //OutStreamAppendDir(zipOutStream, dirName);
                    act(zipOutStream);
                    memoryStream.Position = 0;
                    Debug.WriteLine(" memoryStream 长度 3:" + memoryStream.Length);
                    var zipfile2 = "s2.zip";
                    zipfile2 = zipfile;
                    using (FileStream fileStream = new FileStream(zipfile2, FileMode.Create, FileAccess.Write))
                    {
                        byte[] bytes = new byte[memoryStream.Length];
                        memoryStream.Read(bytes, 0, (int)memoryStream.Length);
                        Debug.WriteLine(" 写入文件 长度 :" + memoryStream.Length);
                        fileStream.Write(bytes, 0, bytes.Length);
                        fileStream.Close();
                        memoryStream.Close();
                    }
                }
            }
        }

        public static void AppendDirectory(string zipfile, string dirName)
        {
            AppendAct(zipfile,
                (zipOutStream) =>
            {
                OutStreamAppendDir(zipOutStream, dirName);
            });
            
        }

        public static void AppendTextFile(string zipfile, string fileName, string fileContent)
        {
            AppendAct(zipfile,
                (zipOutStream) =>
                {
                    OutStreamAppendText(zipOutStream, fileName, fileContent);
                });
            
        }

        private static void AddEntry(ZipOutputStream zipOutStream, string entryName)
        {
            ZipEntry entry1 = new ZipEntry(entryName);
            zipOutStream.PutNextEntry(entry1);
        }

        private static void OutStreamAppendDir(ZipOutputStream zipOutStream, string dirName)
        {
            AddEntry(zipOutStream, dirName);
            zipOutStream.Finish();
        }

        private static void OutStreamAppendText(ZipOutputStream zipOutStream ,string fileName, string fileContent)
        {
            //ZipEntry entry1 = new ZipEntry(fileName);
            //zipOutStream.PutNextEntry(entry1);
            AddEntry(zipOutStream, fileName);
            var data = Encoding.UTF8.GetBytes(fileContent);
            zipOutStream.Write(data, 0, data.Length);
            zipOutStream.Finish();
        }

        private static void CopyZipEntry(ZipOutputStream zipOutStream, ZipInputStream zipInputStream)
        {
            ZipEntry zipEntry;
            while ((zipEntry = zipInputStream.GetNextEntry()) != null)
            {
                zipOutStream.PutNextEntry(zipEntry);
                string fileName = Path.GetFileName(zipEntry.Name);
                Debug.WriteLine(" CopyZipEntry :" + zipEntry.Name);
                if (fileName != String.Empty)
                {
                    int size;
                    byte[] data = new byte[2048];
                    while (true)
                    {
                        size = zipInputStream.Read(data, 0, data.Length);
                        Debug.WriteLine(" size :" + size);
                        if (size > 0)
                        {
                            zipOutStream.Write(data, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        public static void SafeAddDirectory(ZipFile zip, string path )
        {
            var entry = zip.GetEntry(path);
            if (entry == null)
            {
                try
                {
                    zip.AddDirectory(path);
                }
                catch(System.ArgumentException ae)
                {

                }
            }
        }
    }
}
