using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeamSpeak3QueryApi.Net;
using TeamSpeak3QueryApi.Net.Specialized;
using TeamSpeak3QueryApi.Net.Specialized.Notifications;
using TeamSpeak3QueryApi.Net.Specialized.Responses;

namespace TS3GameBot
{
     class GameBot
    {
		public String Name { get; set; } = "GameBot by MrDj";

		public String Host { get; set; } = "127.0.0.1";


		private String LoginName { get; set; } = "serveradmin";
		private String LoginPass { get; set; } = "OY3pSQF4";

		public TeamSpeakClient TSClient { get; private set; }
		public WhoAmI Who { get; private set; }

		public static GameBot Instance { get; } = new GameBot();

		private GameBot()
		{
			
		}

		public async Task Login()
		{
			TSClient = new TeamSpeakClient(this.Host);
			
			await TSClient.Connect();
			
			await TSClient.Login(this.LoginName, this.LoginPass);

			await TSClient.UseServer(1);
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

			TSClient.Subscribe<ClientMoved>(data =>
			{
				StringBuilder msg = new StringBuilder();
				foreach (var shit in data)
				{
					msg.
						Append("\nName: " + shit.InvokerName).
						Append("\nTarget: " + shit.TargetChannel).
						Append("\nReason: " + shit.Reason).
						Append("\nIds:" + shit.ClientIds).
						Append("\n\n\n");

				}
				Console.WriteLine(msg.ToString());
			});
		}

	}
}
