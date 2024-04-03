namespace RegularCardGame.Data
{
    // Interpreter of the language
    public class Interpreter
    {
        public Parser parser; // parser of the code
        public Dictionary<string,int> GLOBAL_SCOPE; // global variables

        // class constructor
        public Interpreter(Parser parser){
            this.parser=parser;
            this.GLOBAL_SCOPE=new Dictionary<string,int>();
        }

        // error
        public void error(){
            Console.WriteLine("Operator not found");
            System.Environment.Exit(0);
        }

        // generic node visitor
        public int visit(AST node){
            // call the corresponding method based on the type of node
            if(node.GetType()==typeof(BinOp)) return visitBinOp((BinOp)node);
            if(node.GetType()==typeof(UnaryOp)) return visitUnaryOp((UnaryOp)node);
            if(node.GetType()==typeof(Num)) return visitNum((Num)node);
            if(node.GetType()==typeof(String)) return visitString((String)node);
            if(node.GetType()==typeof(Compound)) return visitCompound((Compound)node);
            if(node.GetType()==typeof(Assign)) return visitAssign((Assign)node);
            if(node.GetType()==typeof(Var)) return visitVar((Var)node);
            if(node.GetType()==typeof(NoOp)) return visitNoOp((NoOp)node);
            if(node.GetType()==typeof(Conditional)) return visitConditional((Conditional)node);
            if(node.GetType()==typeof(Function)) return visitFunction((Function)node);
            error(); // node type not found
            return 0;
        }

        // Binary Operation node visitor
        public int visitBinOp(BinOp node){
            if(node.op.type=="PLUS"){
                return visit(node.left)+visit(node.right);
            }
            if(node.op.type=="MINUS"){
                return visit(node.left)-visit(node.right);
            }
            if(node.op.type=="MUL"){
                return visit(node.left)*visit(node.right);
            }
            if(node.op.type=="DIV"){
                return visit(node.left)/visit(node.right);
            }
            if(node.op.type=="REMAINDER"){
                return visit(node.left)%visit(node.right);
            }
            if(node.op.type=="AND"){
                return Utils.boolToInt(Utils.intToBool(visit(node.left))&Utils.intToBool(visit(node.right)));
            }
            if(node.op.type=="OR"){
                return Utils.boolToInt(Utils.intToBool(visit(node.left))|Utils.intToBool(visit(node.right)));
            }
            if(node.op.type=="EQUAL"){
                return Utils.boolToInt(visit(node.left)==visit(node.right));
            }
            if(node.op.type=="DIFFER"){
                return Utils.boolToInt(visit(node.left)!=visit(node.right));
            }
            if(node.op.type=="GREATEREQUAL"){
                return Utils.boolToInt(visit(node.left)>=visit(node.right));
            }
            if(node.op.type=="LESSEREQUAL"){
                return Utils.boolToInt(visit(node.left)<=visit(node.right));
            }
            if(node.op.type=="GREATER"){
                return Utils.boolToInt(visit(node.left)>visit(node.right));
            }
            if(node.op.type=="LESSER"){
                return Utils.boolToInt(visit(node.left)<visit(node.right));
            }
            return 0;
        }

        // Unary Operation node visitor
        public int visitUnaryOp(UnaryOp node){
            if(node.op.type=="PLUS"){
                return +visit(node.expr);
            }
            if(node.op.type=="MINUS"){
                return -visit(node.expr);
            }
            if(node.op.type=="NOT"){
                if(visit(node.expr)==1)return 0;
                if(visit(node.expr)==0)return 1;
                error();
                return 0;
            }
            return 0;
        }

        // Number node visitor
        public int visitNum(Num node){
            return node.value;
        }

        // String node visitor
        public int visitString(String node){
            return 0;
        }

        // Compound node visitor
        public int visitCompound(Compound node){
            // visit all the statements until a break();
            foreach(AST child in node.children){
                int v=visit(child);
                if((child.GetType()==typeof(Compound) && v==1) 
                || (child.GetType()==typeof(Conditional) && v==1)
                || (child.GetType()==typeof(Function) && ((Function)child).name=="break") )return 1;
            }
            return 0;
        }

        // Assignement node visitor
        public int visitAssign(Assign node){
            string varName=node.left.value;
            GLOBAL_SCOPE[varName]=visit(node.right);
            return 0;
        }

        // Variable node visitor
        public int visitVar(Var node){
            string varName=node.value;
            if(GLOBAL_SCOPE.ContainsKey(varName)){
                return GLOBAL_SCOPE[varName];
            }
            // declaration error
            Console.WriteLine("Variable '"+varName+"' not declared");
            System.Environment.Exit(0);
            return 0;
        }

        // Empty node visitor
        public int visitNoOp(NoOp node){
            return 0;
        }

        // Conditional node visitor
        public int visitConditional(Conditional node){
            if(node.type=="IF"){
                // if node confitions are met, visit statements
                if(Utils.intToBool(visit(node.condition)))return visit(node.body);
                return 0;
            }
            if(node.type=="WHILE"){
                // while node confitions are met, visit statements
                while(Utils.intToBool(visit(node.condition))){
                    if(visit(node.body)==1)break;
                }
                return 0;
            }
            error(); // non-recognized conditional statement
            return 0;
        }

        // Function node visitor
        public int visitFunction(Function node){
            List<int> args=new List<int>();
            if(node.name=="print")Commands.printedText=((String)node.args[0]).text;
            foreach(AST arg in node.args){
                args.Add(visit(arg));
            }
            return Commands.activate(node.name,args);
        }

        // visit the parsed tree
        public int interpret(){
            AST tree=parser.parse();
            return visit(tree);
        }
    }
}