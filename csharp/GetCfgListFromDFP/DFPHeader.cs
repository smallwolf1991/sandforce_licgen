/*
 * Created by SharpDevelop.
 * User: SMALLWOLF
 * Date: 2017/2/27
 * Time: 12:42
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Data;

namespace GetCfgListFromDFP
{
    public enum SectionType { UInt32 = 1, String = 2, Bool = 3, Hex = 4, BINTOTEXT = 5 };
    public class ConfigSection
    {
        public string name { get; set; }
        public int offset { get; set; }
        public int size { get; set; }
        public SectionType type { get; set; }
    }

    /// <summary>
    /// Description of DFPHeader.
    /// </summary>
    public class DFPHeader
    {
        public UInt32 HeaderCheckSum { get; set; }
        public UInt32 FileCheckSum { get; set; }
        public string DFPReleaseName { get; set; }
        public UInt32 DFPReleaseID { get; set; }
        public string DFPVersion { get; set; }
        public UInt32 DFPFileLength { get; set; }
        public UInt32 DFPReallyFileLength { get; set; }
        public UInt32 ManifestOffset { get; set; }
        public UInt32 ManifestCount { get; set; }
        public UInt32 ManifestLength { get; set; }
        public UInt32 ConfigurationOffset { get; set; }
        public UInt32 ConfigurationCount { get; set; }
        public UInt32 ConfigurationLength { get; set; }
        public string Timestamp { get; set; }
        public DataTable Configurations { get; set; }

        public ConfigSection[] Config_Offset = new ConfigSection[]{
			new ConfigSection(){name="CLI Config ID", offset=0, size=64, type = SectionType.String},
			new ConfigSection(){name="Release Config ID", offset=64, size=64, type = SectionType.String},
			new ConfigSection(){name="Firmware Type", offset=128, size=32, type = SectionType.String},
			new ConfigSection(){name="Title", offset=160, size=256, type = SectionType.String},
			new ConfigSection(){name="Flash Vendor", offset=416, size=32, type = SectionType.String},
			new ConfigSection(){name="Flash Geometry", offset=448, size=4, type = SectionType.UInt32},
			new ConfigSection(){name="Flash Type", offset=452, size=16, type = SectionType.String},
			new ConfigSection(){name="Flash Part Number", offset=468, size=128, type = SectionType.String},
			new ConfigSection(){name="Package Type", offset=596, size=16, type = SectionType.String},
			new ConfigSection(){name="Package Count", offset=612, size=4, type = SectionType.UInt32},
			new ConfigSection(){name="Package Density", offset=616, size=4, type = SectionType.UInt32},
			new ConfigSection(){name="Package Die Count", offset=620, size=4, type = SectionType.UInt32},
			new ConfigSection(){name="Raw Capacity", offset=624, size=4, type = SectionType.UInt32},
			new ConfigSection(){name="User Capacity", offset=628, size=4, type = SectionType.UInt32},
			new ConfigSection(){name="512 LBA Count", offset=632, size=4, type = SectionType.UInt32},
			new ConfigSection(){name="512 Plus DIF LBA Count", offset=636, size=4, type = SectionType.UInt32},
			new ConfigSection(){name="520 LBA Count", offset=640, size=4, type = SectionType.UInt32},
			new ConfigSection(){name="524 LBA Count", offset=644, size=4, type = SectionType.UInt32},
			new ConfigSection(){name="528 LBA Count", offset=648, size=4, type = SectionType.UInt32},
			new ConfigSection(){name="4096 LBA Count", offset=652, size=4, type = SectionType.UInt32},
			new ConfigSection(){name="4096 Plus DIF LBA Count", offset=656, size=4, type = SectionType.UInt32},
			new ConfigSection(){name="Non 512 Sector Size", offset=660, size=4, type = SectionType.UInt32},
			new ConfigSection(){name="RAISE ON", offset=664, size=4, type = SectionType.Bool},
			new ConfigSection(){name="Version Demo", offset=668, size=64, type = SectionType.String},
			new ConfigSection(){name="Version Release", offset=732, size=64, type = SectionType.String},
			new ConfigSection(){name="Topology ID", offset=796, size=32, type = SectionType.String},
			new ConfigSection(){name="SATA Speed", offset=828, size=4, type = SectionType.UInt32},
			new ConfigSection(){name="Write IOPs", offset=832, size=4, type = SectionType.UInt32},
            new ConfigSection(){name="Firmware Fuse", offset=836, size=4, type = SectionType.BINTOTEXT},
			new ConfigSection(){name="Industrial Features", offset=840, size=4, type = SectionType.UInt32},
			new ConfigSection(){name="CLI Build ID", offset=844, size=16, type = SectionType.String},
			new ConfigSection(){name="Release Build ID", offset=860, size=16, type = SectionType.String},
			new ConfigSection(){name="Valid Flag", offset=876, size=4, type = SectionType.UInt32},
			new ConfigSection(){name="Release Status", offset=880, size=16, type = SectionType.String},
			new ConfigSection(){name="Firmware Worksheet", offset=896, size=16, type = SectionType.String},
			new ConfigSection(){name="Validated", offset=912, size=8, type = SectionType.String},
			new ConfigSection(){name="Parent Folder", offset=920, size=256, type = SectionType.String}
		};
        public Dictionary<string, int> HEADER_OFFSET = new Dictionary<string, int>(){
			{"headerchecksum",		0x0},
			{"filechecksum",		0x4},
			{"header", 				0x8},
			{"releasename",			0x8},
			{"releaseid",			0x108},
			{"version",				0x10C},
			{"filelength",			0x12C},
			{"manifest",			0x130},
			{"manifestcount",		0x134},
			{"manifestlength",		0x138},
			{"configuration",		0x13C},
			{"configurationlength",		0x140},
			{"configurationcount",		0x144},
			{"unknow",		0x148},
			{"unknowlength",		0x14C},
			{"stamp",		0x15C},
		};

        private int headerblockSize = 1120;
        private int manifestBlockSize = 1024;
        private int configdigestBlockSize = 1176;
        private MemoryStream FileSteam;
        private BinaryReader FileReader;
        private byte[] HeaderSection;
        private Dictionary<UInt32, string[]> FUSEVALUES = new Dictionary<UInt32, string[]>(){
                                                          { 0x0000, new string[]{"A", "All firmware","Blank fuse (to be used on evaluation drives"} },
                                                          { 0x8000, new string[]{"A", "All firmware","Blank fuse (to be used on evaluation drives"} },
                                                          { 0xC000, new string[]{"B", "B,C,D,E,F,G,H,I","Enterprise SATA overclocked"} },
                                                          { 0xE000, new string[]{"C", "C,D,E,F,G,H,I","Enterprise SAS"} },
                                                          { 0xF000, new string[]{"D", "D,E,F,G,H,I","Enterprise SATA"} },
                                                          { 0xF800, new string[]{"E", "E,F,G,H","Industrial"} },
                                                          { 0xFC00, new string[]{"F", "F,G,H","Client 6G SATA"} },
                                                          { 0xFE00, new string[]{"G", "G,H","Client 3G SATA"} },
                                                          { 0xFF00, new string[]{"H", "H","Client 3G SATA - 4 Channel"} },
                                                          { 0xFA00, new string[]{"I", "F,G,H,I","Cloud 6G SATA"} },
                                                          { 0xFFFF, new string[]{"NA", "NA","Unknown Fuse Value"} },
                                                          };
        public DFPHeader(byte[] buff)
        {
            FileSteam = new MemoryStream(buff);
            FileReader = new BinaryReader(FileSteam, Encoding.ASCII);
            FileReader.BaseStream.Position = 0;
            HeaderCheckSum = FileReader.ReadUInt32();
            FileCheckSum = FileReader.ReadUInt32();
            HeaderSection = FileReader.ReadBytes(headerblockSize);

            FileReader.BaseStream.Position = HEADER_OFFSET["releasename"];
            DFPReleaseName = this.ReadString();

            FileReader.BaseStream.Position = HEADER_OFFSET["releaseid"];
            DFPReleaseID = FileReader.ReadUInt32();
            DFPVersion = this.ReadString();

            FileReader.BaseStream.Position = HEADER_OFFSET["filelength"];
            DFPFileLength = FileReader.ReadUInt32();

            FileReader.BaseStream.Position = HEADER_OFFSET["manifest"];
            ManifestOffset = FileReader.ReadUInt32();
            ManifestCount = FileReader.ReadUInt32();
            ManifestLength = FileReader.ReadUInt32();


            FileReader.BaseStream.Position = HEADER_OFFSET["configuration"];
            ConfigurationOffset = FileReader.ReadUInt32();
            ConfigurationLength = FileReader.ReadUInt32();
            ConfigurationCount = FileReader.ReadUInt32();


            FileReader.BaseStream.Position = HEADER_OFFSET["stamp"];
            Timestamp = this.ReadString();
            Configurations = new DataTable();
            Configurations.Columns.Add(new DataColumn() { ColumnName = "CLI Config ID", DataType = System.Type.GetType("System.String") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "Release Config ID", DataType = System.Type.GetType("System.String") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "Firmware Type", DataType = System.Type.GetType("System.String") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "Title", DataType = System.Type.GetType("System.String") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "Flash Vendor", DataType = System.Type.GetType("System.String") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "Flash Geometry", DataType = System.Type.GetType("System.String") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "Flash Type", DataType = System.Type.GetType("System.String") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "Flash Part Number", DataType = System.Type.GetType("System.String") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "Package Type", DataType = System.Type.GetType("System.String") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "Package Count", DataType = System.Type.GetType("System.UInt32") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "Package Density", DataType = System.Type.GetType("System.UInt32") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "Package Die Count", DataType = System.Type.GetType("System.UInt32") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "Raw Capacity", DataType = System.Type.GetType("System.UInt32") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "User Capacity", DataType = System.Type.GetType("System.UInt32") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "512 LBA Count", DataType = System.Type.GetType("System.UInt32") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "512 Plus DIF LBA Count", DataType = System.Type.GetType("System.UInt32") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "520 LBA Count", DataType = System.Type.GetType("System.UInt32") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "524 LBA Count", DataType = System.Type.GetType("System.UInt32") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "528 LBA Count", DataType = System.Type.GetType("System.UInt32") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "4096 LBA Count", DataType = System.Type.GetType("System.UInt32") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "4096 Plus DIF LBA Count", DataType = System.Type.GetType("System.UInt32") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "Non 512 Sector Size", DataType = System.Type.GetType("System.UInt32") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "RAISE ON", DataType = System.Type.GetType("System.Boolean") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "Version Demo", DataType = System.Type.GetType("System.String") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "Version Release", DataType = System.Type.GetType("System.String") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "Topology ID", DataType = System.Type.GetType("System.String") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "SATA Speed", DataType = System.Type.GetType("System.UInt32") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "Write IOPs", DataType = System.Type.GetType("System.UInt32") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "Firmware Fuse", DataType = System.Type.GetType("System.String") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "Industrial Features", DataType = System.Type.GetType("System.UInt32") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "CLI Build ID", DataType = System.Type.GetType("System.String") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "Release Build ID", DataType = System.Type.GetType("System.String") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "Valid Flag", DataType = System.Type.GetType("System.UInt32") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "Release Status", DataType = System.Type.GetType("System.String") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "Firmware Worksheet", DataType = System.Type.GetType("System.String") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "Validated", DataType = System.Type.GetType("System.String") });
            Configurations.Columns.Add(new DataColumn() { ColumnName = "Parent Folder", DataType = System.Type.GetType("System.String") });

        }

        public string ReadString()
        {
            List<byte> str = new List<byte>();
            char ch = this.FileReader.ReadChar();
            while (ch != '\0')
            {
                str.Add((byte)ch);
                ch = this.FileReader.ReadChar();
            }

            return Encoding.ASCII.GetString(str.ToArray());
        }

        public string ReadString(BinaryReader binreader)
        {
            List<byte> str = new List<byte>();
            char ch = binreader.ReadChar();
            while (ch != '\0')
            {
                str.Add((byte)ch);
                ch = binreader.ReadChar();
            }

            return Encoding.ASCII.GetString(str.ToArray());
        }

        public void ReadConfigurations(byte[] buff)
        {
            if (configdigestBlockSize * ConfigurationCount != ConfigurationLength)
            {
                throw new Exception("配置文件个数与配置文件块大小不匹配");
            }
            using (MemoryStream ms = new MemoryStream(buff))
            {
                using (BinaryReader binreader = new BinaryReader(ms))
                {
                    for (int index = 0; index < ConfigurationCount; index++)
                    {
                        int baseOffset = (int)ConfigurationOffset + index * configdigestBlockSize;
                        DataRow row = Configurations.NewRow();
                        for (int i = 0; i < Config_Offset.Length; i++)
                        {
                            ConfigSection cs = Config_Offset[i];
                            int offset = baseOffset + cs.offset;
                            binreader.BaseStream.Position = offset;
                            string name = cs.name;
                            string value = "";
                            switch (cs.type)
                            {
                                case SectionType.Bool:
                                    value = binreader.ReadUInt32() == 0 ? "False" : "True";
                                    break;
                                case SectionType.String:
                                    value = this.ReadString(binreader);
                                    break;
                                case SectionType.UInt32:
                                    value = binreader.ReadUInt32().ToString();
                                    break;
                                case SectionType.Hex:
                                    value = binreader.ReadUInt32().ToString("X");
                                    break;
                                case SectionType.BINTOTEXT:
                                    UInt32 fv = binreader.ReadUInt32();
                                    if (FUSEVALUES.ContainsKey(fv))
                                    {
                                        string[] values = FUSEVALUES[fv];
                                        //value = string.Join("|", values);
                                        value = values[2];
                                    }
                                    else
                                    {
                                        value = FUSEVALUES[0xFFFF][2];
                                    }
                                    break;
                                default:
                                    char[] tempHex = new char[4];
                                    binreader.Read(tempHex, 0, 4);
                                    value = string.Join(",", tempHex);
                                    break;
                            }
                            row[name] = value;
                        }
                        Configurations.Rows.Add(row);
                    }
                }
            }
        }

    }
}
/*
 `anonymous namespace'::ConfigurationTypeNames = 1;
  std::string::string((int)&unk_83B2B44, "Manufacturing");
  std::string::string((int)&unk_83B2B48, "Manufacturing");
  dword_83B2B4C = 2;
  std::string::string((int)&unk_83B2B50, "Flashware");
  std::string::string((int)&unk_83B2B54, "Flashware");
  dword_83B2B58 = 0;
  std::string::string((int)&unk_83B2B5C, "Unknown Configuration Type");
  std::string::string((int)&unk_83B2B60, "Unknown Configuration Type");
  __cxa_atexit(_tcf_0_2, 0, &_dso_handle);
  `anonymous namespace'::configurationTypeNames = (int)&`anonymous namespace'::ConfigurationTypeNames;
  dword_83B2B68 = 3;
  `anonymous namespace'::FileTypeNames = 0;
  std::string::string((int)&unk_83B2B84, "Unknown Type");
  std::string::string((int)&unk_83B2B88, "Unknown Type");
  dword_83B2B8C = 1;
  std::string::string((int)&unk_83B2B90, "SandForce Firmware Header");
  std::string::string((int)&unk_83B2B94, "SandForce Firmware Header");
  dword_83B2B98 = 2;
  std::string::string((int)&unk_83B2B9C, "SandForce Firmware Body");
  std::string::string((int)&unk_83B2BA0, "SandForce Firmware Body");
  dword_83B2BA4 = 3;
  std::string::string((int)&unk_83B2BA8, "SandForce Firmeware");
  std::string::string((int)&unk_83B2BAC, "SandForce Firmeware");
  dword_83B2BB0 = 4;
  std::string::string((int)&unk_83B2BB4, "Third Party Firmware");
  std::string::string((int)&unk_83B2BB8, "Third Paty Firmware");
  dword_83B2BBC = 5;
  std::string::string((int)&unk_83B2BC0, "Third Party Configuration");
  std::string::string((int)&unk_83B2BC4, "Third Party Configuration");
  dword_83B2BC8 = 6;
  std::string::string((int)&unk_83B2BCC, "SandForce License");
  std::string::string((int)&unk_83B2BD0, "SandForce License");
  dword_83B2BD4 = 7;
  std::string::string((int)&unk_83B2BD8, "SandForce Release Notes");
  std::string::string((int)&unk_83B2BDC, "SandForce Release Notes");
  dword_83B2BE0 = 0;
  std::string::string((int)&unk_83B2BE4, "Unknown Type");
  std::string::string((int)&unk_83B2BE8, "Unknown Type");
  __cxa_atexit(_tcf_1_0, 0, &_dso_handle);
  `anonymous namespace'::fileTypeNames = (int)&`anonymous namespace'::FileTypeNames;
  dword_83B2BF0 = 9;
  `anonymous namespace'::PackageTypeNames = 0;
  std::string::string((int)&unk_83B2C04, "Unknown Type");
  std::string::string((int)&unk_83B2C08, "Unknown Type");
  dword_83B2C0C = 1;
  std::string::string((int)&unk_83B2C10, "DFP Package");
  std::string::string((int)&unk_83B2C14, "DFP Package");
  dword_83B2C18 = 2;
  std::string::string((int)&unk_83B2C1C, "MFP Package");
  std::string::string((int)&unk_83B2C20, "MFP Package");
  dword_83B2C24 = 3;
  std::string::string((int)&unk_83B2C28, "FFP Package");
  std::string::string((int)&unk_83B2C2C, "FFP Package");
  dword_83B2C30 = 0;
  std::string::string((int)&unk_83B2C34, "Unknown Type");
  std::string::string((int)&unk_83B2C38, "Unknown Type");
  __cxa_atexit(_tcf_2_0, 0, &_dso_handle);
  `anonymous namespace'::packageTypeNames = (int)&`anonymous namespace'::PackageTypeNames;
  dword_83B2C40 = 5;
  `anonymous namespace'::fuseValues = 0;
  std::string::string((int)&unk_83B2C64, "0x0000");
  std::string::string((int)&unk_83B2C68, "A");
  std::string::string((int)&unk_83B2C6C, "All firmware");
  std::string::string((int)&unk_83B2C70, "Blank fuse (to be used on evaluation drives");
  dword_83B2C74 = 0x8000;
  std::string::string((int)&unk_83B2C78, "0x8000");
  std::string::string((int)&unk_83B2C7C, "A");
  std::string::string((int)&unk_83B2C80, "All firmware");
  std::string::string((int)&unk_83B2C84, "Blank fuse (to be used on evaluation drives");
  dword_83B2C88 = 49152;
  std::string::string((int)&unk_83B2C8C, "0xC000");
  std::string::string((int)&unk_83B2C90, "B");
  std::string::string((int)&unk_83B2C94, "B,C,D,E,F,G,H,I");
  std::string::string((int)&unk_83B2C98, "Enterprise SATA overclocked");
  dword_83B2C9C = 57344;
  std::string::string((int)&unk_83B2CA0, "0xE000");
  std::string::string((int)&unk_83B2CA4, "C");
  std::string::string((int)&unk_83B2CA8, "C,D,E,F,G,H,I");
  std::string::string((int)&unk_83B2CAC, "Enterprise SAS");
  dword_83B2CB0 = 61440;
  std::string::string((int)&unk_83B2CB4, "0xF000");
  std::string::string((int)&unk_83B2CB8, "D");
  std::string::string((int)&unk_83B2CBC, "D,E,F,G,H,I");
  std::string::string((int)&unk_83B2CC0, "Enterprise SATA");
  dword_83B2CC4 = 63488;
  std::string::string((int)&unk_83B2CC8, "0xF800");
  std::string::string((int)&unk_83B2CCC, "E");
  std::string::string((int)&unk_83B2CD0, "E,F,G,H");
  std::string::string((int)&unk_83B2CD4, "Industrial");
  dword_83B2CD8 = 64512;
  std::string::string((int)&unk_83B2CDC, "0xFC00");
  std::string::string((int)&unk_83B2CE0, "F");
  std::string::string((int)&unk_83B2CE4, "F,G,H");
  std::string::string((int)&unk_83B2CE8, "Client 6G SATA");
  dword_83B2CEC = 65024;
  std::string::string((int)&unk_83B2CF0, "0xFE00");
  std::string::string((int)&unk_83B2CF4, "G");
  std::string::string((int)&unk_83B2CF8, "G,H");
  std::string::string((int)&unk_83B2CFC, "Client 3G SATA");
  dword_83B2D00 = 65280;
  std::string::string((int)&unk_83B2D04, "0xFF00");
  std::string::string((int)&unk_83B2D08, "H");
  std::string::string((int)&unk_83B2D0C, "H");
  std::string::string((int)&unk_83B2D10, "Client 3G SATA - 4 Channel");
  dword_83B2D14 = 64000;
  std::string::string((int)&unk_83B2D18, "0xFA00");
  std::string::string((int)&unk_83B2D1C, "I");
  std::string::string((int)&unk_83B2D20, "F,G,H,I");
  std::string::string((int)&unk_83B2D24, "Cloud 6G SATA");
  dword_83B2D28 = 0xFFFF;
  std::string::string((int)&unk_83B2D2C, "0xFFFF");
  std::string::string((int)&unk_83B2D30, &byte_82FED0B);
  std::string::string((int)&unk_83B2D34, &byte_82FED0B);
  std::string::string((int)&unk_83B2D38, "Unknown Fuse Value");
*/