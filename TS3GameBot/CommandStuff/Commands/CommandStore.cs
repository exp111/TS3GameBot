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
		public List<Item> items { get; } = new List<Item>(new Item[]
		{ new Item("Test", "This is a description", 5, "test_item") });

		public CommandStore(string label, string description) : base(label, description)
		{
			this.Usage = "<list | buy> [item]";
			this.NeedsRegister = true;
			this.WIP = true;
			this.Enabled = false;
		}

		internal override bool Execute(List<string> args, TextMessage message, PersonDb db)
		{
			if (args.Count < 1) //Not enough parameters => Show Usage
			{
				CommandManager.AnswerCall(message, "\nUsage:\n" + CommandManager.CmdIndicator + this.Label + " " + this.Usage);
				return false;
			}

			String arg0 = args[0].ToLower();
			if (arg0 != "buy" && arg0 != "list")
			{
				CommandManager.AnswerCall(message, "\nUsage:\n" + CommandManager.CmdIndicator + this.Label + " " + this.Usage);
				return false;
			}

			StringBuilder outMessage = new StringBuilder();
			outMessage.Append("\n");

			if (arg0 == "buy") //buy item
			{
				if (args.Count == 2 && GetItem(args[1]) != null) //if we have no item to buy -> show list
				{
					//Get Invoker
					CasinoPlayer invoker = DbInterface.GetPlayer(message.InvokerUid, db);

					Item item = GetItem(args[1]);
					if (invoker.Points < item.Price) //Not enough points
					{
						CommandManager.AnswerCall(message, Responses.NotEnoughPoints);
						return false;
					}
					//Change the points nauw
					if (!DbInterface.AlterPoints(invoker, -item.Price)) //Can't change points for some reason
					{
						CommandManager.AnswerCall(message, "An Unknown Error Occured! \nGive this to your Admin: 'Alter Points failed in CommandStore.cs'");
						throw new Exception("Alter Points failed in CommandStore.cs");
					}
					DbInterface.GiveItem(invoker, item.ID);
					DbInterface.SaveChanges(db);
					
					//Tell the peepz about their item
					CommandManager.AnswerCall(message, $"{Utils.Utils.ApplyColor(Color.DarkGreen)}\nYou've bought {item.Name} for {item.Price} Points![/COLOR]\nYou now have {invoker.Points} Points");
					return true;
				}
				else
				{
					ListItems(args, message, ref outMessage);
					CommandManager.AnswerCall(message, outMessage.ToString());
					return true;
				}
			}

			if (arg0 == "list") //List items
			{
				if (args.Count == 2 && GetItem(args[1]) != null) //if we have a item show more information about it else list items
				{
					Item item = GetItem(args[1]);
					if (item != null)
					{
						outMessage.Append($"{item.Name} | {item.Description} | {item.Price} Points");
					}
				}
				else
				{
					//List
					ListItems(args, message, ref outMessage);
				}
				CommandManager.AnswerCall(message, outMessage.ToString());
				return true;
			}

			return true;
		}

		public Item GetItem(string Name)
		{
			return items.Find(i => i.Name.Equals(Name));
		}
		public Item GetItemFromID(string ItemID)
		{
			return items.Find(i => i.ID.Equals(ItemID));
		}

		private void ListItems(List<string> args, TextMessage message, ref StringBuilder outMessage)
		{
			foreach (var item in items)
			{
				outMessage.Append($"{item.Name} | {item.Price} Points\n");
			}
		}
	}
}
