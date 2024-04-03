namespace Game
{
    public class Effect
    {
        public string when { get; set;}
        public string conditions { get; set;}
        public string actions { get; set;}

        public Effect(string when, string conditions, string actions){
            this.when=when;
            this.conditions=conditions;
            this.actions=actions;
        }

        public Effect(){
            this.when="";
            this.conditions="";
            this.actions="";
        }

        public void raise(){
            Lexer lexer=new Lexer(actions);
            Parser parser=new Parser(lexer);
            Game.match.interpreter.parser=parser;
            Game.match.interpreter.interpret();
        }

        public void tryRaise(){
            if(satisfiesConditions())raise();
        }

        public bool satisfiesConditions(){
            Lexer lexer=new Lexer(conditions);
            Parser parser=new Parser(lexer);
            AST node=parser.booleanExpr();
            return Game.match.interpreter.visit(node)==1;
        }
    }
}