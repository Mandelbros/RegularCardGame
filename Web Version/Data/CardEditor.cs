using System.Text.Json; 

namespace RegularCardGame.Data
{
    // Helper Methods of the Card Editor
    public static class CardEditor
    {
        // returns a list of the names of all the existing cards (.json files) in a given directory
        public static List<string> getCardNames(string dir){
            List<string> names = new List<string> (Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories));
            
            for(int i=0 ; i<names.Count ; i++){
                names[i]=names[i].Substring(dir.Length);
                names[i]=names[i].Substring(0,names[i].Length-5);
            }
            return names;
        }

        // returns a list of all the existing cards in the /Cards folder
        public static List<Card> getCards(){
            List<string> cardNames=getCardNames("./Cards/");
            List<Card> cards=new List<Card>();

            for(int i=0;i<cardNames.Count ;i++){
                string jsonString=File.ReadAllText("./Cards/"+cardNames[i]+".json");
                Card card=JsonSerializer.Deserialize<Card>(jsonString);
                cards.Add(card);
            }
            return cards;
        }
    }
}