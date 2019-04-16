using System;
using System.Collections.Generic;

namespace YoYo
{
    public static class Reader
    {
        public static Object[] Read(String s)
        {
            Tokenizer tokenizer = new Tokenizer(s);
            List<object> v = tokenizer.Tokenize();
            return ReadUntil(v, "");
        }

        static Object[] ReadUntil(List<object> v, String end)
        {
            List<object> result = new List<object>();
            while (v.Count > 0)
            {
                if (Peek(v).Equals(end))
                {
                    Skip(v);
                    break;
                }
                result.Add(ReadTokenOrCompound(v));
            }
            return result.ToArray();
        }
       
        static Object ReadTokenOrCompound(List<object> v)
        {
            object result = ReadToken(v);
            if (Peek(v).Equals("."))
            {
                return GatherParts(v, result);
            }
            return result;
        } 

        static Object ReadToken(List<object> v)
        {
            if (Peek(v).Equals("["))
            {
                Skip(v);
                return ReadUntil(v, "]");
            }
            if (Peek(v).Equals("("))
            {
                Skip(v);
                return new InstructionList(ReadUntil(v, ")"));
            }
            return ReadString(Next(v));
        }

        static CompoundSymbol GatherParts(List<object> v, object first)
        {
            List<object> result = new List<object>();
            result.Add(first);
            while(true)
            {
                if (!Peek(v).Equals(".")) return new CompoundSymbol(result);
                Skip(v);
                result.Add(ReadToken(v));
            }
        }

        static Object ReadString(String s)
        {
            if (s[0] == '\"')
            {
                return s.Substring(1, s.Length - 1);
            }
            if (s[0] == '-' && s.Length == 1)
            {
                return Symbol.lookup("-");
            }

            if (s[0] == '$' && s.Length > 1)
            {
                if (Int64.TryParse(s.Substring(1, s.Length), System.Globalization.NumberStyles.HexNumber, null, out Int64 result))
                {
                    return result;
                }
            }
            

            if (PossibleNumberp(s))
            {
                if (Int64.TryParse(s, out Int64 result))
                {
                    return result;
                }
                if (Double.TryParse(s, out Double dResult))
                {
                    return dResult;
                }
            }
            return Symbol.lookup(s);
        }

        static Boolean PossibleNumberp(String s)
        {
            foreach(char c in s)
            {
                if ("eE.+-0123456789".IndexOf(c) == -1) return false;
            }
            return true;
        }

        static String Next(List<object> v)
        {
            if (v.Count == 0)
            {
                return "";
            }
            Object o = v[0];
            v.RemoveAt(0);
            return o as String;
        }

        static String Peek(List<object> v)
        {
            if (v.Count == 0)
            {
                return "";
            }
            Object o = v[0];
            return o as String;
        }

        static void Skip(List<object> v)
        {
            v.RemoveAt(0);
        }

    }
}
