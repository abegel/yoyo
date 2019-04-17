using System;
using System.Threading;

namespace YoYo
{
    public class PrimControl : YoYoMethods
    {
        public PrimControl()
        {
        }

        public string[] InfixList()
        {
            return null;
        }

        public string[] MethodList()
        {
            String[] output =
            {
                "repeat", "PrimRepeat",
                "stop", "PrimStop",
                "output", "PrimOutput",
                "if", "PrimIf",
                "ifelse", "PrimIfElse",
                "while", "PrimWhile",
                "loop", "PrimLoop",
                "carefully", "PrimCarefully",
                "always", "PrimAlways",
                "run", "PrimRun",
                "ask", "PrimAsk",
                "dotimes", "PrimDoTimes",
                "launch", "PrimLaunch",
                "thread-self", "PrimThreadSelf",
                "throw", "PrimThrow",
                "error", "PrimError",
                "error-type", "PrimErrorType",
                "system-yield", "PrimYield",
                "wait", "PrimWait",
                "case", "PrimCase",
            };
            return output;
        }

        public void PostPrimCall(Primitive p, Context c)
        {
}

        public void PrePrimCall(Primitive p, object[] arglist, Context c)
        {
        }

        public static Symbol symerror = Symbol.lookup("system-error");


        public static void PrimStop(Context c)
        {
            c.ufunreturn = Context.novalue;
            c.ilistSkipToEnd();
        }

        public static void PrimOutput(Object v, Context c)
        {
            c.ufunreturn = v;
            c.ilistSkipToEnd();
        }

        public static Object PrimError(Context c)
        {
            if (c.errorObject != null)
            {
                if (c.errorObject is ThrowReturn)
                {
                    return ((ThrowReturn)c.errorObject).value;
                }
                else return c.errorObject.ToString();
            }
            else return YoYo.symfalse;
        }

        public static Object PrimErrorType(Context c)
        {
            if (c.errorObject != null)
            {
                if (c.errorObject is ThrowReturn)
                {
                    return ((ThrowReturn)c.errorObject).errortype;
                }
                else return symerror;
            }
            else return YoYo.symfalse;
        }

        public static void PrimTHrow(Object v1, Object v2, Context c)
        {
            Symbol s = YoYo.aSymbol(c, v1);
            if (s == symerror)
            {
                LogoError.Error(YoYo.aString(c, v2), c);
            }
            else throw new ThrowReturn(s, v2);
        }

        public static void PrimRepeat(Object v0, Object v1, Context c)
        {

            if (v1 is Object[])
            {
                long n = YoYo.aInteger(c, v0);
                if (n > 0)
                {
                    Object[] listtorun = YoYo.aList(c, v1);
                    for (int i = 0; i < n; i++)
                    {
                        YoYo.EvalList(listtorun, c);
                        if (c.ufunreturn != null) return;
                    }
                }
            }
        }

        public static Object PrimIf(Object v0, Object v1, Context c)
        {
            Object[] list = YoYo.aList(c, v1);
            if (YoYo.aBoolean(c, v0))
            {
                Object value = YoYo.EvalList(list, c);
                return value;
            }
            return null;
        }

        public static Object PrimIfElse(Object v0, Object v1, Object v2, Context c)
        {
            int oldnargs = c.nargs;
            Object[] cons = YoYo.aList(c, v1);
            Object[] alt = YoYo.aList(c, v2);
            try
            {
                c.nargs = 1;
                if (YoYo.aBoolean(c, v0))
                {
                    return YoYo.EvalList(cons, c);
                }
                else
                {
                    return YoYo.EvalList(alt, c);
                }
            }
            finally
            {
                c.nargs = oldnargs;
            }
        }

        public static Object PrimWhile(Object v0, Object v1, Context c)
        {

            Object[] test = YoYo.aList(c, v0);
            Object[] torun = YoYo.aList(c, v1);
            int oldnargs = c.nargs;

            try
            {
                while (true)
                {
                    c.nargs = 1;
                    Object pred = YoYo.EvalList(test, c);
                    if (c.ufunreturn != null) return null;
                    c.nargs = 0;
                    if (!YoYo.aBoolean(c, pred)) return null;
                    YoYo.EvalList(torun, c);
                    if (c.ufunreturn != null) return null;
                }
            }
            finally
            {
                c.nargs = oldnargs;
            }
        }

        public static Object PrimLoop(Object v0, Context c)
        {

            Object[] list = YoYo.aList(c, v0);
            while (true)
            {
                YoYo.EvalList(list, c);
                if (c.ufunreturn != null) return null;
            }
        }

        public static Object PrimCase(Object v0, Object v1, Context c)
        {

            Object[] list = YoYo.aList(c, v1);
            Object[] oldilist = c.ilist;
            int oldilistposptr = c.ilistposptr;

            try
            {
                c.ilist = list;
                c.ilistposptr = 0;

                while (c.ilistPeek() != null)
                {
                    Object test = YoYo.EvalOneFromHere(c);
                    if (c.ufunreturn != null) return null;
                    if (c.ilistPeek() == null)
                        LogoError.Error("Too few reporters in case", c);
                    Object thing = c.ilistNext();
                    if (YoYo.equal(v0, test) == YoYo.symtrue || test == YoYo.symtrue)
                    {
                        return YoYo.EvalList(YoYo.aList(c, thing), c);
                    }
                }
            }
            finally
            {
                c.ilist = oldilist;
                c.ilistposptr = oldilistposptr;
                if (c.ufunreturn != null) c.ilistSkipToEnd();
            }
            return null;
        }

        public static Object PrimCarefully(Object v0, Object v1, Context c)
        {

            Object[] run = YoYo.aList(c, v0);
            Object[] exc = YoYo.aList(c, v1);
            LogoError olderrorobj = c.errorObject;
            try
            {
                return YoYo.EvalList(run, c);
            }
            catch (LogoError e)
            {
                c.errorObject = e;
                Object val = c.ufunreturn;
                c.ufunreturn = null;
                try
                {
                    return YoYo.EvalList(exc, c);
                }
                finally
                {
                    if (c.ufunreturn == null)
                    {
                        c.ufunreturn = val;
                    }
                }
            }
            finally
            {
                c.errorObject = olderrorobj;
            }
        }

        public static Object PrimAlways(Object v0, Object v1, Context c)
        {
            Object[] run = YoYo.aList(c, v0);
            Object[] exc = YoYo.aList(c, v1);
            try
            {
                return YoYo.EvalList(run, c);
            }
            finally
            {
                Object val = c.ufunreturn;
                c.ufunreturn = null;
                try
                {
                    YoYo.EvalList(exc, c);
                }
                finally
                {
                    if (c.ufunreturn == null)
                    {
                        c.ufunreturn = val;
                    }
                }
            }
        }

        public static Object PrimRun(Object v0, Context c)
        {
            Object[] l = YoYo.aList(c, v0);
            int oldnargs = c.nargs; c.nargs = 1; Object value = null;
            try { value = YoYo.EvalList(l, c); }
            finally { c.nargs = oldnargs; }
            return value;
        }

        public static Object PrimAsk(Object v0, Object v1, Context c)
        {
            Object[] l = YoYo.aList(c, v1);
            YoYoObject oldYoYobj = c.objEnv;
            c.objEnv = YoYo.aYoYoObject(c, v0);
            int oldnargs = c.nargs; c.nargs = 1; Object value = null;
            try { value = YoYo.EvalList(l, c); }
            finally { c.nargs = oldnargs; c.objEnv = oldYoYobj; }
            return value;
        }

        public static Object PrimLaunch(Object v0, Context c)
        {
            Object[] i = YoYo.aList(c, v0);
            Context newc = new Context(c);
            Thread t = new Thread(() =>
            {
                newc.Run();
            });
            newc.RunList(i);
            return newc.self;
        }

        public static Object PrimThreadSelf(Context c)
        {
            return c.self;
        }

        public static void PrimYield(Context c)
        {
            Thread.Yield();
        }

        public static Object PrimDoTimes(Object v0, Object v1, Context c)
        {

            Object[] deflist = YoYo.aList(c, v0);
            Object[] torun = YoYo.aList(c, v1);

            Object oldvalue = null;
            LocalRef l;
            long value;
            Symbol name;

            Object[] oldilist = c.ilist;
            int oldilistposptr = c.ilistposptr;

            try
            {
                c.ilist = deflist;
                c.ilistposptr = 0;

                Object var = c.ilistNext();
                if (var is LocalRef)
                {
                    l = (LocalRef)var;
                    oldvalue = c.locals.LookupValue(l);
                }
                else
                {
                    name = YoYo.aSymbol(c, var);
                    if (c.locals.boundp(name))
                    {
                        int index = c.locals.Index(name);
                        l = new LocalRef(index, name);
                        c.SetPrev(l);
                        oldvalue = c.locals.LookupValue(l);
                    }
                    else
                    {
                        int index = c.locals.DefineVar(name);
                        l = new LocalRef(index, name);
                        c.SetPrev(l);
                        oldvalue = null;
                    }
                }
                if (c.ufunObj != null)
                {
                    if (c.locals.names.Length > c.ufunObj.ArgList.Length)
                        c.ufunObj.ArgList = c.locals.names;
                }
                value = YoYo.aInteger(c, YoYo.EvalOneFromHere(c));
            }
            finally
            {
                c.ilist = oldilist;
                c.ilistposptr = oldilistposptr;
            }

            try
            {
                long current = 1;
                while (true)
                {
                    if (current > value) break;
                    c.locals.SetValue(l, current++);
                    YoYo.EvalList(torun, c);
                    if (c.ufunreturn != null) return null;
                }
                return null;
            }
            catch (Stop e)
            {
                //System.out.println("time to stop: dotimes");
                throw e;
            }
            finally
            {
                if (oldvalue != null) c.locals.SetValue(l, oldvalue);
                else c.locals.UndefineVar(l);
            }
        }

        public static void PrimWait(Object v0, Context c)
        {
            int time = (int)(1000 * YoYo.aDouble(c, v0));
            try
            {
                while (time >= 0)
                {
                    YoYo.PollStop(c);
                    Thread.Sleep(10);
                    time -= 10;
                }
            }
            catch (ThreadInterruptedException) { }
        }
    }
}
