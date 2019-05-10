using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace YoYo
{
    public class YoYo
    {
        public static Symbol symtrue = Symbol.lookup("true");
        public static Symbol symfalse = Symbol.lookup("false");

        public static void InitConstants(YoYoObject globals)
        {
            globals.defineValue(Symbol.lookup("pi"), Math.PI);
            globals.defineValue(Symbol.lookup("min-int"), Int64.MinValue);
            globals.defineValue(Symbol.lookup("max-int"), Int64.MaxValue);
            globals.defineValue(Symbol.lookup("min-num"), Double.MinValue);
            globals.defineValue(Symbol.lookup("max-num"), Double.MaxValue);
            globals.defineValue(YoYo.symtrue, YoYo.symtrue);
            globals.defineValue(YoYo.symfalse, YoYo.symfalse);
            globals.defineValue(Symbol.lookup("system-version"), 4.0);
        }

        public static void EvalExternal(Context c)
        {
            Symbol[] newlocals = PrimFile.ReadLocals(new Symbol[0], new Symbol[0], c.ilist, 0);
            try
            {
                c.locals.DefineLocals(newlocals);
                while (true)
                {
                    object obj = (c.ilistposptr < c.ilist.Length ? c.ilist[c.ilistposptr++] : null);
                    if (obj == null) return;
                    Eval(obj, c);
                }
            }
            catch (LogoError le)
            {
                c.errOutput.WriteLine(le.ToString());
            }
            catch (Stop)
            {
                c.stopNow = false;
            }
            finally
            {
                c.locals.UndefineLocals();
            }
        }

        public static Object EvalList(InstructionList i, Context c)
        {
            return EvalList(i.list, c);
        }

        public static Object EvalList(Object[] o, Context c)
        {
            int oldilistposptr = c.ilistposptr;
            Object[] oldilist = c.ilist;
            int oldprec = c.precedence;
            try
            {
                c.precedence = 0;
                c.ilistposptr = 0;
                c.ilist = o;
                return EvalFromHere(c);
            }
            finally
            {
                c.ilistposptr = oldilistposptr;
                c.ilist = oldilist;
                c.precedence = oldprec;
                if (c.ufunreturn != null)
                {
                    c.ilistSkipToEnd();
                }
            }
        }

        public static Object EvalFromHere(Context c)
        {
            object output = null;
            try
            {
                while (true)
                {
                    object obj = (c.ilistposptr < c.ilist.Length) ? c.ilist[c.ilistposptr++] : null;
                    if (obj == null) return output;
                    output = Eval(obj, c);
                }
            }
            finally
            {
                if (c.ufunreturn != null) {
                    c.ilistSkipToEnd();
                }
            }
        }

        public static Object EvalOne(InstructionList i, Context c)
        {
            return EvalOne(i.list, c);
        }

        public static Object EvalOne(Object[] o, Context c)
        {
            Object[] oldilist = c.ilist;
            int oldilistposptr = c.ilistposptr;
            try
            {
                c.ilist = o;
                c.ilistposptr = 0;
                return EvalOneFromHere(c);
            }
            finally
            {
                c.ilist = oldilist;
                c.ilistposptr = oldilistposptr;
            }
        }

        public static Object EvalOneFromHere(Context c)
        {
            int oldnargs = c.nargs;
            try
            {
                c.nargs = 1;
                object obj = (c.ilistposptr < c.ilist.Length) ? c.ilist[c.ilistposptr++] : null;
                if (obj == null) return null;
                return Eval(obj, c);
            }
            finally
            {
                c.nargs = oldnargs;
            }
        }

        public static void PollStop(Context c)
        {
            if (c.stopNow)
            {
                throw new Stop();
            }
        }

        public static Object Eval(Object o, Context c)
        {
            if (c.stopNow)
            {
                throw new Stop();
            }
            Object output = o;
            if (o is Primitive)
            {
                Primitive p = (Primitive)o;
                output = EvalFunction(p.symName, p, c.objEnv, c);
                if (output != null && c.nargs == 0)
                {
                    LogoError.Error("Too few reporters " + PrintToString(output), c);
                }
            } else if (o is LocalRef)
            {
                output = c.locals.LookupValue(o as LocalRef);
                if (output is Function)
                {
                    output = EvalFunction(((LocalRef)o).name, (Function)output, c.objEnv, c);
                    if (output != null && c.nargs == 0)
                    {
                        LogoError.Error("Too many reporters " + PrintToString(output), c);
                    }
                } else if (output == null)
                {
                    LogoError.Error("I don't know how to " + o, c);
                }
            } else if (o is Sym)
            {
                output = EvalSymbol((Sym)o, c);
            } else if (o is InstructionList)
            {
                output = EvalList((InstructionList)o, c);
            }
            if (output != null)
            {
                return rtv(output, c);
            }
            return null;
        }

        public static Object rtv(Object o, Context c)
        {
            if (o == null)
            {
                return null;
            }
            if (c.nargs == 0)
            {
                LogoError.Error("Too many reporters " + PrintToString(o), c);
            }

            object moo = (c.ilistposptr < c.ilist.Length) ? c.ilist[c.ilistposptr] : null;
            if (moo == null)
            {
                return o;
            }
            Primitive foo = null;
            if (moo is Symbol)
            {
                foo = ((Symbol)moo).primitive;
                if (foo == null) return o;
            }
            else if (moo is Primitive)
            {
                foo = (Primitive)moo;
            }
            else return o;

            int args = foo.GetNargs();
            if (args < 0 && args < c.precedence)
            {
                c.ilistposptr++;
                c.SetPrev(foo);
                return InfixCall(args, o, foo, c);
            }
            return o;
        }

        public static Object LookupSymbol(Sym s, Context c)
        {
            Object o = null;
            if (s is Symbol)
            {
                o = GetSymbolValue(c, (Symbol)s, c);
            }
            else if (s is CompoundSymbol)
            {
                Closure cs = EvalCompound((CompoundSymbol)s, c);
                if (cs.obj is YoYoObject) {
                    o = EvalLast((YoYoObject)cs.obj, cs.last, c);
                } else
                {
                    o = EvalLast(cs.obj, cs.last, c);
                }
            }
            return o;
        }

        public static Object EvalSymbol(Sym s, Context c)
        {
            Object o = null;
            YoYoObject env = c.objEnv;

            if (s is Symbol)
            {
                o = GetSymbolValue(c, (Symbol)s, c);
            }
            else if (s is CompoundSymbol)
            {
                Closure cs = EvalCompound((CompoundSymbol)s, c);
                if (cs.obj is YoYoObject)
                {
                    o = EvalLast((YoYoObject)cs.obj, cs.last, c);
                    env = (YoYoObject)cs.obj;
                }
                else
                {
                    o = EvalLast(cs.obj, cs.last, c);
                }
            }
            if (o == null)
            {
                LogoError.Error("I don't know how to " + s, c);
            }

            if (o is Function)
            {
                object output = EvalFunction((Sym)s, (Function)o, env, c);
                if (output != null)
                {
                    if (c.nargs == 0)
                    {
                        LogoError.Error("Too many reporters in " + PrintToString(output), c);
                    }
                    return output;
                }
                return null;
            }
            return o;
        }

        public static Object GetSymbolValue(ExprList el, Symbol s, Context c)
        {
            Object output = s.primitive;
            if (output != null)
            {
                el.SetPrev(output);
                return output;
            }

            output = c.objEnv.lookupValue(s);
            if (output != null) return output;

            if (c.objEnv != c.ufunenv)
            {
                output = c.ufunenv.lookupValue(s);
                if (output != null) return output;
            }

            output = c.GetGlobalObject().lookupValue(s);

            return output;
        }

        public static object GetNativeValue(Object o, Symbol s, Context c)
        {
            throw new NotImplementedException();
            // is it a field? 
            // is it a method?
        }

        public static object EvalPart(CompoundSymbol s, YoYoObject y, Object p, Context c)
        {
            if (p is LocalRef)
            {
                object value = c.locals.LookupValue((LocalRef)p);
                if (value is Function)
                {
                    if (((Function)value).GetNargs() == 0)
                    {
                        return EvalFunction(((LocalRef)p).name, (Function)value, c.objEnv, c);
                    }
                    else LogoError.Error("Too few reporters in " + ((LocalRef)p).name, c);
                }
                return value;
            }
            if (p is InstructionList)
            {
                p = EvalOne((InstructionList)p, c);
            }
            if (p is YoYoObject)
            {
                return p;
            }
            if (p is String)
            {
                p = Symbol.lookup((String)p);
            }
            if (p is Symbol)
            {
                object value = (y == null) ? GetSymbolValue(s, (Symbol)p, c) : y.lookupValue((Symbol)p);
                if (value is Function)
                {
                    if (((Function)value).GetNargs() == 0)
                    {
                        return EvalFunction((Sym)p, (Function)value, c.objEnv, c);
                    }
                    else LogoError.Error("Too few reporters in " + p, c);
                }
                return value;
            }
            LogoError.Error("I don't know what " + PrintToString(p), c);
            return null;
        }

        public static object EvalPart(CompoundSymbol s, Object y, Object p, Context c)
        {
            if (p is LocalRef)
            {
                object value = c.locals.LookupValue((LocalRef)p);
                if (value is Function)
                {
                    if (((Function)value).GetNargs() == 0)
                    {
                        return EvalFunction(((LocalRef)p).name, (Function)value, c.objEnv, c);
                    }
                    else LogoError.Error("Too few reporters in " + ((LocalRef)p).name, c);
                }
                return value;
            }
            if (p is InstructionList)
            {
                p = EvalOne((InstructionList)p, c);
            }
            if (p is YoYoObject)
            {
                return p;
            }
            if (p is String)
            {
                p = Symbol.lookup((String)p);
            }
            if (p is Symbol)
            {
                object value = (y == null) ? GetSymbolValue(s, (Symbol)p, c) : GetNativeValue(y, (Symbol)p, c);
                if (value is MethodInfo)
                {
                    value = new Primitive(((Symbol)p).Name, (MethodInfo)value, y);
                }
                if (value is Function)
                {
                    if (((Function)value).GetNargs() == 0)
                    {
                        return EvalFunction((Sym)p, (Function)value, c.objEnv, c);
                    }
                    else LogoError.Error("Too few reporters in " + p, c);
                }
                return value;
            }
            LogoError.Error("I don't know what " + PrintToString(p), c);
            return null;
        }

        public static Object EvalLast(YoYoObject y, Object p, Context c)
        {
            object output = null;
            if (p is String) p = Symbol.lookup((String)p);
            if (p is Symbol) output = y.lookupValue((Symbol)p);
            else if (p is InstructionList)
            {
                output = y.lookupValue(YoYo.aSymbol(c, EvalOne((InstructionList)p, c)));
            }
            if (output == null) return YoYo.symfalse;
            return output;
        }

        public static Object EvalLast(Object y, Object p, Context c)
        {
            object output = null;
            if (p is String) p = Symbol.lookup((String)p);
            if (p is Symbol)
            {
                output = GetNativeValue(y, (Symbol)p, c);
                if (p is MethodInfo)
                {
                    output = new Primitive(((Symbol)p).Name, (MethodInfo)output, y);
                }
            }
            else if (p is InstructionList)
            {
                output = GetNativeValue(y, YoYo.aSymbol(c, EvalOne((InstructionList)p, c)), c);
                if (output is MethodInfo)
                {
                    output = new Primitive(((Symbol)p).Name, (MethodInfo)output, y);
                }
            }
            if (output == null) return YoYo.symfalse;
            return output;
        }

        public static Object GetCompoundValue(CompoundSymbol s, Context c)
        {
            Closure cs = EvalCompound(s, c);
            if (cs.obj is YoYoObject)
            {
                return EvalLast((YoYoObject)cs.obj, cs.last, c);
            } else
            {
                return EvalLast(cs.obj, cs.last, c);
            }
        }

        public static Closure EvalCompound(CompoundSymbol s, Context c)
        {
            object[] parts = s.parts;
            object first = null;

            for (int i = 0; i < parts.Length - 1; i++)
            {
                Object next = null;
                if (first == null || first is YoYoObject)
                {
                    next = EvalPart(s, (YoYoObject)first, parts[i], c);
                }
                else
                {
                    next = EvalPart(s, first, parts[i], c);
                }
                if (next == null)
                {
                    LogoError.Error("I don't know how to " + parts[i].ToString(), c);
                    if (!(next is YoYoObject))
                    {
                    }
                    first = next;
                }
            }
            return new Closure(first, parts[parts.Length - 1]);
        }

        public static Object EvalFunction(Sym name, Function command, YoYoObject env, Context c)
        {
            Sym oldcurrentfun = c.currentFun;
            int oldnargs = c.nargs;
            int oldprec = c.precedence;
            YoYoObject oldenv = c.objEnv;
            Object r = null;
            Object[] v;
            int i;
            Debug.Assert(command.GetNargs() >= 0);

            try
            {
                for (c.nargs = command.GetNargs(), v = command.GetArgArray(c), i = 0, c.precedence = 0;
                    c.nargs > 0; c.nargs--, i++)
                {
                    if (c.ilistposptr >= c.ilist.Length)
                    {
                        LogoError.Error("Too few reporters in " + name, c);
                    }
                    r = null;
                    Object foo = Eval((c.ilistposptr < c.ilist.Length) ? c.ilist[c.ilistposptr++] : null, c);
                    if (foo == null)
                    {
                        LogoError.Error("Too few reporters in " + name, c);
                    }
                    v[i] = foo;
                }

                c.currentFun = name;
                c.objEnv = env;
                r = command.Run(v, c);
            }
            finally
            {
                c.currentFun = oldcurrentfun;
                c.nargs = oldnargs;
                c.precedence = oldprec;
                c.objEnv = oldenv;
            }
            return r;

        }

        public static object EvalFunction(Sym name, Function command, YoYoObject env, Object[] argArray, Context c)
        {
            Sym oldcurrentfun = c.currentFun;
            int oldnargs = c.nargs;
            int oldprec = c.precedence;
            YoYoObject oldenv = c.objEnv;

            Object r = null;

            try
            {
                c.currentFun = name;
                c.nargs = 0;
                c.precedence = 0;

                if (command.GetNargs() < argArray.Length)
                {
                    LogoError.Error("Too few reporters in " + name, c);
                }
                if (command.GetNargs() > argArray.Length)
                {
                    LogoError.Error("Too many reporters in " + name, c);
                }

                c.objEnv = env;
                r = command.Run(argArray, c);
            }
            finally
            {
                c.currentFun = oldcurrentfun;
                c.nargs = oldnargs;
                c.precedence = oldprec;
                c.objEnv = oldenv;
            }
            return r;
        }

        public static Object InfixCall(int nargs, Object firstarg, Function f, Context c)
        {
            Sym oldcurrentfun = c.currentFun;
            int oldnargs = c.nargs;
            int oldprec = c.precedence;
            YoYoObject oldenv = c.objEnv;

            Object r = null;

            c.currentFun = f.GetName();
            c.nargs = 1;
            c.precedence = nargs;
            Object[] v = f.GetArgArray(c);
            try
            {
                v[0] = firstarg;
                if (c.ilistposptr > c.ilist.Length)
                {
                    LogoError.Error("Too few reporters in " + c.currentFun, c);
                }
                r = null;
                v[1] = Eval((c.ilistposptr < c.ilist.Length) ? c.ilist[c.ilistposptr++] : null, c);
                if (v[1] == null)
                {
                    LogoError.Error("Too few reporters in " + c.currentFun, c);
                }
                r = f.Run(v, c);
            } finally
            {
                c.currentFun = oldcurrentfun;
                c.nargs = oldnargs;
                c.precedence = oldprec;
                c.objEnv = oldenv;
            }
            if (r != null)
            {
                return rtv(r, c);
            }
            return null;
        }



        public static String aString(Context c, Object o)
        {
            if (o is String) return (String)o;
            if (o is Sym) return ((Sym)o).ToString();
            if (o is Char)
            {
                return new String( new char[1] { (Char)o });
            }
            return PrintToString(o);
        }

        public static YoYoObject aYoYoObject(Context c, object o)
        {
            try
            {
                return (YoYoObject)o; 
            } catch (Exception)
            {
                LogoError.Error(PrintToString(o) + " is not an object.", c);
            }
            return null;
        }

        public static Boolean aBoolean(Context c, Object o)
        {
            if (o is Symbol)
            {
                return ((Symbol)o != symfalse);
            }
            else return true;
        }

        public static InstructionList aIlist(Context c, Object o)
        {
            if (o is InstructionList) return (InstructionList)o;
            if (o is Object[]) return new InstructionList((object[])o);
            LogoError.Error(PrintToString(o) + " is not a list.", c);
            return null;
        }

        public static object[] aList(Context c, Object o)
        {
            if (o is object[]) return (Object[])o;
            LogoError.Error(PrintToString(o) + " is not a list.", c);
            return null;
        }

        public static Sym aSym(Context c, Object o)
        {
            if (o is Sym) return (Sym)o;
            if (o is String) return Symbol.lookup((String)o);
            LogoError.Error(PrintToString(o) + " is not a symbol.", c);
            return null;
        }

        public static Symbol aSymbol(Context c, Object o)
        {
            if (o is Symbol) return (Symbol)o;
            if (o is String) return Symbol.lookup((String)o);
            LogoError.Error(PrintToString(o) + " is not a symbol.", c);
            return null;
        }

        public static Double aNumber(Context c, Object o)
        {
            try
            {
                if (isNumber(o))
                {
                    return (Double)o;
                }
                else
                {
                    LogoError.Error(PrintToString(o) + " is not a number.", c);
                }
            } catch (InvalidCastException)
            {
                LogoError.Error(PrintToString(o) + " is not a number.", c);
            }
            return 0.0;
        }

        public static Double aDouble(Context c, Object o)
        {
            try
            {
                return (Double)o;
            }
            catch (InvalidCastException)
            {
                LogoError.Error(PrintToString(o) + " is not a number.", c);
            }
            return 0.0;
        }

        public static float aFloat(Context c, Object o)
        {
            try
            {
                return (float)o;
            }
            catch (InvalidCastException)
            {
                LogoError.Error(PrintToString(o) + " is not a number.", c);
            }
            return 0.0f;
        }

        public static Double aDoubleNoWarn(Context c, Object o)
        {
            try
            {
                return (Double)o;
            }
            catch (InvalidCastException)
            {

            }
            return 0.0;
        }

        public static Int64 aInteger(Context c, Object o)
        {
            try
            {
                return (Int64)o;
            }
            catch (InvalidCastException)
            {
                LogoError.Error(PrintToString(o) + " is not a number.", c);
            }
            return 0L;
        }

        public static Boolean isNumber(Object o)
        {
            return o is sbyte
            || o is byte
            || o is short
            || o is ushort
            || o is int
            || o is uint
            || o is long
            || o is ulong
            || o is float
            || o is double
            || o is decimal;
        }


        public static Symbol equal(Object v0, Object v1)
        {
            if (v0 is Object[] && v1 is Object[]) {
                Object[] w0 = (Object[])v0;
                Object[] w1 = (Object[])v1;
                if (w0.Length != w1.Length)
                {
                    return YoYo.symfalse;
                }
                for (int i = 0; i < w0.Length; i++)
                {
                    if (!(YoYo.symtrue == (equal(w0[i], w1[i]))))
                        return YoYo.symfalse;
                }
                return YoYo.symtrue;
            }
            if (isNumber(v0) && isNumber(v1))
                return truefalse((Double)v0 == (Double)v1);
            if (v0.GetType().Equals(v1.GetType())) return truefalse(v0.Equals(v1));
            if (v0 is Symbol) return truefalse(((Symbol)v0).Equals(v1));
            if (v1 is Symbol) return truefalse(((Symbol)v1).Equals(v0));
            return truefalse(v0.ToString().Equals(v1.ToString()));
        }

        public static Symbol truefalse(Boolean tf)
        {
            return (tf) ? YoYo.symtrue : YoYo.symfalse;
        }

        public static String PrintToString(Object o)
        {
            return PrintToString(o, false);
        }

        public static String PrintToString(Object o, Boolean hexp)
        {
            if (o == null) return "<C# null>";
            if (PrimVar.WeakReferencep(o))
            {
                Object ret = PrimVar.aWeakReference(o, null);
                if (ret == null) return "false";
                return "<Weak reference to " + PrintToString(ret, hexp) + ">";
            }
            if (o is Object[])
        return ObjectArrayToString((Object[])o, '[', ']', hexp);
            if (o is List<object>)
        return ListToString((List<object>)o, hexp);
            if (o is int[])
        return IntArrayToString((int[])o, '[', ']', hexp);
            if ((o is Double) && ((Double)o) == (Int64)o)
                return ((Int64)o).ToString(); 
            //    if (o instanceof String)
            //return "\"" + (String)o + "\"";
            return o.ToString();
        }

        public static String ListToString(List<Object> v, Boolean hexp)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('<');
            foreach(var element in v)
            {
                sb.Append(PrintToString(element, hexp));
                sb.Append(' ');
            }
            sb.Append('>');
            return sb.ToString();
        }

        public static String ObjectArrayToString(Object[] o, char start,
                             char end, Boolean hexp)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(start);
            if (o.Length > 0)
            {
                for (int i = 0; i < o.Length - 1; i++)
                {
                    sb.Append(PrintToString(o[i], hexp));
                    sb.Append(' ');
                }
                sb.Append(PrintToString(o[o.Length - 1], hexp));
            }
            sb.Append(end);
            return sb.ToString();
        }

        public static String IntArrayToString(int[] o, char start,
                          char end, Boolean hexp)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(start);
            if (o.Length > 0)
            {
                for (int i = 0; i < o.Length - 1; i++)
                {
                    if (hexp)
                    {
                        sb.Append("0x" + o[i].ToString("X") + " ");
                        sb.Append(' ');
                    }
                    else
                    {
                        sb.Append(o[i].ToString());
                        sb.Append(' ');
                    }
                }
                if (hexp)
                {
                    sb.Append(o[o.Length - 1].ToString("X"));
                } else
                {
                    sb.Append(o[o.Length - 1].ToString());
                }

            }
            sb.Append(end);
            return sb.ToString();
        }


    }
}
