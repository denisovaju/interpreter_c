using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class LexicalAnalysis
    {
        private string value1, value2, operation;

        internal const string INTEGER = "INTEGER";
        internal const string PLUS = "PLUS";
        internal const string MINUS = "MINUS";

        internal const string ADD = "+";
        internal const string SUBTRACTION = "-";

        public LexicalAnalysis()
        {
            value1 = null;
            value2 = null;
            operation = null;
        }

        public Dictionary<string, string> token = new Dictionary<string, string>();

        private string characters;
        private string lexema = "0123456789+-";

        public List<char> error = new List<char>();

        public string Lexema
        {
            get { return lexema; }
        }

        public string Characters
        {
            get { return characters; }
            set
            {
                foreach (char j in value)
                {
                    for (int i = 0; i < Lexema.Length; i++)
                    {
                        if (j == Lexema[i])
                        {
                            break;
                        }
                        else if (j != Lexema[i] && i == Lexema.Length - 1)
                        {
                            error.Add(j);
                            break;
                        }
                    }
                }
                if (error.Count == 0)
                {
                    characters = value;
                    //Console.WriteLine("O-la-la");
                }
                else
                {
                    characters = "0";
                    for (int i = 0; i < error.Count; i++)
                    {
                        Console.WriteLine("Error symbols: " + error[i]);
                    }
                    Console.WriteLine("Input Mistake!\n");
                }
            }
        }
        
        public Dictionary<string, string> get_next_token(string s)
        {
            int counter = 0;
            int index = 0;
            int c = 0;

            for (int i = 0; i < s.Length; i++ )
            {
                if (s.Contains(ADD) && counter != 1)
                {
                    index = s.IndexOf(ADD);
                    string temp = s.Remove(0, index+1);
                    if (temp.Contains(ADD))
                    {
                        break;
                    }
                    else
                    {
                        counter = 1;
                        continue;
                    }
                }
                else if (s.Contains(SUBTRACTION) && c != 1)
                {
                    index = s.IndexOf(SUBTRACTION);

                    string temp = s.Remove(0, index + 1);
                    if (temp.Contains(SUBTRACTION))
                    {
                        break;
                    }
                    else
                    {
                        c = 1;
                        continue;
                    }
                }
                else
                {
                    continue;
                }
            }
            if ((counter == 1 ^ c == 1))
            {
                if (((!s.StartsWith(ADD)) && (!s.EndsWith(ADD))) && ((!s.StartsWith(SUBTRACTION)) && (!s.EndsWith(SUBTRACTION))))
                {
                    value1 = s.Substring(0, index);
                    value2 = s.Remove(0, index + 1);
                    operation = s.Substring(index, 1);

                    token.Add(value1, INTEGER);
                    token.Add(value2, INTEGER);

                    if (counter == 1)
                    {
                        token.Add(operation, PLUS);
                    }
                    else if (c == 1)
                    {
                        token.Add(operation, MINUS);
                    }
                    return token;
                }
                else
                {
                    Console.WriteLine("Sorry, but you don't write '+' or '-' at the start or end");
                    return token;
                }
            }
            else if(counter != 0)
            {
                Console.WriteLine("Sorry, but you don't write '+' or '-' more one ");
                return token;
            }
            else
            {
                Console.WriteLine("Write '+' or '-'");
                return token;
            }
        }

        /*public void show(Dictionary<string, string> dic)
        {
            ICollection<string> keys = dic.Keys;
            foreach (string j in keys)
            {
                Console.WriteLine(j);
                Console.WriteLine(dic[j]);
            }
        }*/
    }

    class Parser : LexicalAnalysis
    {
        int a, b, result;

        public Parser()
        {
            a = 0;
            b = 0;
            result = 0;
        }

        public int number(Dictionary<string, string> dic)
        {
            int counter = 0;
            ICollection<string> keys = dic.Keys;
            foreach (string j in keys)
            {
                if (dic[j] == INTEGER)
                {
                    if (counter == 0)
                    {
                        a = Convert.ToInt32(j);
                        counter++;
                    }
                    else if (counter == 1)
                    {
                        b = Convert.ToInt32(j);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
            }
            if (counter == 1)
            {
                foreach (string j in keys)
                {
                    if (dic[j] == PLUS)
                    {
                        result = a + b;
                        break;
                    }
                    else if (dic[j] == MINUS)
                    {
                        result = a - b;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                return result;
            }
            else
            {
                Console.WriteLine("Don't consider expression");
                return 404;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                LexicalAnalysis lexema = new LexicalAnalysis();
                Console.WriteLine("Enter your expression: " + lexema.Lexema);
                lexema.Characters = Console.ReadLine();

                Dictionary<string, string> tokena = new Dictionary<string, string>();

                tokena = lexema.get_next_token(lexema.Characters);
                //lexema.show(tokena);

                Parser p = new Parser();
                int x = p.number(tokena);
                Console.WriteLine("Result: " + x);
                //Console.ReadKey();
            }
        }
    }
}
