using System;
namespace YoYo
{
    public class Locals
    {
        Locals parent;
        public Symbol[] names;
        Object[] values;

        static Symbol[] noNames = new Symbol[0];
        static Object[] empty = new Object[0];

        public static object Undefined = new object();

        public Locals()
        {
            parent = null;
            names = noNames;
            values = empty;
        }

        public Locals(Locals l)
        {
            parent = l;
            names = noNames;
            values = empty;
        }

        public Locals(Symbol[] argnames, object[] values)
        {
            parent = null;
            this.names = argnames;
            this.values = values;
        }

        public void defineVars(Symbol[] s)
        {
            Symbol[] newNames = new Symbol[names.Length + s.Length];
            Object[] newValues = new object[values.Length + s.Length];
            Array.Copy(names, newNames, names.Length);
            Array.Copy(values, newValues, values.Length);
            Array.Copy(s, 0, newNames, names.Length, s.Length);
            for(int i = 0; i < s.Length; i++)
            {
                values[i + names.Length] = Undefined;
            }
            names = newNames;
            values = newValues;
        }

        public void DefineLocals(Symbol[] s)
        {
            names = s;
            values = new Object[s.Length];
        }

        public int DefineVar(Symbol s)
        {
            Symbol[] newnames = new Symbol[names.Length + 1];
            Object[] newvalues = new Object[values.Length + 1];
            Array.Copy(names, newnames, names.Length);
            Array.Copy(values, newvalues, values.Length);
            newnames[names.Length] = s;
            newvalues[values.Length] = Undefined;
            names = newnames;
            values = newvalues;
            return names.Length - 1; // index of new var
        }

        public void DefineValue(Symbol s, object o)
        {
            Symbol[] newnames = new Symbol[names.Length + 1];
            Object[] newvalues = new Object[values.Length + 1];
            Array.Copy(names, newnames, names.Length);
            Array.Copy(values, newvalues, values.Length);
            newnames[names.Length] = s;
            newvalues[values.Length] = o;
            names = newnames;
            values = newvalues;
        }

        public void SetValue(Symbol s, Object o)
        {
            for(int i = 0; i < names.Length; i++)
            {
                if (names[i] == s)
                {
                    values[i] = o;
                    return;
                }
            }
            DefineValue(s, o);
        }

        public void SetValue(LocalRef l, Object o)
        {
            values[l.num] = o;
        }

        public Object LookupValue(Symbol s)
        {
            for (int i = 0; i < names.Length; i++)
            {
                if (names[i] == s)
                {
                    if (values[i] == Undefined) return null;
                    else return values[i];
                }
            }
            return null;
        }

        public object LookupValue(LocalRef l)
        {
            if (values[l.num] == Undefined)
            {
                return null;
            }
            else return values[l.num];
        }

        public void UndefineVar(LocalRef l)
        {
            values[l.num] = Undefined;
        }

        public void UndefineLocals()
        {
            names = new Symbol[0];
            values = new Object[0];
        }

        public Boolean boundp(Symbol s)
        {
            for (int i = 0; i < names.Length; i++)
            {
                if (names[i] == s)
                {
                    if (values[i] == Undefined) return false;
                    else return true;
                }
            }
            return false;
        }

        public Boolean boundp(LocalRef l)
        {
            if (values[l.num] != Undefined) return true;
            return false;
        }

        public int Index(Symbol s)
        {
            for (int i = 0; i < names.Length; i++)
            {
                if (names[i] == s)
                {
                    return i;
                }
            }
            return -1;
        }



    }
}
