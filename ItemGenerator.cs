using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace CommandLine
{
	public class ItemGenerator
	{
		//instance of this class
		static ItemGenerator instance = new ItemGenerator();
		static string tclassFile = "treasureClass.csv";
		string[,] treasureClass;

		static string weaponTypesFile = "WeaponTypes.csv";
		string[,] weaponTypes;
		static string weaponsFile = "Weapons.csv";
		string[,] weapons;
		//stores the index for rarity, durability, etc...
		Dictionary<string,int> weaponsDictionary = new Dictionary<string,int>();

		static string armorTypesFile = "ArmorTypes.csv";
		string[,] armorTypes;
		static string armorFile = "Armors.csv";
		string[,] armor;
		//stores the index for rarity, durability, etc...
		Dictionary<string, int> armorDictionary = new Dictionary<string, int>();

		Random r = new Random();

		static string dataFolder = "dataFiles\\";

		//range 0 - 400
		private float _magicFind = 100;
		public float magicFind
		{
			get
			{
				return _magicFind;
			}
			set
			{
				if (value <= 0)
				{
					value = 0;
				}else if(value >= 400)
				{
					value = 400;
				}
				_magicFind = value;
			}
		}

		//constructor
		public ItemGenerator()
		{

		}

		//main
		public static void Main()
		{
			instance.GetData();
			Item item = instance.GenerateItem();

			string itemData = item.GetData();
			instance.PrintToConsole(itemData);
		}

		//gets all data from text files
		void GetData()
		{
			//PrintToConsole("entered GetData()");
			string path;
			string data;

			path = dataFolder + tclassFile;
			data = System.IO.File.ReadAllText(path);
			treasureClass = ConvertToArray(data);
			//PrintToConsole(treasureClass);

			#region weapon class data
			path = dataFolder + weaponTypesFile;
			data = System.IO.File.ReadAllText(path);
			weaponTypes = ConvertToArray(data);

			path = dataFolder + weaponsFile;
			data = System.IO.File.ReadAllText(path);
			weaponsDictionary = GenerateDictionary(data);
			weapons = ConvertToArray(data);
			#endregion

			#region armor class data
			path = dataFolder + armorTypesFile;
			data = System.IO.File.ReadAllText(path);
			armorTypes = ConvertToArray(data);

			path = dataFolder + armorFile;
			data = System.IO.File.ReadAllText(path);

			armorDictionary = GenerateDictionary(data);
			armor = ConvertToArray(data);
			#endregion

			//PrintToConsole("readched end of getData");
		}

		void PrintToConsole(string s)
		{
			Console.WriteLine(s);
		}

		string[,] ConvertToArray(string input, int startingLine = 1)
		{
			char lineBreak = '\n';
			char unitBreak = ',';
			string extra = "\r";
			input = input.Replace(extra, "");

			//remove all instances of extra from string

			//start with line 2, 
			string[] rows = input.Split(lineBreak);
			string[] topRow = rows[0].Split(unitBreak);
			int xLength = topRow.Length;
			int yLength = rows.Length - startingLine;

			string[,] output = new string[xLength, yLength];
			//PrintToConsole(" ");
			for (int y = 0; y < yLength; y++)
			{
				string[] splitRow = rows[y + startingLine].Split(unitBreak);
				//string r = "";
				for (int x = 0; x < xLength; x++)
				{
					output[x, y] = splitRow[x];
					//PrintToConsole(splitRow[x]);
					//r += ", " + output[x, y];
				}
				//PrintToConsole(r);
			}

			return output;
		}

		//creates and returns a dictionary from the top row
		Dictionary<string, int> GenerateDictionary(string input)
		{
			Dictionary<string, int> output = new Dictionary<string, int>();

			char lineBreak = '\n';
			string extra = "\r";
			char unitBreak = ',';
			input = input.Replace(extra,"");

			//iterate throught top row
			string[] rows = input.Split(lineBreak);
			//PrintToConsole(rows[0]);
			string[] topRow = rows[0].Split(unitBreak);

			for(int i = 0; i < topRow.Length; i++)
			{
				string s = topRow[i];
				output.Add(s, i);
			}

			return output;
		}

		string[] GenerateListFromRarity(string[,] dataFile, int rarityIndex = 1)
		{
			List<string> tempList = new List<string>();
			//PrintToConsole("data file y length: " + dataFile.GetLength(1));
			//PrintToConsole("y length" + dataFile.GetLength(1));
			for (int y = 0; y < dataFile.GetLength(1); y++)
			{
				//PrintToConsole("entered for loop");
				//get rarity number
				//rarity is x = 1
				int rarity = Convert.ToInt32(dataFile[rarityIndex, y]);
				//PrintToConsole(rarity.ToString());
				int randomInt = r.Next(1, rarity + 1);
				//PrintToConsole("rarity: " + rarity + "; randomInt: " + randomInt + "; name: " + dataFile[0, y]);
				if (rarity == randomInt)
				{
					//add to list
					tempList.Add(dataFile[0, y]);
				}
			}
			//convert list to array
			string[] output = new string[tempList.Count];
			for (int i = 0; i < output.Length; i++)
			{
				output[i] = tempList[i];
			}

			//return array
			return output;
		}

		//generates list of indexes
		int[] GenerateListFromRarity(string[,] dataFile, string type, int typeIndex = 1, int rarityIndex = 2)
		{
			List<int> tempList = new List<int>();
			//PrintToConsole("y length" + dataFile.GetLength(1));
			for (int y = 0; y < dataFile.GetLength(1); y++)
			{
				if (dataFile[typeIndex, y] == type)
				{
					//get rarity number
					//rarity is x = 1
					int rarity = Convert.ToInt32(dataFile[rarityIndex, y]);
					//PrintToConsole(rarity.ToString());
					int randomInt = r.Next(1, rarity + 1);
					//PrintToConsole("rarity: " + rarity + "; randomInt: " + randomInt + "; name: " + dataFile[0, y]);
					if (rarity == randomInt)
					{
						//add to list
						tempList.Add(y);
					}
				}
			}
			//convert list to array
			int[] output = new int[tempList.Count];
			for (int i = 0; i < output.Length; i++)
			{
				output[i] = tempList[i];
			}

			//return array
			return output;
		}

		//returns the generated item
		Item GenerateItem()
		{
			//PrintToConsole("entered GenerateItem()");
			Item output;
			//pick random treasure class
			string[] tClassList = GenerateListFromRarity(treasureClass, 1);
			int tClassIndex = r.Next(0, tClassList.Length);
			//PrintToConsole("generatedIndex: " + tClassIndex + "; listLength: " + tClassList.Length);
			string tClass = tClassList[tClassIndex];
			//PrintToConsole("generated treasureClass");
			output = GenerateByTreasureClass(tClass);
			return output;
		}

		Item GenerateByTreasureClass(string tc)
		{
			string specialType = "";
			switch (tc)
			{
				case "weapon":
					specialType = GenerateSpecialType();
					return GenerateWeapon(specialType);
				case "armor":
					specialType = GenerateSpecialType();
					return GenerateArmor(specialType);
				default:
					return new Item(tc);
			}
		}

		string GenerateSpecialType()
		{
			string output = "";
			float normalRarity = 1;
			float magicRarity = 0.1f * (1 + magicFind * 0.01f);
			float total = normalRarity + magicRarity;

			float rand = (float)r.NextDouble() * (total);
			if(rand <= normalRarity)
			{
				output = "normal";
			}else
			{
				output = "magic";
			}
			return output;
		}

		Item GenerateWeapon(string specialType)
		{
			//PrintToConsole("entered GenerateWeapon()");
			Weapon output;
			//generate a weapon type, then pick a random weapon of that type
			//type list
			string[] list = GenerateListFromRarity(weaponTypes, 1);
			//type index
			int index = r.Next(0, list.Length);
			//type
			string type = list[index];
			//PrintToConsole("generated Type");

			//weapon list
			int[] indexlist = GenerateListFromRarity(weapons,type,weaponsDictionary["type"],weaponsDictionary["rarity"]);
			//list index
			index = r.Next(0, indexlist.Length);
			//weapons index
			index = indexlist[index];
			//PrintToConsole("generated Index");

			//weapon
			string name = weapons[weaponsDictionary["name"], index];
			int minDamage = Convert.ToInt32(weapons[weaponsDictionary["minDam"], index]);
			int maxDamage = Convert.ToInt32(weapons[weaponsDictionary["maxDam"], index]);
			int speed = Convert.ToInt32(weapons[weaponsDictionary["speed"], index]);
			int durability = Convert.ToInt32(weapons[weaponsDictionary["durability"], index]);
			output = new CommandLine.Weapon(specialType, type, name, minDamage, maxDamage, speed, durability);

			return output;
		}

		Item GenerateArmor(string specialType)
		{
			Armor output;
			//type list
			string[] list = GenerateListFromRarity(armorTypes, 1);
			//type index
			int index = r.Next(0, list.Length);
			//type
			string type = list[index];

			//armor index list
			int[] indexList = GenerateListFromRarity(armor, type, armorDictionary["type"], armorDictionary["rarity"]);
			//list index
			index = r.Next(0, indexList.Length);
			//armor index
			index = indexList[index];

			string name = armor[weaponsDictionary["name"], index];
			int minDefense = Convert.ToInt32(armor[armorDictionary["minDefense"], index]);
			int maxDefense = Convert.ToInt32(armor[armorDictionary["maxDefense"], index]);
			int defense = r.Next(minDefense, maxDefense + 1);
			int durability = Convert.ToInt32(armor[armorDictionary["durability"], index]);
			output = new Armor(specialType, type, name, defense, durability);

			return output;
		}
	}

	public class Item
	{
		public string treasureClass;
		public string type;
		public string name;

		public Item(string inputTreasureClass)
		{
			treasureClass = inputTreasureClass;
		}

		//function that outputs all item data in form of string
		//call this and print to command line
		//subclasses have overrides of this to output the correct data
		public virtual string GetData()
		{
			string output = "Nothing Dropped";
			return output;
		}
	}

	public class Weapon: Item
	{
		//stats

		//normal | magic | rare | unique | set
		public readonly string specialType;
		public readonly int minDamage;
		public readonly int maxDamage;
		public readonly int speed;
		public readonly int durability;
		private int _currentDurability;
		public int currentDurability
		{
			get
			{
				return _currentDurability;
			}
			set
			{
				if(value > durability)
				{
					value = durability;
				}else if (value <= 0)
				{
					//broken
					value = 0;
				}
				_currentDurability = value;
			}
		}

		public Weapon(string inputSpecialType, string inputType, string inputName, int inputMinDam, int inputMaxDam, int inputSpeed, int inputDurability, string inputTreasureClass = "weapon") : base(inputTreasureClass)
		{
			specialType = inputSpecialType;
			type = inputType;
			name = inputName;
			minDamage = inputMinDam;
			maxDamage = inputMaxDam;
			speed = inputSpeed;
			durability = inputDurability;

			currentDurability = durability;
		}

		public override string GetData()
		{
			//name
			//treasure class - type
			//Damage: min - max
			//Speed: speed
			//Durability: 
			string output = specialType + '\n' + name + '\n' + treasureClass + " - " + type + '\n' + "Damage: " + minDamage + " - " + maxDamage + '\n' + "Speed: " + speed + '\n' + "Durability: " + currentDurability + "/" + durability;
			return output;
		}
	}

	public class Armor : Item
	{
		//stats

		//normal | magic | rare | unique | set
		public readonly string specialType;
		public readonly int defense;
		public readonly int durability;
		private int _currentDurability;
		public int currentDurability
		{
			get
			{
				return _currentDurability;
			}
			set
			{
				if (value > durability)
				{
					value = durability;
				}
				else if (value <= 0)
				{
					//broken
					value = 0;
				}
				_currentDurability = value;
			}
		}

		public Armor(string inputSpecialType, string inputType, string inputName, int inputDefense, int inputDurability, string inputTreasureClass = "armor") : base(inputTreasureClass)
		{
			specialType = inputSpecialType;
			type = inputType;
			name = inputName;
			defense = inputDefense;
			durability = inputDurability;

			_currentDurability = durability;
		}

		public override string GetData()
		{
			//name
			//treasure class - type
			//Defense
			//Durability
			string output = specialType + '\n' + name + '\n' + treasureClass + " - " + type + '\n' + "Defense: " + defense + '\n' + "Durability: " + currentDurability + "/" + durability;
			return output;
		}
	}
}
