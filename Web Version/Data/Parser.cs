namespace RegularCardGame.Data
{
    // Language Parser
    public class Parser
    {
        public Lexer lexer; // lexer
        public Token currentToken; // current readed token
        public string curError; // current error

        // class constructor
        public Parser(Lexer lexer){
            this.lexer=lexer;
            this.currentToken=lexer.getNextToken();
            this.curError="";
        }

        // error
        public void error(){
            // Console.WriteLine("Invalid syntax");
            // Console.WriteLine("Current token type: "+currentToken.type);
            // Console.WriteLine("Current token value: "+currentToken.value);
            // Console.WriteLine("Code:");
            // Console.WriteLine(lexer.text.Substring(0,lexer.pos));
            // Console.WriteLine("-----------------------------ERROR-----------------------------");
            // Console.WriteLine(lexer.text.Substring(lexer.pos));
            // System.Environment.Exit(0);
            curError="Invalid syntax\n";
            curError+="Current token type: "+currentToken.type+"\n";
            curError+="Current token value: "+currentToken.value+"\n";
            curError+="Code:\n";
            curError+=(lexer.text.Substring(0,lexer.pos))+"\n";
            curError+="-----------------------------ERROR-----------------------------\n";
            curError+=(lexer.text.Substring(lexer.pos))+"\n";
            throw new Exception(curError);
        }

        // expects to read and dispose a token of a given type
        public void eat(string tokenType){
            try{
                if(currentToken.type==tokenType){
                    currentToken=lexer.getNextToken();
                }
                else{
                    curError="Invalid syntax: ";
                    curError+="Current token type: "+currentToken.type+". ";
                    curError+="Expected token type: "+tokenType+". ";
                    curError+="Code:\n";
                    curError+=(lexer.text.Substring(0,lexer.pos))+"\n";
                    curError+="-----------------------------ERROR-----------------------------\n";
                    curError+=(lexer.text.Substring(lexer.pos))+"\n";
                    throw new Exception(curError);
                }
            }
            catch(Exception e){
                throw;
            }
        }
        
        // expects to read a mathematical factor
        public AST factor(){
            try{
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
            catch(Exception e){
                throw;
            }
        }

        // expects to read a mathematical term
        public AST term(){
            try{
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
            catch(Exception e){
                throw;
            }
        }

        // expects to read a mathematical expression
        public AST expr(){
            try{
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
            catch(Exception e){
                throw;
            }
        }
        
        // expects to read a boolean term
        public AST booleanTerm(){
            try{
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
            catch(Exception e){
                throw;
            }
        }

        // expects to read a boolean expression
        public AST booleanExpr(){
            try{
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
            catch(Exception e){
                throw;
            }
        }

        // returns an empty node
        public NoOp empty(){
            try{
                return new NoOp();
            }
            catch(Exception e){
                throw;
            }
        }

        // expects to read a variable
        public Var variable(){
            try{
                Var node=new Var(currentToken);
                eat("ID");
                return node;
            }
            catch(Exception e){
                throw;
            }
        }

        // expects to read an assignment statement
        public Assign assignmentStatement(){
            try{
                Var left=variable();
                Token token=currentToken;
                eat("ASSIGN");
                AST right=expr();
                Assign node=new Assign(left,token,right);
                return node;
            }
            catch(Exception e){
                throw;
            }
        }

        // expects to read a conditional statement
        public Conditional conditionalStatement(){
            try{
                string type=currentToken.type;
                if(type=="WHILE")eat("WHILE");
                else eat("IF");
                eat("(");
                AST condition=booleanExpr();
                eat(")");
                Compound body=compoundStatement();
                return new Conditional(type,condition,body);
            }
            catch(Exception e){
                throw;
            }
        }

        // expects to read a function statement
        public Function functionStatement(string name){
            try{
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
            catch(Exception e){
                throw;
            }
        }

        // expects to read a valid statement
        public AST statement(){
            try{
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
                if(currentToken.type=="}"){
                    return empty();
                }
                error(); // not a valid statement
                return new NoOp();
            }
            catch(Exception e){
                throw;
            }
        }

        // expects to read a list of statements
        public List<AST> statementList(){
            try{
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
            catch(Exception e){
                throw;
            }
        }

        // expects to read a block of statements
        public Compound compoundStatement(){
            try{
                eat("{");
                List<AST> nodes=statementList();
                eat("}");

                Compound root=new Compound();
                foreach(AST node in nodes){
                    root.children.Add(node);
                }

                return root;
            }
            catch(Exception e){
                throw;
            }
        }

        // expects to read a valid program
        public AST program(){
            try{
                AST node=compoundStatement();
                return node;
            }
            catch(Exception e){
                throw e;
            }
        }

        // expects to parse the code passed to the lexer
        public AST parse(){
            try{
                AST node=program();
                if(currentToken.type!="EOF"){
                    error();
                }
                return node;
            }
            catch(Exception e){
                AST node=new NoOp();
                return node;
            }
        }
    }
}