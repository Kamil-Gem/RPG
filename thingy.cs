using System.Text;
using System.Text.Json;

namespace rpg
{

    static class MapsBeaten
    {
        public static int Maps = 0;
    }

    static class Functions
    {
        public static List<int> coords = new List<int>();
        static List<List<string>> ImportFormat(string toConvert)
        {
            List<List<string>> list = new List<List<string>>();
            List<string> toaddArray = toConvert.Split(' ').ToList();

            for (int i = 0; i < toaddArray.Count; i++)
            {
                List<string> res = new List<string>();

                for (int j = 0; j < toaddArray[i].Length; j++)
                {
                    res.Add(toaddArray[i][j].ToString());
                }

                list.Add(res);
            }
            return list;
        }

        static List<string> AddMultiple(List<string> original, List<string> toAdd)
        {
            foreach (string s in toAdd)
            {
                original.Add(s);
            }
            return original;
        }

        static string ExportFormat(List<List<string>> toConvert)
        {
            string res = "";
            foreach (var item in toConvert)
            {
                foreach (string str in item)
                {
                    res += str;
                }
                res += " ";
            }
            return res;
        }

        public static void ExportToJson(List<List<string>> map, List<int> playerCoords, int CoinsCollected)
        {
            if (!File.Exists("RPGMap.json"))
            {
                File.Create("RPGMap.json");
            }

            if (!File.Exists("PlayerCoords.json"))
            {
                File.Create("PlayerCoords.json");
            }

            if (!File.Exists("CoinsCollected.json"))
            {
                File.Create("CoinsCollected.json");
            }

            string toExportMap = ExportFormat(map);
            string toExportCoords = $"{playerCoords[0]} {playerCoords[1]}";
            string toExportCoins = CoinsCollected.ToString();

            string jsonStringMap = JsonSerializer.Serialize(toExportMap, new JsonSerializerOptions { WriteIndented = true });
            string jsonStringCoords = JsonSerializer.Serialize(toExportCoords, new JsonSerializerOptions { WriteIndented = true });
            string jsonStringCoins = JsonSerializer.Serialize(toExportCoins, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText("RPGMap.json", jsonStringMap);
            File.WriteAllText("PlayerCoords.json", jsonStringCoords);
            File.WriteAllText("CoinsCollected.json", jsonStringCoins);

            return;
        }

        public static List<List<string>> ImportFromJsonMap()
        {
            if (File.Exists("RPGMap.json"))
            {
                string jsonString = File.ReadAllText("RPGMap.json");
                Console.WriteLine(jsonString);
                StringBuilder stringBuilder = new StringBuilder();

                for (int i = 1; i < jsonString.Length - 1; i++)
                {
                    stringBuilder.Append(jsonString[i]);
                }

                List<List<string>> res = ImportFormat(stringBuilder.ToString());

                foreach (var i in res)
                {
                    foreach (var j in i)
                    {
                        Console.Write($"{j}, ");
                    }
                    Console.Write("\n");
                }
                return res;
            }
            else
            {
                throw new MissingMemberException();
            }
        }

        public static List<int> ImportFromJsonCoords()
        {
            if (File.Exists("PlayerCoords.json"))
            {
                string jsonString = File.ReadAllText("PlayerCoords.json");
                Console.WriteLine(jsonString);

                StringBuilder stringBuilder = new StringBuilder();

                for (int i = 1; i < jsonString.Length - 1; i++)
                {
                    stringBuilder.Append(jsonString[i]);
                }

                string[] jsonList = stringBuilder.ToString().Split(" ");
                Console.WriteLine(stringBuilder);
                List<int> res = new List<int>();

                foreach (string j in jsonList)
                {
                    res.Add(Convert.ToInt32(j));
                }

                return res;
            }
            else
            {
                throw new MissingMemberException();
            }
        }

        public static int ImportFromJsonCoins()
        {
            if (File.Exists("CoinsCollected.json"))
            {
                string jsonString = File.ReadAllText("CoinsCollected.json");

                StringBuilder stringBuilder = new StringBuilder();

                for (int i = 1; i < jsonString.Length - 1; i++)
                {
                    stringBuilder.Append(jsonString[i]);
                }

                return Convert.ToInt32(stringBuilder.ToString());
            }
            else
            {
                throw new MissingMemberException();
            }
        }

        public static List<List<string>> secretEvent(List<List<string>> map)
        {
            for (int i = 0; i < 10; i++)
            {
                map[0].Add("W");
                map[2].Add("W");
            }
            for (int i = 0; i < 9; i++)
            {
                map[1].Add("_");
            }
            map[1].Add("5");
            return map;
        }

        public static List<List<string>> CreateMap(bool secretCreate)
        {
            List<List<string>> toCreate = new List<List<string>>();

            Random Random = new Random();
			int Size;
			
			if (secretCreate) {
				Size = Random.Next(11,30);
			else {
				Size = Random.Next(5, 11);
			}
			
            toCreate = new List<List<string>>(new List<string>[Size]);

            // initialise each row to being empty
            for (int i = 0; i < Size; i++)
            {
                toCreate[i] = new List<string>(new string[Size * 2]);

                for (int j = 0; j < Size * 2; j++)
                {
                    toCreate[i][j] = "_";
                }
            }

            List<int[]> usedCoords = new List<int[]>();

            List<string> toAdd = new List<string>();

            int Coins = Random.Next(Size / 2, Size);

            for (int i = 0; i < Coins; i++)
            {
                toAdd.Add("C");
            }

            int Walls = Random.Next(Size, Size * 2);

            for (int i = 0; i < Walls; i++)
            {
                toAdd.Add("W");
            }

            int Number = Random.Next(Coins / 2, Coins);

            toAdd.Add($"{Number}");

            toAdd.Add("P");

            foreach (string currentAdd in toAdd)
            {
                int Column = Random.Next(0, Size);
                int Row = Random.Next(0, Size * 2);

                int[] Coords = new int[] { Column, Row };

                while (usedCoords.Contains(Coords))
                {
                    Column = Random.Next(0, Size);
                    Row = Random.Next(0, Size * 2);
                    Coords = new int[] { Column, Row };
                }

                if (currentAdd == "P")
                {
                    coords = Coords.ToList();
                    int temp = coords[0];
                    coords[0] = coords[1];
                    coords[1] = temp;
                }

                

                usedCoords.Add(Coords);
                toCreate[Column][Row] = currentAdd;
            }

            if (Random.Next(0,10) != 10 && !secretCreate)
            {
                toCreate = secretEvent(toCreate);
            }

            /*
            foreach (var i in toCreate)
            {
                foreach (var j in i)
                {
                    Console.Write($"{j}, ");
                }
                Console.Write("\n");
            } stfu wat is this commented part
            */

            return toCreate;
        }

        public static void importLevelCount()
        {
            if (!File.Exists("levels.json"))
            {
                MapsBeaten.Maps = 0;
            }
            string levels = File.ReadAllText("levels.json");
            MapsBeaten.Maps = Convert.ToInt32(levels);
        }
    }


    class Map
    {
        public List<List<string>> map = new List<List<string>>(new List<string>[5]);
        public List<int> playerCoords = new List<int>(new int[2]);
        public int CoinsCollected = 0;

        public void MapCreate()
        {
            playerCoords[0] = 0; playerCoords[1] = map.Count - 1;
            for (int i = 0; i < map.Count; i++)
            {
                map[i] = new List<string>(new string[10]);

                for (int j = 0; j < 10; j++)
                {
                    map[i][j] = "_";
                }
            }
        }

        static bool IsInteger(string n)
        {
            bool res = true;
            try
            {
                Convert.ToInt32(n);
            }
            catch
            {
                res = false;
            }
            return res;
        }

        public void Move(int x_Offset, int y_Offset)

        { // this works by working
            try
            {
                if (map[playerCoords[1] + y_Offset][playerCoords[0] + x_Offset] != "W")
                {
                    if (map[playerCoords[1] + y_Offset][playerCoords[0] + x_Offset] == "C")
                    {
                        CoinsCollected++;
                    }

                    bool pass = true;

                    if (IsInteger(map[playerCoords[1] + y_Offset][playerCoords[0] + x_Offset]))
                    {
                        int COUNTER = Convert.ToInt32(map[playerCoords[1] + y_Offset][playerCoords[0] + x_Offset]);
                        Console.WriteLine("hello bro imn here;");
                        pass = false;
                        if (CoinsCollected >= Convert.ToInt32(map[playerCoords[1] + y_Offset][playerCoords[0] + x_Offset]))
                        { 
                            pass = true;
                        }
                        Console.WriteLine(Convert.ToInt32(map[playerCoords[1] + y_Offset][playerCoords[0] + x_Offset]));
                        Console.WriteLine(CoinsCollected);
                        Console.WriteLine(pass);

                        if (pass)
                        {
                            map = Functions.CreateMap(false);
                            playerCoords = Functions.coords;
                            CoinsCollected -= COUNTER;
                            MapsBeaten.Maps++;
                            File.WriteAllText("levels.json", MapsBeaten.Maps.ToString());
                            pass = false;
                        }
                    }

                    if (pass)
                    {
                        map[playerCoords[1]][playerCoords[0]] = "_";
                        playerCoords[1] += y_Offset;
                        playerCoords[0] += x_Offset;
                    }

                }

                else
                {
                    //Console.WriteLine("nah bro u hit a wall");
                }
            }
            catch
            {
                //Console.WriteLine("nah bro u at edge");
                //Console.WriteLine($"Exception is {e.Message}");
            }
            Console.WriteLine($"{map[0].Count}, {map.Count}");
            map[playerCoords[1]][playerCoords[0]] = "P";
        }



        /* up = -1, 0
         * down = 1, 0
         * right = 0, 1
         * left = 0, -1
         */
        public void Play()
        {
            char h = Console.ReadKey(true).KeyChar;
            int x_Offset = 0;
            int y_Offset = 0;

            switch (h)
            {
                case 'w': y_Offset = -1; break;
                case 's': y_Offset = 1; break;
                case 'd': x_Offset = 1; break;
                case 'a': x_Offset = -1; break;
                case 'q': Functions.ExportToJson(map, playerCoords, CoinsCollected); break;
                case 'p': map = Functions.ImportFromJsonMap(); playerCoords = Functions.ImportFromJsonCoords(); CoinsCollected = Functions.ImportFromJsonCoins(); break;
                case 'r': map = Functions.CreateMap(); playerCoords = Functions.coords; CoinsCollected = 0; break;
                default: Console.WriteLine("nit a vlid movement bro"); break;
            }
            Move(x_Offset, y_Offset);
        }
    }

    static class Entry
    {
        static void Main(string[] args)
        {
            Map map = new Map();
            map.MapCreate();
            map.map = Functions.CreateMap();
            map.playerCoords = Functions.coords;

            foreach (var i in map.map)
            {
                foreach (var j in i)
                {
                    Console.Write(j);
                }
                Console.Write("\n");
            }
            Console.WriteLine($"You have {map.CoinsCollected} coins.");

            while (true)
            {
                Functions.importLevelCount();
                map.Play();
                Console.Clear();
                foreach (var i in map.map)
                {
                    foreach (var j in i)
                    {
                        Console.Write(j);
                    }
                    Console.Write("\n");
                }
                Console.WriteLine($"You have {map.CoinsCollected} coins.");
                Console.WriteLine($"You have beaten {MapsBeaten.Maps} maps.");
            }
        }
    }
}