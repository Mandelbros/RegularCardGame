namespace Game
{
    public class Deck : ICloneable
    {
        public List<Card> cards;

        public Deck(List<string> cards){
            
            this.cards=new List<Card>();
            Card card=new Card();
            
            foreach(string name in cards)
            {
                card.import("./Cards/"+name+".json");
                this.cards.Add(card.Clone());
            }
        }

        public Deck(Deck other){
            
            this.cards=new List<Card>();

            foreach(Card card in other.cards)
            {
                this.cards.Add(card.Clone());
            }
        }

        public void shuffle(){
            Random rng=new Random();
            cards=cards.OrderBy(a=>rng.Next()).ToList();
        }

        public Card draw(){
            if(cards.Count==0) return null;

            Card top=cards[cards.Count-1];
            cards.RemoveAt(cards.Count-1);
            return top;
        }

        public Deck(){
            this.cards=new List<Card>();
        }

        public Deck Clone(){
            return new Deck(this);
        }

        object ICloneable.Clone(){
            return Clone();
        }
    }
}