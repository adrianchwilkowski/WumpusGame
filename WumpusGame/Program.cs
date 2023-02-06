using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

public class info
{
    public string visited { get; set; }
    public string agent { get; set; }
    public string danger { get; set; }
    public info()
    {
        visited = "       ";
        agent = "       ";
        danger = "       ";
    }
    public void visit()
    {
        visited = "   V   ";
    }
    public void isAgent()
    {
        agent = $"  {globalVariables.agentIcon}  ";
    }
    public void wumpus()
    {
        danger = "   W   ";
    }
    public void pit()
    {
        danger = "   P   ";
    }
    public void W_P()
    {
        danger = " W + P ";
    }
    public void killedWumpus()
    {
        danger = "       ";
    }
    public void restart()
    {
        agent = "       ";
    }
}
public class start
{
    public struct coordinates
    {
        public int x;
        public int y;
        public static bool operator ==(coordinates first, coordinates second)
        {
            return Equals(first, second);
        }
        public static bool operator !=(coordinates first, coordinates second)
        {
            return !(first == second);
        }
    }
    public coordinates agent;
    public coordinates end;
    public coordinates wumpus;
    public List<coordinates> danger = new List<coordinates>();
    public coordinates gold;
    public int shots;
    public coordinates shotCoordinates;
    public List<coordinates> inUse = new List<coordinates>();
    public start()
    {
        agent.x = 0;
        agent.y = 0;
        end.x = 3;
        end.y = 3;
        inUse.Add(agent);
        inUse.Add(end);
        shots = 1;
        Random rand = new Random();
        while (true)
        {
            wumpus.x = rand.Next(4);
            wumpus.y = rand.Next(4);
            if (wumpus != agent)
            {
                inUse.Add(wumpus);
                break;
            }
        }
        int amountOfPits = rand.Next(1,4);
        coordinates availabilityCheck;
        bool check;
        for(int i = 0 ; i <= amountOfPits ; i++)
        {
            availabilityCheck.x = rand.Next(4);
            availabilityCheck.y = rand.Next(4);
            check = true;
            foreach(coordinates j in inUse)
            {
                if (availabilityCheck == j)
                {
                    check = false;
                    i--;
                    break;
                }
            }
            if (check)
            {
                inUse.Add(availabilityCheck);
                danger.Add(availabilityCheck);
            }
        }
        while (true) 
        {
            availabilityCheck.x = rand.Next(4);
            availabilityCheck.y = rand.Next(4);
            check = true;
            foreach (coordinates j in inUse)
            {
                if (availabilityCheck == j)
                {
                    check = false;
                    break;
                }
            }
            if (check)
            {
                inUse.Add(availabilityCheck);
                gold=availabilityCheck;
                break;
            }
        }
    }
    public void move(ConsoleKey input) 
    {
        
        if(input == ConsoleKey.DownArrow && agent.x - 1 >= 0)
        {
            agent.x -= 1;
        }
        if (input == ConsoleKey.UpArrow && agent.x + 1 <= 3)
        {
            agent.x += 1;
        }
        if (input == ConsoleKey.LeftArrow && agent.y - 1 >= 0)
        {
            agent.y -= 1;
        }
        if (input == ConsoleKey.RightArrow && agent.y + 1 <= 3)
        {
            agent.y += 1;
        }
    }
    public bool smellsWumpus()
    {
        if(agent.x == wumpus.x)
        {
            if (agent.y == wumpus.y + 1 || agent.y == wumpus.y - 1) return true;
        }
        if (agent.y == wumpus.y)
        {
            if (agent.x == wumpus.x + 1 || agent.x == wumpus.x - 1) return true;
        }
        return false;
    }
    public bool feelsBreeze()
    {
        foreach (coordinates i in danger)
        {
            if (agent.x == i.x)
            {
                if (agent.y == i.y + 1 || agent.y == i.y - 1) return true;
            }
            if (agent.y == i.y)
            {
                if (agent.x == i.x + 1 || agent.x == i.x - 1) return true;
            }
        }
        
        return false;
    }
    public bool killedByWumpus()
    {
        if (agent.x == wumpus.x && agent.y == wumpus.y)
        {
            return true;
        }
        return false;
    }
    public bool foundTreasure()
    {
        if (agent == gold)
        {
            return true;
        }
        return false;
    }
    public bool fallIntoPit()
    {
        foreach (coordinates i in danger)
        {
            if (agent.x == i.x && agent.y == i.y)
            {
                return true;
            }
        }
        return false;
    }
    public bool killWumpus()
    {
        var input = Console.ReadKey().Key;
        int x = agent.x;
        int y = agent.y;
        shots -= 1;
        if (input == ConsoleKey.DownArrow && agent.x - 1 >= 0)
        {
            x -= 1;
        }
        else if (input == ConsoleKey.UpArrow && agent.x <= 3)
        {
            x += 1;
        }
        else if (input == ConsoleKey.LeftArrow && agent.y - 1 >= 0)
        {
            y -= 1;
        }
        else if (input == ConsoleKey.RightArrow && agent.y + 1 <= 3)
        {
            y += 1;
        }
        else
        {
            shots += 1;
        }
        if (x==wumpus.x && y == wumpus.y)
        {
            wumpus.x = 10;
            wumpus.y = 10;
            shotCoordinates.x = x;
            shotCoordinates.y = y;
            return true;
        }
        return false;
    }
}
static class globalVariables
{
    public static string agentIcon = " @ ";
    public static bool hasTreasure = false;
    public static string file = System.IO.File.ReadAllText(@"records.json");
    public static readJSON read = JsonSerializer.Deserialize<readJSON>(file);
}

class readJSON
{
    public int wins {get; set; }
    public int games { get; set; }
}

class Program
    {
        public static bool menu()
        {
            string play=">play<";
            string records="records";
            string quit="quit";
            int option=1;
            ConsoleKey input = new ConsoleKey();
            while(true){
                switch(option)
                {
                    case 1:
                        play = ">play<";
                        records = "records";
                        quit="quit";
                        break;
                    case 2:
                        play = "play";
                        records = ">records<";
                        quit="quit";
                        break;
                    case 3:
                        play = "play";
                        records = "records";
                        quit=">quit<";
                        break;
                }
                Console.Clear();
                Console.SetCursorPosition((Console.WindowWidth ) / 2 - play.Length/2, Console.WindowHeight/2 - 1);
                Console.WriteLine(play);
                Console.SetCursorPosition((Console.WindowWidth ) /2 - records.Length/2, Console.CursorTop);
                Console.WriteLine(records);
                Console.SetCursorPosition((Console.WindowWidth ) /2 - quit.Length/2, Console.CursorTop);
                Console.WriteLine(quit);
                input = Console.ReadKey().Key;
                switch(input)
                {
                    case ConsoleKey.UpArrow:
                        if(option>1) option--;
                        else option=3;
                        break;
                    case ConsoleKey.DownArrow:
                        if(option<3) option++;
                        else option=1;
                        break;
                    case ConsoleKey.Enter:
                        switch(option)
                        {
                            case 1:
                                Program.play();
                                globalVariables.read.games++;
                                break;
                            case 2:
                                Program.records();
                                break;
                            case 3:
                                return true;
                                break;
                        }
                        break;
                }
            }
        }
        public static void records()
        {
            Console.Clear();
            Console.SetCursorPosition((Console.WindowWidth ) / 2 - 2, Console.WindowHeight/2 - 1);
            Console.WriteLine("games: {0}",globalVariables.read.games);
            Console.SetCursorPosition((Console.WindowWidth ) / 2 - 2, Console.CursorTop);
            Console.WriteLine("wins: {0}", globalVariables.read.wins);
            Console.ReadKey();
        }
        public static void output(info[,] variables)
        {
            Console.Clear();
            Console.WriteLine("---------------------------------");
            Console.WriteLine("|{0}|{1}|{2}|{3}|", variables[3, 0].visited, variables[3, 1].visited, variables[3, 2].visited, variables[3, 3].visited);
            Console.WriteLine("|{0}|{1}|{2}|{3}|", variables[3, 0].agent, variables[3, 1].agent, variables[3, 2].agent, variables[3, 3].agent);
            Console.WriteLine("|{0}|{1}|{2}|{3}|", variables[3, 0].danger, variables[3, 1].danger, variables[3, 2].danger, variables[3, 3].danger);
            Console.WriteLine("---------------------------------");
            Console.WriteLine("|{0}|{1}|{2}|{3}|", variables[2, 0].visited, variables[2, 1].visited, variables[2, 2].visited, variables[2, 3].visited);
            Console.WriteLine("|{0}|{1}|{2}|{3}|", variables[2, 0].agent, variables[2, 1].agent, variables[2, 2].agent, variables[2, 3].agent);
            Console.WriteLine("|{0}|{1}|{2}|{3}|", variables[2, 0].danger, variables[2, 1].danger, variables[2, 2].danger, variables[2, 3].danger);
            Console.WriteLine("---------------------------------");
            Console.WriteLine("|{0}|{1}|{2}|{3}|", variables[1, 0].visited, variables[1, 1].visited, variables[1, 2].visited, variables[1, 3].visited);
            Console.WriteLine("|{0}|{1}|{2}|{3}|", variables[1, 0].agent, variables[1, 1].agent, variables[1, 2].agent, variables[1, 3].agent);
            Console.WriteLine("|{0}|{1}|{2}|{3}|", variables[1, 0].danger, variables[1, 1].danger, variables[1, 2].danger, variables[1, 3].danger);
            Console.WriteLine("---------------------------------");
            Console.WriteLine("|{0}|{1}|{2}|{3}|", variables[0, 0].visited, variables[0, 1].visited, variables[0, 2].visited, variables[0, 3].visited);
            Console.WriteLine("|{0}|{1}|{2}|{3}|", variables[0, 0].agent, variables[0, 1].agent, variables[0, 2].agent, variables[0, 3].agent);
            Console.WriteLine("|{0}|{1}|{2}|{3}|", variables[0, 0].danger, variables[0, 1].danger, variables[0, 2].danger, variables[0, 3].danger);
            Console.WriteLine("---------------------------------");
        }
        public static void gameOverWumpus() 
        { 
            Console.Clear();
            Console.WriteLine("wumpus");
        }
        public static void gameOverPit() 
        { 
            Console.Clear();
            Console.WriteLine("pit");
        }
        public static void gameWon() 
        { 
            Console.Clear();
            Console.WriteLine("you win!");
            globalVariables.read.wins++;
        }
        public static void play()
        {
            globalVariables.agentIcon = " @ ";
            globalVariables.hasTreasure = false;
            info[,] variables = new info[4, 4];
            info d = new info();
            for(int i = 0; i < 4; i++) 
            { 
                for(int j = 0; j < 4; j++)
                {
                variables[i,j] = new info();
                }
            }
            
            start game = new start();
            
            while (true)
            {
                if(game.foundTreasure())
                {
                    globalVariables.agentIcon = "@+T";
                    globalVariables.hasTreasure=true;
                }
                variables[game.agent.x,game.agent.y].isAgent();
                variables[game.agent.x, game.agent.y].visit();
                if (game.smellsWumpus() || game.feelsBreeze())
                {
                    if (game.feelsBreeze() && game.smellsWumpus())
                    {
                        variables[game.agent.x,game.agent.y].W_P();
                    }
                    else if (game.feelsBreeze())
                    {
                        variables[game.agent.x,game.agent.y].pit();
                    }
                    else if(game.smellsWumpus())
                    {
                        variables[game.agent.x,game.agent.y].wumpus();
                    }
                }
                else 
                    {
                        variables[game.agent.x,game.agent.y].killedWumpus();
                    }
                
                if (game.killedByWumpus()) 
                    {
                        gameOverWumpus();
                        break;
                    }
                if(game.fallIntoPit())
                    {
                        gameOverPit();
                        break;
                    }
                if(globalVariables.hasTreasure==true && game.agent == game.end) 
                { 
                    gameWon();
                    break;
                }
                output(variables);
                variables[game.agent.x,game.agent.y].restart();
                var input = Console.ReadKey().Key;
                if (input == ConsoleKey.K && game.shots==1)
                {
                    if(game.killWumpus()==true)
                        {
                            variables[game.shotCoordinates.x,game.shotCoordinates.y].visited="   K   ";
                        }
                }
                else
                {
                    game.move(input);
                }
            }
            Console.WriteLine();
            Console.WriteLine("press any key to go back to the menu");
            var a = Console.ReadKey();
        }
        static void Main(string[] args)
        {
            menu();
            var options = new JsonSerializerOptions { WriteIndented = true };
            string text = JsonSerializer.Serialize(globalVariables.read, options);
            StreamWriter sw = new StreamWriter("records.json");
            sw.WriteLine(text);
            sw.Close();
        }
    }