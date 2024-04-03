using System.Text.Json; 
namespace Game
{
    public class UserEditor
    {
        public static void Start(){
            while(true){
                Console.Clear();
                Menu menu=new Menu("User Editor","Create new user","Edit user","Delete user","Back to Main Menu");
                menu.print();
                int option=menu.selectOption();

                if(option==1){
                    string name;
                    Console.Clear();
                    Console.WriteLine("Enter User Name:");
                    name=Console.ReadLine();
                    List<string> cards=new List<string>();
                    cards=cardAdder(cards);

                    User user=new User(name,cards);
                    user.export("./Users/"+name+".json");
                }
                else if(option==2){
                    editUser();
                }
                else if(option==3){
                    deleteUser();
                }
                else{
                    break;
                }
            }
        }

        public static List<string> printUsers(){
            List<string> userNames=getUserNames("./Users/");

            Utils.print("Existing Users are:",ConsoleColor.DarkBlue);
            Console.WriteLine();

            for(int i=0;i<userNames.Count ;i++){
                Utils.print(Convert.ToString(i+1)+". ",ConsoleColor.Blue);
                Console.WriteLine(userNames[i]);
            }

            return userNames;
        }

        public static List<string> getUserNames(string dir){
            List<string> names = new List<string> (Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories));
            
            for(int i=0 ; i<names.Count ; i++)
            {
                names[i]=names[i].Substring(dir.Length);
                names[i]=names[i].Substring(0,names[i].Length-5);
            }

            return names;
        }

        public static void deleteUser(){
            while(true){
                Console.Clear();
                List<string> userNames=printUsers();
                int index=Utils.inputInt("Select the user you want to delete or "
                +Convert.ToString(userNames.Count+1)+" to go back",1,userNames.Count+1);
                
                if(index>userNames.Count)return;
                File.Delete("./Users/"+userNames[index-1]+".json");
            }
        }

        public static void editUser(){
            Console.Clear();
            List<string> userNames=printUsers();

            int index=Utils.inputInt("Select the user you want to edit or "
            +Convert.ToString(userNames.Count+1)+" to go back",1,userNames.Count+1);
            if(index>userNames.Count)return;

            User user=new User();
            user.import("./Users/"+userNames[index-1]+".json");

            while(true){
                Console.Clear();
                Menu menu=new Menu("Edit user","Edit Name","Add Card to Deck","Remove Card from Deck","Back to User Edit Menu");
                menu.print();
                int option=menu.selectOption();

                string oldName=user.name;
                if(option==1){
                    Console.WriteLine("Enter New User Name:");
                    user.name=Console.ReadLine();
                }
                else if(option==2){
                    user.cards=cardAdder(user.cards);
                    user.export("./Users/"+user.name+".json");
                }
                else if(option==3){
                    while(true){
                        Console.Clear();
                        showDeck(user.cards);
                        index=Utils.inputInt("Select the card you want to remove from the deck or "
                        +Convert.ToString(user.cards.Count+1)+" to go back",1,user.cards.Count+1);
                        if(index>user.cards.Count)break;
                        user.cards.RemoveAt(index-1);
                    }
                }
                else break;

                File.Delete("./Users/"+oldName+".json");
                user.export("./Users/"+user.name+".json");
            }
        }

        public static void showDeck(List<string> cardNames){
            Utils.print("The cards added to the user are:",ConsoleColor.DarkBlue);
            Console.WriteLine();
            for(int i=0;i<cardNames.Count;i++){
                Console.Write(i+1);
                Console.Write(". ");
                string jsonString=File.ReadAllText("./Cards/"+cardNames[i]+".json");
                Card card=JsonSerializer.Deserialize<Card>(jsonString);
                card.print();
            }
        }

        public static List<string> cardAdder(List<string> cards){
            List<string> cardNames=CardEditor.getCardNames("./Cards/");
            while(true){
                Console.Clear();
                CardEditor.printCards();
                showDeck(cards);

                int index=Utils.inputInt("Select a new card for the user's deck or "
                +Convert.ToString(cardNames.Count+1)+" to finish the deck",1,cardNames.Count+1);

                if(index>cardNames.Count)break;
                cards.Add(cardNames[index-1]);
            }
            return cards;
        }
    }
}