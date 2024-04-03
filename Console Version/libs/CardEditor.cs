using System.Text.Json; 
namespace Game
{
    public static class CardEditor
    {
        public static void Start(){

            while(true){
                Console.Clear();
                
                Menu menu=new Menu("Card Editor","Create new card","Edit card","Delete card","Back to Main Menu");
                menu.print();
                int option=menu.selectOption();

                if(option==1){
                    string name;
                    int life;
                    int attack;
                    int cost;
                    List<Effect> effects=new List<Effect>();

                    Console.Clear();
                    Console.Write("Enter Card Name: ");
                    name=Console.ReadLine();
                    life=Utils.inputInt("Enter Card Life Points: ");
                    attack=Utils.inputInt("Enter Card Attack Points: ");
                    cost=Utils.inputInt("Enter Card Mana Cost: ");
                    effects=enterEffects();

                    Card card=new Card(name,life,attack,cost,effects);
                    card.export("./Cards/"+name+".json");
                }
                else if(option==2){
                    editCardMenu();
                }
                else if(option==3){
                    deleteCard();
                }
                else break;
            }
        }

        public static List<Effect> enterEffects(){
            List<string> whens=new List<string>();
            whens.Add("placed");
            whens.Add("attacks");
            whens.Add("attacked");
            whens.Add("destroyed");
            whens.Add("turnBegin");
            whens.Add("turnEnd");
            List<Effect> effects=new List<Effect>();
            while(true){
                Console.Clear();

                Menu menu=new Menu("Effects menu","Add a new effect","Finish the card");
                menu.print();
                int option=menu.selectOption();

                if(option==1){
                    Console.Clear();
                    string when;
                    menu=new Menu("Moment when the effect takes place"
                                ,"Card is placed on field"
                                ,"Card attacks"
                                ,"Card gets attacked"
                                ,"Card gets destroyed"
                                ,"At the beginning of each turn that the card is on the field"
                                ,"At the end of each turn that the card is on the field");
                    menu.print();
                    option=menu.selectOption();
                    when=whens[option-1];
                    string conditions="";
                    Console.WriteLine("Code to be runned to check the conditions for the effect to be activated.");
                    Console.WriteLine("Enter each line one at a time and end whith the line \"$\":");
                    while(true){
                        string line=Console.ReadLine();
                        if(line=="$")break;
                        conditions+=line;
                    }
                    string actions="";
                    Console.WriteLine("Code to be runned each time the effect is activated.");
                    Console.WriteLine("Enter each line one at a time and end whith the line \"$\":");
                    while(true){
                        string line=Console.ReadLine();
                        if(line=="$")break;
                        actions+=line+"\n";
                    }
                    Effect effect=new Effect(when,conditions,actions);
                    effects.Add(effect);
                }
                else return effects;
            }
        }
        public static void editCardMenu(){
            while(true){
                Console.Clear();
                List<string> cardNames=printCards();
                int index=Utils.inputInt("Select the card you want to edit or "+Convert.ToString(cardNames.Count+1)+" to go back",1,cardNames.Count+1);
                
                if(index>cardNames.Count || index<1)break;

                Card card=new Card();
                card.import("./Cards/"+cardNames[index-1]+".json");

                editCard(card);
            }
        }

        public static void editCard(Card card){
            
            while(true){
                string oldName=card.name;

                Console.Clear();
                card.print();
                Menu menu=new Menu("","Edit Name","Edit Life Points"
                                    ,"Edit Attack Points","Edit Attack Points"
                                    ,"Edit Mana Cost","Edit Effects","Back to Card Edit Menu");
                menu.printOptions();
                int option=menu.selectOption();

                if(option==1)
                {
                    Console.WriteLine("Enter Card Name:");
                    card.name=Console.ReadLine();
                }
                else if(option==2)
                {
                    card.life=Utils.inputInt("Enter Card Life Points: ");
                }
                else if(option==3)
                {
                    card.attack=Utils.inputInt("Enter Card Attack Points: ");
                }
                else if(option==4){
                    card.cost=Utils.inputInt("Enter Card Mana Cost: ");
                }
                else if(option==5){
                    card.effects=enterEffects();
                }
                else break; 

                File.Delete("./Cards/"+oldName+".json");
                card.export("./Cards/"+card.name+".json");
            }
        }

        public static void deleteCard(){
            while(true){
                Console.Clear();
                List<string> cardNames=printCards();
                int index=Utils.inputInt("Select the card you want to delete or "+Convert.ToString(cardNames.Count+1)+" to go back",1,cardNames.Count+1);
                
                if(index>cardNames.Count || index<1)break;
                File.Delete("./Cards/"+cardNames[index-1]+".json");
            }
        }

        public static List<string> printCards()
        {
            List<string> cardNames=getCardNames("./Cards/");

            Utils.print("The existing cards are:",ConsoleColor.DarkBlue);
            Console.WriteLine();
            for(int i=0;i<cardNames.Count ;i++){
                Console.Write(Convert.ToString(i+1)+". ");
                string jsonString=File.ReadAllText("./Cards/"+cardNames[i]+".json");
                Card card=JsonSerializer.Deserialize<Card>(jsonString);
                card.print();
            }
            Console.WriteLine();

            return cardNames;
        }

        public static List<string> getCardNames(string dir)
        {
            List<string> names = new List<string> (Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories));
            
            for(int i=0 ; i<names.Count ; i++){
                names[i]=names[i].Substring(dir.Length);
                names[i]=names[i].Substring(0,names[i].Length-5);
            }

            return names;
        }


    }
}