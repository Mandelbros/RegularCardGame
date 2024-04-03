using System.Text.Json;

namespace RegularCardGame.Data
{
    // Card of the game
    public class Card : ICloneable, IComparable
    {
        public string name { get; set;} // name
        public int life { get; set;} // health points
        public int attack { get; set;} // attack points
        public int cost { get; set;} // mana cost
        public bool canAttack { get; set;} // whether it can attack in the current turn
        public List<Effect> effects {get; set;} // effects it may activate
        public Dictionary<string,int> memory {get; set;} // variables
        public string description {get; set;} // description of the card
        public string img {get; set;} // image file name

        // empty class constructor
        public Card(){
            this.name="";
            this.life=0;
            this.attack=0;
            this.cost=0;
            this.canAttack=false;
            this.effects=new List<Effect>();
            this.memory=new Dictionary<string,int>();
            this.description="ERROR 404: CARD NOT FOUND";
            this.img="Null.png";
        }

        // class constructor
        public Card(string name,int life,int attack,int cost,List<Effect> effects,string description,string img){
            this.name=name;
            this.life=life;
            this.attack=attack;
            this.cost=cost;
            this.canAttack=false;
            this.effects=effects;
            this.memory=new Dictionary<string,int>();
            this.description=description;
            this.img=img;
        }

        // class constructor (copy of another card)
        public Card(Card other){
            this.name=other.name;
            this.life=other.life;
            this.attack=other.attack;
            this.cost=other.cost;
            this.canAttack=other.canAttack;
            this.effects=other.effects;
            this.memory=other.memory;
            this.description=other.description;
            this.img=other.img;
        }

        // returns a clone of this card
        public Card Clone(){
            return new Card(this);
        }

        // clone method of the ICloneable interface
        object ICloneable.Clone(){
            return Clone();
        }

        // overrites card with imported card in a .json file from a given path
        public void import(string fileName){
            string jsonString=File.ReadAllText(fileName);
            Card imported=JsonSerializer.Deserialize<Card>(jsonString);
            this.name=imported.name;
            this.life=imported.life;
            this.attack=imported.attack;
            this.cost=imported.cost;
            this.effects=imported.effects;
            this.memory=imported.memory;
            this.img=imported.img;
            this.description=imported.description;
        }

        // exports the card as a .json to a given path
        public void export(string fileName){
            JsonSerializerOptions options=new JsonSerializerOptions{WriteIndented=true};
            string jsonString=JsonSerializer.Serialize(this,options);
            File.WriteAllText(fileName, jsonString);
        }

        // tries to raise the effects with a given (when), and receives the id of the player, index of the field and name of this card
        public void raiseEffects(string when,int activatorHeroId,int activatorfieldIndex,string activatorName){
            Commands.activatorHeroId=activatorHeroId;
            Commands.activatorOpponentId=(activatorHeroId+1)%2;
            Commands.activatorfieldIndex=activatorfieldIndex;
            Commands.activatorName=activatorName;
            Game.match.interpreter.GLOBAL_SCOPE=memory; // the interpreter will use this card's memory
            foreach(Effect effect in effects){
                if(effect.when==when)effect.tryRaise(); // if the (when) coincide tries to raise the effect
            }
        }

        // compares this card with another based on mana cost
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

        // compares this card with another based on attack points
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