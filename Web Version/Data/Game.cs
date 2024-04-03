using System.Text.Json;

namespace RegularCardGame.Data
{
    // Game controller
    public static class Game
    {
        public static Match match; // current match
        public static Card hoveredCard; // currently hovered card
        public static string[] userName; // names of the matched users
        public static bool[] bot=new bool[2]{false,false}; // is autoplay
        public static bool gameOver=false; // match is over
        public static double volume=60; // music volume

        // match initializer
        public static void Main(){
            // clear the events log and match variables
            Commands.eventLog=new List<string>();
            gameOver=false;

            // set first player
            string jsonString;
            jsonString=File.ReadAllText("./Users/"+userName[0]+".json");
            User user0=JsonSerializer.Deserialize<User>(jsonString);
            Deck deck0=new Deck(user0.cards);
            Player player0=new Player(user0.name,deck0,0,user0.image);

            // set second player
            jsonString=File.ReadAllText("./Users/"+userName[1]+".json");
            User user1=JsonSerializer.Deserialize<User>(jsonString);
            Deck deck1=new Deck(user1.cards);
            Player player1=new Player(user1.name,deck1,1,user1.image);
            
            // start new match
            Player[] players={player0,player1};
            match=new Match(players,bot);
            match.start();
        }

        // sets the currently hovered card
        public static void setHoveredCard(int id,int index,bool onField){
            // card is in the field
            if(onField){
                if(match.players[id].field[index]!=null)hoveredCard=match.players[id].field[index].Clone();
            }
            // card is in the hand
            else if(match.players[id].hand.Count>index && match.players[id].hand[index]!=null)hoveredCard=match.players[id].hand[index].Clone();
        }

        // sets new players names
        public static void setUserName(string[] _userName){
            userName=_userName;
        }

        // sets new players roles
        public static void setRole(string[] role){
            bot=new bool[2]{false,false};
            if(role[0]=="Human")bot[0]=false;
            else bot[0]=true;
            if(role[1]=="Human")bot[1]=false;
            else bot[1]=true;
        }
    }
}
