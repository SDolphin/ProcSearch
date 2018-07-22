using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SSearh.Helpers
{
    public static class Extra
    {

        

        public static Icon IconFromFilePath(string filePath)
        {
            var result = (Icon)null;

            try
            {
                result = Icon.ExtractAssociatedIcon(filePath);
            }
            catch (System.Exception)
            {
                // swallow and return nothing. You could supply a default Icon here as well
            }

            return result;
        }


        public static string GetShortcutTarget(string file)
        {
            try
            {
                if (System.IO.Path.GetExtension(file).ToLower() != ".lnk")
                {
                    throw new Exception("Supplied file must be a .LNK file");
                }

                FileStream fileStream = File.Open(file, FileMode.Open, FileAccess.Read);
                using (System.IO.BinaryReader fileReader = new BinaryReader(fileStream))
                {
                    fileStream.Seek(0x14, SeekOrigin.Begin);     
                    uint flags = fileReader.ReadUInt32();        
                    if ((flags & 1) == 1)
                    {                      
                                           
                        fileStream.Seek(0x4c, SeekOrigin.Begin); 
                        uint offset = fileReader.ReadUInt16();   
                        fileStream.Seek(offset, SeekOrigin.Current); 
                    }

                    long fileInfoStartsAt = fileStream.Position; 
                                                                 
                    uint totalStructLength = fileReader.ReadUInt32(); 
                    fileStream.Seek(0xc, SeekOrigin.Current);
                    uint fileOffset = fileReader.ReadUInt32(); 
                                                              
                    fileStream.Seek((fileInfoStartsAt + fileOffset), SeekOrigin.Begin);
                                                                                        
                    long pathLength = (totalStructLength + fileInfoStartsAt) - fileStream.Position - 2;
                                                                                                       
                    char[] linkTarget = fileReader.ReadChars((int)pathLength); 
                    var link = new string(linkTarget);

                    int begin = link.IndexOf("\0\0");
                    if (begin > -1)
                    {
                        int end = link.IndexOf("\\\\", begin + 2) + 2;
                        end = link.IndexOf('\0', end) + 1;

                        string firstPart = link.Substring(0, begin);
                        string secondPart = link.Substring(end);

                        return firstPart + secondPart;
                    }
                    else
                    {
                        return link;
                    }
                }
            }
            catch
            {
                return "";
            }
        }

    }
}
