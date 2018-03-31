using System;
using System.Collections.Generic;
using System.Text;
using TeamSpeak3QueryApi.Net.Specialized.Notifications;
using TS3GameBot.DBStuff;

namespace TS3GameBot.CommandStuff.Commands
{
    class CommandInventory : CommandBase
    {
		//TODO: get item list some other way
		private CommandStore store = new CommandStore("store", "fuckme");

		public CommandInventory(string label, string description) : base(label, description)
		{
			this.Usage = "";
			this.NeedsRegister = true;
		}

		internal override bool Execute(List<string> args, TextMessage message, PersonDb db)
		{
			StringBuilder outMessage = new StringBuilder();
			CasinoPlayer invoker = DbInterface.GetPlayer(message.InvokerUid, db);

			outMessage.Append("\n");
			if (invoker.Inventory != null || invoker.Inventory.Length > 0) //Don't have any items? frick off
			{
				String[] inventory = invoker.Inventory.Split(";");

				foreach (string itemID in inventory)
				{
					Item item = store.GetItemFromID(itemID);
					if (item != null)
					{
						outMessage.Append($"{item.Name} | {item.Description}\n");
					}
				}
			}
			else
			{
				outMessage.Append($"You don't have any Items!\nYou can buy some with {CommandManager.CmdIndicator}{store.Label}");
			}

			CommandManager.AnswerCall(message, outMessage.ToString());

			return false;
		}
	}
}
