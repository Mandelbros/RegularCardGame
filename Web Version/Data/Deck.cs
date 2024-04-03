namespace RegularCardGame.Data
{
    // Set of cards
    public class Deck : ICloneable
    {
        public List<Card> cards; // contained cards

        // class constructor
        public Deck(List<string> cards){
            this.cards=new List<Card>();
            Card card=new Card();
            
            // searches the given cards in the /Cards folder
            foreach(string name in cards){
                card.import("./Cards/"+name+".json");
                this.cards.Add(card.Clone());
            }
        }

        // class constructor (copy of another deck)
        public Deck(Deck other){
            this.cards=new List<Card>();
            foreach(Card card in other.cards){
                this.cards.Add(card.Clone());
            }
        }

        // empty class constructor
        public Deck(){
            this.cards=new List<Card>();
        }

        // returns a clone of this deck
        public Deck Clone(){
            return new Deck(this);
        }

        // clone method of the ICloneable interface
        object ICloneable.Clone(){
            return Clone();
        }

        // reorganizes the cards in a pseudo-random order
        public void shuffle(){
            Random rng=new Random();
            cards=cards.OrderBy(a=>rng.Next()).ToList();
        }

        // removes and returns the last card (top) of the deck
        public Card draw(){
            if(cards.Count==0) return null; // deck is empty

            Card top=cards[cards.Count-1];
            cards.RemoveAt(cards.Count-1);
            return top;
        }
    }
}