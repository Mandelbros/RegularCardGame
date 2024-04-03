namespace RegularCardGame.Data
{
    // Language Lexer
    public class Lexer{
        public string text; // code
        public int pos; // current index in the text
        public char currentChar; // current char
        public Dictionary<string,Token> reservedKeywords; // reserved keywords
        
        // class contructor
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

        // reads an ID token
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

        // invalid character error
        public void error(){
            Console.WriteLine("Invalid character '"+currentChar+"' at position "+Convert.ToInt32(pos));
            Console.WriteLine("Code:");
            Console.WriteLine(text);
            System.Environment.Exit(0);
        }

        // returns the next character without increasing the current index
        public char peek(){
            int peekPos=pos+1;
            if(peekPos>=text.Length){
                return '\0';
            }
            return text[peekPos];
        }

        // increases the current index and updates the current char
        public void advance(){
            pos++;
            if(pos>=text.Length){
                currentChar='\0';
            }
            else{
                currentChar=text[pos];
            }
        }

        // skips all next whitespaces, tabulations, endLines and null characters
        public void skipWhitespace(){
            while(currentChar!='\0' && (currentChar==' ' || currentChar=='\n' || currentChar=='\t')){
                advance();
            }
        }

        // reads an integer
        public string integer(){
            string result="";
            while(currentChar!='\0' && Utils.isDigit(currentChar)){ // while current character is a digit
                result+=currentChar;
                advance();
            }
            return result;
        }

        // reads a string
        public string _string(){
            string result="";
            while(currentChar!='\0' && currentChar!='\"'){
                result+=currentChar;
                advance();
            }
            return result;
        }

        // reads the next token
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
                // error();
                return new Token("ERROR",Convert.ToString(pos)+currentChar);
            }
            
            Token eof=new Token("EOF","");
            return eof;
        }
    }
}