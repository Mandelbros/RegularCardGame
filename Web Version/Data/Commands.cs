namespace RegularCardGame.Data
{
    // Commands controller
    public static class Commands
    {
        public static HashSet<string> functionNames; // set of the existing commands
        public static int activatorHeroId; // id of the owner of the card that triggers the current effect
        public static int activatorOpponentId; // id of the opponent of the owner of the card that triggers the current effect
        public static int activatorfieldIndex; // index (1...7) in the field of the card that triggered the current effect
        public static string activatorName; // name of the card that triggered the current effect
        public static string printedText; // string passed to the last print() command called
        public static List<string> eventLog; // list of the narrated events of the match
        public static int inputValue; // integer passed to the last input() command called

        // commands initializer
        public static void init(){
            // fill the reserved command names list
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
            // functionNames.Add("selectAllied");
            // functionNames.Add("selectEnemy");
            functionNames.Add("getEnemiesCount");
            functionNames.Add("getAlliesCount");
            functionNames.Add("getHeroLife");
            functionNames.Add("getOpponentLife");
            functionNames.Add("getTurn");
            functionNames.Add("random");
            // functionNames.Add("input");
            functionNames.Add("output");
            functionNames.Add("print");
            functionNames.Add("break");
            functionNames.Add("thereIsEnemy");
            functionNames.Add("thereIsAllied");
            functionNames.Add("getEnemyLife");
            functionNames.Add("getAlliedLife");
            functionNames.Add("getEnemyAttack");
            functionNames.Add("getAlliedAttack");
            functionNames.Add("getMyPos");
        }

        // calls a given command with some arguments
        public static int activate(string name,List<int> args){
            // store event in the log
            string s=Game.match.players[activatorHeroId].name+"'s \""+activatorName+"\" at position "+Convert.ToString(activatorfieldIndex+1)+" called "+name+"(";
            if(args.Count>0)s+=Convert.ToString(args[0]);
            for(int i=1;i<args.Count;i++){
                s+=", "+Convert.ToString(args[i]);
            }
            eventLog.Add(s+") command.");

            // call the corresponding method
            if(name=="affectHeroLife")return Commands.affectHeroLife(args[0]);
            if(name=="affectOpponentLife")return Commands.affectOpponentLife(args[0]);
            if(name=="affectHeroMana")return Commands.affectHeroMana(args[0]);
            if(name=="affectOpponentMana")return Commands.affectOpponentMana(args[0]);
            if(name=="affectAlliedLife")return Commands.affectAlliedLife(args[0],args[1]);
            if(name=="affectEnemyLife")return Commands.affectEnemyLife(args[0],args[1]);
            if(name=="affectAlliedAttack")return Commands.affectAlliedAttack(args[0],args[1]);
            if(name=="affectEnemyAttack")return Commands.affectEnemyAttack(args[0],args[1]);
            // if(name=="selectAllied")return Commands.selectAllied();
            // if(name=="selectEnemy")return Commands.selectEnemy();
            if(name=="getEnemiesCount")return Commands.getEnemiesCount();
            if(name=="getAlliesCount")return Commands.getAlliesCount();
            if(name=="getHeroLife")return Commands.getHeroLife();
            if(name=="getOpponentLife")return Commands.getOpponentLife();
            if(name=="getTurn")return Commands.getTurn();
            if(name=="random")return Commands.random(args[0],args[1]);
            // if(name=="input")return Commands.input();
            if(name=="output")return Commands.output(args[0]);
            if(name=="print")return Commands.print();
            if(name=="break")return Commands._break();
            if(name=="thereIsEnemy")return Commands.thereIsEnemy(args[0]);
            if(name=="thereIsAllied")return Commands.thereIsAllied(args[0]);
            if(name=="getEnemyLife")return Commands.getEnemyLife(args[0]);
            if(name=="getAlliedLife")return Commands.getAlliedLife(args[0]);
            if(name=="getEnemyAttack")return Commands.getEnemyAttack(args[0]);
            if(name=="getAlliedAttack")return Commands.getAlliedAttack(args[0]);
            if(name=="getMyPos")return Commands.getMyPos();

            // error
            Console.WriteLine("Command not found");
            System.Environment.Exit(0);
            return 0;
        }

        // adds c to the health points of the player
        public static int affectHeroLife(int c){
            // store the event in the log
            string s=Game.match.players[activatorHeroId].name+" ";
            if(c<0)s+="lost";
            else s+="gained";
            s+=" "+Convert.ToString(Math.Abs(c))+" points of life.";
            eventLog.Add(s);

            int t=Game.match.players[activatorHeroId].life+c;
            // smooth animation
            while(Game.match.players[activatorHeroId].life!=t){
                if(Game.match.players[activatorHeroId].life<t)Game.match.players[activatorHeroId].life++;
                else Game.match.players[activatorHeroId].life--;
            }

            // if player's health points are non-positive, opponent wins
            if(Game.match.players[activatorHeroId].life<=0)win(activatorOpponentId);
            return 0;
        }

        // adds c to the health points of the opponent
        public static int affectOpponentLife(int c){
            // store the event in the log
            string s=Game.match.players[activatorOpponentId].name+" ";
            if(c<0)s+="lost";
            else s+="gained";
            s+=" "+Convert.ToString(Math.Abs(c))+" points of life.";
            eventLog.Add(s);

            int t=Game.match.players[activatorOpponentId].life+c;
            // smooth animation
            while(Game.match.players[activatorOpponentId].life!=t){
                if(Game.match.players[activatorOpponentId].life<t)Game.match.players[activatorOpponentId].life++;
                else Game.match.players[activatorOpponentId].life--;
            }

            // if opponent's health points are non-positive, player wins
            if(Game.match.players[activatorOpponentId].life<=0)win(activatorHeroId);
            return 0;
        }

        // adds c to the mana points of the player
        public static int affectHeroMana(int c){
            // store the event in the log
            string s=Game.match.players[activatorHeroId].name+" ";
            if(c<0)s+="lost";
            else s+="gained";
            s+=" "+Convert.ToString(Math.Abs(c))+" points of mana.";
            eventLog.Add(s);

            Game.match.players[activatorHeroId].mana+=c;
            // mana points can't be negative
            if(Game.match.players[activatorHeroId].mana<0)Game.match.players[activatorHeroId].mana=0;
            return 0;
        }

        // adds c to the mana points of the opponent
        public static int affectOpponentMana(int c){
            // store the event in the log
            string s=Game.match.players[activatorOpponentId].name+" ";
            if(c<0)s+="lost";
            else s+="gained";
            s+=" "+Convert.ToString(Math.Abs(c))+" points of life.";
            eventLog.Add(s);

            Game.match.players[activatorOpponentId].mana+=c;
            // mana points can't be negative
            if(Game.match.players[activatorOpponentId].mana<0)Game.match.players[activatorOpponentId].mana=0;
            return 0;
        }

        // adds c to the health points of a friendly card at index i of field
        public static int affectAlliedLife(int i,int c){
            i--; // 0-indexed
            // invalid or empty index
            if(i<0 || i>6 || Game.match.players[activatorHeroId].field[i]==null){
                eventLog.Add("There was no card there.");
                return 0;
            }

            // store the event in the log
            string s=Game.match.players[activatorHeroId].name+"'s '"
            +Game.match.players[activatorHeroId].field[i].name
            +"' at position "+Convert.ToString(i+1)+" ";
            if(c<0)s+="lost";
            else s+="gained";
            s+=" "+Convert.ToString(Math.Abs(c))+" points of life.";
            eventLog.Add(s);

            Game.match.players[activatorHeroId].field[i].life+=c;
            // if card health points are non-positive, it's destroyed
            if(Game.match.players[activatorHeroId].field[i].life<=0){
                Game.match.players[activatorHeroId].field[i]=null;
            }
            return 0;
        }

        // adds c to the health points of an enemy card at index i of field
        public static int affectEnemyLife(int i,int c){
            i--; // 0-indexed
            // invalid or empty index
            if(i<0 || i>6 || Game.match.players[activatorOpponentId].field[i]==null){
                eventLog.Add("There was no card there.");
                return 0;
            }

            // store the event in the log
            string s=Game.match.players[activatorOpponentId].name+"'s '"
            +Game.match.players[activatorOpponentId].field[i].name
            +"' at position "+Convert.ToString(i+1)+" ";
            if(c<0)s+="lost";
            else s+="gained";
            s+=" "+Convert.ToString(Math.Abs(c))+" points of life.";
            eventLog.Add(s);

            Game.match.players[activatorOpponentId].field[i].life+=c;
            // if card health points are non-positive, it's destroyed
            if(Game.match.players[activatorOpponentId].field[i].life<=0){
                Game.match.players[activatorOpponentId].field[i]=null;
            }
            return 0;
        }

        // adds c to the attack points of a friendly card at index i of field
        public static int affectAlliedAttack(int i,int c){
            i--; // 0-indexed
            // invalid or empty index
            if(i<0 || i>6 || Game.match.players[activatorHeroId].field[i]==null){
                eventLog.Add("There was no card there.");
                return 0;
            }

            // store the event in the log
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

        // adds c to the attack points of an enemy card at index i of field
        public static int affectEnemyAttack(int i,int c){
            i--; // 0-indexed
            // invalid or empty index
            if(i<0 || i>6 || Game.match.players[activatorOpponentId].field[i]==null){
                eventLog.Add("There was no card there.");
                return 0;
            }

            // store the event in the log
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

        // console-user-input of a friendly card index
        public static int selectAllied(){
            // assert player's field is not empty
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

            // request until valid input
            while(true){
                int i=Utils.inputInt("Select a card from your field",1,7);

                if(Game.match.players[activatorHeroId].field[i-1]==null){ // no card in that index
                    Console.WriteLine("Not valid: there is no card in that position.");
                    continue;
                }
                else{
                    // store the event in the log
                    s=Game.match.players[activatorHeroId].name+" selected \""
                    +Game.match.players[activatorHeroId].field[i-1].name+"\" at position "+Convert.ToString(i);
                    eventLog.Add(s);

                    return i;
                }
            }
            return 0;
        }

        // console-user-input of an enemy card index
        public static int selectEnemy(){
            // assert opponent's field is not empty
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

            // request until valid input
            while(true){
                int i=Utils.inputInt("Select a card from your field",1,7);

                if(Game.match.players[activatorOpponentId].field[i-1]==null){ // no card in that index
                    Console.WriteLine("Not valid: there is no card in that position.");
                    continue;
                }
                // store the event in the log
                else{
                    s=Game.match.players[activatorHeroId].name+" selected \""
                    +Game.match.players[activatorOpponentId].field[i-1].name+"\" at position "+Convert.ToString(i);
                    eventLog.Add(s);
                    return i;
                }
            }
            return 0;
        }

        // returns the amount of friendly cards in the field
        public static int getAlliesCount(){
            int c=0;
            for(int i=0;i<7;i++){
                if(Game.match.players[activatorHeroId].field[i]!=null)c++;
            }
            return c;
        }

        // returns the amount of enemy cards in the field
        public static int getEnemiesCount(){
            int c=0;
            for(int i=0;i<7;i++){
                if(Game.match.players[activatorOpponentId].field[i]!=null)c++;
            }
            return c;
        }

        // returns player's health points
        public static int getHeroLife(){
            return Game.match.players[activatorHeroId].life;
        }

        // returns opponent's health points
        public static int getOpponentLife(){
            return Game.match.players[activatorOpponentId].life;
        }
        
        // returns current turn number
        public static int getTurn(){
            return Game.match.turn;
        }

        // returns a pseudo-random number between a and b
        public static int random(int a,int b){
            Random rd=new Random();
            return rd.Next(a,b);
        }

        // requests and returns a console-user-input integer
        public static int input(){
            // Console.WriteLine(inputValue);
            return inputValue;
        }

        // outputs a given integer
        public static int output(int a){
            // store the event in the log
            eventLog.Add(Convert.ToString(a));
            // Console.WriteLine(a);
            return 0;
        }

        // outputs a string
        public static int print(){
            // store the event in the log
            string s=Game.match.players[activatorHeroId].name+"'s '"
            +activatorName
            +"' at position "+Convert.ToString(activatorfieldIndex+1)+" says \""+printedText+"\"";
            eventLog.Add(s);
            // Console.WriteLine(s);
            return 0;
        }

        // break; statement
        public static int _break(){
            return 1;
        }

        // ends the match
        public static int win(int player){
            Game.gameOver=true;
            return 0;
        }

        // returns whether there is a card at position i of opponent's field
        public static int thereIsEnemy(int i){
            if(i<1 || i>7 || Game.match.players[activatorOpponentId].field[i-1]==null)return 0;
            return 1;
        }

        // returns whether there is a card at position i of player's field
        public static int thereIsAllied(int i){
            if(i<1 || i>7 || Game.match.players[activatorHeroId].field[i-1]==null)return 0;
            return 1;
        }

        // returns the health points of the card (if exists) at position i of opponent's field
        public static int getEnemyLife(int i){
            if(thereIsEnemy(i)==0)return -1; // no card there
            return Game.match.players[activatorOpponentId].field[i-1].life;
        }

        // returns the health points of the card (if exists) at position i of player's field
        public static int getAlliedLife(int i){
            if(thereIsAllied(i)==0)return -1; // no card there
            return Game.match.players[activatorHeroId].field[i-1].life;
        }

        // returns the attack points of the card (if exists) at position i of opponent's field
        public static int getEnemyAttack(int i){
            if(thereIsEnemy(i)==0)return -1; // no card there
            return Game.match.players[activatorOpponentId].field[i-1].attack;
        }

        // returns the attack points of the card (if exists) at position i of player's field
        public static int getAlliedAttack(int i){
            if(thereIsAllied(i)==0)return -1; // no card there
            return Game.match.players[activatorHeroId].field[i-1].attack;
        }

        // returns the index of the field of the card that called this command
        public static int getMyPos(){
            return activatorfieldIndex+1; // 1-indexed
        }
    }
}