using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TS3GameBot.Utils.Settings;

namespace TS3GameBot.Utils
{
    class CredManager
    {


		CredManager()
		{

		}
		

		public static bool CredCheck(String filePath)
		{
			return File.Exists(filePath);
		}

		internal static Creds CreateCreds()
		{
			StringBuilder msg = new StringBuilder();
			Dictionary<String, String> creds = new Dictionary<string, string>();

			msg.Clear().
				Append("\nHello, we will now create your Credentials together!").
				Append("\n\nPlease Enter your Database Name").
				Append("\n> ");

			Console.Write(msg);
			creds["DBName"] = Console.ReadLine();

			msg.Clear().
				Append("\n\nPlease Enter your Database Username").
				Append("\n> ");

			Console.Write(msg);
			creds["DBUser"] = Console.ReadLine();

			msg.Clear().
				Append("\n\nPlease Enter your Database Password").
				Append("\n> ");

			Console.Write(msg);
			creds["DBPass"] = Console.ReadLine();

			foreach (var item in CreateTS3Info())
			{
				creds[item.Key] = item.Value;
			}			

			Creds shit = new Creds(creds);

			return shit;

		}

		internal static Dictionary<String, String> CreateTS3Info()
		{
			StringBuilder msg = new StringBuilder();
			Dictionary<String, String> creds = new Dictionary<string, string>();

			Console.Clear();
			msg.Clear().
				Append("Now we are gonna add a new TS3 Server. Enter the Description of this Server here (Any name you like)").
				Append("\n> ");

			Console.Write(msg);
			creds["TS3CustomName"] = Console.ReadLine();

			msg.Clear().
				Append("\n\nPlease Enter your TS3Server Address").
				Append("\n> ");

			Console.Write(msg);
			creds["TS3Server"] = Console.ReadLine();

			msg.Clear().
				Append("\n\nPlease Enter your VirtualServerID (default = 1)").
				Append("\n> ");

			Console.Write(msg);
			creds["VirtualServerID"] = Console.ReadLine();

			msg.Clear().
				Append("\n\nPlease Enter your TS3Query Username").
				Append("\n> ");

			Console.Write(msg);
			creds["TS3User"] = Console.ReadLine();

			msg.Clear().
				Append("\n\nPlease Enter your TS3Query Password").
				Append("\n> ");

			Console.Write(msg);
			creds["TS3Pass"] = Console.ReadLine();

			return creds;
		}
	}
}
