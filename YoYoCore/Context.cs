using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace YoYo
{
    public class Context : YoYoMethods, ExprList
    {
        UberContext ub;
        public TextWriter output;
        public TextWriter errOutput;
        public TextWriter logOutput;
        public Uri baseURL;

        public Object[] ilist;
        public int ilistposptr = 0;
        public Sym currentFun;
        public Symbol ufun;
        public Ufun ufunObj;
        public Locals locals;
        public YoYoObject ufunenv, self;
        public YoYoObject objEnv;
        public List<Object> savedynamics;
        List<Object> traced = new List<object>();
        public int nargs = 0, precedence = 0, indent = 0, indentlevel = 0;
        long time = 0;
        protected Boolean goNow = false, dieNow = false;
        public Boolean stopNow = false;
        public YoYoCaller caller;
        int contextNum;

        public LogoError errorObject = null;
        public object ufunreturn = null;
        public static object novalue = new object();

        public object runningp = new object();
        public Boolean runningnowp = false;


        public int ContextNumber
        {
            get;
            set;
        }

        public Context() { }

        public Context(Context c) :
            this(null, c.baseURL, c.output, c.errOutput, c.logOutput, c.caller, c.ub)
        {

        }

        public Context(Context c, String s) :
            this(Reader.Read(s), c.baseURL, c.output, c.errOutput, c.logOutput, c.caller, c.ub)
        {
        }

        public Context(Context c, Object[] i) :
            this(i, c.baseURL, c.output, c.errOutput, c.logOutput, c.caller, c.ub)
        {
        }

        public Context(Context c, TextWriter output, TextWriter errOutput) :
            this(null, c.baseURL, output, errOutput, errOutput, c.caller, c.ub)
        {

        }

        public Context(Context c, TextWriter output, TextWriter errOutput, TextWriter logOutput) :
            this(null, c.baseURL, output, errOutput, logOutput, c.caller, c.ub)
        {

        }

        public Context(Context c, TextWriter output, YoYoCaller who) :
            this(null, c.baseURL, output, output, output, who, c.ub)
        {

        }

        public Context(Object[] ilist, Uri baseURL, TextWriter output, YoYoCaller who, UberContext ub) :
            this(ilist, baseURL, output, output, output, who, ub)
        {

        }

        public Context(object[] ilist, Uri urlBase, TextWriter output, TextWriter errOutput, TextWriter log, YoYoCaller who, UberContext ub)
        {
            this.ilist = ilist;
            this.baseURL = urlBase;
            this.ub = ub;
            this.locals = new Locals();
            this.objEnv = new YoYoObject();
            ufunenv = objEnv;
            this.output = output;
            this.errOutput = errOutput;
            this.logOutput = log;
            caller = who;
            ub.AddContext(this);
            self = ToObject();
            // new Thread(this).start();
        }

        public YoYoObject GetGlobalObject()
        {
            return ub.globalObject;
        }

        public void Run()
        {
            Thread cur = Thread.CurrentThread;
            try
            {
                while(true)
                {
                    try
                    {
                        if (dieNow) break;
                        lock(runningp)
                        {
                            if (!goNow)
                            {
                                runningnowp = false;
                                Monitor.Wait(runningp);
                            }
                            try
                            {
                                YoYo.EvalExternal(this);
                            } 
                            finally
                            {
                                goNow = false;
                                runningnowp = false;
                                if (caller != null)
                                {
                                    caller.Done(this);
                                }
                            }
                        }
                    } catch (ThreadInterruptedException) { }
                    catch(Exception e)
                    {
                        errOutput.WriteLine("Uncaught exception in YoYo.Context.Run():");
                        errOutput.WriteLine(e.StackTrace);
                        if (errOutput != logOutput)
                        {
                            logOutput.WriteLine("Uncaught exception in YoYo.Context.Run():");
                            logOutput.WriteLine(e.StackTrace);
                        }
                        output.WriteLine("Uncaught exception in YoYo.Context.Run():");
                        output.WriteLine(e.StackTrace);
                    }
                }
            } finally
            {
                ub.RemoveContext(this);
            }


        }

        public void RunList(Object[] i)
        {
            if (runningnowp)
            {
                WaitUntilDone();
            }
            lock (runningp)
            {
                ilist = i;
                ilistposptr = 0;
                indent = 0;
                stopNow = false;
                goNow = true;
                runningnowp = true;
                Monitor.Pulse(runningp);
            }
        }

        public void Stop()
        {
            stopNow = true;
            WaitUntilDone();
        }

        public void WaitUntilDone()
        {
            while (runningnowp)
            {
                try
                {
                    Thread.Sleep(250);
                }
                catch (ThreadInterruptedException e) { }
            }
        }

        public void Die()
        {
            Stop();
            dieNow = true;
        }

        public YoYoObject ToObject()
        {
            YoYoObject obj = new YoYoObject();
            //PrimStartup.dload("yoyo.Context", obj);
            obj.defineValue(YoYoObject.kindSym, "thread");
            obj.defineValue(Symbol.lookup("self"), this);
            return obj;
        }

        public string[] MethodList()
        {
            String[] output = {
                "stop-thread", "PrimStop",
                "ask-thread", "PrimRunList"
            };
            return output;
        }

        public String[] InfixList()
        {
            return null;
        }

        public void PrePrimCall(Primitive p, Object[] arglist, Context c) { }
        public void PostPrimCall(Primitive p, Context c) { }

        public static void PrimRunList(Object v0, Object v1, Context c)
        {
            ((v0 as YoYoObject).lookupValue(Symbol.lookup("self")) as Context).RunList(YoYo.aList(c, v1));
        }

        public static void PrimStop(Object v0, Context c)
        {
            ((v0 as YoYoObject).lookupValue(Symbol.lookup("self")) as Context).Stop();
        }

        public Boolean IsRunning()
        {
            return runningnowp;
        }

        public object ilistNext()
        {
            if (ilistposptr < ilist.Length)
            {
                return ilist[ilistposptr++];
            }
            else return null;
        }

        public object ilistPeek()
        {
            if (ilistposptr < ilist.Length)
            {
                return ilist[ilistposptr];
            }
            else return null;
        }

        public void ilistSkip()
        {
            ilistposptr++;
        }

        public void ilistSkipToEnd()
        {
            ilistposptr = ilist.Length;
        }

        public void SetPrev(object o)
        {
            lock (ilist)
            {
                if (ilist[ilistposptr - 1] is InstructionList)
                {
                    ilist[ilist.Length - 1] = o;
                }
                else
                {
                    ilist[ilistposptr - 1] = o;
                }
            }
        }



        public void TraceEnter(String name, Object[] arglist)
        {
            throw new NotImplementedException();
        }

        public void TraceExit(String name, Object value)
        {
            throw new NotImplementedException();
        }

        public void TraceThrow(String name, Exception e)
        {
            throw new NotImplementedException();
        }


    }
}
