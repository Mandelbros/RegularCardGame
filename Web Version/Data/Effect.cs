namespace RegularCardGame.Data
{
    // Card Effect
    public class Effect
    {
        public string when { get; set; } // moment of the match in which the effect is activated
        public string conditions { get; set; } // conditions (binary expression) for the effect to be activated
        public string actions { get; set; } // actions (code) of the effect

        // class constructor
        public Effect(string when, string conditions, string actions){
            this.when=when;
            this.conditions=conditions;
            this.actions=actions;
        }

        // empty class constructor
        public Effect(){
            this.when="";
            this.conditions="";
            this.actions="";
        }

        // raises the effect
        public void raise(){
            Lexer lexer=new Lexer(actions);
            Parser parser=new Parser(lexer);
            Game.match.interpreter.parser=parser;
            Game.match.interpreter.interpret();
        }

        // raises the effect if the conditions are met
        public void tryRaise(){
            if(satisfiesConditions())raise();
        }

        // returns whether the conditions are met
        public bool satisfiesConditions(){
            Lexer lexer=new Lexer(conditions);
            Parser parser=new Parser(lexer);
            AST node=parser.booleanExpr();
            return Game.match.interpreter.visit(node)==1;
        }
    }
}