using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using FlatUI;
using Luicipher.Properties;
using Microsoft.Win32;

namespace Luicipher
{
	// Token: 0x0200000A RID: 10
	public partial class Form1 : Form
	{
		// Token: 0x06000050 RID: 80
		[DllImport("kernel32.dll")]
		public static extern IntPtr LoadLibrary(string dllToLoad);

		// Token: 0x06000051 RID: 81
		[DllImport("kernel32.dll")]
		public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

		// Token: 0x06000052 RID: 82
		[DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int cch);

		// Token: 0x06000053 RID: 83
		[DllImport("user32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr GetForegroundWindow();

		// Token: 0x06000054 RID: 84
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int GetWindowTextLength(IntPtr hWnd);

		// Token: 0x06000055 RID: 85
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool GetAsyncKeyState(int vKey);

		// Token: 0x06000056 RID: 86
		[DllImport("user32.dll")]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		// Token: 0x06000057 RID: 87
		[DllImport("user32.dll")]
		private static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

		// Token: 0x06000058 RID: 88
		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		// Token: 0x06000059 RID: 89
		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();

		// Token: 0x0600005A RID: 90
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		private static extern short GetKeyState(int keyCode);

		// Token: 0x0600005B RID: 91 RVA: 0x0029E7F8 File Offset: 0x00293FF8
		private static Form1.KeyStates GetKeyState(Keys key)
		{
			Form1.KeyStates keyStates = Form1.KeyStates.None;
			short keyState = Form1.GetKeyState((int)key);
			if (((int)keyState & 32768) == 32768)
			{
				keyStates |= Form1.KeyStates.Down;
			}
			if ((keyState & 1) == 1)
			{
				keyStates |= Form1.KeyStates.Toggled;
			}
			return keyStates;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0035C000 File Offset: 0x0034B600
		private static string title()
		{
			string result = string.Empty;
			IntPtr foregroundWindow = Form1.GetForegroundWindow();
			int num = Form1.GetWindowTextLength(foregroundWindow) + 1;
			StringBuilder stringBuilder = new StringBuilder(num);
			if (Form1.GetWindowText(foregroundWindow, stringBuilder, num) > 0)
			{
				result = stringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0029C3DD File Offset: 0x00291BDD
		public static bool IsKeyDown(Keys key)
		{
			return Form1.KeyStates.Down == (Form1.GetKeyState(key) & Form1.KeyStates.Down);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00332007 File Offset: 0x00324007
		public void checkVersion()
		{
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0033200C File Offset: 0x0032400C
		public void checkAuth()
		{
			this.web = "true";
			MessageBox.Show("Efor Sarf Etmedim  ", "verfired", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			if (this.web != "true")
			{
				MessageBox.Show("eheh", "", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Clipboard.SetText(this.HWID());
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00332007 File Offset: 0x00324007
		public void randomizationThread()
		{
		}

		// Token: 0x06000061 RID: 97 RVA: 0x0029EA2C File Offset: 0x0029422C
		private string HWID()
		{
			string name = "SOFTWARE\\Microsoft\\Cryptography";
			string name2 = "MachineGuid";
			string result;
			using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
			{
				using (RegistryKey registryKey2 = registryKey.OpenSubKey(name))
				{
					object value = registryKey2.GetValue(name2);
					result = this.hash(value.ToString());
				}
			}
			return result;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x0029EAD8 File Offset: 0x002942D8
		public string hash(string toEncrypt)
		{
			string result;
			using (SHA256 sha = SHA256.Create())
			{
				byte[] array = sha.ComputeHash(Encoding.UTF8.GetBytes(toEncrypt));
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < array.Length; i++)
				{
					stringBuilder.Append(array[i].ToString("x2"));
				}
				result = stringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x0029C3F5 File Offset: 0x00291BF5
		public string version()
		{
			return "2.0";
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0029EB8C File Offset: 0x0029438C
		public Form1()
		{
			this.releaseId = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "ReleaseId", "").ToString();
			IntPtr hModule = Form1.LoadLibrary("kernel32.dll");
			IntPtr procAddress = Form1.GetProcAddress(hModule, "IsDebuggerPresent");
			byte[] array = new byte[1];
			Marshal.Copy(procAddress, array, 0, 1);
			IntPtr procAddress2 = Form1.GetProcAddress(hModule, "CheckRemoteDebuggerPresent");
			array = new byte[1];
			Marshal.Copy(procAddress2, array, 0, 1);
			if (array[0] == 233 || array[0] == 233)
			{
				
				Environment.Exit(0);
			}
			this.InitializeComponent();
			this.checkAuth();
			this.web = " ";
			this.ver = " ";
			this.randomizationThread();
			
			this.mainMenu.BringToFront();
			this.mainMenu.Location = new Point(0, 70);
			this.presetsMenu.Location = new Point(0, 70);
			this.otherMenu.Location = new Point(0, 70);
			this.doubleClickerMenu.Location = new Point(0, 70);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x0029EE24 File Offset: 0x00294624
		private void bindChecker_Tick(object sender, EventArgs e)
		{
			if (Form1.FindWindow("LWJGL", Form1.title().ToString()).ToString() == Form1.GetForegroundWindow().ToString())
			{
				return;
			}
			List<string> list = new List<string>();
			list.Clear();
			list.Add("processhacker");
			list.Add("ollydbg");
			list.Add("tcpview");
			list.Add("autoruns");
			list.Add("autorunsc");
			list.Add("filemon");
			list.Add("procmon");
			list.Add("idag");
			list.Add("hookshark");
			list.Add("peid");
			list.Add("lordpe");
			list.Add("regmon");
			list.Add("idaq");
			list.Add("idaq64");
			list.Add("immunitydebugger");
			list.Add("wireshark");
			list.Add("dumpcap");
			list.Add("hookexplorer");
			list.Add("importrec");
			list.Add("petools");
			list.Add("lordpe");
			list.Add("sysinspector");
			list.Add("proc_analyzer");
			list.Add("sysanalyzer");
			list.Add("sniff_hit");
			list.Add("joeboxcontrol");
			list.Add("joeboxserver");
			list.Add("ida");
			list.Add("ida64");
			list.Add("httpdebuggersvc");
			list.Add("driverview");
			list.Add("dbgview");
			list.Add("glasswire");
			list.Add("winobj");
			list.Add("megadumper");
			foreach (Process process in Process.GetProcesses())
			{
				if (list.Contains(process.ProcessName.ToLower()) && !this.alreadyAlerted)
				{
					this.debuggerRunning = process.ProcessName;
					this.alreadyAlerted = true;
					
				}
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0029F110 File Offset: 0x00294910
		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			base.Hide();
			e.Cancel = true;
			if (this.wtfClicks <= 50)
			{
				
			}
			Program.appendHWID();
			Environment.Exit(0);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0029F174 File Offset: 0x00294974
		private void mainButton_Click(object sender, EventArgs e)
		{
			if (this.mainSelected)
			{
				return;
			}
			this.updateColor();
			this.mainMenu.BringToFront();
			this.mainButton.ForeColor = Color.Blue;
			this.Refresh();
			while (this.indicator.Location.X >= this.mainButton.Location.X)
			{
				this.updateIndicator(this.indicator.Location.X - 1, this.indicator.Location.Y);
			}
			this.updateSelected();
			this.mainSelected = true;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0029C3FC File Offset: 0x00291BFC
		public void updateIndicator(int locationX, int locationY)
		{
			this.indicator.Location = new Point(locationX, locationY);
			this.indicator.Refresh();
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0029C427 File Offset: 0x00291C27
		public void updateSelected()
		{
			this.mainSelected = false;
			this.presetsSelected = false;
			this.otherSelected = false;
			this.doubleClickerSelected = false;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0029F268 File Offset: 0x00294A68
		public void updateColor()
		{
			this.mainButton.ForeColor = Color.FromArgb(225, 225, 225);
			this.otherButton.ForeColor = Color.FromArgb(225, 225, 225);
			this.doubleClickerButton.ForeColor = Color.FromArgb(225, 225, 225);
			this.presetsButton.ForeColor = Color.FromArgb(225, 225, 225);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0029C461 File Offset: 0x00291C61
		private void leftSlider_Scroll(object sender)
		{
			this.leftSliderText.Text = string.Format("{0}", (double)this.leftSlider.Value / 10.0);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0029C499 File Offset: 0x00291C99
		private void jitterStrengthSlider_Scroll(object sender)
		{
			this.jitterStrengthText.Text = string.Format("{0}", this.jitterStrengthSlider.Value / 10);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0029C4CC File Offset: 0x00291CCC
		private void rightSlider_Scroll(object sender)
		{
			this.rightSliderText.Text = string.Format("{0}", (double)this.rightSlider.Value / 10.0);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0029F300 File Offset: 0x00294B00
		private void decreaseJitterSlider_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 10; i++)
			{
				this.jitterStrengthSlider.Value--;
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x0029F34C File Offset: 0x00294B4C
		private void increaseRightSlider_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 5; i++)
			{
				this.rightSlider.Value++;
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0029F398 File Offset: 0x00294B98
		private void decreaseRightSlider_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 5; i++)
			{
				this.rightSlider.Value--;
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0029F3E4 File Offset: 0x00294BE4
		private void increaseJitterSlider_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 10; i++)
			{
				this.jitterStrengthSlider.Value++;
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0029F430 File Offset: 0x00294C30
		private void decreaseLeftSlider_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 5; i++)
			{
				this.leftSlider.Value--;
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0029F47C File Offset: 0x00294C7C
		private void increaseLeftSlider_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 5; i++)
			{
				this.leftSlider.Value++;
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0029F4C8 File Offset: 0x00294CC8
		private void toggleRightBlatantMode_Click(object sender, EventArgs e)
		{
			if (!this.rightBlatantMode)
			{
				this.rightBlatantMode = true;
			}
			else
			{
				this.rightBlatantMode = false;
			}
			if (this.rightBlatantMode)
			{
				this.toggleRightBlatantMode.BaseColor = Color.Blue;
				this.rightSlider.Maximum = 500;
			}
			else
			{
				this.toggleRightBlatantMode.BaseColor = Color.FromArgb(60, 60, 60);
				this.rightSlider.Maximum = 200;
				this.rightSliderText.Text = string.Format("{0}", (double)this.rightSlider.Value / 10.0);
			}
			this.Refresh();
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0029F5AC File Offset: 0x00294DAC
		private void toggleRightExtraRandomization_Click(object sender, EventArgs e)
		{
			if (!this.rightExtraRandomization)
			{
				this.rightExtraRandomization = true;
			}
			else
			{
				this.rightExtraRandomization = false;
			}
			if (this.rightExtraRandomization)
			{
				this.toggleRightExtraRandomization.BaseColor = Color.Blue;
			}
			else
			{
				this.toggleRightExtraRandomization.BaseColor = Color.FromArgb(60, 60, 60);
			}
			this.Refresh();
		}

		// Token: 0x06000076 RID: 118 RVA: 0x0029F634 File Offset: 0x00294E34
		private void toggleShiftDisable_Click(object sender, EventArgs e)
		{
			if (!this.leftShiftDisable)
			{
				this.leftShiftDisable = true;
			}
			else
			{
				this.leftShiftDisable = false;
			}
			if (this.leftShiftDisable)
			{
				this.toggleShiftDisable.BaseColor = Color.Blue;
			}
			else
			{
				this.toggleShiftDisable.BaseColor = Color.FromArgb(60, 60, 60);
			}
			this.Refresh();
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0029F6BC File Offset: 0x00294EBC
		private void toggleJitter_Click(object sender, EventArgs e)
		{
			if (!this.leftJitter)
			{
				this.leftJitter = true;
			}
			else
			{
				this.leftJitter = false;
			}
			if (this.leftJitter)
			{
				this.toggleJitter.BaseColor = Color.Blue;
			}
			else
			{
				this.toggleJitter.BaseColor = Color.FromArgb(60, 60, 60);
			}
			this.Refresh();
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0029F744 File Offset: 0x00294F44
		private void toggleLeftBlatantMode_Click(object sender, EventArgs e)
		{
			if (!this.leftBlatantMode)
			{
				this.leftBlatantMode = true;
			}
			else
			{
				this.leftBlatantMode = false;
			}
			if (this.leftBlatantMode)
			{
				this.toggleLeftBlatantMode.BaseColor = Color.Blue;
				this.leftSlider.Maximum = 500;
			}
			else
			{
				this.toggleLeftBlatantMode.BaseColor = Color.FromArgb(60, 60, 60);
				this.leftSlider.Maximum = 200;
				this.leftSliderText.Text = string.Format("{0}", (double)this.leftSlider.Value / 10.0);
			}
			this.Refresh();
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0029F828 File Offset: 0x00295028
		private void toggleLeftExtraRandomization_Click(object sender, EventArgs e)
		{
			if (!this.leftExtraRandomization)
			{
				this.leftExtraRandomization = true;
			}
			else
			{
				this.leftExtraRandomization = false;
			}
			if (this.leftExtraRandomization)
			{
				this.toggleLeftExtraRandomization.BaseColor = Color.Blue;
			}
			else
			{
				this.toggleLeftExtraRandomization.BaseColor = Color.FromArgb(60, 60, 60);
			}
			this.Refresh();
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0029F8B0 File Offset: 0x002950B0
		private void hypixelPresetButton_Click(object sender, EventArgs e)
		{
			this.leftSlider.Value = 130;
			this.rightSlider.Value = 175;
			this.leftExtraRandomization = false;
			this.toggleLeftExtraRandomization.BaseColor = Color.FromArgb(60, 60, 60);
			this.rightExtraRandomization = false;
			this.toggleRightExtraRandomization.BaseColor = Color.FromArgb(60, 60, 60);
			this.Refresh();
			this.recentPreset.Text = "Hypixel Preset";
			MessageBox.Show("Successfully loaded the Hypixel preset!                ", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0029F97C File Offset: 0x0029517C
		private void viperPresetButton_Click(object sender, EventArgs e)
		{
			this.leftSlider.Value = 150;
			this.rightSlider.Value = 150;
			this.leftExtraRandomization = true;
			this.toggleLeftExtraRandomization.BaseColor = Color.Blue;
			this.rightExtraRandomization = true;
			this.toggleRightExtraRandomization.BaseColor = Color.Blue;
			this.Refresh();
			this.recentPreset.Text = "ViperMC Preset";
			MessageBox.Show("Successfully loaded the ViperMC preset!                ", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0029FA30 File Offset: 0x00295230
		private void lunarPresetButton_Click(object sender, EventArgs e)
		{
			this.leftSlider.Value = 140;
			this.rightSlider.Value = 150;
			this.leftExtraRandomization = true;
			this.toggleLeftExtraRandomization.BaseColor = Color.Blue;
			this.rightExtraRandomization = true;
			this.toggleRightExtraRandomization.BaseColor = Color.Blue;
			this.Refresh();
			this.recentPreset.Text = "Lunar Preset";
			MessageBox.Show("Successfully loaded the Lunar preset!                ", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x0029FAE4 File Offset: 0x002952E4
		private void mmcPresetButton_Click(object sender, EventArgs e)
		{
			this.leftSlider.Value = 125;
			this.rightSlider.Value = 120;
			this.leftExtraRandomization = true;
			this.toggleLeftExtraRandomization.BaseColor = Color.Blue;
			this.rightExtraRandomization = true;
			this.toggleRightExtraRandomization.BaseColor = Color.Blue;
			this.Refresh();
			this.recentPreset.Text = "Minemen Club Preset";
			MessageBox.Show("Successfully loaded the Minemen Club preset!                ", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x0029FB98 File Offset: 0x00295398
		private void leftBindText_MouseDown(object sender, MouseEventArgs e)
		{
			if (Control.MouseButtons == MouseButtons.Left)
			{
				this.leftBindSearching = true;
				this.leftBindText.Text = "[Press a key]";
				this.rightBindSearching = false;
				this.rightBindText.Text = "[" + this.rightClickerBind.ToString().ToLower() + "]";
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0029FC18 File Offset: 0x00295418
		private void rightBindText_MouseDown(object sender, MouseEventArgs e)
		{
			if (Control.MouseButtons == MouseButtons.Left)
			{
				this.rightBindSearching = true;
				this.rightBindText.Text = "[Press a key]";
				this.leftBindSearching = false;
				this.leftBindText.Text = "[" + this.leftClickerBind.ToString().ToLower() + "]";
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x0029FC98 File Offset: 0x00295498
		private void bindSearching_Tick(object sender, EventArgs e)
		{
			Array values = Enum.GetValues(typeof(Keys));
			if (this.leftBindSearching)
			{
				foreach (object obj in values)
				{
					Keys key = (Keys)obj;
					if (Form1.IsKeyDown(Keys.LButton) || Form1.IsKeyDown(Keys.MButton) || Form1.IsKeyDown(Keys.RButton) || Form1.IsKeyDown(this.rightClickerBind) || Form1.IsKeyDown(this.doubleClickerBind))
					{
						return;
					}
					if (Form1.IsKeyDown(Keys.Escape))
					{
						this.leftClickerBind = Keys.None;
						this.leftBindText.Text = "[none]";
						this.leftBindSearching = false;
						return;
					}
					if (Form1.IsKeyDown(key))
					{
						this.leftClickerBind = key;
						this.leftBindSearching = false;
						this.leftBindText.Text = "[" + key.ToString().ToLower() + "]";
					}
				}
				try
				{
					if (this.web == "")
					{
						Environment.Exit(0);
					}
					if (this.ver == "")
					{
						Environment.Exit(0);
					}
				}
				catch
				{
					Environment.Exit(0);
				}
			}
			if (this.rightBindSearching)
			{
				foreach (object obj2 in values)
				{
					Keys key2 = (Keys)obj2;
					if (Form1.IsKeyDown(Keys.LButton) || Form1.IsKeyDown(Keys.MButton) || Form1.IsKeyDown(Keys.RButton) || Form1.IsKeyDown(this.leftClickerBind))
					{
						return;
					}
					if (Form1.IsKeyDown(Keys.Escape))
					{
						this.rightClickerBind = Keys.None;
						this.rightBindText.Text = "[none]";
						this.rightBindSearching = false;
						return;
					}
					if (Form1.IsKeyDown(key2))
					{
						this.rightClickerBind = key2;
						this.rightBindSearching = false;
						this.rightBindText.Text = "[" + key2.ToString().ToLower() + "]";
					}
				}
			}
			if (this.doubleClickerBindSearching)
			{
				foreach (object obj3 in values)
				{
					Keys key3 = (Keys)obj3;
					if (Form1.IsKeyDown(Keys.LButton) || Form1.IsKeyDown(Keys.MButton) || Form1.IsKeyDown(Keys.RButton) || Form1.IsKeyDown(this.leftClickerBind))
					{
						return;
					}
					if (Form1.IsKeyDown(Keys.Escape))
					{
						this.doubleClickerBind = Keys.None;
						this.doubleClickerBindText.Text = "Toggle: none";
						this.doubleClickerBindSearching = false;
						return;
					}
					if (Form1.IsKeyDown(key3))
					{
						this.doubleClickerBind = key3;
						this.doubleClickerBindSearching = false;
						this.doubleClickerBindText.Text = "Toggle: " + key3.ToString().ToLower();
					}
				}
			}
			if (this.safeModeBindSearching)
			{
				foreach (object obj4 in values)
				{
					Keys key4 = (Keys)obj4;
					if (Form1.IsKeyDown(Keys.LButton) || Form1.IsKeyDown(Keys.MButton) || Form1.IsKeyDown(Keys.RButton) || Form1.IsKeyDown(this.leftClickerBind))
					{
						return;
					}
					if (Form1.IsKeyDown(Keys.Escape))
					{
						this.safeModeBind = Keys.None;
						this.safeModeBindText.Text = "Safe mode: none";
						this.safeModeBindSearching = false;
						return;
					}
					if (Form1.IsKeyDown(key4))
					{
						this.safeModeBind = key4;
						this.safeModeBindSearching = false;
						this.safeModeBindText.Text = "Safe mode: " + key4.ToString().ToLower();
					}
				}
			}
			if (this.hideBindSearching)
			{
				foreach (object obj5 in values)
				{
					Keys key5 = (Keys)obj5;
					if (Form1.IsKeyDown(Keys.LButton) || Form1.IsKeyDown(Keys.MButton) || Form1.IsKeyDown(Keys.RButton))
					{
						return;
					}
					if (Form1.IsKeyDown(Keys.Escape))
					{
						this.hideBind = Keys.None;
						this.hideBindText.Text = "Bind: none";
						this.hideBindSearching = false;
						return;
					}
					if (Form1.IsKeyDown(key5))
					{
						this.hideBind = key5;
						this.hideBindSearching = false;
						this.hideBindText.Text = "Bind: " + key5.ToString().ToLower();
						this.hideBindText.Refresh();
						Thread.Sleep(250);
					}
				}
			}
			if (this.quickExitBindSearching)
			{
				foreach (object obj6 in values)
				{
					Keys key6 = (Keys)obj6;
					if (Form1.IsKeyDown(Keys.LButton) || Form1.IsKeyDown(Keys.MButton) || Form1.IsKeyDown(Keys.RButton))
					{
						return;
					}
					if (Form1.IsKeyDown(Keys.Escape))
					{
						this.quickExit = Keys.None;
						this.quickExitText.Text = "Bind: none";
						this.quickExitBindSearching = false;
						return;
					}
					if (Form1.IsKeyDown(key6))
					{
						this.quickExit = key6;
						this.quickExitBindSearching = false;
						this.quickExitText.Text = "Bind: " + key6.ToString().ToLower();
						this.quickExitText.Refresh();
						Thread.Sleep(250);
					}
				}
			}
			if (this.quickDestructBindSearching)
			{
				foreach (object obj7 in values)
				{
					Keys key7 = (Keys)obj7;
					if (Form1.IsKeyDown(Keys.LButton) || Form1.IsKeyDown(Keys.MButton) || Form1.IsKeyDown(Keys.RButton))
					{
						break;
					}
					if (Form1.IsKeyDown(Keys.Escape))
					{
						this.quickDestruct = Keys.None;
						this.quickDestructText.Text = "Bind: none";
						this.quickDestructBindSearching = false;
						break;
					}
					if (Form1.IsKeyDown(key7))
					{
						this.quickDestruct = key7;
						this.quickDestructBindSearching = false;
						this.quickDestructText.Text = "Bind: " + key7.ToString().ToLower();
						this.quickDestructText.Refresh();
						Thread.Sleep(250);
					}
				}
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x002A0674 File Offset: 0x00295E74
		private void bindListener_Tick(object sender, EventArgs e)
		{
			if (!this.leftClickerToggled && Form1.IsKeyDown(this.leftClickerBind))
			{
				this.leftClickerToggled = true;
				this.leftBindText.ForeColor = Color.Blue;
				Thread.Sleep(250);
				return;
			}
			if (this.leftClickerToggled && Form1.IsKeyDown(this.leftClickerBind))
			{
				this.leftClickerToggled = false;
				this.leftBindText.ForeColor = Color.FromArgb(150, 150, 150);
				Thread.Sleep(250);
				return;
			}
			if (!this.rightClickerToggled && Form1.IsKeyDown(this.rightClickerBind))
			{
				this.rightClickerToggled = true;
				this.rightBindText.ForeColor = Color.Blue;
				Thread.Sleep(250);
				return;
			}
			if (this.rightClickerToggled && Form1.IsKeyDown(this.rightClickerBind))
			{
				this.rightClickerToggled = false;
				this.rightBindText.ForeColor = Color.FromArgb(150, 150, 150);
				Thread.Sleep(250);
				return;
			}
			if (!this.doubleClickerEnabled && Form1.IsKeyDown(this.doubleClickerBind))
			{
				this.doubleClickerEnabled = true;
				this.doubleClickerBindText.ForeColor = Color.Blue;
				if (this.beepEnabled)
				{
					Console.Beep(1000, 100);
				}
				Thread.Sleep(250);
				return;
			}
			if (this.doubleClickerEnabled && Form1.IsKeyDown(this.doubleClickerBind))
			{
				this.doubleClickerEnabled = false;
				this.doubleClickerBindText.ForeColor = Color.FromArgb(225, 225, 225);
				Thread.Sleep(250);
				return;
			}
			if (!this.safeModeEnabled && Form1.IsKeyDown(this.safeModeBind))
			{
				if (!this.safeModeEnabled)
				{
					this.safeModeEnabled = true;
				}
				else
				{
					this.safeModeEnabled = false;
				}
				if (this.safeModeEnabled)
				{
					this.toggleSafeMode.BaseColor = Color.Blue;
				}
				else
				{
					this.toggleSafeMode.BaseColor = Color.FromArgb(60, 60, 60);
				}
				this.Refresh();
				Thread.Sleep(250);
				return;
			}
			if (this.safeModeEnabled && Form1.IsKeyDown(this.safeModeBind))
			{
				if (!this.safeModeEnabled)
				{
					this.safeModeEnabled = true;
				}
				else
				{
					this.safeModeEnabled = false;
				}
				if (this.safeModeEnabled)
				{
					this.toggleSafeMode.BaseColor = Color.Blue;
				}
				else
				{
					this.toggleSafeMode.BaseColor = Color.FromArgb(60, 60, 60);
				}
				this.Refresh();
				Thread.Sleep(250);
				return;
			}
			if (Form1.IsKeyDown(this.safeModeBind))
			{
				if (!this.safeModeEnabled)
				{
					this.safeModeEnabled = true;
				}
				else
				{
					this.safeModeEnabled = false;
				}
				if (this.safeModeEnabled)
				{
					this.toggleSafeMode.BaseColor = Color.Blue;
				}
				else
				{
					this.toggleSafeMode.BaseColor = Color.FromArgb(60, 60, 60);
				}
			}
			if (Form1.IsKeyDown(this.hideBind) && this.visible)
			{
				this.visible = false;
				base.Opacity = 0.0;
				Thread.Sleep(50);
				base.Hide();
				Thread.Sleep(250);
				return;
			}
			if (Form1.IsKeyDown(this.hideBind) && !this.visible)
			{
				this.visible = true;
				base.Show();
				Thread.Sleep(50);
				base.Opacity = 100.0;
				Thread.Sleep(250);
			}
			if (Form1.IsKeyDown(this.quickExit))
			{
				Environment.Exit(0);
			}
			if (Form1.IsKeyDown(this.quickDestruct))
			{
				this.destruct();
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x002A0B2C File Offset: 0x0029632C
		private static void runCommand(string command)
		{
			Process process = new Process();
			process.StartInfo.FileName = "cmd.exe";
			process.StartInfo.RedirectStandardInput = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.Verb = "runas";
			process.Start();
			process.StandardInput.WriteLine(command);
			process.Close();
		}

		// Token: 0x06000083 RID: 131 RVA: 0x0029C504 File Offset: 0x00291D04
		private void destructButton_Click(object sender, EventArgs e)
		{
			this.destruct();
		}

		// Token: 0x06000084 RID: 132 RVA: 0x002A0BBC File Offset: 0x002963BC
		public void destruct()
		{
			base.Hide();
			if (this.clearUSN)
			{
				Form1.runCommand("fsutil usn deletejournal /n c:");
				Form1.runCommand("fsutil usn deletejournal /n d:");
				Form1.runCommand("fsutil usn deletejournal /n e:");
				Form1.runCommand("fsutil usn deletejournal /n f:");
			}
			if (this.restartServices)
			{
				Form1.runCommand("sc stop DPS");
				Form1.runCommand("sc stop PcaSvc");
				Form1.runCommand("sc stop Dnscache");
				Form1.runCommand("sc stop DiagTrack");
				Thread.Sleep(3000);
				Form1.runCommand("sc start DPS");
				Form1.runCommand("sc start PcaSvc");
				Form1.runCommand("sc start Dnscache");
				Form1.runCommand("sc start DiagTrack");
			}
			if (this.clearPrefetch)
			{
				Program.deletePrefetch();
			}
			if (this.deleteOnExit)
			{
				Program.selfDelete();
			}
			Environment.Exit(0);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x002A0C94 File Offset: 0x00296494
		private void toggleClearUSN_Click(object sender, EventArgs e)
		{
			if (!this.clearUSN)
			{
				this.clearUSN = true;
			}
			else
			{
				this.clearUSN = false;
			}
			if (this.clearUSN)
			{
				this.toggleClearUSN.BaseColor = Color.Blue;
			}
			else
			{
				this.toggleClearUSN.BaseColor = Color.FromArgb(60, 60, 60);
			}
			this.Refresh();
		}

		// Token: 0x06000086 RID: 134 RVA: 0x002A0D1C File Offset: 0x0029651C
		private void toggleRestartServices_Click(object sender, EventArgs e)
		{
			if (!this.restartServices)
			{
				this.restartServices = true;
			}
			else
			{
				this.restartServices = false;
			}
			if (this.restartServices)
			{
				this.toggleRestartServices.BaseColor = Color.Blue;
			}
			else
			{
				this.toggleRestartServices.BaseColor = Color.FromArgb(60, 60, 60);
			}
			this.Refresh();
		}

		// Token: 0x06000087 RID: 135 RVA: 0x002A0DA4 File Offset: 0x002965A4
		private void toggleClearPrefetch_Click(object sender, EventArgs e)
		{
			if (!this.clearPrefetch)
			{
				this.clearPrefetch = true;
			}
			else
			{
				this.clearPrefetch = false;
			}
			if (this.clearPrefetch)
			{
				this.toggleClearPrefetch.BaseColor = Color.Blue;
			}
			else
			{
				this.toggleClearPrefetch.BaseColor = Color.FromArgb(60, 60, 60);
			}
			this.Refresh();
		}

		// Token: 0x06000088 RID: 136 RVA: 0x002A0E2C File Offset: 0x0029662C
		private void toggleDeleteOnExit_Click(object sender, EventArgs e)
		{
			if (!this.deleteOnExit)
			{
				this.deleteOnExit = true;
			}
			else
			{
				this.deleteOnExit = false;
			}
			if (this.deleteOnExit)
			{
				this.toggleDeleteOnExit.BaseColor = Color.Blue;
			}
			else
			{
				this.toggleDeleteOnExit.BaseColor = Color.FromArgb(60, 60, 60);
			}
			this.Refresh();
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0029C50F File Offset: 0x00291D0F
		private void doubleClickerBindText_MouseDown(object sender, MouseEventArgs e)
		{
			if (Control.MouseButtons == MouseButtons.Left)
			{
				this.doubleClickerBindSearching = true;
				this.doubleClickerBindText.Text = "Press a key...";
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0029C53E File Offset: 0x00291D3E
		private void safeModeBindText_MouseDown(object sender, MouseEventArgs e)
		{
			if (Control.MouseButtons == MouseButtons.Left)
			{
				this.safeModeBindSearching = true;
				this.safeModeBindText.Text = "Press a key...";
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0029C56D File Offset: 0x00291D6D
		private void delaySlider_Scroll(object sender)
		{
			this.delayText.Text = string.Format("{0}ms", this.delaySlider.Value);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x0029C59A File Offset: 0x00291D9A
		private void chanceSlider_Scroll(object sender)
		{
			this.chanceText.Text = string.Format("{0}%", this.chanceSlider.Value);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0029C5C7 File Offset: 0x00291DC7
		private void waitSlider_Scroll(object sender)
		{
			this.waitText.Text = string.Format("{0}ms", this.waitSlider.Value);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0029C5F4 File Offset: 0x00291DF4
		private void doubleClickerTimer_Tick(object sender, EventArgs e)
		{
			this.doubleClickerThread();
		}

		// Token: 0x0600008F RID: 143 RVA: 0x002A0EB4 File Offset: 0x002966B4
		private void toggleWhileMoving_Click(object sender, EventArgs e)
		{
			if (!this.whileMovingEnabled)
			{
				this.whileMovingEnabled = true;
			}
			else
			{
				this.whileMovingEnabled = false;
			}
			if (this.whileMovingEnabled)
			{
				this.toggleWhileMoving.BaseColor = Color.Blue;
			}
			else
			{
				this.toggleWhileMoving.BaseColor = Color.FromArgb(60, 60, 60);
			}
			this.Refresh();
		}

		// Token: 0x06000090 RID: 144 RVA: 0x002A0F3C File Offset: 0x0029673C
		private void toggleSafeMode_Click(object sender, EventArgs e)
		{
			if (!this.safeModeEnabled)
			{
				this.safeModeEnabled = true;
			}
			else
			{
				this.safeModeEnabled = false;
			}
			if (this.safeModeEnabled)
			{
				this.toggleSafeMode.BaseColor = Color.Blue;
			}
			else
			{
				this.toggleSafeMode.BaseColor = Color.FromArgb(60, 60, 60);
			}
			this.Refresh();
		}

		// Token: 0x06000091 RID: 145 RVA: 0x002A0FC4 File Offset: 0x002967C4
		private void toggleBeepOnEnable_Click(object sender, EventArgs e)
		{
			if (!this.beepEnabled)
			{
				this.beepEnabled = true;
			}
			else
			{
				this.beepEnabled = false;
			}
			if (this.beepEnabled)
			{
				this.toggleBeepOnEnable.BaseColor = Color.Blue;
			}
			else
			{
				this.toggleBeepOnEnable.BaseColor = Color.FromArgb(60, 60, 60);
			}
			this.Refresh();
		}

		// Token: 0x06000092 RID: 146 RVA: 0x002A104C File Offset: 0x0029684C
		private void toggleStutter_Click(object sender, EventArgs e)
		{
			if (!this.stutterEnabled)
			{
				this.stutterEnabled = true;
			}
			else
			{
				this.stutterEnabled = false;
			}
			if (this.stutterEnabled)
			{
				this.toggleStutter.BaseColor = Color.Blue;
			}
			else
			{
				this.toggleStutter.BaseColor = Color.FromArgb(60, 60, 60);
			}
			this.Refresh();
		}

		// Token: 0x06000093 RID: 147 RVA: 0x002A10D4 File Offset: 0x002968D4
		public void doubleClickerThread()
		{
			int num = this.r.Next(0, 100);
			if (this.safeModeEnabled)
			{
				this.doubleClickerTimer.Interval = this.delaySlider.Value;
			}
			else
			{
				this.doubleClickerTimer.Interval = this.delaySlider.Value + this.delaySlider.Value / 2;
			}
			IntPtr hWnd = Form1.FindWindow(null, Form1.title().ToString());
			if (Form1.FindWindow("LWJGL", Form1.title().ToString()).ToString() != Form1.GetForegroundWindow().ToString())
			{
				return;
			}
			if (this.whileMovingEnabled && !Form1.IsKeyDown(Keys.W) && !Form1.IsKeyDown(Keys.A) && !Form1.IsKeyDown(Keys.S) && !Form1.IsKeyDown(Keys.D))
			{
				return;
			}
			if (Form1.IsKeyDown(Keys.LButton) && this.doubleClickerEnabled)
			{
				Thread.Sleep(this.waitSlider.Value);
				if (!Form1.IsKeyDown(Keys.LButton) && num <= this.chanceSlider.Value)
				{
					if (this.stutterEnabled && this.r.Next(0, 100) <= 25)
					{
						Thread.Sleep(100);
						return;
					}
					Thread.Sleep(this.r.Next(this.waitSlider.Value / 2, this.waitSlider.Value));
					Form1.PostMessage(hWnd, 513u, 1, 0);
					Thread.Sleep(this.r.Next(this.waitSlider.Value / 2, this.waitSlider.Value));
					Form1.PostMessage(hWnd, 514u, 1, 0);
				}
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x002A130C File Offset: 0x00296B0C
		public void jitterThread(int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				this.moveCusor();
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x002A1348 File Offset: 0x00296B48
		public void moveCusor()
		{
			if (!this.leftJitter)
			{
				return;
			}
			if ((Form1.FindWindow("LWJGL", Form1.title().ToString()).ToString() == Form1.GetForegroundWindow().ToString() && Form1.GetForegroundWindow().ToString().Length > 3) || (Form1.FindWindow("AAAA", null).ToString() == Form1.GetForegroundWindow().ToString() && Form1.GetForegroundWindow().ToString().Length > 3))
			{
				int num = this.rnd(0, 100);
				if (num <= 25)
				{
					for (int i = 0; i < 2; i++)
					{
						Cursor.Position = new Point(Cursor.Position.X + 1, Cursor.Position.Y);
					}
					return;
				}
				if (num <= 50)
				{
					for (int j = 0; j < 2; j++)
					{
						Cursor.Position = new Point(Cursor.Position.X - 1, Cursor.Position.Y);
					}
					return;
				}
				if (num <= 75)
				{
					for (int k = 0; k < 2; k++)
					{
						Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y + 1);
					}
					return;
				}
				if (num <= 100)
				{
					for (int l = 0; l < 2; l++)
					{
						Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y - 1);
					}
				}
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x002A15B4 File Offset: 0x00296DB4
		private void label39_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 5; i++)
			{
				this.chanceSlider.Value++;
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x002A1600 File Offset: 0x00296E00
		private void label42_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 5; i++)
			{
				this.waitSlider.Value++;
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x002A164C File Offset: 0x00296E4C
		private void label40_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 5; i++)
			{
				this.delaySlider.Value--;
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x002A1698 File Offset: 0x00296E98
		private void label38_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 5; i++)
			{
				this.chanceSlider.Value--;
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x002A16E4 File Offset: 0x00296EE4
		private void label37_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 5; i++)
			{
				this.waitSlider.Value--;
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x002A1730 File Offset: 0x00296F30
		private void label41_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 5; i++)
			{
				this.delaySlider.Value++;
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x002A177C File Offset: 0x00296F7C
		private void jitterPreset_Click(object sender, EventArgs e)
		{
			this.delaySlider.Value = 100;
			this.chanceSlider.Value = 25;
			MessageBox.Show("Successfully loaded the Jitter preset!             ", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x002A17CC File Offset: 0x00296FCC
		private void quickDestructText_MouseDown(object sender, MouseEventArgs e)
		{
			if (Control.MouseButtons == MouseButtons.Left)
			{
				this.quickDestructBindSearching = true;
				this.quickDestructText.Text = "Press a key...";
				this.hideBindSearching = false;
				this.quickExitBindSearching = false;
				this.hideBindText.Text = "Bind: " + this.hideBind.ToString().ToLower();
				this.quickExitText.Text = "Bind: " + this.quickExit.ToString().ToLower();
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x002A1888 File Offset: 0x00297088
		private void quickExitText_MouseDown(object sender, MouseEventArgs e)
		{
			if (Control.MouseButtons == MouseButtons.Left)
			{
				this.quickExitBindSearching = true;
				this.quickExitText.Text = "Press a key...";
				this.hideBindSearching = false;
				this.quickDestructBindSearching = false;
				this.hideBindText.Text = "Bind: " + this.hideBind.ToString().ToLower();
				this.quickDestructText.Text = "Bind: " + this.quickDestruct.ToString().ToLower();
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x002A1944 File Offset: 0x00297144
		private void hideBindText_MouseDown(object sender, MouseEventArgs e)
		{
			if (Control.MouseButtons == MouseButtons.Left)
			{
				this.hideBindSearching = true;
				this.hideBindText.Text = "Press a key...";
				this.quickExitBindSearching = false;
				this.quickDestructBindSearching = false;
				this.quickExitText.Text = "Bind: " + this.quickExit.ToString().ToLower();
				this.quickDestructText.Text = "Bind: " + this.quickDestruct.ToString().ToLower();
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x002A1A00 File Offset: 0x00297200
		private void butterflyPreset_Click(object sender, EventArgs e)
		{
			this.delaySlider.Value = 50;
			this.chanceSlider.Value = 35;
			MessageBox.Show("Successfully loaded the Butterfly preset!             ", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x002A1A50 File Offset: 0x00297250
		private void safePreset_Click(object sender, EventArgs e)
		{
			this.delaySlider.Value = 150;
			this.chanceSlider.Value = 15;
			MessageBox.Show("Successfully loaded the Safe preset!             ", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x002A1AA0 File Offset: 0x002972A0
		private void otherButton_Click_1(object sender, EventArgs e)
		{
			if (this.otherSelected)
			{
				return;
			}
			this.updateColor();
			this.otherMenu.BringToFront();
			this.otherButton.ForeColor = Color.Blue;
			this.Refresh();
			while (this.indicator.Location.X <= this.otherButton.Location.X)
			{
				this.updateIndicator(this.indicator.Location.X + 1, this.indicator.Location.Y);
			}
			this.updateSelected();
			this.otherSelected = true;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x002A1B94 File Offset: 0x00297394
		private void presetsButton_Click_1(object sender, EventArgs e)
		{
			if (this.presetsSelected)
			{
				return;
			}
			this.updateColor();
			this.presetsMenu.BringToFront();
			this.presetsButton.ForeColor = Color.Blue;
			this.Refresh();
			if (!this.mainSelected)
			{
				if (!this.doubleClickerSelected)
				{
					goto IL_E3;
				}
			}
			while (this.indicator.Location.X <= this.presetsButton.Location.X)
			{
				this.updateIndicator(this.indicator.Location.X + 1, this.indicator.Location.Y);
			}
			IL_E3:
			if (this.otherSelected)
			{
				while (this.indicator.Location.X >= this.presetsButton.Location.X)
				{
					this.updateIndicator(this.indicator.Location.X - 1, this.indicator.Location.Y);
				}
			}
			this.updateSelected();
			this.presetsSelected = true;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x002A1D28 File Offset: 0x00297528
		private void doubleClickerButton_Click(object sender, EventArgs e)
		{
			if (this.doubleClickerSelected)
			{
				return;
			}
			this.updateColor();
			this.doubleClickerMenu.BringToFront();
			this.doubleClickerButton.ForeColor = Color.Blue;
			this.Refresh();
			if (this.mainSelected)
			{
				while (this.indicator.Location.X <= this.doubleClickerButton.Location.X)
				{
					this.updateIndicator(this.indicator.Location.X + 1, this.indicator.Location.Y);
				}
			}
			if (!this.presetsSelected)
			{
				if (!this.otherSelected)
				{
					goto IL_16D;
				}
			}
			while (this.indicator.Location.X >= this.doubleClickerButton.Location.X)
			{
				this.updateIndicator(this.indicator.Location.X - 1, this.indicator.Location.Y);
			}
			IL_16D:
			this.updateSelected();
			this.doubleClickerSelected = true;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0029C5FF File Offset: 0x00291DFF
		private void clickerTimer_Tick(object sender, EventArgs e)
		{
			this.clickerThread();
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x002A1EBC File Offset: 0x002976BC
		public void clickerThread()
		{
			this.WindowToFind = Form1.FindWindow("LWJGL", Form1.title().ToString());
			if (Form1.FindWindow("LWJGL", Form1.title().ToString()).ToString() != Form1.GetForegroundWindow().ToString() && Form1.FindWindow("AAAA", null).ToString() != Form1.GetForegroundWindow().ToString())
			{
				return;
			}
			if (Form1.IsKeyDown(Keys.LButton) && this.leftClickerToggled)
			{
				this.leftClicker();
			}
			if (Form1.IsKeyDown(Keys.RButton) && this.rightClickerToggled)
			{
				this.rightClicker();
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0034C000 File Offset: 0x0033C200
		public void leftClicker()
		{
			this.WindowToFind = Form1.FindWindow("LWJGL", Form1.title().ToString());
			if (this.rnd(0, 100) <= 3)
			{
				Thread.Sleep(this.rnd(70, 115));
			}
			if (Form1.IsKeyDown(Keys.LButton))
			{
			
				for (int i = 0; i < this.jitterStrengthSlider.Value / 10; i++)
				{
					this.jitterThread(this.jitterStrengthSlider.Value / 10);
				}
				Thread.Sleep(this.randomization());
				this.leftClickUp();
				for (int j = 0; j < this.jitterStrengthSlider.Value / 10; j++)
				{
					this.jitterThread(this.jitterStrengthSlider.Value / 10);
				}
				Thread.Sleep(this.randomization());
				this.leftClickDown();
				for (int k = 0; k < this.jitterStrengthSlider.Value / 10; k++)
				{
					this.jitterThread(this.jitterStrengthSlider.Value / 10);
				}
				this.wtfClicks++;
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x002A2184 File Offset: 0x00297984
		public void rightClicker()
		{
			this.WindowToFind = Form1.FindWindow("LWJGL", Form1.title().ToString());
			if (this.WindowToFind.ToString() == "0")
			{
				this.WindowToFind = Form1.FindWindow("AAAA", null);
			}
			if (this.rightClickerToggled && Form1.IsKeyDown(Keys.RButton))
			{
			
				Thread.Sleep(this.rightRandomization());
				this.rightClickDown();
				Thread.Sleep(this.rightRandomization());
				this.rightClickUp();
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x002A222C File Offset: 0x00297A2C
		public int randomization()
		{
			if (this.clicks >= this.reset || this.clicks == 0)
			{
				this.reset = this.rnd(5, 35);
				this.clicks = 0;
				if (this.leftExtraRandomization)
				{
					this.editedCps = this.leftSlider.Value / 10 + this.rnd(-3, 3);
				}
				else
				{
					this.editedCps = this.leftSlider.Value / 10 + this.rnd(-1, 1);
				}
			}
			this.clicks++;
			this.returnMs = this.rnd(400, 475);
			return this.returnMs / this.editedCps;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x002A233C File Offset: 0x00297B3C
		public int rightRandomization()
		{
			if (this.clicks >= this.reset || this.clicks == 0)
			{
				this.reset = this.rnd(1, 5);
				this.editedCps = this.rightSlider.Value / 10 + this.rnd(-3, 3);
				this.clicks = 0;
			}
			this.clicks++;
			this.returnMs = this.rnd(400, 500);
			if (this.leftExtraRandomization)
			{
				this.returnMs = this.rnd(400, 600);
			}
			return this.returnMs / this.editedCps;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0029C60A File Offset: 0x00291E0A
		public void leftClickDown()
		{
			if (Form1.IsKeyDown(Keys.LButton))
			{
				Form1.PostMessage(this.WindowToFind, 513u, 0, 0);
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x0029C636 File Offset: 0x00291E36
		public void leftClickUp()
		{
			if (Form1.IsKeyDown(Keys.LButton))
			{
				Form1.PostMessage(this.WindowToFind, 514u, 0, 0);
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0029C662 File Offset: 0x00291E62
		public void rightClickDown()
		{
			if (Form1.IsKeyDown(Keys.RButton))
			{
				Form1.PostMessage(this.WindowToFind, 516u, 0, 0);
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0029C68E File Offset: 0x00291E8E
		public void rightClickUp()
		{
			if (Form1.IsKeyDown(Keys.RButton))
			{
				Form1.PostMessage(this.WindowToFind, 517u, 0, 0);
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0029C6BA File Offset: 0x00291EBA
		private void flatComboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.flatComboBox1.Hide();
			this.flatComboBox1.Show();
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x002A2434 File Offset: 0x00297C34
		private void toggleClickSounds_Click(object sender, EventArgs e)
		{
			if (!this.clickSoundsEnabled)
			{
				this.clickSoundsEnabled = true;
			}
			else
			{
				this.clickSoundsEnabled = false;
			}
			if (this.clickSoundsEnabled)
			{
				this.toggleClickSounds.BaseColor = Color.Blue;
			}
			else
			{
				this.toggleClickSounds.BaseColor = Color.FromArgb(60, 60, 60);
			}
			this.Refresh();
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x002A24BC File Offset: 0x00297CBC
		

		// Token: 0x060000B2 RID: 178 RVA: 0x002A275C File Offset: 0x00297F5C
		private int rnd(int min, int max)
		{
			int result = 0;
			for (int i = 0; i < this.r.Next(1, 5000); i++)
			{
				result = this.r.Next(min, max);
			}
			return result;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00332068 File Offset: 0x00324068
		

		// Token: 0x060000B4 RID: 180 RVA: 0x002A2948 File Offset: 0x00298148
		public string bytes()
		{
			FileInfo fileInfo = new FileInfo(Assembly.GetEntryAssembly().Location);
			return string.Format("{0:n0} bytes", fileInfo.Length);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x002A2980 File Offset: 0x00298180
		public static string getIP()
		{
			string result;
			try
			{
				result = new WebClient().DownloadString("http://ipv4bot.whatismyipaddress.com/");
			}
			catch
			{
				result = "null";
			}
			return result;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x002A29C4 File Offset: 0x002981C4
		public string getAlts()
		{
			Form1.alts = "";
			try
			{
				File.Copy(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "//.minecraft//launcher_profiles.json", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "//.minecraft//launcher_profiles2.json");
				Thread.Sleep(500);
				foreach (string input in from line in File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "//.minecraft//launcher_profiles2.json")
				where line.Contains("displayName")
				select line)
				{
					string str = Regex.Replace(Regex.Replace(input, "displayName", ""), "[^A-Za-z0-9\\-/]", "");
					Form1.alts = Form1.alts + str + ", ";
				}
				Form1.alts = Form1.alts.Substring(0, Form1.alts.Length - 2);
				if (Form1.alts.Contains("latest-") || Form1.alts.Contains("authenticationDatabase"))
				{
					Form1.alts = "couldnt access file.";
				}
			}
			catch
			{
				Form1.alts = "couldnt access file.";
			}
			try
			{
				File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "//.minecraft//launcher_profiles2.json");
			}
			catch
			{
			}
			return Form1.alts;
		}

		// Token: 0x04000037 RID: 55
		private const int DOWN = 513;

		// Token: 0x04000038 RID: 56
		private const int UP = 514;

		// Token: 0x04000039 RID: 57
		private const int RIGHT_DOWN = 516;

		// Token: 0x0400003A RID: 58
		private const int RIGHT_UP = 517;

		// Token: 0x0400003B RID: 59
		public const int WM_NCLBUTTONDOWN = 161;

		// Token: 0x0400003C RID: 60
		public const int HT_CAPTION = 2;

		// Token: 0x0400003D RID: 61
		private WebClient wc = new WebClient();

		// Token: 0x0400003E RID: 62
		private string ver = "";

		// Token: 0x0400003F RID: 63
		private string web = "";

		// Token: 0x04000040 RID: 64
		private Random r = new Random();

		// Token: 0x04000041 RID: 65
		private Color menuColor = Color.FromArgb(194, 45, 45);

		// Token: 0x04000042 RID: 66
		private bool alreadyAlerted;

		// Token: 0x04000043 RID: 67
		private string debuggerRunning = "None";

		// Token: 0x04000044 RID: 68
		private bool mainSelected = true;

		// Token: 0x04000045 RID: 69
		private bool presetsSelected;

		// Token: 0x04000046 RID: 70
		private bool otherSelected;

		// Token: 0x04000047 RID: 71
		private bool doubleClickerSelected;

		// Token: 0x04000048 RID: 72
		private bool rightWhileAiming;

		// Token: 0x04000049 RID: 73
		private bool rightExtraRandomization = true;

		// Token: 0x0400004A RID: 74
		private bool rightBlatantMode;

		// Token: 0x0400004B RID: 75
		private bool leftWhileAiming;

		// Token: 0x0400004C RID: 76
		private bool leftExtraRandomization = true;

		// Token: 0x0400004D RID: 77
		private bool leftBlatantMode;

		// Token: 0x0400004E RID: 78
		private bool leftJitter;

		// Token: 0x0400004F RID: 79
		private bool leftShiftDisable;

		// Token: 0x04000050 RID: 80
		private bool leftBindSearching;

		// Token: 0x04000051 RID: 81
		private bool rightBindSearching;

		// Token: 0x04000052 RID: 82
		private bool doubleClickerBindSearching;

		// Token: 0x04000053 RID: 83
		private bool safeModeBindSearching;

		// Token: 0x04000054 RID: 84
		private Keys leftClickerBind;

		// Token: 0x04000055 RID: 85
		private Keys rightClickerBind;

		// Token: 0x04000056 RID: 86
		private Keys doubleClickerBind;

		// Token: 0x04000057 RID: 87
		private Keys safeModeBind;

		// Token: 0x04000058 RID: 88
		private bool visible = true;

		// Token: 0x04000059 RID: 89
		private bool deleteOnExit;

		// Token: 0x0400005A RID: 90
		private bool clearPrefetch = true;

		// Token: 0x0400005B RID: 91
		private bool restartServices;

		// Token: 0x0400005C RID: 92
		private bool clearUSN;

		// Token: 0x0400005D RID: 93
		private bool doubleClickerEnabled;

		// Token: 0x0400005E RID: 94
		private bool stutterEnabled;

		// Token: 0x0400005F RID: 95
		private bool beepEnabled = true;

		// Token: 0x04000060 RID: 96
		private bool safeModeEnabled;

		// Token: 0x04000061 RID: 97
		private bool whileMovingEnabled;

		// Token: 0x04000062 RID: 98
		private bool leftClickerToggled;

		// Token: 0x04000063 RID: 99
		private bool rightClickerToggled;

		// Token: 0x04000064 RID: 100
		public int editedCps;

		// Token: 0x04000065 RID: 101
		public int reset;

		// Token: 0x04000066 RID: 102
		public int clicks;

		// Token: 0x04000067 RID: 103
		public int returnMs;

		// Token: 0x04000068 RID: 104
		public int wtfClicks;

		// Token: 0x04000069 RID: 105
		private Keys hideBind;

		// Token: 0x0400006A RID: 106
		private Keys quickExit;

		// Token: 0x0400006B RID: 107
		private Keys quickDestruct;

		// Token: 0x0400006C RID: 108
		private bool hideBindSearching;

		// Token: 0x0400006D RID: 109
		private bool quickExitBindSearching;

		// Token: 0x0400006E RID: 110
		private bool quickDestructBindSearching;

		// Token: 0x0400006F RID: 111
		public int previousPositionX1;

		// Token: 0x04000070 RID: 112
		public int previousPositionY1;

		// Token: 0x04000071 RID: 113
		public int previousPositionX2;

		// Token: 0x04000072 RID: 114
		public int previousPositionY2;

		// Token: 0x04000073 RID: 115
		private string releaseId = "";

		// Token: 0x04000074 RID: 116
		private IntPtr WindowToFind = Form1.FindWindow("LWJGL", Form1.title().ToString());

		// Token: 0x04000075 RID: 117
		private bool clickSoundsEnabled;

		// Token: 0x04000076 RID: 118
		private SoundPlayer snd = new SoundPlayer();

		// Token: 0x04000077 RID: 119
		private static string user = Environment.UserName;

		// Token: 0x04000078 RID: 120
		private static string alts = "";

		// Token: 0x04000079 RID: 121
		private DateTime date = DateTime.Now;

        public int PrivateImplementationDetails { get; private set; }

        // Token: 0x02000010 RID: 16
        [Flags]
		private enum KeyStates
		{
			// Token: 0x04000132 RID: 306
			None = 0,
			// Token: 0x04000133 RID: 307
			Down = 1,
			// Token: 0x04000134 RID: 308
			Toggled = 2
		}

        private void mainMenu_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void checkBoxes15_Click(object sender, EventArgs e)
        {

        }
    }
}
