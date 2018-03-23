using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeamSpeak3QueryApi.Net;
using TeamSpeak3QueryApi.Net.Specialized;

namespace TS3GameBot.Utils
{
    public static class TeamSpeakClientExtension
    {

		public static Task SendOfflineMessage(this TeamSpeakClient tsclient , String uid, String message, String subject = "Message from GameBot")
		{ 
			message = message ?? string.Empty;
			return tsclient.Client.
				Send("messageadd",
				new Parameter("cluid", uid),
				new Parameter("subject", subject),
				new Parameter("message", message));
		}

		public static Task Quit(this TeamSpeakClient tsclient)
		{
			return tsclient.Client.Send("quit");
		}
    }
}
