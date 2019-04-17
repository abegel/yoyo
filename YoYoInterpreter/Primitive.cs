using System;
using System.Collections.Generic;
using System.Reflection;

namespace YoYo
{
    public class Primitive : Function
    {
        static List<Symbol> tracedFuncs = new List<Symbol>();

        String name = "";
        public Symbol symName;
        MethodInfo method;
        int nargs;
        object[] emptyarglist;
        Boolean trace = false;
        Boolean yoyoprimp = true;
        object nativeobj = null;
        YoYoMethods definingClass;


        public Primitive(string name, MethodInfo obj, YoYoMethods definingClass)
        {
            this.name = name;
            symName = Symbol.lookup(name);
            symName.primitive = this;
            method = obj;
            this.definingClass = definingClass;
            this.nargs = obj.GetParameters().Length - 1; // first param is special
            if (this.nargs == 0) emptyarglist = new Object[1];
        }

        public Primitive(string name, MethodInfo obj, Object nativeObj)
        {
            this.name = name;
            if (nativeobj != null)
            {
                this.yoyoprimp = false;
                this.nativeobj = nativeObj;
            }
            symName = Symbol.lookup(name);
            method = obj;
            this.nargs = obj.GetParameters().Length - ((yoyoprimp) ? 1 : 0);
            if (this.nargs == 0) emptyarglist = new object[(yoyoprimp) ? 1 : 0];
        }

        public Primitive(String name, MethodInfo obj, int prec, YoYoMethods definingClass)
        {
            this.name = name;
            symName = Symbol.lookup(name);
            symName.primitive = this;
            method = obj;
            this.definingClass = definingClass;
            this.nargs = prec;
        }

        public Symbol GetName()
        {
            return symName;
        }

        public int GetNargs()
        {
            return nargs;
        }

        public Object[] GetArgArray(Context c)
        {
            if (nargs > 0) return new object[nargs + ((yoyoprimp) ? 1 : 0)];
            else if (nargs == 0)
            {
                return emptyarglist;
            } else
            {
                return new object[3];
            }
        }

        public object Run(Object[] arglist, Context c)
        {
            if (trace) c.TraceEnter(name, arglist);
            YoYoObject oldenv = c.objEnv;

            if (yoyoprimp) arglist[arglist.Length - 1] = c;
            Object value = null;
            try
            {
                if (!yoyoprimp)
                {
                    for(int i = 0; i < arglist.Length; i++)
                    {
                        if (arglist[i] == YoYo.symtrue) arglist[i] = true;
                        else if (arglist[i] == YoYo.symfalse) arglist[i] = false;
                    }
                }
                if (definingClass != null)
                {
                    definingClass.PrePrimCall(this, arglist, c);
                }
                value = method.Invoke((yoyoprimp) ? null : nativeobj, arglist);
           } 
           catch (MethodAccessException)
            {
                LogoError.Error("You forgot to make " + this.ToString() + " public", c);
            }
           catch (TargetInvocationException f)
            {
                Exception g = f.InnerException;
                if (g is LogoError)
                {
                    if (trace) c.TraceThrow(name, g);
                    throw g as LogoError;
                }
                if (g is Stop) throw g as Stop;
                if (trace) c.TraceThrow(name, g);
                c.errOutput.WriteLine(g.StackTrace);
                LogoError.Error(g.ToString(), c);
            } finally
            {
                if (arglist == emptyarglist) { arglist[0] = null; }
                if (trace) c.TraceExit(name, c);
                if (definingClass != null)
                {
                    definingClass.PostPrimCall(this, c);
                }
            }
            return value;
        }

        public void Trace()
        {
            trace = true;
        }

        public void Untrace()
        {
            trace = false;
        }





    }
}
