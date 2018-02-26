/*
 * Created by SharpDevelop.
 * User: SMALLWOLF
 * Date: 2017/2/27
 * Time: 12:41
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Security.Cryptography;
using System.Threading;
using System.IO;


namespace GetCfgListFromDFP
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        // dfp
        private Stream fs;
        private DFPHeader dfpHeader;
        const int MB_UNIT_SIZE = 1024 * 1024;



        // checksum
        const string RootName = "customer";
        string NEWLINE = "\n";
        private byte[] BYTENEWLINE = null;
        private bool manualInput = true;

        public MainForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            BYTENEWLINE = Encoding.ASCII.GetBytes(NEWLINE);
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        #region Checksum相关方法

        public string GetSHA256Hash(byte[] bytValue)
        {
            byte[] prefixData = Encoding.ASCII.GetBytes("This is the super secret intial string @!I#U*(!HDHF!*");
            byte[] comboBuff = new byte[prefixData.Length + bytValue.Length];
            prefixData.CopyTo(comboBuff, 0);
            bytValue.CopyTo(comboBuff, prefixData.Length);
            try
            {
                SHA256 sha256 = new SHA256CryptoServiceProvider();
                byte[] retVal = sha256.ComputeHash(comboBuff);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetSHA256Hash() fail,Error:" + ex.Message);
            }
        }
        private void btn_Gen_Click(object sender, EventArgs e)
        {
            string customerName = this.txt_CustomerName.Text;
            string timestamp = this.lic_createTimestamp.Value.ToString("MM-dd-yyyy HH:mm");
            string customerId1 = this.txt_CustomerID1.Text;
            string customerId2 = this.txt_CustomerID2.Text;
            List<string> configIds = new List<string>();

            if (string.IsNullOrEmpty(customerName) || string.IsNullOrEmpty(customerId1) || string.IsNullOrEmpty(customerId2) || string.IsNullOrEmpty(timestamp))
            {
                MessageBox.Show("请输入完整的数据后再生成！");
                return;
            }

            if (manualInput)
            {
                configIds = this.lic_txt_manual.Text.Trim().Split(new char[] { ',' }).ToList<string>();
            }
            else
            {
                int startSpan = (int)this.numericUpDown1.Value;
                int endSpan = (int)this.numericUpDown2.Value;
                for (int i = startSpan; i <= endSpan; i++)
                {
                    configIds.Add(i.ToString());
                }
            }


            if (string.IsNullOrEmpty(customerName) || string.IsNullOrEmpty(customerId1) || string.IsNullOrEmpty(customerId2) || string.IsNullOrEmpty(timestamp) || configIds.Count == 0)
            {
                MessageBox.Show(this, "请输入完整的数据后再生成！", "错误：请填写全部的区域", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            XmlDocument xmldoc = new XmlDocument();
            //加入XML的声明段落,<?xml version="1.0" encoding="gb2312"?>
            XmlDeclaration xmldecl;
            xmldecl = xmldoc.CreateXmlDeclaration("1.0", null, null);
            xmldoc.AppendChild(xmldecl);

            //加入一个根元素
            XmlElement xmlroot = xmldoc.CreateElement(null, RootName, null);
            xmlroot.SetAttribute("date_created", timestamp);
            xmldoc.AppendChild(xmlroot);


            XmlNode root = xmldoc.SelectSingleNode(RootName);//查找<Employees> 
            XmlElement xmlCustomerNameNode = xmldoc.CreateElement("customer_name");//创建一个<Node>节点 
            xmlCustomerNameNode.InnerText = customerName;
            XmlElement xmlCustomerID1Node = xmldoc.CreateElement("customer_id1");//创建一个<Node>节点 
            xmlCustomerID1Node.InnerText = customerId1;
            XmlElement xmlCustomerID2Node = xmldoc.CreateElement("customer_id2");//创建一个<Node>节点 
            xmlCustomerID2Node.InnerText = customerId2;
            root.AppendChild(xmlCustomerNameNode);
            root.AppendChild(xmlCustomerID1Node);
            root.AppendChild(xmlCustomerID2Node);


            XmlElement xmlStampFunctionNode1 = xmldoc.CreateElement("stampfunction_manufacturing");//创建一个<Node>节点 
            xmlStampFunctionNode1.InnerText = this.cb_SM.Checked ? "True" : "False";
            XmlElement xmlStampFunctionNode2 = xmldoc.CreateElement("stampfunction_flashware");//创建一个<Node>节点 
            xmlStampFunctionNode2.InnerText = this.cb_SF.Checked ? "True" : "False";
            XmlElement xmlStampFunctionNode3 = xmldoc.CreateElement("stampfunction_flashware_encrypted");//创建一个<Node>节点 
            xmlStampFunctionNode3.InnerText = this.cb_SFE.Checked ? "True" : "False";
            root.AppendChild(xmlStampFunctionNode1);
            root.AppendChild(xmlStampFunctionNode2);
            root.AppendChild(xmlStampFunctionNode3);

            foreach (string id in configIds)
            {
                XmlElement config = xmldoc.CreateElement("configuration_id");
                config.SetAttribute("id", id.Trim());
                root.AppendChild(config);//添加到<Employees>节点中 
            }
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.ASCII;
            settings.Indent = true;
            settings.IndentChars = " ";
            settings.NewLineChars = NEWLINE;
            settings.NewLineOnAttributes = false;
            MemoryStream ms = new MemoryStream();
            XmlWriter xmlWriter = XmlWriter.Create(ms, settings);
            xmldoc.Save(xmlWriter);
            xmlWriter.Close();
            Stream st = this.lic_savefile.OpenFile();
            byte[] buff = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(buff, 0, (int)ms.Length);
            byte[] addedSpaceBuff = new byte[buff.Length + BYTENEWLINE.Length];
            buff.CopyTo(addedSpaceBuff, 0);
            BYTENEWLINE.CopyTo(addedSpaceBuff, buff.Length);

            string strChecksum = GetSHA256Hash(addedSpaceBuff);

            byte[] checksumNodeBuff = Encoding.ASCII.GetBytes(string.Format("<checksum>{1}</checksum>", NEWLINE, strChecksum));
            byte[] resultBuff = new byte[addedSpaceBuff.Length + checksumNodeBuff.Length];
            addedSpaceBuff.CopyTo(resultBuff, 0);
            checksumNodeBuff.CopyTo(resultBuff, addedSpaceBuff.Length);
            st.Write(resultBuff, 0, resultBuff.Length);
            st.Close();
            st.Dispose();
            Thread.Sleep(500);

            //System.Diagnostics.Process.Start(Environment.CurrentDirectory + "\\sf.exe", this.saveFileDialog1.FileName);
            MessageBox.Show(this, "生成成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void lic_radiomanual_CheckedChanged(object sender, EventArgs e)
        {
            this.lic_txt_manual.Enabled = true;
            this.lic_panelSpan.Enabled = false;
            manualInput = true;
        }

        private void lic_span_CheckedChanged(object sender, EventArgs e)
        {
            this.lic_panelSpan.Enabled = true;
            this.lic_txt_manual.Enabled = false;
            manualInput = false;
        }

        private void lic_savefile_Click(object sender, EventArgs e)
        {
            this.lic_savefile.ShowDialog(this);
        }

        private void lic_savefile_FileOk(object sender, CancelEventArgs e)
        {
            this.lic_savepath.Text = this.lic_savefile.FileName;
        }

        private void txt_CustomerName_TextChanged(object sender, EventArgs e)
        {
            string fileName = this.txt_CustomerName.Text;
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return;
            }
            this.lic_savepath.Text = Environment.CurrentDirectory + "\\" + fileName + ".lic";
            this.lic_savefile.FileName = this.lic_savepath.Text;
        }

        private void lic_savefile_TextChanged(object sender, EventArgs e)
        {
            this.lic_savefile.FileName = this.lic_savepath.Text;
        }

        #endregion

        #region DFP 相关方法
        private void writeLog(DFPHeader dfp)
        {
            DateTime stamp = DateTime.Parse(dfp.Timestamp);
            this.richTextBox1.AppendText(string.Format(" 文件头Checksum: \t0x{0:X8}\r\n", dfp.HeaderCheckSum));
            this.richTextBox1.AppendText(string.Format(" 文件Checksum: \t\t0x{0:X8}\r\n", dfp.FileCheckSum));
            this.richTextBox1.AppendText(string.Format(" 发布名称: \t\t{0}\r\n", dfp.DFPReleaseName));
            this.richTextBox1.AppendText(string.Format(" 发布版本: \t\t{0:D}\r\n", dfp.DFPReleaseID));
            this.richTextBox1.AppendText(string.Format(" 发布时间: \t\t{0}\r\n", stamp.ToString("yyyy/MM/dd HH:mm:ss")));
            this.richTextBox1.AppendText(string.Format(" Manifest起始位置: \t{0:D}(0x{0:X8})\r\n", dfp.ManifestOffset));
            this.richTextBox1.AppendText(string.Format(" Manifest文件长度: \t{0:D}(0x{0:X8})字节\r\n", dfp.ManifestLength));
            this.richTextBox1.AppendText(string.Format(" Manifest文件数: \t{0:D}(0x{0:X8})个\r\n", dfp.ManifestCount));
            this.richTextBox1.AppendText(string.Format(" 配置文件起始位置: \t{0:D}(0x{0:X8})\r\n", dfp.ConfigurationOffset));
            this.richTextBox1.AppendText(string.Format(" 配置文件长度: \t\t{0:D}(0x{0:X8})字节\r\n", dfp.ConfigurationLength));
            this.richTextBox1.AppendText(string.Format(" 配置文件数: \t\t{0:D}(0x{0:X8})个\r\n", dfp.ConfigurationCount));
            this.richTextBox1.AppendText(string.Format(" 版本号: \t\t{0}\r\n", dfp.DFPVersion));
        }
        private void writeLog(string msg)
        {
            this.richTextBox1.AppendText(msg);
        }
        private bool readFile()
        {
            if (File.Exists(this.dfp_txtDFPPath.Text))
            {
                fs = this.openFileDialog1.OpenFile();
                byte[] headerSection = new byte[1024 * 4];
                int readLen = fs.Read(headerSection, 0, headerSection.Length);
                if (readLen != headerSection.Length)
                {
                    MessageBox.Show("文件头长度错误.");
                    return false;
                }

                Encrypt AESDecrypt = new Encrypt();
                byte[] decryptHeaderBuff = AESDecrypt.decrypt(headerSection);
                dfpHeader = new DFPHeader(decryptHeaderBuff);
                writeLog(dfpHeader);
                writeLog(" 正在解析配置文件...\r\n");
                fs.Seek(0, SeekOrigin.Begin);

                int length = (int)dfpHeader.ConfigurationOffset + (int)dfpHeader.ConfigurationLength;
                length = length - length % 256 + length + 256;
                byte[] cfgbuff = new byte[length];
                fs.Read(cfgbuff, 0, length);

                byte[] decryptCfgBuff = AESDecrypt.decrypt(cfgbuff);
                dfpHeader.ReadConfigurations(decryptCfgBuff);
                writeLog(" 解析配置文件完毕！\r\n");
                return true;
            }
            return false;
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.dfp_txtDFPPath.Text = this.openFileDialog1.FileName;
            this.button3.Enabled = false;
        }

        private void dfp_txtDFPPath_TextChanged(object sender, EventArgs e)
        {
            string fileName = this.dfp_txtDFPPath.Text;
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return;
            }
            this.openFileDialog1.FileName = fileName;
            this.dfp_textExcelPath.Text = string.Format("{0}.xlsx", fileName.Substring(0, fileName.Length - 4));
            this.saveFileDialog1.FileName = this.dfp_textExcelPath.Text;
            this.button3.Enabled = false;
        }
        private void textBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.openFileDialog1.ShowDialog(this);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string fileName = this.dfp_textExcelPath.Text;
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }
            try
            {
                if (OutDataToExcel(fileName, dfpHeader.Configurations))
                {
                    writeLog(string.Format(" 导出Excel成功. 保存路径:{0}\r\n", fileName));
                }
                else
                {
                    writeLog(" 导出Excel失败.\r\n");
                }
            }
            catch
            {
                writeLog(" 导出Excel失败.\r\n");
                SaveCSV(fileName, dfpHeader.Configurations);
            }
        }

        private void textBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.saveFileDialog1.ShowDialog(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (readFile())
                {
                    this.button3.Enabled = true;
                    //this.dfp_saveDeDfp.Enabled = true;
                }
                else
                {
                    this.button3.Enabled = false;
                    //this.dfp_saveDeDfp.Enabled = false;

                }
            }
            catch (Exception ex)
            {
                writeLog(string.Format(" 这个DFP文件貌似格式不太对,或者已经是解密的文件了. [{0}]\r\n", ex.Message));
            }
        }

        public void SaveCSV(string fileName, DataTable dt)
        {
            fileName = fileName.Substring(0, fileName.Length - 5) + ".csv";
            writeLog(" 转换为csv文件导出...\r\n");
            FileStream fs = new FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            string data = "";

            //写出列名称
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                data += dt.Columns[i].ColumnName.ToString();
                if (i < dt.Columns.Count - 1)
                {
                    data += ",";
                }
            }
            sw.WriteLine(data);

            //写出各行数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                data = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    data += dt.Rows[i][j].ToString();
                    if (j < dt.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);
            }

            sw.Close();
            fs.Close();
            writeLog(" CSV文件保存成功！\r\n");
        }

        public bool OutDataToExcel(string excelFilePath, System.Data.DataTable srcDataTable)
        {
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            object missing = System.Reflection.Missing.Value;
            if (xlApp == null)
            {
                MessageBox.Show("无法创建Excel对象，可能您的电脑未安装Excel!");
                return false;
            }

            Microsoft.Office.Interop.Excel.Workbooks xlBooks = xlApp.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook xlBook = xlBooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            //导出到execl   
            try
            {

                Microsoft.Office.Interop.Excel.Worksheet xlSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlBook.Worksheets[1];

                //让后台执行设置为不可见，为true的话会看到打开一个Excel，然后数据在往里写  
                xlApp.Visible = false;

                object[,] objData = new object[srcDataTable.Rows.Count + 1, srcDataTable.Columns.Count];
                //首先将数据写入到一个二维数组中  
                for (int i = 0; i < srcDataTable.Columns.Count; i++)
                {
                    objData[0, i] = srcDataTable.Columns[i].ColumnName;
                }
                if (srcDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < srcDataTable.Rows.Count; i++)
                    {
                        for (int j = 0; j < srcDataTable.Columns.Count; j++)
                        {
                            objData[i + 1, j] = srcDataTable.Rows[i][j];
                        }
                    }
                }

                string startCol = "A";
                int iCnt = ((srcDataTable.Columns.Count - 1) / 26);
                string endColSignal = (iCnt == 0 ? "" : ((char)('A' + (iCnt - 1))).ToString());
                string endCol = endColSignal + ((char)('A' + srcDataTable.Columns.Count - iCnt * 26 - 1)).ToString();
                Microsoft.Office.Interop.Excel.Range range = xlSheet.get_Range(startCol + "1", endCol + srcDataTable.Rows.Count.ToString());

                range.Value = objData; //给Exccel中的Range整体赋值  
                range.EntireColumn.AutoFit(); //设定Excel列宽度自适应  
                xlSheet.get_Range(startCol + "1", endCol + "1").Font.Bold = 1;//Excel文件列名 字体设定为Bold  

                //设置禁止弹出保存和覆盖的询问提示框  
                xlApp.DisplayAlerts = false;
                xlApp.AlertBeforeOverwriting = false;

                if (xlSheet != null)
                {
                    xlSheet.SaveAs(excelFilePath, missing, missing, missing, missing, missing, missing, missing, missing, missing);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {

                xlBook.Close(false, missing, missing);
                xlBooks.Close();
                xlApp.Quit();
                xlBooks = null;
                xlBook = null;
                xlApp = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.dfp_textExcelPath.Text = this.saveFileDialog1.FileName;
        }



        private void dfp_saveDeDfp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.dfp_txtDFPPath.Text.Trim()))
            {
                return;
            }
            this.dfp_saveDeFile.ShowDialog(this);

        }

        #endregion

        Thread th = null;
        private void dfp_saveDeFile_FileOk(object sender, CancelEventArgs e)
        {
            if (th != null && th.ThreadState == ThreadState.Running)
            {
                return;
            }
            th = new Thread(() =>
            {
                FileInfo fi = new FileInfo(this.openFileDialog1.FileName);
                long fileLength = (int)fi.Length;
                Stream saveFile = this.dfp_saveDeFile.OpenFile();
                fs = this.openFileDialog1.OpenFile();
                byte[] blockBuff = new byte[MB_UNIT_SIZE * 32];
                int readLen = 1;
                Encrypt AESDecrypt = new Encrypt();
                byte[] decryptHeaderBuff = null;
                while (fs.Position < fs.Length)
                {
                    this.Invoke(new Action(() => { writeLog(string.Format(" 正在解密, 还剩余 {0} MByte...\r\n", (int)Math.Floor((double)(fileLength - saveFile.Position - 1) / MB_UNIT_SIZE))); }));
                    readLen = fs.Read(blockBuff, 0, blockBuff.Length);
                    if (blockBuff.Length > readLen)
                    {
                        blockBuff = blockBuff.Take(readLen).ToArray();
                    }
                    decryptHeaderBuff = AESDecrypt.decrypt(blockBuff);
                    saveFile.Write(decryptHeaderBuff, 0, decryptHeaderBuff.Length);
                    Array.Clear(blockBuff, 0, blockBuff.Length);
                }
                saveFile.Close();
                saveFile.Dispose();
                this.Invoke(new Action(() => { writeLog(" 解密完毕！\r\n"); }));
            });

            th.Start();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (th != null && th.ThreadState == ThreadState.Running)
            {
                th.Abort();
            };
        }

        private void dfp_textExcelPath_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
