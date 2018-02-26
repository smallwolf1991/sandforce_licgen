/*
 * Created by SharpDevelop.
 * User: SMALLWOLF
 * Date: 2017/2/27
 * Time: 12:41
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace GetCfgListFromDFP
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.dfp_txtDFPPath = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dfp_saveDeDfp = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.dfp_textExcelPath = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lic_panelBox = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.btn_Gen = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cb_SFE = new System.Windows.Forms.CheckBox();
            this.cb_SF = new System.Windows.Forms.CheckBox();
            this.cb_SM = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lic_panelSpan = new System.Windows.Forms.Panel();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lic_savepath = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lic_txt_manual = new System.Windows.Forms.TextBox();
            this.lic_span = new System.Windows.Forms.RadioButton();
            this.lic_radiomanual = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lic_createTimestamp = new System.Windows.Forms.DateTimePicker();
            this.txt_CustomerName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_CustomerID2 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txt_CustomerID1 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.lic_manualcfgid = new System.Windows.Forms.ToolTip(this.components);
            this.lic_savefile = new System.Windows.Forms.SaveFileDialog();
            this.dfp_saveDeFile = new System.Windows.Forms.SaveFileDialog();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.lic_panelBox.SuspendLayout();
            this.lic_panelSpan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "DFP 文件|*.DFP|所有文件|*.*";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // dfp_txtDFPPath
            // 
            this.dfp_txtDFPPath.Location = new System.Drawing.Point(98, 7);
            this.dfp_txtDFPPath.Name = "dfp_txtDFPPath";
            this.dfp_txtDFPPath.Size = new System.Drawing.Size(545, 21);
            this.dfp_txtDFPPath.TabIndex = 1;
            this.toolTip1.SetToolTip(this.dfp_txtDFPPath, "双击文本框可以手动选择DFP文件");
            this.dfp_txtDFPPath.TextChanged += new System.EventHandler(this.dfp_txtDFPPath_TextChanged);
            this.dfp_txtDFPPath.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseDoubleClick);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(649, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "解析";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "DFP文件路径：";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dfp_saveDeDfp);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.dfp_textExcelPath);
            this.panel1.Controls.Add(this.dfp_txtDFPPath);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 581);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(888, 66);
            this.panel1.TabIndex = 4;
            // 
            // dfp_saveDeDfp
            // 
            this.dfp_saveDeDfp.Location = new System.Drawing.Point(730, 7);
            this.dfp_saveDeDfp.Name = "dfp_saveDeDfp";
            this.dfp_saveDeDfp.Size = new System.Drawing.Size(144, 23);
            this.dfp_saveDeDfp.TabIndex = 1;
            this.dfp_saveDeDfp.Text = "保存解密文件";
            this.dfp_saveDeDfp.UseVisualStyleBackColor = true;
            this.dfp_saveDeDfp.Click += new System.EventHandler(this.dfp_saveDeDfp_Click);
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(649, 37);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "导出";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Excel路径：";
            // 
            // dfp_textExcelPath
            // 
            this.dfp_textExcelPath.Location = new System.Drawing.Point(98, 38);
            this.dfp_textExcelPath.Name = "dfp_textExcelPath";
            this.dfp_textExcelPath.Size = new System.Drawing.Size(545, 21);
            this.dfp_textExcelPath.TabIndex = 2;
            this.toolTip2.SetToolTip(this.dfp_textExcelPath, "双击文本框可以选择文件夹");
            this.dfp_textExcelPath.TextChanged += new System.EventHandler(this.dfp_textExcelPath_TextChanged);
            this.dfp_textExcelPath.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBox2_MouseDoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTextBox1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(888, 578);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "解析结果";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(3, 17);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(882, 558);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 200;
            this.toolTip1.AutoPopDelay = 3000;
            this.toolTip1.ForeColor = System.Drawing.Color.Violet;
            this.toolTip1.InitialDelay = 200;
            this.toolTip1.ReshowDelay = 40;
            // 
            // toolTip2
            // 
            this.toolTip2.AutoPopDelay = 3000;
            this.toolTip2.InitialDelay = 500;
            this.toolTip2.ReshowDelay = 100;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Excel 2007|*.xlsx";
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(902, 675);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(894, 649);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "解析固件";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.AllowDrop = true;
            this.tabPage2.Controls.Add(this.lic_panelBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(894, 649);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "生成License";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lic_panelBox
            // 
            this.lic_panelBox.Controls.Add(this.label13);
            this.lic_panelBox.Controls.Add(this.btn_Gen);
            this.lic_panelBox.Controls.Add(this.label12);
            this.lic_panelBox.Controls.Add(this.label11);
            this.lic_panelBox.Controls.Add(this.cb_SFE);
            this.lic_panelBox.Controls.Add(this.cb_SF);
            this.lic_panelBox.Controls.Add(this.cb_SM);
            this.lic_panelBox.Controls.Add(this.label10);
            this.lic_panelBox.Controls.Add(this.label8);
            this.lic_panelBox.Controls.Add(this.label9);
            this.lic_panelBox.Controls.Add(this.lic_panelSpan);
            this.lic_panelBox.Controls.Add(this.button1);
            this.lic_panelBox.Controls.Add(this.lic_savepath);
            this.lic_panelBox.Controls.Add(this.label7);
            this.lic_panelBox.Controls.Add(this.lic_txt_manual);
            this.lic_panelBox.Controls.Add(this.lic_span);
            this.lic_panelBox.Controls.Add(this.lic_radiomanual);
            this.lic_panelBox.Controls.Add(this.label5);
            this.lic_panelBox.Controls.Add(this.label4);
            this.lic_panelBox.Controls.Add(this.lic_createTimestamp);
            this.lic_panelBox.Controls.Add(this.txt_CustomerName);
            this.lic_panelBox.Controls.Add(this.label3);
            this.lic_panelBox.Controls.Add(this.txt_CustomerID2);
            this.lic_panelBox.Controls.Add(this.label14);
            this.lic_panelBox.Controls.Add(this.txt_CustomerID1);
            this.lic_panelBox.Controls.Add(this.label15);
            this.lic_panelBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.lic_panelBox.Location = new System.Drawing.Point(3, 3);
            this.lic_panelBox.Name = "lic_panelBox";
            this.lic_panelBox.Size = new System.Drawing.Size(556, 643);
            this.lic_panelBox.TabIndex = 1;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Location = new System.Drawing.Point(312, 137);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(125, 12);
            this.label13.TabIndex = 25;
            this.label13.Text = "注意:不勾选则为False";
            // 
            // btn_Gen
            // 
            this.btn_Gen.Location = new System.Drawing.Point(464, 270);
            this.btn_Gen.Name = "btn_Gen";
            this.btn_Gen.Size = new System.Drawing.Size(75, 23);
            this.btn_Gen.TabIndex = 6;
            this.btn_Gen.Text = "生  成";
            this.btn_Gen.UseVisualStyleBackColor = true;
            this.btn_Gen.Click += new System.EventHandler(this.btn_Gen_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.Red;
            this.label12.Location = new System.Drawing.Point(311, 105);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(125, 12);
            this.label12.TabIndex = 25;
            this.label12.Text = "注意:不勾选则为False";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(311, 74);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(125, 12);
            this.label11.TabIndex = 25;
            this.label11.Text = "注意:不勾选则为False";
            // 
            // cb_SFE
            // 
            this.cb_SFE.AutoSize = true;
            this.cb_SFE.Checked = true;
            this.cb_SFE.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_SFE.Location = new System.Drawing.Point(222, 136);
            this.cb_SFE.Name = "cb_SFE";
            this.cb_SFE.Size = new System.Drawing.Size(84, 16);
            this.cb_SFE.TabIndex = 24;
            this.cb_SFE.Text = "设置为True";
            this.cb_SFE.UseVisualStyleBackColor = true;
            // 
            // cb_SF
            // 
            this.cb_SF.AutoSize = true;
            this.cb_SF.Checked = true;
            this.cb_SF.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_SF.Location = new System.Drawing.Point(197, 104);
            this.cb_SF.Name = "cb_SF";
            this.cb_SF.Size = new System.Drawing.Size(84, 16);
            this.cb_SF.TabIndex = 24;
            this.cb_SF.Text = "设置为True";
            this.cb_SF.UseVisualStyleBackColor = true;
            // 
            // cb_SM
            // 
            this.cb_SM.AutoSize = true;
            this.cb_SM.Checked = true;
            this.cb_SM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_SM.Location = new System.Drawing.Point(197, 73);
            this.cb_SM.Name = "cb_SM";
            this.cb_SM.Size = new System.Drawing.Size(84, 16);
            this.cb_SM.TabIndex = 24;
            this.cb_SM.Text = "设置为True";
            this.cb_SM.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 137);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(209, 12);
            this.label10.TabIndex = 23;
            this.label10.Text = "stampfunction_flashware_encrypted:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 105);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(149, 12);
            this.label8.TabIndex = 21;
            this.label8.Text = "stampfunction_flashware:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 74);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(173, 12);
            this.label9.TabIndex = 19;
            this.label9.Text = "stampfunction_manufacturing:";
            // 
            // lic_panelSpan
            // 
            this.lic_panelSpan.Controls.Add(this.numericUpDown1);
            this.lic_panelSpan.Controls.Add(this.numericUpDown2);
            this.lic_panelSpan.Controls.Add(this.label6);
            this.lic_panelSpan.Enabled = false;
            this.lic_panelSpan.Location = new System.Drawing.Point(145, 237);
            this.lic_panelSpan.Name = "lic_panelSpan";
            this.lic_panelSpan.Size = new System.Drawing.Size(394, 26);
            this.lic_panelSpan.TabIndex = 7;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(3, 3);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(180, 21);
            this.numericUpDown1.TabIndex = 12;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(211, 2);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(180, 21);
            this.numericUpDown2.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(188, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 15;
            this.label6.Text = "到";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(386, 270);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 18;
            this.button1.Text = "浏览";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.lic_savefile_Click);
            // 
            // lic_savepath
            // 
            this.lic_savepath.Location = new System.Drawing.Point(68, 271);
            this.lic_savepath.Name = "lic_savepath";
            this.lic_savepath.Size = new System.Drawing.Size(312, 21);
            this.lic_savepath.TabIndex = 17;
            this.lic_savepath.TextChanged += new System.EventHandler(this.lic_savefile_TextChanged);
            this.lic_savepath.DoubleClick += new System.EventHandler(this.lic_savefile_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 274);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "保存目录:";
            // 
            // lic_txt_manual
            // 
            this.lic_txt_manual.Location = new System.Drawing.Point(145, 204);
            this.lic_txt_manual.Name = "lic_txt_manual";
            this.lic_txt_manual.Size = new System.Drawing.Size(394, 21);
            this.lic_txt_manual.TabIndex = 13;
            this.lic_manualcfgid.SetToolTip(this.lic_txt_manual, "只需要输入id即可，多个id之间仅用英文逗号(\",\")隔开即可。\r\n例如:  \r\n单个ID:  20001\r\n多个ID:  20001，20002");
            // 
            // lic_span
            // 
            this.lic_span.AutoSize = true;
            this.lic_span.Location = new System.Drawing.Point(68, 241);
            this.lic_span.Name = "lic_span";
            this.lic_span.Size = new System.Drawing.Size(71, 16);
            this.lic_span.TabIndex = 11;
            this.lic_span.TabStop = true;
            this.lic_span.Text = "指定范围";
            this.lic_span.UseVisualStyleBackColor = true;
            this.lic_span.CheckedChanged += new System.EventHandler(this.lic_span_CheckedChanged);
            // 
            // lic_radiomanual
            // 
            this.lic_radiomanual.AutoSize = true;
            this.lic_radiomanual.Checked = true;
            this.lic_radiomanual.Location = new System.Drawing.Point(68, 207);
            this.lic_radiomanual.Name = "lic_radiomanual";
            this.lic_radiomanual.Size = new System.Drawing.Size(71, 16);
            this.lic_radiomanual.TabIndex = 10;
            this.lic_radiomanual.TabStop = true;
            this.lic_radiomanual.Text = "手动输入";
            this.lic_radiomanual.UseVisualStyleBackColor = true;
            this.lic_radiomanual.CheckedChanged += new System.EventHandler(this.lic_radiomanual_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 207);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "配置ID:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "生成日期:";
            // 
            // lic_createTimestamp
            // 
            this.lic_createTimestamp.CustomFormat = "MM-dd-yyyy HH:mm";
            this.lic_createTimestamp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.lic_createTimestamp.Location = new System.Drawing.Point(68, 37);
            this.lic_createTimestamp.Name = "lic_createTimestamp";
            this.lic_createTimestamp.Size = new System.Drawing.Size(471, 21);
            this.lic_createTimestamp.TabIndex = 7;
            // 
            // txt_CustomerName
            // 
            this.txt_CustomerName.Location = new System.Drawing.Point(68, 8);
            this.txt_CustomerName.Name = "txt_CustomerName";
            this.txt_CustomerName.Size = new System.Drawing.Size(471, 21);
            this.txt_CustomerName.TabIndex = 5;
            this.txt_CustomerName.Text = "LSI";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "客户名称:";
            // 
            // txt_CustomerID2
            // 
            this.txt_CustomerID2.Location = new System.Drawing.Point(351, 165);
            this.txt_CustomerID2.Name = "txt_CustomerID2";
            this.txt_CustomerID2.Size = new System.Drawing.Size(188, 21);
            this.txt_CustomerID2.TabIndex = 3;
            this.txt_CustomerID2.Text = "0";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(290, 169);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(59, 12);
            this.label14.TabIndex = 2;
            this.label14.Text = "客户 ID2:";
            // 
            // txt_CustomerID1
            // 
            this.txt_CustomerID1.Location = new System.Drawing.Point(68, 165);
            this.txt_CustomerID1.Name = "txt_CustomerID1";
            this.txt_CustomerID1.Size = new System.Drawing.Size(183, 21);
            this.txt_CustomerID1.TabIndex = 1;
            this.txt_CustomerID1.Text = "666";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 169);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(59, 12);
            this.label15.TabIndex = 0;
            this.label15.Text = "客户 ID1:";
            // 
            // lic_manualcfgid
            // 
            this.lic_manualcfgid.ToolTipTitle = "手动录入配置ID提醒";
            // 
            // lic_savefile
            // 
            this.lic_savefile.Filter = "授权文件|*.lic|所有文件|*.*";
            this.lic_savefile.FileOk += new System.ComponentModel.CancelEventHandler(this.lic_savefile_FileOk);
            // 
            // dfp_saveDeFile
            // 
            this.dfp_saveDeFile.FileOk += new System.ComponentModel.CancelEventHandler(this.dfp_saveDeFile_FileOk);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(902, 675);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "Sandforce 工具";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.lic_panelBox.ResumeLayout(false);
            this.lic_panelBox.PerformLayout();
            this.lic_panelSpan.ResumeLayout(false);
            this.lic_panelSpan.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox dfp_txtDFPPath;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox dfp_textExcelPath;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolTip lic_manualcfgid;
        private System.Windows.Forms.SaveFileDialog lic_savefile;
        private System.Windows.Forms.Panel lic_panelBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btn_Gen;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox cb_SFE;
        private System.Windows.Forms.CheckBox cb_SF;
        private System.Windows.Forms.CheckBox cb_SM;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel lic_panelSpan;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox lic_savepath;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox lic_txt_manual;
        private System.Windows.Forms.RadioButton lic_span;
        private System.Windows.Forms.RadioButton lic_radiomanual;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker lic_createTimestamp;
        private System.Windows.Forms.TextBox txt_CustomerName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_CustomerID2;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txt_CustomerID1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button dfp_saveDeDfp;
        private System.Windows.Forms.SaveFileDialog dfp_saveDeFile;
	}
}
