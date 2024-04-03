using System.Text.Json; 
using System;
using System.Threading;
using System.Timers;

namespace RegularCardGame.Data
{
    // Game Match between two players
    public class Match
    {
        public int turn; // current turn number
        public int current; // current player's id
        public int opponent; // next player's id
        public Player[] players; // matched players
        public Interpreter interpreter; // interpreter
        public int attackerIndex; // index of the current attacking card
        public bool[] bot; // true if autoplay, false otherwise

        // class constructor
        public Match(Player[] players,bool[] bot){
            this.turn=0;
            this.current=0;
            this.opponent=0;
            this.attackerIndex=-1;
            this.players=players;
            Lexer lexer=new Lexer("{}");
            Parser parser=new Parser(lexer);
            this.interpreter=new Interpreter(parser);
            this.bot=bot;
        }

        // ends current turn and starts next
        public void endTurn(){
            attackerIndex=-1;
            // allows player's cards to attack
            for(int i=0;i<7;i++){
                if(players[current].field[i]!=null)players[current].field[i].canAttack=true;
            }

            // raise all the when-turnEnd effects of cards in the field
            for(int i=0;i<7;i++){
                if(players[current].field[i]!=null)players[current].field[i].raiseEffects("turnEnd",current,i,players[current].field[i].name);
                if(players[opponent].field[i]!=null)players[opponent].field[i].raiseEffects("turnEnd",opponent,i,players[opponent].field[i].name);
            }

            // store events in the log
            Commands.eventLog.Add(players[current].name+"'s turn ends.");
            Commands.eventLog.Add(players[opponent].name+"'s turn starts.");

            // update turn count, current and next player's ids
            turn++;
            current=turn%2;
            opponent=(current+1)%2;

            // update player's mana points and make it draw a card
            players[current].mana=Math.Min(10,((turn/2)+1));
            players[current].draw();

            // raise all the when-turnBegin effects of cards in the field
            for(int i=0;i<7;i++){
                if(players[current].field[i]!=null)players[current].field[i].raiseEffects("turnBegin",current,i,players[current].field[i].name);
                if(players[opponent].field[i]!=null)players[opponent].field[i].raiseEffects("turnBegin",opponent,i,players[opponent].field[i].name);
            }

            // if current player is a bot, autoplay
            if(bot[current])autoPlay();
        }

        // places a card at a given index of the hand
        public void placeCard(int id,int index){
            if(id==current){
                players[id].placeFromHand(index);
            }
        }

        // starts the match
        public void start(){
            // store event in the log
            Commands.eventLog.Add("'The match starts.");

            // initialize current and next player
            current=turn%2;
            opponent=(current+1)%2;

            // store event in the log
            Commands.eventLog.Add(players[current].name+"'s turn starts.");

            // initialize first player's mana points and make it draw a card
            players[current].mana=Math.Min(10,((turn/2)+1));
            players[current].draw();

            // raise all the when-turnBegin effects of cards in the field
            for(int i=0;i<7;i++){
                if(players[current].field[i]!=null)players[current].field[i].raiseEffects("turnBegin",current,i,players[current].field[i].name);
                if(players[opponent].field[i]!=null)players[opponent].field[i].raiseEffects("turnBegin",opponent,i,players[opponent].field[i].name);
            }

            // if first player is a bot, autoplay
            if(bot[current])autoPlay();
        }

        // sets the current attacker/defender
        public void attacking(int id,int index){
            // if id corresponds to current player and index is valid and card can attack, update attackerIndex
            if(id==current && index<7 && players[current].field[index].canAttack){
                attackerIndex=index;
            }
            // if id corresponds to the opponent and there is an attacker
            if(id==opponent && attackerIndex!=-1){
                // set attacker
                Card attacker=players[current].field[attackerIndex].Clone();
                // direct attack
                if(index==7){
                    attackOpponent(attacker,attackerIndex);
                }
                // attack an enemy
                else{
                    // set attacked card
                    Card defender=players[opponent].field[index].Clone();
                    attackEnemy(attacker,defender,attackerIndex,index);
                }
                attackerIndex=-1; // attack is over
            }
        }

        // direct attack
        public void attackOpponent(Card attacker,int currentIndex){
            players[current].field[currentIndex].canAttack=false;
            Commands.activatorHeroId=current;
            Commands.activatorOpponentId=opponent;
            Commands.affectOpponentLife(-attacker.attack);
            // raise attacker's when-attacks effects
            attacker.raiseEffects("attacks",current,currentIndex,attacker.name);
        }
        
        // attack an enemy
        public void attackEnemy(Card attacker, Card defender,int currentIndex,int opponentIndex){
            Commands.activatorHeroId=current;
            Commands.activatorOpponentId=opponent;
            Commands.affectEnemyLife(opponentIndex+1,-attacker.attack);
            Commands.affectAlliedLife(currentIndex+1,-defender.attack);

            // raise attacker's when-attacks effects
            attacker.raiseEffects("attacks",current,currentIndex,attacker.name);
            // raise defender's when-attacked effects
            defender.raiseEffects("attacked",opponent,opponentIndex,defender.name);

            // if the enemy card was destroyed, raise it's when-destroyed effects
            if(players[opponent].field[opponentIndex]==null){
                defender.raiseEffects("destroyed",opponent,opponentIndex,defender.name);
            }
            // if the attacking card was destroyed, raise it's when-destroyed effects
            if(players[current].field[currentIndex]==null){
                attacker.raiseEffects("destroyed",current,currentIndex,attacker.name);
            }
            else {
                players[current].field[currentIndex].canAttack=false; // card cannot attack again in the turn
            }
        }

        // virtual player auto-play
        public void autoPlay(){
            Thread.Sleep(1000);
            autoPlace();
            autoAttack();
            endTurn();
        }

        // virtual player auto-place
        public void autoPlace(){  
            // while it has enough mana and cards to place
            while(true){
                // Thread.Sleep(1000);
                int cardsInField=0;

                for(int i=0 ; i<7 ; i++)
                {
                    if(players[current].field[i]!=null){
                        cardsInField++;
                    }
                }

                if(cardsInField==7){
                    return;
                }
                
                
                // sort the hand's cards
                players[current].hand.Sort();
                players[current].hand.Reverse();

                // place cards with cost not greater than current mana points
                bool flag=false; // to check if at least a card was placed
                for(int i=0 ; i<players[current].hand.Count ; i++){
                    if(players[current].hand[i].cost<=players[current].mana){
                        players[current].placeFromHand(i);
                        flag=true;
                        break;
                    }
                }

                // if no card was placed, end
                if(flag==false){
                    return;
                }
            }
        }

        // virtual player auto-attack
        public void autoAttack(){
            List<Tuple<Card,int>> myCards=new List<Tuple<Card,int>> ();

            int totalAttack=0;

            // store friendly placed cards that can attack and find their attack points sum
            for(int i=0 ; i<7 ; i++){
                if(players[current].field[i]!=null){
                    Card card=players[current].field[i];

                    if(card.canAttack==true){
                        myCards.Add(Tuple.Create(card,i));
                        totalAttack+=card.attack;
                    }
                }
            }

            // sort cards based on attack points
            myCards.Sort(delegate(Tuple <Card ,int > x , Tuple<Card,int> y){
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

            // if the sum of the attack points is not lesser than opponent's health points
            if(totalAttack>=players[opponent].life){
                // attack directly with all the cards
                for(int i=0 ; i<myCards.Count ; i++){
                    attackOpponent(myCards[i].Item1,myCards[i].Item2);
                }
            }

            // store opponent's placed cards
            List<Tuple<Card,int>> opponentCards=new List<Tuple<Card,int>> (); 
            for(int i=0 ; i<7 ; i++){
                if(players[opponent].field[i]!=null){
                    opponentCards.Add(Tuple.Create(players[opponent].field[i],i));
                }
            }

            // sort opponent's placed cards based on remaining health points
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

            // while valid attacks are posible
            while(true){
                // Thread.Sleep(1000);
                if(myCards.Count==0){
                    break;
                }

                // opponent has no placed cards left, attack directly with remaining cards
                if(opponentCards.Count==0){
                    attackOpponent(myCards[0].Item1,myCards[0].Item2);
                    myCards.RemoveAt(0);
                    continue;
                }

                // attack enemies
                attackEnemy(myCards[0].Item1,opponentCards[0].Item1,myCards[0].Item2,opponentCards[0].Item2);
                // enemy was destroyed, remove it
                if(players[opponent].field[opponentCards[0].Item2]==null){
                    opponentCards.RemoveAt(0);
                }
                myCards.RemoveAt(0); // card cannot attack again in the turn
            }
        }
    }
}