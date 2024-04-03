namespace Game
{
    public static class Utils{
        public static bool isDigit(char c){
            return (c>='0' && c<='9');
        }

        public static bool isAlpha(char c){
            return ((c>='a' && c<='z') || (c>='A' && c<='Z') || c=='_');
        }

        public static bool isAlnum(char c){
            return (isDigit(c)||isAlpha(c));
        }

        public static bool intToBool(int a){
            if(a==0)return false;
            return true;
        }

        public static int boolToInt(bool a){
            if(a)return 1;
            return 0;
        }

        public static bool isNumber(string s){
            if(s.Length==0)return false;
            for(int i=0;i<s.Length;i++){
                if(s[i]<'0' || s[i]>'9'){
                    if(i!=0 || s[i]!='-')return false;
                }
            }
            return true;
        }

        public static void fancyPrint(int l,string s){
            Console.Write(s);
            for(int i=0;i<l-s.Length;i++)Console.Write(" ");
        }

        public static void fancyPrint(int l,string s,ConsoleColor c){
            Console.ForegroundColor=c;
            fancyPrint(l,s);
            Console.ForegroundColor=ConsoleColor.Gray;
        }

        public static void print(string s,ConsoleColor c){
            Console.ForegroundColor=c;
            Console.Write(s);
            Console.ForegroundColor=ConsoleColor.Gray;
        }

        public static int inputInt(){
            while(true){
                string s=Console.ReadLine();
                if(Utils.isNumber(s)){
                    if(s[0]=='0' && s.Length>1){
                        Console.WriteLine("Not valid: the number starts with '0'.");
                        continue;
                    }
                    if(s.Length>9){
                        Console.WriteLine("Not valid: the number has too many digits.");
                        continue;
                    }
                    return Convert.ToInt32(s);
                }
                else Console.WriteLine("Not valid: that is not a number.");
            }
            return 0;
        }

        public static int inputInt(string text){
            while(true){
                Console.Write(text);
                string s=Console.ReadLine();
                if(Utils.isNumber(s)){
                    if(s[0]=='0' && s.Length>1){
                        Console.WriteLine("Not valid: the number starts with '0'.");
                        continue;
                    }
                    if(s.Length>9){
                        Console.WriteLine("Not valid: the number has too many digits.");
                        continue;
                    }
                    return Convert.ToInt32(s);
                }
                else Console.WriteLine("Not valid: that is not a number.");
            }
            return 0;
        }

        public static int inputInt(int l,int r){
            while(true){
                string s=Console.ReadLine();
                if(Utils.isNumber(s)){
                    if(s[0]=='0' && s.Length>1){
                        Console.WriteLine("Not valid: the number starts with '0'.");
                        continue;
                    }
                    if(s.Length>9){
                        Console.WriteLine("Not valid: the number has too many digits.");
                        continue;
                    }
                    int i=Convert.ToInt32(s);
                    if(i<l || i>r){
                        Console.WriteLine("The number must be between {0} and {1}",l,r);
                        continue;
                    }
                    return i;
                }
                else Console.WriteLine("Not valid: that is not a number.");
            }
            return 0;
        }

        public static int inputInt(string text,int l,int r){
            while(true){
                Console.Write(text);
                Console.Write("({0},...,{1}): ",l,r);
                string s=Console.ReadLine();
                if(Utils.isNumber(s)){
                    if(s[0]=='0' && s.Length>1){
                        Console.WriteLine("Not valid: the number starts with '0'.");
                        continue;
                    }
                    if(s.Length>9){
                        Console.WriteLine("Not valid: the number has too many digits.");
                        continue;
                    }
                    int i=Convert.ToInt32(s);
                    if(i<l || i>r){
                        Console.WriteLine("The number must be between {0} and {1}",l,r);
                        continue;
                    }
                    return i;
                }
                else Console.WriteLine("Not valid: that is not a number.");
            }
            return 0;
        }
    }
}