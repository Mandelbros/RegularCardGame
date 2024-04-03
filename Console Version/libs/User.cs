using System.Text.Json;

namespace Game
{
    public class User
    {
        public string name { get; set;}
        public List<string> cards { get; set;}

        public User(string name,List<string> cards){
            this.name=name;
            this.cards=cards;
        }

        public User(){
            this.name="";
            this.cards=new List<string>();
        }

        public void import(string fileName){
            string jsonString=File.ReadAllText(fileName);
            User imported=JsonSerializer.Deserialize<User>(jsonString);
            this.name=imported.name;
            this.cards=imported.cards;
        }
        public void export(string fileName){
            JsonSerializerOptions options=new JsonSerializerOptions{WriteIndented=true};
            string jsonString=JsonSerializer.Serialize(this, options);
            File.WriteAllText(fileName, jsonString);
        }
    }
}