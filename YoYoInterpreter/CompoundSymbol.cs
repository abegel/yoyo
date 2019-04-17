using System;
using System.Collections.Generic;
using System.Text;

namespace YoYo
{
    public class CompoundSymbol : Sym, ExprList
    {
        public Object[] parts
        {
            get;
            set;
        }

        public CompoundSymbol()
        {
            this.parts = new Object[0];
        }

        public CompoundSymbol(List<object> v)
        {
            parts = v.ToArray();
        }

        public Boolean Empty()
        {
            return parts.Length == 0;
        }

        public void SetPrev(Object o)
        {
            parts[0] = o;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < parts.Length - 1; i++)
            {
                sb.Append(YoYo.PrintToString(parts[i]));
                sb.Append(".");
            }
            sb.Append(YoYo.PrintToString(parts[parts.Length - 1]));
            return sb.ToString();
        }

    }
}
