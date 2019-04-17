using System;
using System.Collections.Generic;

namespace YoYo
{
    public class Symbol : Sym
    {
        private static Dictionary<string,Symbol> hash = new Dictionary<string,Symbol>();
        public string Name
        {
            get;
            set;
        }

        public Primitive primitive = null;
        object value = null;


        public Symbol(String name)
        {
            this.Name = name;
            hash.Add(name.ToLower(), this);
        }

        public static Symbol lookup(String n) 
        {
            if (!hash.TryGetValue(n.ToLower(), out Symbol s))
            {
                return new Symbol(n);
            }
            else
            {
                return s;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
