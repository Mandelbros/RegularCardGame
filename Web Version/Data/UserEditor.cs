using System.Text.Json; 
namespace RegularCardGame.Data
{
    // Helper Methods of the User Editor
    public class UserEditor
    {
        // returns the names of the existing users (.json files) in a given directory
        public static List<string> getUserNames(string dir){
            List<string> names = new List<string> (Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories));
            
            for(int i=0 ; i<names.Count ; i++)
            {
                names[i]=names[i].Substring(dir.Length);
                names[i]=names[i].Substring(0,names[i].Length-5);
            }

            return names;
        }
    }
}