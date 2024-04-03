namespace RegularCardGame.Data
{
    // Abstract Syntax Tree (generic node of the tree)
    public abstract class AST{
        // prints this node and its descendants with a given starting indentation
        public abstract void print(string indentation);
        // returns a list of the tokens in the order of the code
        public abstract List<Token> tokenize();
    }

    // Binary operation (left[AST] op[Token +,-,*,/,%,&&,||] right[AST])
    public class BinOp:AST{
        public AST left; // left operand
        public Token op; // binary operator
        public AST right; // right operand

        // class constructor
        public BinOp(AST left,Token op,AST right){
            this.left=left;
            this.op=op;
            this.right=right;
        }

        public override void print(string indentation){
            Console.WriteLine(indentation+"-Binary Operator:");
            Console.WriteLine(indentation+"Left Operand:");
            left.print(indentation+'\t');
            Console.WriteLine(indentation+"Operator: "+op.type+" "+op.value);
            Console.WriteLine(indentation+"Right Operand:");
            right.print(indentation+'\t');
        }

        public override List<Token> tokenize(){
            List<Token> ret=new List<Token>();
            ret.AddRange(left.tokenize());
            ret.Add(op);
            ret.AddRange(right.tokenize());
            return ret;
        }
    }

    // Unary operation (op[Token +,-,!] expr[AST])
    public class UnaryOp:AST{
        public Token op; // unary operator
        public AST expr; // operand

        // class constructor
        public UnaryOp(Token op,AST expr){
            this.op=op;
            this.expr=expr;
        }
        
        public override void print(string indentation){
            Console.WriteLine(indentation+"-Unary Operator:");
            Console.WriteLine(indentation+"Operator: "+op.type+" "+op.value);
            Console.WriteLine(indentation+"Operand:");
            expr.print(indentation+'\t');
        }

        public override List<Token> tokenize(){
            List<Token> ret=new List<Token>();
            ret.Add(op);
            ret.AddRange(expr.tokenize());
            return ret;
        }
    }

    // Numeric value
    public class Num:AST{
        public Token token; // integer token
        public int value; // integer value

        // class constructor
        public Num(Token token){
            this.token=token;
            this.value=Convert.ToInt32(token.value);
        }

        public override void print(string indentation){
            Console.WriteLine(indentation+"-Number:");
            Console.WriteLine(indentation+"Token: "+token.type+" "+token.value);
            Console.WriteLine(indentation+"Value: "+Convert.ToString(value));
        }

        public override List<Token> tokenize(){
            List<Token> ret=new List<Token>();
            ret.Add(token);
            return ret;
        }
    }

    // String of characters ("text")
    public class String:AST{
        public string text; // text

        // class constructor
        public String(string text){
            this.text=text;
        }

        public override void print(string indentation){
            Console.WriteLine(indentation+"-String:");
            Console.WriteLine(indentation+"Text: "+text);
        }

        public override List<Token> tokenize(){
            List<Token> ret=new List<Token>();
            ret.Add(new Token("\"","\""));
            ret.Add(new Token("STRING",text));
            ret.Add(new Token("\"","\""));
            return ret;
        }
    }

    // Block of code ( { [list of statements] } )
    public class Compound:AST{
        public List<AST> children; // statements

        // class constructor
        public Compound(){
            this.children=new List<AST>();
        }

        public override void print(string indentation){
            Console.WriteLine(indentation+"-Compound:");
            Console.WriteLine(indentation+"Children:");
            foreach(AST child in children){
                child.print(indentation+'\t');
            }
        }
        
        public override List<Token> tokenize(){
            List<Token> ret=new List<Token>();
            ret.Add(new Token("ENDL","\n"));
            ret.Add(new Token("{","{"));
            ret.Add(new Token("ENDL","\n"));
            foreach(AST child in children){
                ret.AddRange(child.tokenize());
                if(child.GetType()==typeof(Function))ret.Add(new Token("SEMI",";"));
                ret.Add(new Token("ENDL","\n"));
            }
            ret.Add(new Token("}","}"));
            ret.Add(new Token("ENDL","\n"));
            return ret;
        }
    }

    // if/while statement ( if/while(binary expression){ statement(s) } )
    public class Conditional:AST{
        public string type; // if/while
        public Compound body; // statements
        public AST condition; // binary expression to assert

        // class constructor
        public Conditional(string type,AST condition,Compound body){
            this.type=type;
            this.body=body;
            this.condition=condition;
        }

        public override void print(string indentation){
            Console.WriteLine(indentation+"-"+type);
            Console.WriteLine(indentation+"Condition:");
            condition.print(indentation+'\t');
            Console.WriteLine(indentation+"Body:");
            foreach(AST child in body.children){
                child.print(indentation+'\t');
            }
        }
        
        public override List<Token> tokenize(){
            List<Token> ret=new List<Token>();
            ret.Add(new Token(type,type));
            ret.Add(new Token("(","("));
            ret.AddRange(condition.tokenize());
            ret.Add(new Token(")",")"));
            ret.AddRange(body.tokenize());
            return ret;
        }
    }

    // Language Command ( commandName( argument(s) ) )
    public class Function:AST{
        public string name; // commandName
        public List<AST> args; // arguments of the command

        // class constructor
        public Function(string name, List<AST> args){
            this.name=name;
            this.args=args;
        }

        public override void print(string indentation){
            Console.WriteLine(indentation+"-Function '"+name+"':");
            Console.WriteLine(indentation+"Arguments:");
            foreach(AST child in args){
                child.print(indentation+'\t');
            }
        }
        
        public override List<Token> tokenize(){
            List<Token> ret=new List<Token>();
            ret.Add(new Token("FUNCTION",name));
            ret.Add(new Token("(","("));
            bool comma=false;
            foreach(AST arg in args){
                if(comma)ret.Add(new Token(",",","));
                else comma=true;
                ret.AddRange(arg.tokenize());
            }
            ret.Add(new Token(")",")"));
            return ret;
        }
    }

    // Assignement statement ( variableName = expression; ) 
    public class Assign:AST{
        public Var left; // variable
        public Token op; // operator (=)
        public Token token;
        public AST right; // expression

        // class constructor
        public Assign(Var left, Token op, AST right){
            this.left=left;
            this.token=this.op=op;
            this.right=right;
        }

        public override void print(string indentation){
            Console.WriteLine(indentation+"-Assignment:");
            Console.WriteLine(indentation+"Variable:");
            left.print(indentation+'\t');
            Console.WriteLine(indentation+"Operator: "+op.type+" "+op.value);
            Console.WriteLine(indentation+"Value:");
            right.print(indentation+'\t');
        }
        
        public override List<Token> tokenize(){
            List<Token> ret=new List<Token>();
            ret.Add(left.token);
            ret.Add(op);
            ret.AddRange(right.tokenize());
            ret.Add(new Token("SEMI",";"));
            return ret;
        }
    }

    // Variable (integer)
    public class Var:AST{
        public Token token; // token (ID,name)
        public string value; // integer value

        // class constructor
        public Var(Token token){
            this.token=token;
            this.value=token.value;
        }

        public override void print(string indentation){
            Console.WriteLine(indentation+"-Variable:");
            Console.WriteLine(indentation+"Token: "+token.type+" "+token.value);
            Console.WriteLine(indentation+"Value: "+value);
        }
        
        public override List<Token> tokenize(){
            List<Token> ret=new List<Token>();
            ret.Add(token);
            return ret;
        }
    }

    // Empty node
    public class NoOp:AST{
        public override void print(string indentation){
            Console.WriteLine(indentation+"-Empty");
        }
        
        public override List<Token> tokenize(){
            List<Token> ret=new List<Token>();
            return ret;
        }
    }
}