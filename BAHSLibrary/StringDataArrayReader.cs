using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAHSLibrary
{
    public class StringDataArrayReader
    {
        public List<char[]> Data { get; set; }

        public int CurrentStreamPos = 0;

        public bool Loop = true;

        public void GetStringDataArray(BinaryReader br)
        {
            while (Loop)
            {
                char c2 = br.ReadChar();
                if (c2 != '\0')
                {
                    br.BaseStream.Seek(-1, SeekOrigin.Current);

                    List<char> charDataList = new List<char>();
                    while (true)
                    {
                        char c = br.ReadChar();
                        if (c != '\0')
                        {
                            charDataList.Add(c);
                            CurrentStreamPos++;
                        }
                        else if (c == '\0') break;
                    }

                    Data.Add(charDataList.ToArray());
                }
                else if (c2 == '\0')
                {
                    Loop = false;
                    break;
                }
            }
        }

        public StringDataArrayReader()
        {
            Data = new List<char[]>();
        }
    }
}
