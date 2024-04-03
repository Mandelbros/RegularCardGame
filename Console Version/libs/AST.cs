namespace Game{
    public abstract class AST{
        public abstract void print(string height);
    }

    public class BinOp:AST{
        public AST left;
        public Token op;
        public AST right;
        public BinOp(AST left,Token op,AST right){
            this.left=left;
            this.op=op;
            this.right=right;
        }

        public void error(){
            Console.WriteLine("Non recognized binary operator");
            System.Environment.Exit(0);
        }

        public override void print(string height){
            Console.WriteLine(height+"-Binary Operator:");
            Console.WriteLine(height+"Left Operand:");
            left.print(height+'\t');
            Console.WriteLine(height+"Operator: "+op.type+" "+op.value);
            Console.WriteLine(height+"Right Operand:");
            right.print(height+'\t');
        }
    }

    public class UnaryOp:AST{
        public Token op;
        public AST expr;

        public UnaryOp(Token op,AST expr){
            this.op=op;
            this.expr=expr;
        }

        public void error(){
            Console.WriteLine("Non recognized unary operator");
            System.Environment.Exit(0);
        }

        public override void print(string height){
            Console.WriteLine(height+"-Unary Operator:");
            Console.WriteLine(height+"Operator: "+op.type+" "+op.value);
            Console.WriteLine(height+"Operand:");
            expr.print(height+'\t');
        }
    }

    public class Num:AST{
        public Token token;
        public int value;
        public Num(Token token){
            this.token=token;
            this.value=Convert.ToInt32(token.value);
        }

        public override void print(string height){
            Console.WriteLine(height+"-Number:");
            Console.WriteLine(height+"Token: "+token.type+" "+token.value);
            Console.WriteLine(height+"Value: "+Convert.ToString(value));
        }
    }

    public class String:AST{
        public string text;
        public String(string text){
            this.text=text;
        }

        public override void print(string height){
            Console.WriteLine(height+"-String:");
            Console.WriteLine(height+"Text: "+text);
        }
    }

    public class Compound:AST{
        public List<AST> children;
        public Compound(){
            this.children=new List<AST>();
        }

        public override void print(string height){
            Console.WriteLine(height+"-Compound:");
            Console.WriteLine(height+"Children:");
            foreach(AST child in children){
                child.print(height+'\t');
            }
        }
    }

    public class Conditional:AST{
        public string type;
        public Compound body;
        public AST condition;
        public Conditional(string type,AST condition,Compound body){
            this.type=type;
            this.body=body;
            this.condition=condition;
        }

        public override void print(string height){
            Console.WriteLine(height+"-"+type);
            Console.WriteLine(height+"Condition:");
            condition.print(height+'\t');
            Console.WriteLine(height+"Body:");
            foreach(AST child in body.children){
                child.print(height+'\t');
            }
        }
    }

    public class Function:AST{
        public string name;
        public List<AST> args;
        public Function(string name, List<AST> args){
            this.name=name;
            this.args=args;
        }

        public override void print(string height){
            Console.WriteLine(height+"-Function '"+name+"':");
            Console.WriteLine(height+"Arguments:");
            foreach(AST child in args){
                child.print(height+'\t');
            }
        }
    }

    public class Assign:AST{
        public Var left;
        public Token op;
        public Token token;
        public AST right;
        public Assign(Var left, Token op, AST right){
            this.left=left;
            this.token=this.op=op;
            this.right=right;
        }

        public override void print(string height){
            Console.WriteLine(height+"-Assignment:");
            Console.WriteLine(height+"Variable:");
            left.print(height+'\t');
            Console.WriteLine(height+"Operator: "+op.type+" "+op.value);
            Console.WriteLine(height+"Value:");
            right.print(height+'\t');
        }
    }

    public class Var:AST{
        public Token token;
        public string value;
        public Var(Token token){
            this.token=token;
            this.value=token.value;
        }

        public override void print(string height){
            Console.WriteLine(height+"-Variable:");
            Console.WriteLine(height+"Token: "+token.type+" "+token.value);
            Console.WriteLine(height+"Value: "+value);
        }
    }

    public class NoOp:AST{
        public override void print(string height){
            Console.WriteLine(height+"-Empty");
        }
    }
}