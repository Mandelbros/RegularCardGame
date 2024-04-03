namespace Game
{
    public class Lexer{
        public string text;
        public int pos;
        public char currentChar;
        public Dictionary<string,Token> reservedKeywords;
        
        public Lexer(string text){
            this.text=text;
            this.pos=0;
            this.currentChar=text[0];
            this.reservedKeywords=new Dictionary<string,Token>();
            reservedKeywords["while"]=new Token("WHILE","WHILE");
            reservedKeywords["if"]=new Token("IF","IF");
            foreach(string name in Commands.functionNames){
                reservedKeywords[name]=new Token("FUNCTION",name);
            }
        }

        public Token _id(){
            string result="";
            while(currentChar!='\0' && Utils.isAlnum(currentChar)){
                result+=currentChar;
                advance();
            }
            Token token;
            if(!reservedKeywords.ContainsKey(result)){
                reservedKeywords[result]=new Token("ID",result);
            }
            token=reservedKeywords[result];
            return token;
        }

        public void error(){
            Console.WriteLine("Invalid character '"+currentChar+"' at position "+Convert.ToInt32(pos));
            Console.WriteLine("Code:");
            Console.WriteLine(text);
            System.Environment.Exit(0);
        }

        public char peek(){
            int peekPos=pos+1;
            if(peekPos>=text.Length){
                return '\0';
            }
            return text[peekPos];
        }

        public void advance(){
            pos++;
            if(pos>=text.Length){
                currentChar='\0';
            }
            else{
                currentChar=text[pos];
            }
        }

        public void skipWhitespace(){
            while(currentChar!='\0' && (currentChar==' ' || currentChar=='\n' || currentChar=='\t')){
                advance();
            }
        }

        public string integer(){
            string result="";
            while(currentChar!='\0' && Utils.isDigit(currentChar)){
                result+=currentChar;
                advance();
            }
            return result;
        }

        public string _string(){
            string result="";
            while(currentChar!='\0' && currentChar!='\"'){
                result+=currentChar;
                advance();
            }
            return result;
        }

        public Token getNextToken(){
            while(currentChar!='\0'){
                if(currentChar==' ' || currentChar=='\n' || currentChar=='\t'){
                    skipWhitespace();
                    continue;
                }
                if(Utils.isDigit(currentChar)){
                    Token token=new Token("INTEGER",integer());
                    return token;
                }
                if(currentChar==','){
                    advance();
                    Token token=new Token(",",",");
                    return token;
                }
                if(currentChar=='+'){
                    advance();
                    Token token=new Token("PLUS","+");
                    return token;
                }
                if(currentChar=='-'){
                    advance();
                    Token token=new Token("MINUS","-");
                    return token;
                }
                if(currentChar=='*'){
                    advance();
                    Token token=new Token("MUL","*");
                    return token;
                }
                if(currentChar=='/'){
                    advance();
                    Token token=new Token("DIV","/");
                    return token;
                }
                if(currentChar=='%'){
                    advance();
                    Token token=new Token("REMAINDER","%");
                    return token;
                }
                if(currentChar=='=' && peek()=='='){
                    advance();
                    advance();
                    Token token=new Token("EQUAL","==");
                    return token;
                }
                if(currentChar=='!' && peek()=='='){
                    advance();
                    advance();
                    Token token=new Token("DIFFER","!=");
                    return token;
                }
                if(currentChar=='!'){
                    advance();
                    Token token=new Token("NOT","!");
                    return token;
                }
                if(currentChar=='&' && peek()=='&'){
                    advance();
                    advance();
                    Token token=new Token("AND","&&");
                    return token;
                }
                if(currentChar=='|' && peek()=='|'){
                    advance();
                    advance();
                    Token token=new Token("OR","||");
                    return token;
                }
                if(currentChar=='>' && peek()=='='){
                    advance();
                    advance();
                    Token token=new Token("GREATEREQUAL",">=");
                    return token;
                }
                if(currentChar=='<' && peek()=='='){
                    advance();
                    advance();
                    Token token=new Token("LESSEREQUAL","<=");
                    return token;
                }
                if(currentChar=='>'){
                    advance();
                    Token token=new Token("GREATER",">");
                    return token;
                }
                if(currentChar=='<'){
                    advance();
                    Token token=new Token("LESSER","<");
                    return token;
                }
                if(currentChar=='('){
                    advance();
                    Token token=new Token("(","(");
                    return token;
                }
                if(currentChar==')'){
                    advance();
                    Token token=new Token(")",")");
                    return token;
                }
                if(currentChar=='{'){
                    advance();
                    Token token=new Token("{","{");
                    return token;
                }
                if(currentChar=='}'){
                    advance();
                    Token token=new Token("}","}");
                    return token;
                }
                if(Utils.isAlpha(currentChar)){
                    return _id();
                }
                if(currentChar=='='){
                    advance();
                    Token token=new Token("ASSIGN","=");
                    return token;
                }
                if(currentChar==';'){
                    advance();
                    Token token=new Token("SEMI",";");
                    return token;
                }
                if(currentChar=='.'){
                    advance();
                    Token token=new Token("DOT",".");
                    return token;
                }
                if(currentChar=='\"'){
                    advance();
                    Token token=new Token("STRING",_string());
                    advance();
                    return token;
                }
                error();
            }
            
            Token eof=new Token("EOF","");
            return eof;
        }
    }
}