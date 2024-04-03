namespace Game
{
    public class Player
    {
        public string name;
        public int life;
        public int mana;
        public Deck deck;
        public List<Card> hand;
        public Card[] field;
        public int id;

        public Player(string name,Deck deck,int id)
        {
            this.name=name;
            this.life=30;
            this.mana=0;
            this.deck=deck;
            deck.shuffle();
            this.hand=new List<Card>();
            for(int i=0;i<3;i++){
                this.hand.Add(deck.draw());
            }
            this.field=new Card[7];
            this.id=id;
        }

        public void draw(){
            Card top=deck.draw();
            if(top!=null){
                if(hand.Count<10)hand.Add(top);
                else{
                    Commands.eventLog.Add(name+" has too many cards in the hand. Drawn \""+top.name+"\" was destroyed.");
                }
            }
            else{
                Commands.eventLog.Add(name+" has no more cards in the deck, loses 1 point of life per turn.");
                Commands.eventLog.Add(name+" has lost 1 point of life.");
                life--;
                if(life<=0)Commands.win((id+1)%2);
            }
        }

        public void showHand(){
            Console.Write("The cards in ");
            Utils.print(name,ConsoleColor.Gray);
            Console.WriteLine("'s hand are:");
            foreach(Card card in hand){
                card.print();
            }
        }

        public void showField(){
            Console.Write("The cards in ");
            Utils.print(name,ConsoleColor.Gray);
            Console.WriteLine("'s field are:");

            for(int i=0;i<7;i++){
                Console.Write(Convert.ToString(i+1)+". ");
                if(field[i]!=null)field[i].printBattle();
                else Console.WriteLine(" -");
            }
        }

        public bool placeFromHand(int index){
            if(hand.Count-1<index || index<0){
                Console.WriteLine("Card index out of range");
                return false;
            }
            if(hand[index].cost>mana){
                Console.WriteLine("Not enough mana to play that card");
                return false;
            }
            for(int i=0;i<7;i++){
                if(field[i]==null){
                    field[i]=hand[index].Clone();
                    mana-=field[i].cost;
                    hand.RemoveAt(index);
                    field[i].raiseEffects("placed",id,i,field[i].name);
                    return true;
                }
            }
            Console.WriteLine("The field is already full");
            return false;
        }
    }

}