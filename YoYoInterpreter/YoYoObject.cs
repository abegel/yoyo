using System;
using System.Collections.Generic;

namespace YoYo
{
    public class YoYoObject : Dictionary<Symbol, Object>
    {
        static int numObjects = 1;
        public int num
        {
            get;
            set;
        }

        public static Symbol kindSym = Symbol.lookup("kind");
        public static Symbol runSym = Symbol.lookup("run");
        public static Symbol parentSym = Symbol.lookup("parent");

        public YoYoObject()
        {
            num = numObjects++;
        }

        public YoYoObject(YoYoObject l)
        {
            Add(parentSym, new WeakReference(l));
            foreach(var keyVal in l)
            {
                Add(keyVal.Key, keyVal.Value);
            }
            num = numObjects++;
        }

        public YoYoObject(Context c)
        {
            num = numObjects++;
            Add(runSym, c.GetGlobalObject().lookupValue(runSym));
        }

        public void defineValue(Symbol s, Object o)
        {
            Add(s, o);
        }

        public void setValue(Symbol s, Object o)
        {
            Add(s, o);
        }

        public object lookupValue(Symbol s)
        {
            if (TryGetValue(s, out object value))
            {
                return value;
            } 
            else
            {
                return null;
            }
        }

        public void undefineValue(Symbol s)
        {
            Remove(s);
        }

        public Boolean boundp(Symbol s)
        {
            return ContainsKey(s);
        }

        public string getKind()
        {
            if (TryGetValue(kindSym, out object value))
            {
                return value.ToString();
            } else
            {
                return null;
            }
        }



    }
}
