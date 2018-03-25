using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeamSpeak3QueryApi.Net;
using TeamSpeak3QueryApi.Net.Specialized;
using TeamSpeak3QueryApi.Net.Specialized.Notifications;
using TeamSpeak3QueryApi.Net.Specialized.Responses;
using System.Net.Sockets;
using static TS3GameBot.Program;

namespace TS3GameBot
{
     class GameBot
    {
		public String Name { get; set; } = "GameBot by MrDj";

		public String Server { get; set; } = "127.0.0.1";
		private String LoginName { get; set; } = "serveradmin";
		private String LoginPass { get; set; } = "OY3pSQF4";

		public TeamSpeakClient TSClient { get; private set; }
		public WhoAmI Who { get; private set; }

		public static GameBot Instance { get; } = new GameBot();

		private GameBot()
		{
			
		}

		public async Task Login() => await Login(this.LoginName, this.LoginPass, this.Server);

		public async Task<ConnectionResult> Login(String LoginName, String LoginPass, String Server)
		{
			TSClient = new TeamSpeakClient(Server);
			try
			{		
				await TSClient.Connect();
				
				await TSClient.Login(LoginName, LoginPass);
			}
			catch (SocketException e)
			{
				Console.WriteLine(e.Message);
				return ConnectionResult.SOCKET;
			}
			catch (TeamSpeak3QueryApi.Net.QueryException e)
			{
				Console.WriteLine(e.Error.Message);
				return ConnectionResult.QUERY;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return ConnectionResult.UNKNOWN;
			}
			return ConnectionResult.OK;
		}



		public async Task StartBot(int sid)
		{
			await TSClient.UseServer(sid);
			await TSClient.ChangeNickName(this.Name);
			Who = await TSClient.WhoAmI();

			await TSClient.RegisterServerNotification();
			await TSClient.RegisterChannelNotification(Who.ChannelId);
			await TSClient.RegisterTextChannelNotification();
			await TSClient.RegisterTextPrivateNotification();
			await TSClient.RegisterTextServerNotification();
		}

		public void EventShit()
		{
			TSClient.Subscribe<TextMessage>(data =>
			{
				CommandStuff.CommandManager.ExecuteCommand(data);
			});
		}

		public void GetClientsOnline()
		{
			//var shit = await GameBot.Instance.TSClient.GetClients();
			var shit = Program.CurrentClients;

			if (shit == null)
			{
				return;
			}

			StringBuilder dang = new StringBuilder();

			foreach (var item in shit)
			{
				dang.Append("\n" + item.NickName + ": " + item.Id);
			}
		}

	}
}
