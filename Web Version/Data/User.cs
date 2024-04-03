using System.Text.Json;

namespace RegularCardGame.Data
{
    // Game User
    public class User
    {
        public string name { get; set; } // name
        public List<string> cards { get; set; } // cards names
        public string image { get; set; } // image file name

        // class constructor
        public User(string name,List<string> cards,string image){
            this.name=name;
            this.cards=cards;
            this.image=image;
        }

        // empty class constructor
        public User(){
            this.name="";
            this.cards=new List<string>();
            this.image="User.png";
        }

        // overrites user with imported user in a .json file from a given path
        public void import(string fileName){
            string jsonString=File.ReadAllText(fileName);
            User imported=JsonSerializer.Deserialize<User>(jsonString);
            this.name=imported.name;
            this.cards=imported.cards;
            this.image=imported.image;
        }

        // exports the user as a .json to a given path
        public void export(string fileName){
            JsonSerializerOptions options=new JsonSerializerOptions{WriteIndented=true};
            string jsonString=JsonSerializer.Serialize(this, options);
            File.WriteAllText(fileName, jsonString);
        }
    }
}