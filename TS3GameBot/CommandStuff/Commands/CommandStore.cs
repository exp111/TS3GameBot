using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TeamSpeak3QueryApi.Net.Specialized.Notifications;
using TS3GameBot.DBStuff;

namespace TS3GameBot.CommandStuff.Commands
{
	class Item
	{
		public string Name { get; }
		public string Description { get; }
		public int Price { get; }
		public string ID { get; }

		public Item(string Name, string Description, int Price, string ID)
		{
			this.Name = Name;
			this.Description = Description;
			this.Price = Price;
			this.ID = ID;
		}
	};

	class CommandStore : CommandBase
	{
		public List<Item> items { get; private set; }
		
		public CommandStore(string label, string description) : base(label, description)
		{
			this.Usage = "<list | buy> [item]";
			this.NeedsRegister = true;
			this.WIP = true;

			items.Add(new Item("Test", "This is a description", 5, "test_item"));
		}

		internal override bool Execute(List<string> args, TextMessage message, PersonDb db)
		{
			if (args.Count < 1) //Not enough parameters => Show Usage
			{
				CommandManager.AnswerCall(message, "\nUsage:\n" + CommandManager.CmdIndicator + this.Label + " " + this.Usage);
				return false;
			}

			StringBuilder outMessage = new StringBuilder();
			outMessage.Append("\n");

			if (args[0].ToLower() == "buy") //buy item
			{
				if (args.Count == 2 && GetItem(args[1]) != null) //if we have no item to buy -> show list
				{
					//Get Invoker
					CasinoPlayer invoker = DbInterface.GetPlayer(message.InvokerUid, db);

					Item item = GetItem(args[1]);
					if (invoker.Points < item.Price) //Not enough points
					{
						CommandManager.AnswerCall(message, Utils.Utils.ApplyColor(Color.Red) + "\nNot enough Points in your Wallet![S](get fucked)[/S][/COLOR]");
						return false;
					}
					//Change the points nauw
					if (!DbInterface.AlterPoints(invoker, -item.Price)) //Can't change points for some reason
					{
						CommandManager.AnswerCall(message, "Shitty mcshitfuck");
						throw new Exception("Shitty mcschit fuck again");
					}
					DbInterface.GiveItem(invoker, item.ID);
					DbInterface.SaveChanges();
					
					//Tell the peepz about their item
					CommandManager.AnswerCall(message, Utils.Utils.ApplyColor(Color.DarkGreen) + "\nYou've bought " + item.Name + " for " + item.Price + " Points![/COLOR]\nYou now have " + invoker.Points + " Points");
					return true;
				}
			}

			if (args[0].ToLower() == "list") //List items
			{
				//List
				if (args.Count == 2 && GetItem(args[1]) != null) //if we have a item show more information about it else list items
				{
					Item item = GetItem(args[1]);
					if (item != null)
					{
						outMessage.Append(item.Name + " | " + item.Description + " | " + item.Price + " Points");
					}
				}
				else
				{
					outMessage.AppendFormat("{0, -15} | {1, -15}\n", "Name", "Price");
					foreach (var item in items)
					{
						outMessage.AppendFormat("{0, -15} | {1, -15}\n", item.Name, item.Price);
					}
				}
				CommandManager.AnswerCall(message, outMessage.ToString());
				return true;
			}

			return true;
		}

		private Item GetItem(string Name)
		{
			return items.Find(i => i.Equals(Name));
		}
	}
}