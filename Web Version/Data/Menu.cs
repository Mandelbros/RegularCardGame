namespace RegularCardGame.Data{
    public class Menu{
        public string name;
        public string[] options;

        public Menu(string name,params string[] options){
            this.name=name;
            this.options=options;
        }

        public void print(){
            Utils.print(name,ConsoleColor.DarkBlue);
            Console.WriteLine();
            printOptions();
        }

        public void printOptions(){
            for(int i=0;i<options.Length;i++){
                Utils.print(Convert.ToString(i+1)+". ",ConsoleColor.Blue);
                Console.WriteLine("{0}",options[i]);
            }
        }
        
        public int selectOption(){
            return Utils.inputInt("Select an option",1,options.Length);
        }
    }
}