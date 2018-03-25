using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
				Append("\n\nPlease Enter your DataBase Name").
				Append("\n> ");

			Console.Write(msg);
			creds["DBName"] = Console.ReadLine();

			msg.Clear().
				Append("\n\nPlease Enter your DataBase Username").
				Append("\n> ");

			Console.Write(msg);
			creds["DBUser"] = Console.ReadLine();

			msg.Clear().
				Append("\n\nPlease Enter your DataBase Password").
				Append("\n> ");

			Console.Write(msg);
			creds["DBPass"] = Console.ReadLine();

			msg.Clear().
				Append("\n\nPlease Enter your TS3Server IP").
				Append("\n> ");

			Console.Write(msg);
			creds["TS3Server"] = Console.ReadLine();

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

			Creds shit = new Creds(creds);

			return shit;

		}
	}
}
