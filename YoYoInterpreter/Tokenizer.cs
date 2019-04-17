using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace YoYo
{
    public class Tokenizer
    {
        StringStream inStream;

        public Tokenizer(String input)
        {
            inStream = new StringStream(input);
        }

        public List<object> Tokenize()
        {
            List<object> v = new List<object>();
            while (true)
            {
                if (inStream.Empty()) return v;
                String o = GetNextToken();
                //System.out.println("finished getNextToken");
                if (o != null) v.Add(o);
            }
        }

        public Boolean balancedp(String open, String close)
        {
            int level = 0;
            while (!inStream.Empty()) {
                String o = GetNextToken();
                if (o == null) break;
                //System.out.println("finished getNextToken");
                if (o.Equals(open)) level++;
                if (o.Equals(close)) level--;
            }
            return (level == 0);
        }

        public int EatWhiteSpace()
        {
            //System.out.println("eat white space");
            int t = inStream.Read();
            while (true)
            {
                if (!(Whitespace(t) || Comment(t))) break;
                while (Whitespace(t))
                {
                    if (inStream.Empty()) return -1;
                    t = inStream.Read();
                }
                //System.out.println("look for comment");     
                if (Comment(t))
                {
                    while (true)
                    {
                        //System.out.println("parsing comment");
                        if (inStream.Empty()) return -1;
                        t = inStream.Read();
                        if (t == '\n' || t == '\r')
                        {
                            if (inStream.Empty()) return -1;
                            t = inStream.Peek();
                            if (t == '\n' || t == '\r') continue; //dumbass /n/r and \r\n!
                            t = inStream.Read();
                            break;
                        }
                    }
                }
            }
            //System.out.println("done eating whitespace, not comment nor whitespace");
            return t;
        }

        public String GetNextToken()
        {
            //System.out.println("get next token");
            StringBuilder sb = new StringBuilder();

            int t = EatWhiteSpace();
            //System.out.println("getNextToken after whitespace: " + (int)t);
            if (t == int.MaxValue || t < 0) return null;
            //System.out.println("didn't return, t !< 0");
            sb.Append((char)t);
            if (IsNumber(t)) return GetNextNumber(t);
            if (Delimiter(t)) return sb.ToString();
            if (IsString(t)) return GetNextString();

            while (true)
            {
                if (inStream.Empty()) return sb.ToString();
                int p = inStream.Peek();
                if (Delimiter(p) || IsString(p) ||
                Whitespace(p) || Comment(p)) return sb.ToString();
                int c = inStream.Read();
                if (Escapeint(c)) c = ReadEscape();
                if (c == int.MaxValue || c < 0) return null;
                sb.Append((char)c);
            }
        }

        public String GetNextNumber(int item)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((char)item);
            while (true)
            {
                if (inStream.Empty()) break;
                int p = inStream.Peek();
                if (Delimiternotdot(p) || IsString(p) ||
                Whitespace(p) || Comment(p)) break;
                int c = inStream.Read();
                sb.Append((char)c);
            }
            return sb.ToString();
        }


        public String GetNextString()
        {
            Boolean lispstring = false;
            StringBuilder sb = new StringBuilder();
            sb.Append('"');

            if (inStream.Empty()) { sb.Append('"'); return sb.ToString(); }
            int c = inStream.Read();
            if (c == '"') { sb.Append('"'); return sb.ToString(); }
            if (c == '|')
            {
                lispstring = true;
            }
            else
            {
                if (Escapeint(c)) c = ReadEscape();
                if (c == Int32.MaxValue || c < 0) { sb.Append('"'); return sb.ToString(); }
                sb.Append((char)c);
            }
            while (true)
            {
                if (inStream.Empty()) { sb.Append('"'); return sb.ToString(); }
                c = inStream.Read();
                if (c == '"' || (lispstring && c == '|'))
                { sb.Append('"'); return sb.ToString(); }
                if (Escapeint(c)) c = ReadEscape();
                if (c == Int32.MaxValue || c < 0) { sb.Append('"'); return sb.ToString(); }
                sb.Append((char)c);
            }
        }

        public Boolean Number(int item)
        {
            return (((item >= '0') && (item <= '9')));
        }

        public Boolean IsNumber(int item)
        {
            Boolean num = Number(item);
            if (num) return true;
            int p = inStream.Peek();
            Boolean next = (Number(p) && (item == '.' || item == '-'));
            if (next) return true;
            int pp = inStream.PeekPeek();
            return ((item == '-') && (p == '.') && Number(pp));
        }

        public Boolean IsString (int item) {
    return (item == '"');
  }

    public int ReadEscape()
    {
        if (inStream.Empty()) return Int32.MaxValue;
        int p = inStream.Read();
        switch (p)
        {
            case 'n': return '\n';
            case 't': return '\t';
            case 'b': return '\b';
            case 'r': return '\r';
            case 'f': return '\f';
            case '\\': return '\\';
            case '\'': return '\'';
            case '\"': return '\"';
        }
        return Int32.MaxValue;
    }


    public Boolean Escapeint(int item)
    {
        return (item == '\\');
    }

    public Boolean Delimiter(int item)
    {
        return ((item == '[') || (item == ']') ||
            (item == '(') || (item == ')') ||
            (item == ':') || (item == '.'));
    }

    public Boolean Delimiternotdot(int item)
    {
        return ((item == '[') || (item == ']') ||
            (item == '(') || (item == ')') ||
            (item == ':'));
    }

    public Boolean Whitespace(int item)
    {
        return ((item == ' ') || (item == '\n') ||
            (item == '\r') || (item == '\t'));
    }

    public Boolean Comment(int item)
    {
        return (item == ';');
    }
}

}

