namespace RegularCardGame.Data
{
    public static class Utils
    {
        // returns whether if given character is a digit ('0'...'9')
        public static bool isDigit(char c){
            return (c>='0' && c<='9');
        }

        // returns whether if given character is a letter ('a'...'z' || 'A'...'Z' || '_')
        public static bool isAlpha(char c){
            return ((c>='a' && c<='z') || (c>='A' && c<='Z') || c=='_');
        }

        // returns whether if given character is alphanumeric ('a'...'z' || 'A'...'Z' || '_' || '0'...'9')
        public static bool isAlnum(char c){
            return (isDigit(c)||isAlpha(c));
        }

        // converts given integer to boolean (0=>false,!0=>true)
        public static bool intToBool(int a){
            if(a==0)return false;
            return true;
        }

        // converts given boolean to integer (false=>0,true=>1)
        public static int boolToInt(bool a){
            if(a)return 1;
            return 0;
        }

        // returns whether if given string is a number
        public static bool isNumber(string s){
            if(s.Length==0)return false;
            for(int i=0;i<s.Length;i++){
                if(s[i]<'0' || s[i]>'9'){
                    if(i!=0 || s[i]!='-')return false;
                }
            }
            return true;
        }

        // prints given string and fills remaining l-s.Length with whitespaces
        public static void fancyPrint(int l,string s){
            Console.Write(s);
            for(int i=0;i<l-s.Length;i++)Console.Write(" ");
        }

        // prints given string (in color) and fills remaining l-s.Length with whitespaces
        public static void fancyPrint(int l,string s,ConsoleColor c){
            Console.ForegroundColor=c;
            fancyPrint(l,s);
            Console.ForegroundColor=ConsoleColor.Gray;
        }

        // prints given string (in color)
        public static void print(string s,ConsoleColor c){
            Console.ForegroundColor=c;
            Console.Write(s);
            Console.ForegroundColor=ConsoleColor.Gray;
        }

        // requests user-input until valid integer
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

        // requests user-input until valid integer with given custom request message
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

        // requests user-input until valid integer (in a given range l...r)
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

        // requests user-input until valid integer (in a given range l...r) with given custom request message
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