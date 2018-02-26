/*
 * Created by SharpDevelop.
 * User: SMALLWOLF
 * Date: 2017/2/27
 * Time: 16:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GetCfgListFromDFP
{
    /// <summary>
    /// Description of Encrypt.
    /// </summary>
    public class Encrypt
    {
        RijndaelManaged rijalg;
        private byte[] key = new byte[] { 0x0B, 0x3F, 0x3E, 0xDD, 0x55, 0xA4, 0xC8, 0x85, 0x34, 0x24, 0x15, 0x3E, 0xD7, 0x87, 0xA9, 0x5A };
        private byte[] iv = new byte[] { 0x4A, 0x8D, 0x46, 0x52, 0xB3, 0x56, 0xED, 0xD8, 0x17, 0x5A, 0x9D, 0xB1, 0x3E, 0x69, 0x1B, 0x32 };
        public Encrypt()
        {
            //-----------------  
            //設定 cipher 格式 AES-256-CBC  
            rijalg = new RijndaelManaged();
            rijalg.Padding = PaddingMode.None;
            rijalg.Mode = CipherMode.CBC;
            rijalg.BlockSize = 128;
            rijalg.KeySize = 256;
            rijalg.FeedbackSize = 128;

            rijalg.Key = key;
            rijalg.IV = iv;

        }
         ~Encrypt()
        {
            rijalg.Dispose();

        }

        private void addByte(int count, ref List<byte> buff)
        {
            for (int i = 0; i < count; i++)
            {
                buff.Add(0);
            }
        }

        private byte[] paddingBytes(byte[] buff)
        {
            List<byte> result = new List<byte>(buff);
            if (buff.Length < 16)
            {
                addByte(16 - buff.Length, ref result);
            }
            else if (buff.Length % 16 == 0)
            {
                addByte(16, ref result);
            }
            else if (buff.Length % 16 != 0)
            {
                addByte(16 - buff.Length % 16, ref result);
            }
            return result.ToArray();
        }
        public byte[] decrypt(byte[] buff)
        {
            byte[] tempPaddingBuff = paddingBytes(buff);
            ICryptoTransform decryptor = rijalg.CreateDecryptor();
            using (MemoryStream msDecrypt = new MemoryStream(tempPaddingBuff))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (BinaryReader srDecrypt = new BinaryReader(csDecrypt))
                    {
                        List<byte> debuff = new List<byte>();
                        byte by = srDecrypt.ReadByte();
                        int offset = 0;
                        do
                        {
                            debuff.Add(by);
                            offset++;
                            if (offset >= buff.Length)
                            {
                                break;
                            }
                            by = srDecrypt.ReadByte();
                        }
                        while (true);
                        byte[] tempBuff = debuff.ToArray();
                        return tempBuff;
                    }
                }
            }
        }
    }
}
