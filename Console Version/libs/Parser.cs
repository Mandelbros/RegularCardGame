namespace Game{
    public class Parser{
        public Lexer lexer;
        public Token currentToken;

        public Parser(Lexer lexer){
            this.lexer=lexer;
            this.currentToken=lexer.getNextToken();
        }

        public void error(){
            Console.WriteLine("Invalid syntax");
            Console.WriteLine("Current token type: "+currentToken.type);
            Console.WriteLine("Current token value: "+currentToken.value);
            Console.WriteLine("Code:");
            Console.WriteLine(lexer.text.Substring(0,lexer.pos));
            Console.WriteLine("-----------------------------ERROR-----------------------------");
            Console.WriteLine(lexer.text.Substring(lexer.pos));
            System.Environment.Exit(0);
        }

        public void eat(string tokenType){
            // Console.WriteLine(currentToken.type+" "+currentToken.value);
            if(currentToken.type==tokenType){
                currentToken=lexer.getNextToken();
            }
            else{
                Console.WriteLine("Expected '"+tokenType+"', found '"+currentToken.type+"'.");
                error();
            }
        }
        
        public AST factor(){
            Token token=currentToken;
            if(token.type=="PLUS"){
                eat("PLUS");
                UnaryOp node=new UnaryOp(token,factor());
                return node;
            }
            if(token.type=="MINUS"){
                eat("MINUS");
                UnaryOp node=new UnaryOp(token,factor());
                return node;
            }
            if(token.type=="INTEGER"){
                eat("INTEGER");
                Num node=new Num(token);
                return node;
            }
            else if(token.type=="("){
                eat("(");
                AST result=expr();
                eat(")");
                return result;
            }
            else{
                if(currentToken.type=="ID")return variable();
                else return functionStatement(currentToken.value);
            }
        }

        public AST term(){
            AST node=factor();

            while(currentToken.type=="MUL" || currentToken.type=="DIV" || currentToken.type=="REMAINDER"){
                Token token=currentToken;
                if(token.type=="MUL"){
                    eat("MUL");
                }
                else if(token.type=="DIV"){
                    eat("DIV");
                }
                else if(token.type=="REMAINDER"){
                    eat("REMAINDER");
                }

                node=new BinOp(node,token,factor());
            }
            return node;
        }

        public AST expr(){
            AST node=term();

            while(currentToken.type=="PLUS" || currentToken.type=="MINUS"){
                Token token=currentToken;
                if(token.type=="PLUS"){
                    eat("PLUS");
                }
                else if(token.type=="MINUS"){
                    eat("MINUS");
                }
                node=new BinOp(node,token,term());
            }
            return node;
        }

        
        public AST booleanTerm(){
            Token token=currentToken;
            if(token.type=="NOT"){
                eat("NOT");
                eat("(");
                UnaryOp node=new UnaryOp(token,booleanTerm());
                eat(")");
                return node;
            }
            if(token.type=="("){
                eat("(");
                AST result=booleanTerm();
                eat(")");
                return result;
            }
            AST left=expr();
            token=currentToken;
            if(token.type=="EQUAL")eat("EQUAL");
            else if(token.type=="DIFFER")eat("DIFFER");
            else if(token.type=="GREATEREQUAL")eat("GREATEREQUAL");
            else if(token.type=="LESSEREQUAL")eat("LESSEREQUAL");
            else if(token.type=="LESSER")eat("LESSER");
            else if(token.type=="GREATER")eat("GREATER");
            else error();
            
            AST right=expr();
            return new BinOp(left,token,right);
        }

        public AST booleanExpr(){
            AST node=booleanTerm();
            string type=currentToken.type;
            if(type!="AND" && type!="OR")return node;

            while(currentToken.type=="AND" || currentToken.type=="OR"){
                Token token=currentToken;
                if(token.type=="AND"){
                    if(type=="OR")error();
                    eat("AND");
                }
                else if(token.type=="OR"){
                    if(type=="AND")error();
                    eat("OR");
                }
                node=new BinOp(node,token,booleanTerm());
            }
            return node;
        }

        public NoOp empty(){
            return new NoOp();
        }

        public Var variable(){
            Var node=new Var(currentToken);
            eat("ID");
            return node;
        }

        public Assign assignmentStatement(){
            Var left=variable();
            Token token=currentToken;
            eat("ASSIGN");
            AST right=expr();
            Assign node=new Assign(left,token,right);
            return node;
        }

        public Conditional conditionalStatement(){
            string type=currentToken.type;
            if(type=="WHILE")eat("WHILE");
            else eat("IF");
            eat("(");
            AST condition=booleanExpr();
            eat(")");
            Compound body=compoundStatement();
            return new Conditional(type,condition,body);
        }

        public Function functionStatement(string name){
            List<AST> args=new List<AST>();
            eat("FUNCTION");
            eat("(");
            if(name=="print"){
                string text=currentToken.value;
                eat("STRING");
                eat(")");
                String node=new String(text);
                args.Add(node);
                return new Function(name,args);
            }
            while(currentToken.type!="," && currentToken.type!=")"){
                AST currentArg=expr();
                args.Add(currentArg);
                if(currentToken.type==",")eat(",");
                else if(currentToken.type==")"){
                    eat(")");
                    return new Function(name,args);
                }
            }
            eat(")");
            return new Function(name,args);
        }

        public AST statement(){
            if(currentToken.type=="{"){
                return compoundStatement();
            }
            if(currentToken.type=="WHILE" || currentToken.type=="IF"){
                return conditionalStatement();
            }
            if(currentToken.type=="FUNCTION"){
                Function node=functionStatement(currentToken.value);
                eat("SEMI");
                return node;
            }
            if(currentToken.type=="ID"){
                Assign node=assignmentStatement();
                eat("SEMI");
                return node;
            }
            return empty();
        }

        public List<AST> statementList(){
            List<AST> results=new List<AST>();
            results.Add(statement());

            while(currentToken.type!="}"){
                results.Add(statement());
            }

            if(currentToken.type=="ID"){
                error();
            }
            return results;
        }

        public Compound compoundStatement(){
            eat("{");
            List<AST> nodes=statementList();
            eat("}");

            Compound root=new Compound();
            foreach(AST node in nodes){
                root.children.Add(node);
            }

            return root;
        }

        public AST program(){
            AST node=compoundStatement();
            return node;
        }

        public AST parse(){
            AST node=program();
            // node.print("");
            if(currentToken.type!="EOF"){
                error();
            }
            return node;
        }
    }
}