using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BAHSLibrary
{
    public class BAHS
    {
        public char[] BAHS_Header { get; set; }
        public byte[] Version { get; set; }
        public int FileSize { get; set; }

        public int UnknownData0 { get; set; }
        public int UnknownData1 { get; set; }

        public int StringDataLength { get; set; }

        public int XV4DataSize { get; set; }
        public int XV4DataOffset { get; set; }

        public byte[] UnknownByteArrayArea { get; set; } //304 bytes

        public char[] CharArray { get; set; } //From -> StringDataLength

        public string[] ShaderAttributeNameArray => GetShaderAttributeNameArray();
        public string[] GetShaderAttributeNameArray()
        {
            return (new string(CharArray)).Split('\0').ToArray();
        }

        public UnknownDataArea0 UnknownDataArea_0 { get; set; }
        public class UnknownDataArea0
        {
            public int UnknownDataOffset { get; set; }

            public BAHS_ShaderStructData BAHS_Shader_StructData { get; set; }
            public class BAHS_ShaderStructData
            {
                public int BAHS_ShaderStructDataSize { get; set; }

                public int DefinedShaderCount { get; set; }

                public int UnknownData0 { get; set; }
                public int UnknownData1 { get; set; }
                public int UnknownData2 { get; set; }
                public int UnknownData3 { get; set; }

                public List<DefinedShaderStruct> DefinedShaderStructs { get; set; }
                public class DefinedShaderStruct
                {
                    public char[] DefinedNameChars { get; set; }

                    public string DefinedName => GetDefinedNameCharArray();
                    public string GetDefinedNameCharArray()
                    {
                        return new string(DefinedNameChars);
                    }

                    //4byte * 6
                    public int UnknownData0 { get; set; }
                    public int ShaderStructDataCount { get; set; }
                    public int UnknownData2 { get; set; }
                    public int UnknownData3 { get; set; }
                    public int UnknownData4 { get; set; }
                    public int UnknownData5 { get; set; }

                    public List<ShaderStructData> ShaderStructDatas { get; set; }
                    public class ShaderStructData
                    {
                        public char[] Name;

                        public string ShaderStructName => GetShaderStructName();
                        public string GetShaderStructName()
                        {
                            return new string(Name);
                        }

                        public List<char[]> IndexStringList { get; set; }

                        public int[] GetIndexArray()
                        {
                            List<int> IndexDataList = new List<int>();
                            foreach (var item in IndexStringList)
                            {
                                IndexDataList.Add(int.Parse(new string(item).Replace("\0", "")));
                            }

                            return IndexDataList.ToArray();
                        }

                        public int UnknownData0 { get; set; }
                        public int UnknownData1 { get; set; }
                        public int UnknownData2 { get; set; }
                        public int UnknownData3 { get; set; }

                        public void Read(BinaryReader br, EndianConvert.Endian endian)
                        {
                            ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                            readByteLine.ReadByte(br, 0x00);
                            Name = readByteLine.ConvertToCharArray();

                            StringDataArrayReader stringDataArrayReader = new StringDataArrayReader();
                            stringDataArrayReader.GetStringDataArray(br);
                            IndexStringList = stringDataArrayReader.Data;

                            EndianConvert endianConvert = new EndianConvert(EndianConvert.GetEnumEndianToBytes(endian));

                            UnknownData0 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            UnknownData1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            UnknownData3 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        }

                        public ShaderStructData()
                        {
                            Name = new char[0];
                            IndexStringList = new List<char[]>();

                            UnknownData0 = 0;
                            UnknownData1 = 0;
                            UnknownData2 = 0;
                            UnknownData3 = 0;
                        }
                    }

                    public int UnknownData6 { get; set; }
                    public int UnknownData7 { get; set; }
                    public int UnknownData8 { get; set; }
                    public int UnknownData9 { get; set; }

                    public void Read_DefinedShaderStruct(BinaryReader br, EndianConvert.Endian endian)
                    {
                        EndianConvert endianConvert = new EndianConvert(EndianConvert.GetEnumEndianToBytes(endian));

                        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                        readByteLine.ReadByte(br, 0x00);
                        DefinedNameChars = readByteLine.ConvertToCharArray();

                        UnknownData0 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        ShaderStructDataCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        UnknownData3 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        UnknownData4 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        UnknownData5 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                        if (ShaderStructDataCount != 0)
                        {
                            for (int i = 0; i < ShaderStructDataCount; i++)
                            {
                                ShaderStructData shaderStructData = new ShaderStructData();
                                shaderStructData.Read(br, endian);
                                ShaderStructDatas.Add(shaderStructData);
                            }
                        }

                        UnknownData6 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        UnknownData7 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        UnknownData8 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        UnknownData9 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    }

                    public DefinedShaderStruct()
                    {
                        DefinedNameChars = new char[0];

                        UnknownData0 = 0;
                        ShaderStructDataCount = 0;
                        UnknownData2 = 0;
                        UnknownData3 = 0;
                        UnknownData4 = 0;
                        UnknownData5 = 0;

                        ShaderStructDatas = new List<ShaderStructData>();

                        UnknownData6 = 0;
                        UnknownData7 = 0;
                        UnknownData8 = 0;
                        UnknownData9 = 0;
                    }
                }

                public void Read_BAHS_ShaderStructData(BinaryReader br, EndianConvert.Endian endian)
                {
                    EndianConvert endianConvert = new EndianConvert(EndianConvert.GetEnumEndianToBytes(endian));

                    BAHS_ShaderStructDataSize = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    DefinedShaderCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    UnknownData0 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    UnknownData1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    UnknownData3 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    for (int i = 0; i < DefinedShaderCount; i++)
                    {
                        DefinedShaderStruct definedShaderStruct = new DefinedShaderStruct();
                        definedShaderStruct.Read_DefinedShaderStruct(br, endian);
                        DefinedShaderStructs.Add(definedShaderStruct);
                    }
                }

                public BAHS_ShaderStructData()
                {
                    BAHS_ShaderStructDataSize = 0;

                    DefinedShaderCount = 0;

                    UnknownData0 = 0;
                    UnknownData1 = 0;
                    UnknownData2 = 0;
                    UnknownData3 = 0;

                    DefinedShaderStructs = new List<DefinedShaderStruct>();
                }
            }


            public void Read_UnknownDataArea0(BinaryReader br, EndianConvert.Endian endian)
            {
                EndianConvert endianConvert = new EndianConvert(EndianConvert.GetEnumEndianToBytes(endian));

                UnknownDataOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (UnknownDataOffset != 0)
                {
                    long pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    br.BaseStream.Seek(UnknownDataOffset, SeekOrigin.Current);

                    //Some Data
                    BAHS_Shader_StructData.Read_BAHS_ShaderStructData(br, endian);

                    br.BaseStream.Position = pos;
                }
            }

            public UnknownDataArea0()
            {
                UnknownDataOffset = 0;
                BAHS_Shader_StructData = new BAHS_ShaderStructData();
            }
        }


        

        public void Read_BAHS(BinaryReader br, EndianConvert.Endian endian)
        {
            BAHS_Header = br.ReadChars(4);
            if (new string(BAHS_Header) != "BAHS") throw new Exception("BAHS : Error");

            EndianConvert endianConvert = new EndianConvert(EndianConvert.GetEnumEndianToBytes(endian));

            Version = endianConvert.Convert(br.ReadBytes(4));
            FileSize = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData0 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            StringDataLength = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            XV4DataSize = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            XV4DataOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            UnknownByteArrayArea = br.ReadBytes(304);

            CharArray = br.ReadChars(StringDataLength);

            UnknownDataArea_0.Read_UnknownDataArea0(br, endian);

            //FileSize = BitConverter.ToInt32()
        }

        public BAHS()
        {
            BAHS_Header = "BAHS".ToCharArray();
            Version = new byte[4];
            FileSize = 0;

            UnknownData0 = 0;
            UnknownData1 = 0;

            StringDataLength = 0;

            XV4DataSize = 0;
            XV4DataOffset = 0;

            UnknownByteArrayArea = new byte[0];

            CharArray = new char[0];

            UnknownDataArea_0 = new UnknownDataArea0();

            //UnknownDataOffset = 0;
        }
    }
}
