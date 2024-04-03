namespace RegularCardGame.Data
{
    // Player of a match
    public class Player
    {
        public string name; // name
        public int life; // health points
        public int mana; // mana points
        public Deck deck; // deck
        public List<Card> hand; // current cards in it's hand
        public Card[] field; // side of the field
        public int id; // id (index of play)
        public string image; // image file name

        // class constructor
        public Player(string name,Deck deck,int id,string image)
        {
            this.name=name;
            this.life=30;
            this.mana=0;
            this.deck=deck;
            deck.shuffle();
            this.hand=new List<Card>();
            // draws 3 cards when initialized
            for(int i=0;i<3;i++){
                this.hand.Add(deck.draw());
            }
            this.field=new Card[7];
            this.id=id;
            this.image=image;
        }

        // makes it draw a card form the deck to the hand
        public void draw(){
            Card top=deck.draw(); // take deck's top card (if exists)
            // deck is not empty
            if(top!=null){
                // hand is not full
                if(hand.Count<10)hand.Add(top);
                // hand is full
                else Commands.eventLog.Add(name+" has too many cards in the hand. Drawn \""+top.name+"\" was destroyed.");
            }
            // deck is empty
            else{
                Commands.eventLog.Add(name+" has no more cards in the deck, loses 1 point of life per turn.");
                Commands.eventLog.Add(name+" has lost 1 point of life.");
                life--; // substract a health point
                if(life<=0)Commands.win((id+1)%2); // non-positive health points, opponent wins
            }
        }

        // tries to place the card from given index of the hand and returns whether it succeded
        public bool placeFromHand(int index){
            // invalid index
            if(hand.Count-1<index || index<0){
                Console.WriteLine("Card index out of range");
                return false;
            }
            // not enough mana
            if(hand[index].cost>mana){
                Console.WriteLine("Not enough mana to play that card");
                return false;
            }
            // place the card in leftmost available index
            for(int i=0;i<7;i++){
                if(field[i]==null){ // index is available
                    field[i]=hand[index].Clone();
                    mana-=field[i].cost;
                    hand.RemoveAt(index);
                    // raise when-placed effects of the card
                    field[i].raiseEffects("placed",id,i,field[i].name);
                    return true;
                }
            }
            Console.WriteLine("The field is already full");
            return false;
        }
    }
}