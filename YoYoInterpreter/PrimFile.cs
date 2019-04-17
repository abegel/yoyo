using System;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;

namespace YoYo
{
    public class PrimFile : YoYoMethods
    {

        public string[] InfixList()
        {
            return null;
        }

        public string[] MethodList()
        {
            String[] output =
            {
                "load-yoyo", "PrimLoad",
                "home-directory", "PrimHomeDir",
            };
            return output;
        }

        public void PostPrimCall(Primitive p, Context c)
        {
}

        public void PrePrimCall(Primitive p, object[] arglist, Context c)
        {
        }

        public static Object PrimHomeDir(Context c)
        {
            return c.baseURL.ToString();
        }

        public static void PrimLoad(Object v, Context c)
        {
            String s = YoYo.aString(c, v);
            if (!s.EndsWith(".yoyo", StringComparison.InvariantCultureIgnoreCase)) s += ".yoyo";

            using (var client = new WebClient())
            {
                using (var reader = new StreamReader(client.OpenRead(s)))
                {
                    ReadProcs(reader, c);
                }
            }
        }

        public static void PrimLoadStream(Stream s, Context c)
        {
            using (var reader = new StreamReader(s))
            {
                ReadProcs(reader, c);
            }
        }

        public static void ReadProcs(StreamReader reader, Context c)
        {
            String name;
            Symbol[] args;

            YoYoObject oldufunenv = c.ufunenv;
            Object[] oldilist = c.ilist;
            int oldilistposptr = c.ilistposptr;

            try
            {
                c.ufunenv = c.GetGlobalObject();
                while(true)
                {
                    string title = ReadTitle(c, reader);
                    if (title == null) return;

                    title = title.Substring(3).Trim();
                    int space = title.IndexOf(" ", StringComparison.InvariantCultureIgnoreCase);
                    if (space == -1)
                    {
                        name = title;
                        args = new Symbol[0];
                    } else
                    {
                        name = title.Substring(0, space);
                        Object[] argsarray = Reader.Read(title.Substring(space + 1));
                        args = new Symbol[argsarray.Length];
                        Array.Copy(argsarray, args, argsarray.Length);
                    }

                    String bodyString = ReadBody(reader);
                    Object[] body = Reader.Read(bodyString);
                    Symbol[] locals = ReadLocals(args, new Symbol[0], body, 0);
                    Symbol symname = Symbol.lookup(name);
                    c.ufunenv.defineValue(symname, new Ufun(symname, args, locals, body, bodyString, c.ufunenv));
                }
            } 
            catch(IOException) { }
            finally
            {
                c.ufunenv = oldufunenv;
                c.ilist = oldilist;
                c.ilistposptr = oldilistposptr;
            }
        }

        public static String ReadBody(StreamReader reader)
        {
            StringBuilder body = new StringBuilder();
            String line;
            do
            {
                line = ReadALine(reader);
                if (line == null || line.StartsWith("end", StringComparison.InvariantCultureIgnoreCase))
                {
                    return body.ToString();
                }
                body.Append(line);
                body.Append(Environment.NewLine);
            } while (true);
        }

        public static String ReadALine(StreamReader reader)
        {
            String s = reader.ReadLine();
            if (s == null)
            {
                throw new IOException();
            }
            return s;
        }


        public static String ReadTitle(Context c, StreamReader reader)
        {
            String line;
            do
            {
                line = ReadALine(reader);
                if (line == null) return null;
                if (line.StartsWith("to ", StringComparison.InvariantCultureIgnoreCase))
                {
                    return line;
                }
                if (line.StartsWith("create-object ", StringComparison.InvariantCultureIgnoreCase))
                {
                    CreateObject(line.Substring(14).Trim(), c);
                }
                else if (line.StartsWith("in-object ", StringComparison.InvariantCultureIgnoreCase))
                {
                    SwitchObject(line.Substring(10).Trim(), c);
                }
                else if (line.StartsWith("instance-var ", StringComparison.InvariantCultureIgnoreCase))
                {
                    CreateInstanceVar(line.Substring(13).Trim(), c);
                }
            } while (true);
        }

        public static void CreateObject(String line, Context c)
        {
            Object[] input = Reader.Read(line);
            c.ilist = input;
            c.ilistposptr = 0;
            Object name = c.ilistNext();
            if (name is Symbol)
            {
                Object parent = YoYo.EvalOneFromHere(c);
                if (parent is YoYoObject)
                {
                    YoYoObject newObj = new YoYoObject((YoYoObject)parent);
                    c.GetGlobalObject().defineValue((Symbol)name, newObj);
                    c.ufunenv = newObj;
                    return;
                }
                else
                {
                    LogoError.Error("parent of " + name + " is not an object", c);
                } 
            } else
            {
                LogoError.Error("create-object needs an object name", c);
            }
        }

        public static void SwitchObject(String line, Context c)
        {
            Object[] input = Reader.Read(line);
            Object obj = YoYo.EvalOne(input, c);
            if (obj is YoYoObject)
            {
                c.ufunenv = (YoYoObject)obj;
            }
            else
            {
                LogoError.Error("in-object needs an object", c);
            }
        }

        public static void CreateInstanceVar(String line, Context c)
        {
            Object[] input = Reader.Read(line);
            c.ilist = input;
            c.ilistposptr = 0;
            Object name = c.ilistNext();
            if (name is Symbol)
            {
                Object val = YoYo.EvalOneFromHere(c);
                c.ufunenv.defineValue((Symbol)name, val);
                return;
            } else
            {
                LogoError.Error("instance-var needs a variable name", c);
            }
        }

        public static Symbol letSym = Symbol.lookup("let");
        public static Symbol doListSym = Symbol.lookup("dolist");
        public static Symbol doTimesSym = Symbol.lookup("dotimes");
        public static Symbol setSym = Symbol.lookup("set");
        public static Symbol lambdaSym = Symbol.lookup("lambda");
        public static Symbol repeatSym = Symbol.lookup("repeat");

        public static Symbol[] ReadLocals(Symbol[] arglist, Symbol[] output, Object[] body, int start)
        {
            try
            {
                for (int i = start; i < body.Length; i++)
                {
                    if (body[i] is Symbol)
                    {
                        Symbol token = (Symbol)body[i];
                        if (ReadLocals_Symbol(token, arglist, output, body, i))
                        {
                            continue;
                        }
                        else if (token == setSym)
                        {
                            if (i + 1 >= body.Length)
                            {
                                continue;
                            }
                            if (body[i + 1] is Symbol)
                            {
                                Symbol variable = (Symbol)body[++i];
                                ReadLocals_Symbol(variable, arglist, output, body, i);
                            }
                            else if (body[i + 1] is CompoundSymbol)
                            {
                                CompoundSymbol cs = (CompoundSymbol)body[++i];
                                output = ReadLocals_CompoundSymbol(cs, arglist, output);
                            }
                        }
                        else if (token == doListSym || token == doTimesSym)
                        {
                            if (i + 1 >= body.Length)
                            {
                                continue;
                            }
                            if (body[i + 1] is Object[])
                            {
                                Object[] letlist = (Object[])body[++i];
                                output = ReadLocals_InnerLet(letlist, arglist, output);
                            }
                        }
                        else if (token == lambdaSym)
                        {
                            if (i + 2 >= body.Length)
                            {
                                continue;
                            }
                            if (body[i + 1] is Object[] && body[i + 2] is Object[])
                            {
                                i++;
                            }
                        }
                    }
                    else if (body[i] is CompoundSymbol)
                    {
                        output = ReadLocals_CompoundSymbol((CompoundSymbol)body[i], arglist, output);
                    }
                    else if (body[i] is Object[])
                    {
                        output = ReadLocals(arglist, output, (Object[])body[i], 0);
                    }
                    else if (body[i] is InstructionList)
                    {
                        output = ReadLocals(arglist, output, ((InstructionList)body[i]).list, 0);
                    }
                }
            } catch (NullReferenceException) { }
            return output;
        }

        public static Symbol[] ReadLocals_Let(Object[] letlist, Symbol[] arglist, Symbol[] output)
        {
            if (letlist.Length > 0)
            {
                foreach(var elt in letlist)
                {
                    if (elt is Object[])
                    {
                        Object[] innerletlist = (Object[])elt;
                        output = ReadLocals_InnerLet(innerletlist, arglist, output);
                    }
                }
            }
            return output;
        }

        public static Symbol[] ReadLocals_InnerLet(Object[] innerLetList, Symbol[] arglist, Symbol[] output)
        {
            if (innerLetList.Length > 1)
            {
                if (innerLetList[0] is Symbol)
                {
                    Symbol letvar = (Symbol)innerLetList[0];
                    if (!ReadLocals_Symbol(letvar, arglist, output, innerLetList, 0))
                    {
                        innerLetList[0] = new LocalRef(arglist.Length + output.Length, letvar);
                        Symbol[] newoutput = new Symbol[output.Length + 1];
                        Array.Copy(output, newoutput, output.Length);
                        newoutput[output.Length] = letvar;
                        output = newoutput;
                    }
                    output = ReadLocals(arglist, output, innerLetList, 1);
                }
            }
            return output;
        }

        public static Symbol[] ReadLocals_CompoundSymbol(CompoundSymbol cs, Symbol[] arglist, Symbol[] output)
        {
            if (cs.parts.Length > 0)
            {
                Object first = cs.parts[0];
                if (first is Symbol)
                {
                    Symbol token = (Symbol)first;
                    ReadLocals_Symbol(token, arglist, output, cs.parts, 0);
                }
                foreach (var part in cs.parts)
                {
                    if (part is InstructionList)
                    {
                        output = ReadLocals(arglist, output, ((InstructionList)part).list, 0);
                    }
                }
            }
            return output;
        }


        public static Boolean ReadLocals_Symbol(Symbol var, Symbol[] arglist, Symbol[] output, Object[] body, int i)
        {
            int pos = Array.FindIndex(arglist, elt => elt.Equals(var));
            if (pos >= 0)
            {
                body[i] = new LocalRef(pos, var);
                return true;
            }
            else
            {
                pos = Array.FindIndex(output, elt => elt.Equals(var));
                if (pos >= 0)
                {
                    body[i] = new LocalRef(pos + arglist.Length, var);
                    return true;
                }
            }
            return false;
        }
    }
}
