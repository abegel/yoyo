using System;
using System.Reflection;

namespace YoYo
{
    public class PrimStartup : YoYoMethods
    {
        public static void InitClasses(YoYoObject global)
        {
            try
            {
                dload("YoYo.PrimStartup", global);
                dload("YoYo.Primitives", global);
                dload("YoYo.PrimMath", global);
                //dload("YoYo.PrimList", global);
                dload("YoYo.PrimControl", global);
                dload("YoYo.PrimVar", global);
                //dload("YoYo.PrimObject", global);
                dload("YoYo.PrimFile", global);
                //dload("YoYo.PrimTCP", global);
                //dload("YoYo.StringParser", global);
            } catch (LogoError e)
            {
                Console.Out.WriteLine(e.Message);
            }
        }

        public string[] MethodList()
        {
            String[] output =
            {
                "dload", "PrimDLoad",
                "set-listener-function", "PrimSetListenerFunc"
            };
            return output;
        }

        public String[] InfixList()
        {
            return null;
        }

        public void PrePrimCall(Primitive p, Object[] arglist, Context c) { }
        public void PostPrimCall(Primitive p, Context c) { }

        public static Object PrimDLoad(Object v, Context c)
        {
            dload(YoYo.aString(c, v), c.GetGlobalObject());
            return null;
        }

        static void dload(String s, YoYoObject global)
        {
            Type type = null;
            String[] primlist = null;
            String[] infixlist = null;
            YoYoMethods instance = null;

            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                type = assembly.GetType(s);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.StackTrace);
            }
            if (type == null)
            {
                LogoError.Error("Missing or invalid YoYo Type " + s);
            }

            try
            {
                instance = (YoYoMethods)Activator.CreateInstance(type);
                primlist = instance.MethodList();
                infixlist = instance.InfixList();
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.StackTrace);
                LogoError.Error("Type " + s + " has no YoYo methods inside");
            }

            MethodInfo[] methods = null;
            try
            {
                methods = type.GetMethods();
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.StackTrace);
                LogoError.Error("Type " + s + " has no YoYo methods inside");
            }

            if (primlist != null)
            {
                for (int i = 0; i < primlist.Length; i++)
                {
                    MethodInfo m = null;
                    for (int j = 0; j < methods.Length; j++)
                    {
                        if (methods[j].Name.Equals(primlist[i + 1], StringComparison.InvariantCultureIgnoreCase))
                        {
                            global.defineValue(Symbol.lookup(primlist[i]), new Primitive(primlist[i], methods[j], instance));
                        }
                    }
                }
            }

            int precedence = 2;
            if (infixlist != null)
            {
                for (int i = 0; i < infixlist.Length; i++)
                {
                    MethodInfo m = null;
                    for (int j = 0; j < methods.Length; j++)
                    {
                        if (methods[j].Name.Equals(infixlist[i + 1], StringComparison.InvariantCultureIgnoreCase))
                        {
                            global.defineValue(Symbol.lookup(infixlist[i]), new Primitive(infixlist[i], methods[j], precedence, instance));

                        }
                    }
                }
            }


        }


        public static void PrimSetListenerFunc(Object v0, Context c)
        {
            if (c.caller is Listener)
            {
                ((Listener)c.caller).SetFormatInput(aUfun(c, v0));
            }
        }

        public static Ufun aUfun(Context c, Object o)
        {
            if (o is Ufun) return ((Ufun)o);
            LogoError.Error(YoYo.PrintToString(o) + " is not a procedure.", c);
            return null;
        }
    }

}
