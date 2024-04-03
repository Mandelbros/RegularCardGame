using System.Text.Json; 
using System;
using System.Threading;
using System.Timers;

namespace Game
{
    public class Match
    {
        public int turn;
        public int current;
        public int opponent;
        public Player[] players;
        public Interpreter interpreter;

        public Match(Player[] players){
            this.turn=0;
            this.current=0;
            this.opponent=0;
            this.players=players;
            Lexer lexer=new Lexer("{}");
            Parser parser=new Parser(lexer);
            this.interpreter=new Interpreter(parser);
        }

        
        public void Start(bool isVsPC)
        {
            while(true)
            {
                current=turn%2;
                opponent=(current+1)%2;

                players[current].mana=Math.Min(10,((turn/2)+1));
                players[current].draw();

                for(int i=0;i<7;i++){
                    if(players[current].field[i]!=null)players[current].field[i].raiseEffects("turnBegin",current,i,players[current].field[i].name);
                    if(players[opponent].field[i]!=null)players[opponent].field[i].raiseEffects("turnBegin",opponent,i,players[opponent].field[i].name);
                }

                while(true){
                    printAllUI();

                    if(current==1 && isVsPC){
                        autoPlay();
                        break;
                    } 

                    if(current==0)
                    {
                        Menu menu=new Menu("Actions","Place a card","Attack","End turn");
                        menu.print();
                            
                        int option=menu.selectOption();

                        if(option==1){
                            if(players[current].hand.Count==0){
                                Console.WriteLine("You have no cards in your hand");
                                continue;
                            }
                            int index=Utils.inputInt("Select the index of the card in your hand",1,players[current].hand.Count)-1;
                            players[current].placeFromHand(index);
                        }
                        else if(option==2){
                            attackMenu();
                        }
                        else{
                            break;
                        }
                    }
                }

                for(int i=0;i<7;i++){
                    if(players[current].field[i]!=null)players[current].field[i].canAttack=true;
                }

                for(int i=0;i<7;i++){
                    if(players[current].field[i]!=null)players[current].field[i].raiseEffects("turnEnd",current,i,players[current].field[i].name);
                    if(players[opponent].field[i]!=null)players[opponent].field[i].raiseEffects("turnEnd",opponent,i,players[opponent].field[i].name);
                }      

                turn++;
                 
            }
        }

        public void printAllUI(){
            Console.Clear();
            printLogs();
            printLine();
            printUI();
}

        public void printLogs(){
            Console.WriteLine("Last action's events:");
            foreach(string s in Commands.eventLog){
                Console.WriteLine(s);
            }
            Commands.eventLog.Clear();
        }

        public void printLine(){
            Console.WriteLine("------------------------------------------------------------------");
        }

        public void printUI(){
            Console.Write("Is "+players[current].name+"'s ");
            if(turn/2==0)Console.Write("1st.");
            else if(turn/2==1)Console.Write("2nd.");
            else if(turn/2==2)Console.Write("3rd.");
            else Console.Write("{0}th.",turn/2+1);
            Console.WriteLine(" turn");

            Utils.print("Life",ConsoleColor.Red);
            Console.WriteLine(": {0}",players[current].life);

            Utils.print("Mana",ConsoleColor.Cyan);
            Console.WriteLine(": {0}",players[current].mana);
            
            Console.Write("Opponent´s ");
            Utils.print("life",ConsoleColor.Red);
            Console.WriteLine(": {0}",players[opponent].life);

            Console.WriteLine();
            players[current].showHand();
            Console.WriteLine();
            players[current].showField();
            Console.WriteLine();
            players[opponent].showField();
            Console.WriteLine();
        }

        public bool attackMenu(){
            int currentIndex=Utils.inputInt("Select the index of the card in your field",1,7)-1;
            
            if(players[current].field[currentIndex]==null || !players[current].field[currentIndex].canAttack) return false;
            
            int opponentIndex=Utils.inputInt("Select the index of the card in your opponents's field or 8 for direct attack",1,8)-1;
            
            Card attacker=players[current].field[currentIndex].Clone();
            
            if(opponentIndex==7){
                attackOpponent(attacker,currentIndex);
                return false;
            }
            else if(players[opponent].field[opponentIndex]==null)return false;

            Card defender=players[opponent].field[opponentIndex].Clone();

            attackEnemy(attacker,defender,currentIndex,opponentIndex);
            
            return false;
        }
        public void attackOpponent(Card attacker,int currentIndex){
            players[current].field[currentIndex].canAttack=false;
            Commands.affectOpponentLife(-attacker.attack);
            attacker.raiseEffects("attacks",current,currentIndex,attacker.name);
           
        }
        public void attackEnemy(Card attacker, Card defender,int currentIndex,int opponentIndex){
            Commands.affectEnemyLife(opponentIndex+1,-attacker.attack);
            Commands.affectAlliedLife(currentIndex+1,-defender.attack);
            attacker.raiseEffects("attacks",current,currentIndex,attacker.name);
            defender.raiseEffects("attacked",opponent,opponentIndex,defender.name);

            if(players[opponent].field[opponentIndex]==null){
                defender.raiseEffects("destroyed",opponent,opponentIndex,defender.name);
            }
            if(players[current].field[currentIndex]==null){
                attacker.raiseEffects("destroyed",current,currentIndex,attacker.name);
            }
            else {
                players[current].field[currentIndex].canAttack=false;
            }
        }

        public void autoPlay()
        {
            autoPlace();
            autoAttack();
        }

        public void autoPlace()
        {  
            while(true){
                Console.Clear();
                printLogs();
                printLine();
                printUI();

                Thread.Sleep(1000);

                if(players[current].field[6]!=null){
                    break;
                }
                
                players[current].hand.Sort();
                players[current].hand.Reverse();
                bool flag=false;

                for(int i=0 ; i<players[current].hand.Count ; i++){
                    if(players[current].hand[i].cost<=players[current].mana){
                        players[current].placeFromHand(i);
                        flag=true;
                        break;
                    }
                }

                if(flag==false){
                    return;
                }
            }
        }
        public void autoAttack(){
            printAllUI();

            List<Tuple<Card,int>> myCards=new List<Tuple<Card,int>> ();

            int totalAttack=0;

            for(int i=0 ; i<7 ; i++)
            {
                if(players[current].field[i]!=null){
                    Card card=players[current].field[i];

                    if(card.canAttack==true){
                        myCards.Add(Tuple.Create(card,i));
                        totalAttack+=card.attack;
                    }
                }
            }

            myCards.Sort(delegate(Tuple <Card ,int > x , Tuple<Card,int> y)
            {
                if(x.Item1.attack >y.Item1.attack){
                    return 1;
                }
                else if(x.Item1.attack<y.Item1.attack){
                    return -1;
                }
                else{
                    return 0;
                }
            });

            if(totalAttack>=players[opponent].life){
                for(int i=0 ; i<myCards.Count ; i++){
                    attackOpponent(myCards[i].Item1,myCards[i].Item2);
                }
            }

            List<Tuple<Card,int>> opponentCards=new List<Tuple<Card,int>> ();

            for(int i=0 ; i<7 ; i++)
            {
                if(players[opponent].field[i]!=null){
                    opponentCards.Add(Tuple.Create(players[opponent].field[i],i));
                }
            }

            opponentCards.Sort(delegate(Tuple <Card ,int > x , Tuple<Card,int> y)
            {
                if(x.Item1.life>y.Item1.life){
                    return 1;
                }
                else if(x.Item1.life<y.Item1.life){
                    return -1;
                }
                else{
                    return 0;
                }
            });

            

            while(true){
                Thread.Sleep(1000);
                if(myCards.Count==0){
                    break;
                }

                if(opponentCards.Count==0){
                    attackOpponent(myCards[0].Item1,myCards[0].Item2);
                    myCards.RemoveAt(0);
                    continue;
                }

                attackEnemy(myCards[0].Item1,opponentCards[0].Item1,myCards[0].Item2,opponentCards[0].Item2);

                if(players[opponent].field[opponentCards[0].Item2]==null){
                    opponentCards.RemoveAt(0);
                }
                myCards.RemoveAt(0);
            }
            
        }
    }
}