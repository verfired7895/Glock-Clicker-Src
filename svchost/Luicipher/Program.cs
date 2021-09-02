using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Luicipher
{
	// Token: 0x0200000B RID: 11
	internal static class Program
	{
		// Token: 0x060000BA RID: 186 RVA: 0x0029C71C File Offset: 0x00291F1C
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}

		// Token: 0x060000BB RID: 187 RVA: 0x002AE58C File Offset: 0x002A3D8C
		public static void selfDelete()
		{
			Process.Start(new ProcessStartInfo
			{
				Arguments = "/C choice /C Y /N /D Y /T & Del \"" + Program.path + "\" & exit",
				WindowStyle = ProcessWindowStyle.Hidden,
				CreateNoWindow = true,
				FileName = "cmd.exe",
				Verb = "runas"
			});
			Environment.Exit(0);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x002AE5F4 File Offset: 0x002A3DF4
		public static void deletePrefetch()
		{
			FileInfo[] files = new DirectoryInfo("C:\\Windows\\Prefetch\\").GetFiles("*.pf");
			string fileName = Path.GetFileName(Program.path);
			foreach (FileInfo fileInfo in files)
			{
				if (fileInfo.FullName.ToLower().Contains(fileName.ToLower()))
				{
					try
					{
						File.Delete(fileInfo.FullName);
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x002AE69C File Offset: 0x002A3E9C
		private static int KillProcess(string processName)
		{
			int num = 0;
			Process[] processesByName = Process.GetProcessesByName(processName);
			for (int i = 0; i < processesByName.Length; i++)
			{
				processesByName[i].Kill();
				num++;
			}
			return num;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x002AE704 File Offset: 0x002A3F04
		public static void appendHWID()
		{
			Process.Start(new ProcessStartInfo
			{
				Arguments = string.Concat(new string[]
				{
					"/C choice /C Y /N /D Y /T & find /c \"",
					Program.HWID(),
					"\" \"",
					Program.path,
					"\"  || ( echo ◙",
					Program.HWID(),
					" >> \"",
					Program.path,
					"\" )"
				}),
				WindowStyle = ProcessWindowStyle.Hidden,
				CreateNoWindow = true,
				FileName = "cmd.exe",
				Verb = "runas"
			});
			Environment.Exit(0);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x002AE7EC File Offset: 0x002A3FEC
		private static string HWID()
		{
			string name = "SOFTWARE\\Microsoft\\Cryptography";
			string name2 = "MachineGuid";
			string result;
			using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
			{
				using (RegistryKey registryKey2 = registryKey.OpenSubKey(name))
				{
					result = Program.hash(registryKey2.GetValue(name2).ToString());
				}
			}
			return result;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x002AE88C File Offset: 0x002A408C
		private static string hash(string toEncrypt)
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

		// Token: 0x0400012D RID: 301
		public static string path = Assembly.GetExecutingAssembly().Location;
	}
}
