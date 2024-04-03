using System.Text.Json; 
namespace Game
{
    public static class Game
    {
        public static Match match;
        public static void Main()
        {
            Commands.init();

            while(true){
                Console.Clear();
                Menu mainMenu=new Menu("Main menu","Play","Card editor","User editor","Quit");
                mainMenu.print();
                int option=mainMenu.selectOption();

                if(option==1)
                {
                    SelectPlayMode();
                }
                else if(option==2){
                    CardEditor.Start();
                }
                else if(option==3){
                    UserEditor.Start();
                }
                else{
                    return;
                }
            }
        }

        public static void SelectPlayMode()
        {
            while(true){
                Console.Clear();
                Menu menu=new Menu("Play mode","Versus Human","Versus Machine","Back to Main Menu");
                menu.print();
                int option=menu.selectOption();

                if(option==1)
                {
                    HumanMatchMenu();
                }
                else if(option==2){
                    ComputerMatchMenu();
                }
                else
                {
                    return;
                }
            }
        }

        public static void HumanMatchMenu()
        {
            Player? player0=SelectUserMenu("Select first player's user",0);
            Player? player1=SelectUserMenu("Select second player's user",1);

            Player[] players={player0,player1};

            match=new Match(players);
            match.Start(false);
        }

        public static Player SelectUserMenu(string toPrint,int num)
        {
            Console.Clear();
            List<string> userNames=UserEditor.printUsers();
            int index=Utils.inputInt(toPrint,1,userNames.Count);
            string jsonString=File.ReadAllText("./Users/"+userNames[index-1]+".json");

            User? user=JsonSerializer.Deserialize<User>(jsonString);
            Deck deck=new Deck(user.cards);
            Player player=new Player(user.name,deck,num);

            return player;
        }

        public static void ComputerMatchMenu()
        {
            Player? player=SelectUserMenu("Select player's user",0);
            Player? computer=SelectUserMenu("Select computer's user",1);
            Player[] players={player,computer};

            match=new Match(players);
            match.Start(true);
        }
        
    }
}
