using System;
using System.Reflection;

namespace YoYo
{
    public class PrimVar : YoYoMethods
    {
        public string[] InfixList()
        {
            return null;
        }

        public string[] MethodList()
        {
            string[] output = {
                "set", "PrimSet",
                "weak_reference", "PrimWeakReference",
            };
            return output;
        }

        public void PostPrimCall(Primitive p, Context c)
        {
}

        public void PrePrimCall(Primitive p, object[] arglist, Context c)
        {
        }

        public static void PrimSet(Context c)
        {
            Object o = c.ilistNext();
            if (o is LocalRef)
            {
                c.locals.SetValue((LocalRef)o, YoYo.EvalOneFromHere(c));
                return;
            }

            Sym sym = YoYo.aSym(c, o);
            if (c.ilistPeek() == null)
            {
                LogoError.Error("too few reporters for set", c);
            }

            if (sym is CompoundSymbol)
            {
                PrimSetRecord((CompoundSymbol)sym, c);
                return;
            }

            Symbol s = (Symbol)sym;
            if (c.locals.boundp(s))
            {
                int index = c.locals.Index(s);
                LocalRef l = new LocalRef(index, s);
                c.SetPrev(l);
                c.locals.SetValue(l, YoYo.EvalOneFromHere(c));
                return;
            }

            Object v1 = YoYo.EvalOneFromHere(c);

            if (c. objEnv.boundp(s))
            {
                c.objEnv.setValue(s, v1);
                return;
            }

            if (c.ufunenv != c.objEnv)
            {
                if (c.ufunenv.boundp(s))
                {
                    c.ufunenv.setValue(s, v1);
                    return;
                }
            }

            c.GetGlobalObject().setValue(s, v1);
        }


        public static void PrimSetRecord(CompoundSymbol s, Context c)
        {
            Object value = YoYo.EvalOneFromHere(c);
            Closure cs = YoYo.EvalCompound(s, c);
            if (cs.obj is YoYoObject)
            {
                YoYoObject obj = (YoYoObject)cs.obj;
                Object p = cs.last;
                if (p is String) p = Symbol.lookup((String)p);
                if (p is Symbol) obj.setValue((Symbol)p, value);
                if (p is InstructionList)
                {
                    obj.setValue(YoYo.aSymbol(c, YoYo.EvalOne((InstructionList)p, c)), value);
                }
                return;
            } else
            {
                Object obj = cs.obj;
                Object p = cs.last;
                if (p is String) p = Symbol.lookup((String)p);
                Type type = obj.GetType();
                FieldInfo f = null;
                try
                {
                    if (p is Symbol)
                    {
                        f = type.GetField(((Symbol)p).Name);
                    }
                    else if (p is InstructionList)
                    {
                        f = type.GetField(YoYo.aSymbol(c, YoYo.EvalOne((InstructionList)p, c)).Name);
                    }
                } catch (MissingFieldException) { }
                if (f == null)
                {
                    LogoError.Error("no such field of object type " + type.Name, c);
                }

                try
                {
                    f.SetValue(obj, value);
                } catch (FieldAccessException)
                {
                    LogoError.Error("you forgot to make " + f.ToString() + " public", c);
                }
                return;

            }
        }

        public static Object PrimWeakReference(Object v0, Context c)
        {
            return new WeakReference(v0);
        }

        public static Boolean WeakReferencep(Object v0)
        {
            if (v0 != null)
            {
                return (v0 is WeakReference);
            }
            return false;
        }

        public static Object aWeakReference(Object v0, Context c)
        {
            if (v0 != null)
            {
                if (v0 is WeakReference)
                {
                    Object ret = ((WeakReference)v0).Target;
                    if (ret == null)
                    {
                        return YoYo.symfalse;
                    }
                    return ret;
                }
            }
            LogoError.Error(YoYo.PrintToString(v0) + " is not a weak reference", c);
            return null;
        }

    }
}
