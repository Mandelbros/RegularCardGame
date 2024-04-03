using System.Text.Json;

namespace Game
{
    public class Card : ICloneable, IComparable
    {
        public string name { get; set;}
        public int life { get; set;}
        public int attack { get; set;}
        public int cost { get; set;}
        public bool canAttack { get; set;}
        public List<Effect> effects {get; set;}
        public Dictionary<string,int> memory {get; set;}

        public Card(){
            this.name="";
            this.life=0;
            this.attack=0;
            this.cost=0;
            this.canAttack=false;
            this.effects=new List<Effect>();
            this.memory=new Dictionary<string,int>();
        }
        public Card(string name,int life,int attack,int cost,List<Effect> effects){
            this.name=name;
            this.life=life;
            this.attack=attack;
            this.cost=cost;
            this.canAttack=false;
            this.effects=effects;
            this.memory=new Dictionary<string,int>();
        }

        public Card(Card other){
            this.name=other.name;
            this.life=other.life;
            this.attack=other.attack;
            this.cost=other.cost;
            this.canAttack=other.canAttack;
            this.effects=other.effects;
            this.memory=other.memory;
        }

        public void printBattle(){
            Utils.print("Name",ConsoleColor.Blue);
            Utils.fancyPrint(15,": "+name);
            Utils.print(" Life",ConsoleColor.Red);
            Utils.fancyPrint(4,": "+Convert.ToString(life));
            Utils.print(" Attack",ConsoleColor.Yellow);
            Utils.fancyPrint(4,": "+Convert.ToString(attack));
            if(canAttack)Utils.print("Can attack",ConsoleColor.DarkBlue);
            else Utils.print("Cannot attack",ConsoleColor.DarkBlue);
            Console.WriteLine();
        }

        public void print(){
            Utils.print("Name",ConsoleColor.Blue);
            Utils.fancyPrint(15,": "+name);
            Utils.print(" Life",ConsoleColor.Red);
            Utils.fancyPrint(4,": "+Convert.ToString(life));
            Utils.print(" Attack",ConsoleColor.Yellow);
            Utils.fancyPrint(4,": "+Convert.ToString(attack));
            Utils.print(" Mana cost",ConsoleColor.Cyan);
            Utils.fancyPrint(4,": "+Convert.ToString(cost));
            Console.WriteLine();
        }

        public Card Clone(){
            return new Card(this);
        }

        object ICloneable.Clone(){
            return Clone();
        }

        public void export(string fileName){
            JsonSerializerOptions options=new JsonSerializerOptions{WriteIndented=true};
            string jsonString=JsonSerializer.Serialize(this,options);
            File.WriteAllText(fileName, jsonString);
        }

        public void import(string fileName){
            string jsonString=File.ReadAllText(fileName);
            Card imported=JsonSerializer.Deserialize<Card>(jsonString);
            this.name=imported.name;
            this.life=imported.life;
            this.attack=imported.attack;
            this.cost=imported.cost;
            this.effects=imported.effects;
            this.memory=imported.memory;
        }

        public void raiseEffects(string when,int activatorHeroId,int activatorfieldIndex,string activatorName){
            Commands.activatorHeroId=activatorHeroId;
            Commands.activatorOpponentId=(activatorHeroId+1)%2;
            Commands.activatorfieldIndex=activatorfieldIndex;
            Commands.activatorName=activatorName;
            Game.match.interpreter.GLOBAL_SCOPE=memory;
            foreach(Effect effect in effects){
                if(effect.when==when)effect.tryRaise();
            }
        }

        public int CompareTo(object? obj) {
            Card card = obj as Card;

            if(card.cost>this.cost){
                return 1;
            }
            else if(card.cost<this.cost){
                return -1;
            }
            else{
                return 0;
            }
        }

        public int CompareToAttack(object? obj) {
            Card card = obj as Card;

            if(card.attack>this.attack){
                return 1;
            }
            else if(card.attack<this.attack){
                return -1;
            }
            else{
                return 0;
            }
        }
    }    
}