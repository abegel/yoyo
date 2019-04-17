using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace YoYo
{
    public class Primitives : YoYoMethods
    {
        public string[] InfixList()
        {
            return null;
        }

        public string[] MethodList()
        {
            String[] output =
            {
                "primitives_print", "PrimPrint",
                "primitives_show", "PrimPrint",
                "primitives_type", "PrimType",
                "primitives_resett", "PrimResett",
                "primitives_timer", "PrimTimer",
                "primitives_freememory", "PrimFree<emory",
                "primitives_not", "PrimNot",
                "primitives_ignore", "PrimIgnore",
                //"primitives_system_quit", "PrimSystemQuit",
                "primitives_symbolp", "PrimSymbolp",
                "primitives_booleanp", "PrimBooleanp",
                "primitives_colon", "PrimDelay",
                "primitives_lambda", "PrimLambda",
                "primitives_to_string", "PrimToString",
                "primitives_unquote", "PrimUnquote",
                "primitives_ilistp", "PrimIlistp",
                "primitives_ilist_to_list", "PrimIlisttoList",
                "primitives_list_to_ilist", "PrimListtoIlist",
                "primitives_ascii_to_integer", "PrimA2I",
                "primitives_integer_to_ascii", "PrimI2A",
                "primitives_trace", "PrimTrace",
                "primitives_untrace", "PrimUntrace",
                //"primitives_get_string", "PrimGetString",
                "primitives_untrace_all", "PrimUntrace_all",
                //"primitives_load_sound", "PrimLoadSound",
                //"primitives_setup_sound", "PrimSetupSound",
                //"primitives_play_sound", "PrimPlaySound",
                //"primitives_stop_sound", "PrimStopSound",
                "primitives_system_os", "PrimSystemOS",
                "primitives_system_language", "PrimSystemLanguage",
                //"primitives_system_locale", "PrimSystemLocale",
                "primitives_system_print", "PrimSystemPrint",
                "primitives_system_type", "PrimSystemType"
            };
            return output;
        }

        public void PostPrimCall(Primitive p, Context c)
        {
}

        public void PrePrimCall(Primitive p, object[] arglist, Context c)
        {
        }

        public static void PrimPrint(Object v, Context c)
        {
            c.output.WriteLine(YoYo.PrintToString(v));
        }

        public static void PrimType(Object v, Context c)
        {
            c.output.Write(YoYo.PrintToString(v));
        }

        public static void PrimSystemPrint(Object v, Context c)
        {
            Console.WriteLine(YoYo.PrintToString(v));
        }

        public static void PrimSystemType(Object v, Context c)
        {
            Console.Write(YoYo.PrintToString(v));
        }

        public static Object PrimSymbolp(Object v, Context c)
        {
            return (v is Sym || v is LocalRef) ? YoYo.symtrue : YoYo.symfalse;
        }

        public static Object PrimBooleanp(Object v, Context c)
        {
            return (v == YoYo.symtrue || v == YoYo.symfalse) ? YoYo.symtrue : YoYo.symfalse;
        }

        public static Object PrimDelay(Context c)
        {
            Object o = c.ilistNext();
            if (o is Function) return o;
            if (o is Symbol) return YoYo.GetSymbolValue(c, (Symbol)o, c);
            if (o is LocalRef) return c.locals.LookupValue((LocalRef)o);
            if (o is CompoundSymbol) return YoYo.GetCompoundValue((CompoundSymbol)o, c);
            return o;
        }

        public static Object PrimLambda(Object v0, Object v1, Context c)
        {
            if (v0 is Object[])
            {
                Symbol[] arglist = new Symbol[((Object[])v0).Length];
                Array.Copy((Object[])v0, arglist, arglist.Length);
                Object[] body = YoYo.aList(c, v1);
                Symbol[] locals = PrimFile.ReadLocals(arglist, new Symbol[0], body, 0);
                return new Ufun(arglist, locals, body, YoYo.PrintToString(body), c.ufunenv);
            }
            LogoError.Error(v0 + " is not a list", c);
            return null;
        }

        public static Object PrimUnquote(Object v0, Context c)
        {
            if (v0 is String)
                return Reader.Read((String)v0)[0];
            return v0;
        }


        public static void PrimResett(Context c)
        {
            c.time = DateTime.Now.Ticks;
        }

        public static Object PrimTimer(Context c)
        {
            return (DateTime.Now.Ticks - c.time) / 1000.0;
        }

        public static void PrimIgnore(Object v0, Context c)
        {
        }

        public static Object PrimFreeMemory(Context c)
        {
            using (Process p = Process.GetCurrentProcess())
            {
                p.Refresh();
                return p.PrivateMemorySize64;
            }
        }

        public static Object PrimNot(Object v0, Context c)
        {
            if (YoYo.aBoolean(c, v0))
                return YoYo.symfalse;
            else return YoYo.symtrue;
        }

        public static Object PrimToString(Object v0, Context c)
        {

            Object[] list = YoYo.aList(c, v0);
            Object[] oldilist = c.ilist;
            int oldilistposptr = c.ilistposptr;
            StringBuilder output = new StringBuilder();

            try
            {
                c.ilist = list;
                c.ilistposptr = 0;
                while (true)
                {
                    if (c.ilistPeek() == null) break;
                    output.Append(YoYo.aString(c, YoYo.EvalOneFromHere(c)));
                }
                return output.ToString();
            }
            finally
            {
                c.ilist = oldilist;
                c.ilistposptr = oldilistposptr;
            }
        }

        public static Object PrimIlistp(Object v0, Context c)
        {
            if (v0 is InstructionList) return YoYo.symtrue;
            return YoYo.symfalse;
        }

        public static Object PrimIlistToList(Object v0, Context c)
        {
            if (v0 is InstructionList) return ((InstructionList)v0).list;
            LogoError.Error(v0 + " is not an instruction list");
            return null;
        }

        public static Object PrimListToIlist(Object v0, Context c)
        {
            return YoYo.aIlist(c, v0);
        }

        public static Object PrimA2I(Object v0, Context c)
        {
            String s = YoYo.aString(c, v0);
            return (long)s[0];
        }

        public static Object PrimI2A(Object v0, Context c)
        {
            long i = YoYo.aInteger(c, v0);
            char[] foo = { (char)i };
            return new String(foo);
        }


        public static void PrimTrace(Object v0, Context c)
        {
            if (v0 is Object[]) {
                Object[] list = (Object[])v0;
                for (int i = 0; i < list.Length; i++)
                {
                    traceObj(list[i], c);
                }
            }
    else traceObj(v0, c);
        }

        static void traceObj(Object v0, Context c)
        {
            if (v0 is Function) {
                ((Function)v0).Trace();
                var val = c.Trace((Function)v0);
                c.output.WriteLine("Tracing " + YoYo.ListToString(val.Cast<object>().ToList(), false));
                return;
            }
    else if (v0 is String) {
                Sym name = YoYo.aSym(c, Reader.Read(YoYo.aString(c, v0))[0]);
                Object o = YoYo.LookupSymbol(name, c);
                if (o == null)
                    LogoError.Error("i don't know how to " + YoYo.PrintToString(v0), c);
                if (o is Function) {
                    ((Function)o).Trace();
                    var val = c.Trace((Function)o);
                    c.output.WriteLine("Tracing " + YoYo.ListToString(val.Cast<object>().ToList(), false));
                    return;
                }
            }
            LogoError.Error(YoYo.PrintToString(v0) + " is not a procedure", c);
        }

        public static void prim_untrace(Object v0, Context c)
        {
            if (v0 is Object[]) {
                Object[] list = (Object[])v0;
                for (int i = 0; i < list.Length; i++)
                {
                    untraceObj(list[i], c);
                }
            }
            else untraceObj(v0, c);
        }

        public static void untraceObj(Object v0, Context c)
        {
            if (v0 is Function)
            {
                ((Function)v0).Untrace();
                var val = c.Untrace((Function)v0);
                c.output.WriteLine("Untracing " + YoYo.ListToString(val.Cast<object>().ToList(), false));
                return;
            }
            else if (v0 is String)
            {
                Sym name = YoYo.aSym(c, Reader.Read(YoYo.aString(c, v0))[0]);
                Object o = YoYo.LookupSymbol(name, c);
                if (o == null)
                    LogoError.Error("i don't know how to " + YoYo.PrintToString(v0), c);
                if (o is Function)
                {
                    ((Function)o).Untrace();
                    var val = c.Untrace((Function)o);
                    c.output.WriteLine("Untracing " + YoYo.ListToString(val.Cast<object>().ToList(), false));
                    return;
                }
            }
            LogoError.Error(YoYo.PrintToString(v0) + " is not a procedure", c);
        }

        public static void PrimUntraceAll(Context c)
        {
            c.UntraceAll();
        }

        public static Object PrimSystemOS(Context c)
        {
            return Environment.OSVersion.ToString();
        }

        public static Object PrimSystemLanguage(Context c)
        {
            return CultureInfo.InstalledUICulture.EnglishName;
        }



    }
}
