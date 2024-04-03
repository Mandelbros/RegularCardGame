namespace RegularCardGame.Data
{
    // Language Token
    public class Token{
        public string type; // type
        public string value; // value

        // class constructor
        public Token(string type,string value){
            this.type=type;
            this.value=value;
        }

        // returns corresponding color in the IDE
        public string color(){
            if(type=="WHILE" || type=="IF" || (type=="FUNCTION" && value=="BREAK"))return "var(--keywordColor)";
            if(type=="INTEGER")return "var(--intColor)";
            if(type==",")return "var(--delimiterColor)";
            if(type=="PLUS" || type=="MINUS" || type=="MUL"
            || type=="DIV" || type=="REMAINDER")return "var(--opColor)";
            if(type=="EQUAL" || type=="DIFFER" || type=="NOT" || type=="AND" || type=="OR" 
            || type=="GREATEREQUAL" || type=="LESSEREQUAL" || type=="GREATER" || type=="LESSER")return "var(--binOpColor)";
            if(type=="(" || type==")")return "var(--delimiterColor)";
            if(type=="{" || type=="}")return "var(--delimiterColor)";
            if(type=="ASSIGN")return "var(--opColor)";
            if(type=="SEMI")return "var(--delimiterColor)";
            if(type=="STRING")return "var(--stringColor)";
            if(type=="\"")return "var(--stringColor)";
            if(type=="ID")return "var(--varColor)";
            if(type=="FUNCTION")return "var(--functionColor)";
            return "white";
        }
    }
}