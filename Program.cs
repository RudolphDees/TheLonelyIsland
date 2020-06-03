﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleApp1
{
	class Program
	{
		static void Main(string[] args)
		{
			Random rand = new Random();
			int environment = 0, specialEvent = 0, shoreline = 0, i = 0, j = 0;
			int boardSize = 25;
			bool GG = false;
			string[] inventory = { "Compass" };
			List<string> backpack = new List<string>(inventory);
			Square[,] board = new Square[boardSize, boardSize];
			Player player = new Player(backpack, 0, 0, GG);
			for (j = 0; j < boardSize; j++)
			{
				for (i = 0; i < boardSize; i++)
				{
					int odds = rand.Next(0, 20);
					if (j == 0)
						environment = shoreline;
					else if (odds < 5 && board[i, j - 1].environment < 4 || board[i, j - 1].environment == 0)
						environment = board[i, j - 1].environment + 1;
					else environment = board[i, j - 1].environment;
					if (odds > 16)
						specialEvent = 1;
					else specialEvent = 0;
					board[i, j] = new Square(environment, specialEvent);
				}
			}
			Console.WriteLine("You wake up with a throbbing in your head. You open you eyes to a painfully bright light. The sun is already starting to scorch your skin." + Environment.NewLine + 
			"You stand up and attempt to get your bearings. Luckily you find a backpack nearby. You decide to look around for away off of the abandoned island. Good Luck.");
			board[player.locationX, player.locationY].Description(player, board[player.locationX, player.locationY]);
			for (i = 0; i < 100; i++)
			{
				if (player.GG == false)
				{
					player.doSomething(board[player.locationX, player.locationY], player);
					if (player.GG == false)
						board[player.locationX, player.locationY].Description(player, board[player.locationX, player.locationY]);
				}
			}
		}
	}
}

public class Square
{
	public int environment = 0;
	public int specialEvent = 0;
	public Square(int environment, int specialEvent)
	{
		this.environment = environment;
		this.specialEvent = specialEvent;
	}
	public void Description(Player player, Square square)
	{
		if (this.environment == 0)
			Console.WriteLine("You see waves crashing on a deserted shoreline");
		if (this.environment == 1)
			Console.WriteLine("You see small rocks and shells scattered on the sand");
		if (this.environment == 2)
			Console.WriteLine("You see trees and bushes with rays of light piercing the canopy");
		if (this.environment == 3)
			Console.WriteLine("The light begins to fade as the vegitation thickens");
		if (this.environment == 4)
			Console.WriteLine("You see nothing as no light can penetrate the dense forest");
		if (this.specialEvent == 1)
		{
			bool done = false;
			while (done == false)
			{
				Console.WriteLine("You notice something peculiar around you. Should you investigate?");
				string reply = Console.ReadLine().ToUpper();
				if (reply == "YES")
				{
					this.RandomEvent(player, square);
					done = true;
					square.specialEvent = 0;
				}
				if (reply == "NO")
					done = true;
			}
		}
	}
	public void RandomEvent(Player player, Square square)
	{
		Console.WriteLine("You pick up what appears to be a loaded flare gun with a single round. You put it in your backpack.");
		player.backpack.Insert(player.backpack.Count, "Flare Gun");
	}
}

public class Player
{
	public List<string> backpack = new List<string>();
	public int locationX = 0;
	public int locationY = 0;
	public bool GG = false;
	public Player(List<string> backpack, int locationX, int locationY, bool GG)
	{
		this.backpack = backpack;
		this.locationX = locationX;
		this.locationY = locationY;
		this.GG = GG;
	}
	public void Help()
	{
		Console.WriteLine(Environment.NewLine + "Here are you're following commands:" + Environment.NewLine +
			"1) North to move North" + Environment.NewLine +
			"2) South to move South" + Environment.NewLine +
			"3) East to move East" + Environment.NewLine +
			"4) West to move West" + Environment.NewLine +
			"5) Backpack to view your Backpack" + Environment.NewLine +
			"6) Yes and No" + Environment.NewLine +
			"7) Type the name of anything in your backpack to inspect/use it" + Environment.NewLine +
			"7) Quit to Quit" + Environment.NewLine);
	}
	public void Inventory()
	{
		Console.WriteLine(Environment.NewLine + "You open your back pack and find the following...");
		for (int i = 0; i < this.backpack.Count; i++)
		{
			Console.WriteLine($"{ i + 1 }) {backpack[i]}");
		}
		Console.WriteLine();
	}
	public void doSomething(Square square, Player player)
	{
		Console.WriteLine("What would you like to do?");
		string move = Console.ReadLine().ToUpper();
		if (move == "EAST" && this.locationX < 25)
			this.locationX++;
		else if (move == "WEST" && this.locationX > 0)
			this.locationX--;
		else if (move == "NORTH" && this.locationY < 25)
			this.locationY++;
		else if (move == "SOUTH" && this.locationY > 0)
			this.locationY--;
		else if (move == "HELP")
			this.Help();
		else if (move == "BACKPACK")
			this.Inventory();
		else if (move == "COMPASS")
			Console.WriteLine("You shake the compass but the needle appears to be stuck");
		else if (move == "FLARE GUN")
		{
			Console.WriteLine("You hold a flare gun with a single flare remaining. Do you fire it?");
			string reply = Console.ReadLine().ToUpper();
			if (reply == "YES" && square.environment <= 2)
			{
				Console.WriteLine("You fire the flare directly above your head. As you hear it soar into teh sky and fade away into the clouds you lose hope." + Environment.NewLine +
					"As Despair covers you you lift your head to the horizon and see one small boat. You think it's your imagination and shake your head, however, as second glance the boat begins to turn." + Environment.NewLine +
					"The boat grows bigger and bigger as it approaches the island and eventually lands on the beach. The sailors rush over, give you food and water, and hurry you back to the boat" + Environment.NewLine +
					"Just when you thought all was lost you were saved." + Environment.NewLine +
					"Congratulations!");
				player.GG = true;
			}
			else if (reply == "YES" && square.environment > 2)
			{
				Console.WriteLine("You fire the flare directly above your head. It bounces off the canopy of leaves and branches and quickly goes out. You throw the flare gun on the ground feeling veyr defeated");
				this.backpack.Remove("Flare Gun");
			}
			else if (reply == "NO")
				Console.WriteLine("You put the flare back into your backpack.");
		}
		else Console.WriteLine("Try again");
	}
}
