using System;
using System.Collections.Generic;

namespace YoYo
{
    public class Ufun : Function
    {

        static object[] EmptyArgList = new object[0];
        static List<Symbol> tracedFunctions = new List<Symbol>();
        static Symbol anon = Symbol.lookup("anonymous");


        Symbol Name;
        Symbol[] ArgList;
        int Nargs;
        String bodyString;
        object[] body;
        YoYoObject ufunEnv;
        List<int> lineNumbers;
        Boolean trace = false;


        public Ufun(Symbol[] arglist, Symbol[] locals, Object[] body, String bodyString, YoYoObject ufun) : this(null, arglist, locals, body, bodyString, ufun)
        { 
        }

        public Ufun(Symbol name, Symbol[] arglist, Symbol[] locals, Object[] body, String bodyString, YoYoObject ufun)
        {
            this.body = body;
            this.Name = name;
            this.bodyString = bodyString;
            this.Nargs = arglist.Length;
            this.ArgList = new Symbol[arglist.Length + locals.Length];
            Array.Copy(arglist, this.ArgList, arglist.Length);
            Array.Copy(locals, 0, this.ArgList, arglist.Length, locals.Length);
            ufunEnv = ufun;
        }

        public object[] GetArgArray(Context c)
        {
            if (ArgList.Length == 0)
            {
                return EmptyArgList;
            } 
            else
            {
                return new object[ArgList.Length];
            }
        }

        public Symbol GetName()
        {
            return Name;
        }

        public int GetNargs()
        {
            return Nargs;
        }



        public void Trace()
        {
            trace = true;
        }

        public void Untrace()
        {
            trace = false;
        }

        public object Run(object[] inputargs, Context c)
        {
            object[] oldilist = c.ilist;
            int oldip = c.ilistposptr;
            Locals oldLocals = c.locals;
            YoYoObject oldufunenv = c.ufunenv;
            Symbol oldufun = c.ufun;
            Ufun oldufunobj = c.ufunObj;
            List<Object> oldsavedynamics = c.savedynamics;

            try
            {
                c.locals = new Locals(ArgList, inputargs);
                c.ufunenv = ufunEnv;
                c.ufun = (Name == null) ? anon : Name;
                if (trace) c.TraceEnter(c.ufun.Name, inputargs);
                c.ufunObj = this;
                c.savedynamics = null;
                YoYo.EvalList(body, c);
                Object value = c.ufunreturn;
                if (trace) c.TraceExit((Name == null) ? anon.Name : Name.Name, value);
                if (value == Context.novalue) return null;
                return value;
            }
            catch (LogoError u)
            {
                if (trace) c.TraceThrow(c.ufun.Name, u);
                throw u;
            } 
            finally
            {
                c.locals = oldLocals;
                c.ufunenv = oldufunenv;
                c.ufunObj = oldufunobj;
                c.ufun = oldufun;
                if (c.savedynamics != null)
                {
                    RestoreDynamics(c.savedynamics, c.objEnv);
                }
                c.savedynamics = oldsavedynamics;
                c.ilist = oldilist;
                c.ilistposptr = oldip;
                c.ufunreturn = null;
            }


        }

        void RestoreDynamics(List<object> bindings, YoYoObject obj)
        {
            for (int i = 0; i < bindings.Count; i++)
            {
                Symbol name = bindings[i] as Symbol;
                Object value = bindings[i + 1];
                if (value != null)
                {
                    obj.setValue(name, value);
                }
                else
                {
                    obj.undefineValue(name);
                }
            }
        }


    }
}
