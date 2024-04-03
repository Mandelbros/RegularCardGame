namespace Game
{
    public static class Commands{
        public static HashSet<string> functionNames;
        public static int activatorHeroId;
        public static int activatorOpponentId;
        public static int activatorfieldIndex;
        public static string activatorName;
        public static string printedText;
        public static List<string> eventLog;

        public static void init(){
            eventLog=new List<string>();
            functionNames=new HashSet<string>();
            functionNames.Add("affectHeroLife");
            functionNames.Add("affectOpponentLife");
            functionNames.Add("affectHeroMana");
            functionNames.Add("affectOpponentMana");
            functionNames.Add("affectAlliedLife");
            functionNames.Add("affectEnemyLife");
            functionNames.Add("affectAlliedAttack");
            functionNames.Add("affectEnemyAttack");
            functionNames.Add("selectAllied");
            functionNames.Add("selectEnemy");
            functionNames.Add("getEnemiesCount");
            functionNames.Add("getAlliesCount");
            functionNames.Add("getHeroLife");
            functionNames.Add("getOpponentLife");
            functionNames.Add("getTurn");
            functionNames.Add("random");
            functionNames.Add("input");
            functionNames.Add("output");
            functionNames.Add("print");
            functionNames.Add("break");
        }

        public static int activate(string name,List<int> args){
            string s=Game.match.players[activatorHeroId].name+"'s \""+activatorName+"\" at position "+Convert.ToString(activatorfieldIndex+1)+" called "+name+"(";
            if(args.Count>0)s+=Convert.ToString(args[0]);
            for(int i=1;i<args.Count;i++){
                s+=", "+Convert.ToString(args[i]);
            }
            eventLog.Add(s+") command.");
            if(name=="affectHeroLife")return Commands.affectHeroLife(args[0]);
            if(name=="affectOpponentLife")return Commands.affectOpponentLife(args[0]);
            if(name=="affectHeroMana")return Commands.affectHeroMana(args[0]);
            if(name=="affectOpponentMana")return Commands.affectOpponentMana(args[0]);
            if(name=="affectAlliedLife")return Commands.affectAlliedLife(args[0],args[1]);
            if(name=="affectEnemyLife")return Commands.affectEnemyLife(args[0],args[1]);
            if(name=="affectAlliedAttack")return Commands.affectAlliedAttack(args[0],args[1]);
            if(name=="affectEnemyAttack")return Commands.affectEnemyAttack(args[0],args[1]);
            if(name=="selectAllied")return Commands.selectAllied();
            if(name=="selectEnemy")return Commands.selectEnemy();
            if(name=="getEnemiesCount")return Commands.getEnemiesCount();
            if(name=="getAlliesCount")return Commands.getAlliesCount();
            if(name=="getHeroLife")return Commands.getHeroLife();
            if(name=="getOpponentLife")return Commands.getOpponentLife();
            if(name=="getTurn")return Commands.getTurn();
            if(name=="random")return Commands.random(args[0],args[1]);
            if(name=="input")return Commands.input();
            if(name=="output")return Commands.output(args[0]);
            if(name=="print")return Commands.print();
            if(name=="break")return Commands._break();
            Console.WriteLine("Command not found");
            System.Environment.Exit(0);
            return 0;
        }

        public static int affectHeroLife(int c){
            string s=Game.match.players[activatorHeroId].name+" ";
            if(c<0)s+="lost";
            else s+="gained";
            s+=" "+Convert.ToString(Math.Abs(c))+" points of life.";
            eventLog.Add(s);

            Game.match.players[activatorHeroId].life+=c;
            if(Game.match.players[activatorHeroId].life<=0)win(activatorOpponentId);
            return 0;
        }

        public static int affectOpponentLife(int c){
            string s=Game.match.players[activatorOpponentId].name+" ";
            if(c<0)s+="lost";
            else s+="gained";
            s+=" "+Convert.ToString(Math.Abs(c))+" points of life.";
            eventLog.Add(s);

            Game.match.players[activatorOpponentId].life+=c;
            if(Game.match.players[activatorOpponentId].life<=0)win(activatorHeroId);
            return 0;
        }

        public static int affectHeroMana(int c){
            string s=Game.match.players[activatorHeroId].name+" ";
            if(c<0)s+="lost";
            else s+="gained";
            s+=" "+Convert.ToString(Math.Abs(c))+" points of mana.";
            eventLog.Add(s);

            Game.match.players[activatorHeroId].mana+=c;
            if(Game.match.players[activatorHeroId].mana<0)Game.match.players[activatorHeroId].mana=0;
            return 0;
        }

        public static int affectOpponentMana(int c){
            string s=Game.match.players[activatorOpponentId].name+" ";
            if(c<0)s+="lost";
            else s+="gained";
            s+=" "+Convert.ToString(Math.Abs(c))+" points of life.";
            eventLog.Add(s);

            Game.match.players[activatorOpponentId].mana+=c;
            if(Game.match.players[activatorOpponentId].mana<0)Game.match.players[activatorOpponentId].mana=0;
            return 0;
        }

        public static int affectAlliedLife(int i,int c){
            i--;
            if(i<0 || i>6 || Game.match.players[activatorHeroId].field[i]==null){
                Console.WriteLine("There is no card there.");
                return 0;
            }

            string s=Game.match.players[activatorHeroId].name+"'s '"
            +Game.match.players[activatorHeroId].field[i].name
            +"' at position "+Convert.ToString(i+1)+" ";
            if(c<0)s+="lost";
            else s+="gained";
            s+=" "+Convert.ToString(Math.Abs(c))+" points of life.";
            eventLog.Add(s);

            
            Game.match.players[activatorHeroId].field[i].life+=c;
            if(Game.match.players[activatorHeroId].field[i].life<=0){
                Game.match.players[activatorHeroId].field[i]=null;
            }
            return 0;
        }

        public static int affectEnemyLife(int i,int c){
            i--;
            if(i<0 || i>6 || Game.match.players[activatorOpponentId].field[i]==null){
                Console.WriteLine("There is no card there.");
                return 0;
            }

            string s=Game.match.players[activatorOpponentId].name+"'s '"
            +Game.match.players[activatorOpponentId].field[i].name
            +"' at position "+Convert.ToString(i+1)+" ";
            if(c<0)s+="lost";
            else s+="gained";
            s+=" "+Convert.ToString(Math.Abs(c))+" points of life.";
            eventLog.Add(s);

            Game.match.players[activatorOpponentId].field[i].life+=c;
            if(Game.match.players[activatorOpponentId].field[i].life<=0){
                Game.match.players[activatorOpponentId].field[i]=null;
            }
            return 0;
        }

        public static int affectAlliedAttack(int i,int c){
            i--;
            if(i<0 || i>6 || Game.match.players[activatorHeroId].field[i]==null){
                Console.WriteLine("There is no card there.");
                return 0;
            }

            string s=Game.match.players[activatorHeroId].name+"'s '"
            +Game.match.players[activatorHeroId].field[i].name
            +"' at position "+Convert.ToString(i+1)+" ";
            if(c<0)s+="lost";
            else s+="gained";
            s+=" "+Convert.ToString(Math.Abs(c))+" points of attack.";
            eventLog.Add(s);

            Game.match.players[activatorHeroId].field[i].attack+=c;
            return 0;
        }

        public static int affectEnemyAttack(int i,int c){
            i--;
            if(i<0 || i>6 || Game.match.players[activatorOpponentId].field[i]==null){
                Console.WriteLine("There is no card there.");
                return 0;
            }

            string s=Game.match.players[activatorOpponentId].name+"'s '"
            +Game.match.players[activatorOpponentId].field[i].name
            +"' at position "+Convert.ToString(i+1)+" ";
            if(c<0)s+="lost";
            else s+="gained";
            s+=" "+Convert.ToString(Math.Abs(c))+" points of attack.";
            eventLog.Add(s);

            Game.match.players[activatorOpponentId].field[i].attack+=c;
            return 0;
        }

        public static int selectAllied(){
            bool ok=false;
            for(int i=0;i<7;i++){
                if(Game.match.players[activatorHeroId].field[i]!=null){
                    ok=true;
                    break;
                }
            }
            string s;
            if(!ok){
                s=Game.match.players[activatorHeroId].name+" has no cards in the field to select from.";
                eventLog.Add(s);
                return 0;
            }
            while(true){
                int i=Utils.inputInt("Select a card from your field",1,7);
                if(Game.match.players[activatorHeroId].field[i-1]==null){
                    Console.WriteLine("Not valid: there is no card in that position.");
                    continue;
                }
                else{
                    s=Game.match.players[activatorHeroId].name+" selected \""
                    +Game.match.players[activatorHeroId].field[i-1].name+"\" at position "+Convert.ToString(i);
                    eventLog.Add(s);
                    return i;
                }
            }
            return 0;
        }

        public static int selectEnemy(){
            bool ok=false;
            for(int i=0;i<7;i++){
                if(Game.match.players[activatorOpponentId].field[i]!=null){
                    ok=true;
                    break;
                }
            }
            string s;
            if(!ok){
                s=Game.match.players[activatorOpponentId].name+" has no cards in the field to select from.";
                eventLog.Add(s);
                return 0;
            }
           while(true){
                int i=Utils.inputInt("Select a card from your field",1,7);
                if(Game.match.players[activatorOpponentId].field[i-1]==null){
                    Console.WriteLine("Not valid: there is no card in that position.");
                    continue;
                }
                else{
                    s=Game.match.players[activatorHeroId].name+" selected \""
                    +Game.match.players[activatorOpponentId].field[i-1].name+"\" at position "+Convert.ToString(i);
                    eventLog.Add(s);
                    return i;
                }
            }
            return 0;
        }

        public static int getAlliesCount(){
            int c=0;
            for(int i=0;i<7;i++){
                if(Game.match.players[activatorHeroId].field[i]!=null)c++;
            }
            return c;
        }

        public static int getEnemiesCount(){
            int c=0;
            for(int i=0;i<7;i++){
                if(Game.match.players[activatorOpponentId].field[i]!=null)c++;
            }
            return c;
        }

        public static int getHeroLife(){
            return Game.match.players[activatorHeroId].life;
        }

        public static int getOpponentLife(){
            return Game.match.players[activatorOpponentId].life;
        }
        
        public static int getTurn(){
            return Game.match.turn;
        }


        public static int random(int a,int b){
            Random rd=new Random();
            return rd.Next(a,b);
        }

        public static int input(){
            return Utils.inputInt("Input a number: ");
        }

        public static int output(int a){
            Console.WriteLine(a);
            return 0;
        }

        public static int print(){
            string s=Game.match.players[activatorHeroId].name+"'s '"
            +activatorName
            +"' at position "+Convert.ToString(activatorfieldIndex+1)+" says \""+printedText+"\"";
            eventLog.Add(s);
            Console.WriteLine(s);
            return 0;
        }

        public static int _break(){
            return 1;
        }

        public static int win(int player){
            Game.match.printLine();
            Game.match.printLogs();
            Console.WriteLine(Game.match.players[player].name+" wins.");
            System.Environment.Exit(0);
            return 0;
        }
    }
}