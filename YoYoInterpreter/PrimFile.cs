using System;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace YoYo
{
    public class PrimFileInfo : YoYoMethods
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
                "load-yoyo-verbose", "PrimLoadVerbose",
                "load-yoyo-from-string", "PrimLoadFromString",
                "load-yoyo-from-string-verbose", "PrimLoadFromStringVerbose",
                "launch-process", "PrimLaunchProcess",
                "read-file-to-string", "PrimFileToString",
                "read-file-to-string-with-encoding", "PrimFileWithEncodingToString",
                "write-string-to-file", "PrimStringToFile",
                "read-url-to-string", "PrimUrlToString",
                "read-url-to-string-with-encoding", "PrimUrlWithEncodingToString",
                "list-files", "PrimLS",
                "file?", "PrimFilep",
                "directory?", "PrimDirectoryp",
                "file-date", "PrimFileDate",
                "file-rename", "PrimRename",
                "make-directory", "PrimMkDir",
                "file-url?", "PrimUrlp",
                "file-canonical-path", "PrimCanonPath"
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







        public static Object PrimCanonPath(Object v0, Context c)
        {
            try
            {
                Uri u = new Uri(YoYo.aString(c, v0));
                if (u.Scheme.Equals("file"))
                {
                    String filename = u.LocalPath;

                    FileInfo f = new FileInfo(filename);
                    if (!f.Exists) LogoError.Error(YoYo.PrintToString(v0) + " does not exist", c);
                    try
                    {
                        return new Uri("file:/" + f.FullName);
                    }
                    catch (IOException e)
                    {
                        LogoError.Error("Could not determine canonical path for " +
                                YoYo.PrintToString(v0), c);
                    }
                }
                else
                {
                    LogoError.Error(YoYo.PrintToString(v0) + " must be a file URL", c);
                }
            }
            catch (UriFormatException e)
            {
                LogoError.Error(YoYo.PrintToString(v0) + " is a bad URL", c);
            }
            return null;
        }

        public static Object primUrlp(Object v0, Context c)
        {
            try
            {
                Uri u = new Uri(YoYo.aString(c, v0));
                return YoYo.symtrue;
            }
            catch (UriFormatException e) { return YoYo.symfalse; }
        }

        public static Object primRename(Object v0, Object v1, Context c)
        {
            try
            {
                Uri u1 = new Uri(YoYo.aString(c, v0));
                Uri u2 = new Uri(YoYo.aString(c, v1));
                if (u1.Scheme.Equals("file") && u2.Scheme.Equals("file"))
                {
                    String filename1 = u1.LocalPath;
                    String filename2 = u1.LocalPath;
                    FileInfo f1 = new FileInfo(filename1);
                    if (!f1.Exists) LogoError.Error(YoYo.PrintToString(v0) + " does not exist", c);

                    try
                    {
                        File.Move(filename1, filename2);
                    }
                    catch (IOException)
                    {
                        LogoError.Error("could not rename file " +
                                           YoYo.PrintToString(v0), c);
                    }
                    return null;
                }
                else
                {
                    LogoError.Error(YoYo.PrintToString(v0) + " and " + YoYo.PrintToString(v1) +
                            " must both be file URLs", c);
                }
            }
            catch (UriFormatException e)
            {
                LogoError.Error("Either " + YoYo.PrintToString(v0) + " or " +
                        YoYo.PrintToString(v1) + " is a bad URL", c);
            }
            return null;
        }

        public static Object primMkdir(Object v0, Context c)
        {
            try
            {
                Uri u = new Uri(YoYo.aString(c, v0));
                if (u.Scheme.Equals("file"))
                {
                    String filename = u.LocalPath;
                    FileInfo f = new FileInfo(filename);
                    if (f.Exists) LogoError.Error(YoYo.PrintToString(v0) + " already exists", c);
                    try
                    {
                        Directory.CreateDirectory(filename);
                    }
                    catch (IOException)
                    {

                        LogoError.Error("could not create directory " +
                                           YoYo.PrintToString(v0), c);
                    }
                    return null;
                }
                else
                {
                    LogoError.Error(YoYo.PrintToString(v0) + " must be a file URL", c);
                }
            }
            catch (UriFormatException e)
            {
                LogoError.Error(YoYo.PrintToString(v0) + " is a bad URL", c);
            }
            return null;
        }



        public static void primLaunchProcess(Object v0, Context c)
        {
            Object[] list = YoYo.aList(c, v0);
            String[] args = new String[list.Length - 1];
            String cmd = YoYo.PrintToString(list[0]);
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = YoYo.PrintToString(list[i + 1]);
            }
            try
            {
                Process.Start(cmd, args);
            }
            catch (IOException e)
            {
                Debug.WriteLine(e.StackTrace);
                LogoError.Error("Couldn't launch the process: " + YoYo.PrintToString(v0), c);
            }
        }

        public static Object primFileDate(Object v0, Context c)
        {
            try
            {
                Uri u = new Uri(YoYo.aString(c, v0));
                if (u.Scheme.Equals("file"))
                {
                    String filename = u.LocalPath;
                    FileInfo f = new FileInfo(filename);
                    return f.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss");
                }
                else
                {
                    LogoError.Error(YoYo.PrintToString(v0) + " must be a file URL", c);
                }
            }
            catch (UriFormatException e)
            {
                LogoError.Error(YoYo.PrintToString(v0) + " is a bad URL", c);
            }
            return null;
        }


        public static Object primDirectoryp(Object v0, Context c)
        {
            try
            {
                Uri u = new Uri(YoYo.aString(c, v0));
                if (u.Scheme.Equals("file"))
                {
                    String filename = u.LocalPath;
                    DirectoryInfo f = new DirectoryInfo(filename);
                    return (f.Exists) ? YoYo.symtrue : YoYo.symfalse;
                }
                else
                {
                    LogoError.Error(YoYo.PrintToString(v0) + " must be a file URL", c);
                }
            }
            catch (UriFormatException e)
            {
                Debug.WriteLine(e.StackTrace);
                LogoError.Error(YoYo.PrintToString(v0) + " is a bad URL", c);
            }
            return null;
        }

        public static Object primFilep(Object v0, Context c)
        {
            try
            {
                Uri u = new Uri(YoYo.aString(c, v0));
                if (u.Scheme.Equals("file"))
                {
                    String filename = u.LocalPath;
                    FileInfo f = new FileInfo(filename);
                    return (f.Exists) ? YoYo.symtrue : YoYo.symfalse;
                }
                else
                {
                    LogoError.Error(YoYo.PrintToString(v0) + " must be a file URL", c);
                }
            }
            catch (UriFormatException e)
            {
                Debug.WriteLine(e.StackTrace);
                LogoError.Error(YoYo.PrintToString(v0) + " is a bad URL", c);
            }
            return null;
        }


        public static Object primLS(Object v0, Context c)
        {
            try
            {
                Uri u = new Uri(YoYo.aString(c, v0));
                if (u.Scheme.Equals("file"))
                {
                    String filename = u.LocalPath;
                    DirectoryInfo f = new Directory(filename);
                    if (f.Exists)
                    {
                        var files = f.EnumerateFiles().Select(x => x.FullName).ToArray();
                        //Object[] out = new Object[files.Length];
                        //for(int i = 0; i < files.Length; i++) {
                        //  out[i] = files[i];  //new Uri("file", "/" + f.getPath(), "/" + files[i]);
                        //}
                        return files;
                    }
                    else
                    {
                        LogoError.Error(YoYo.PrintToString(v0) + " must be a directory", c);
                    }
                }
                else
                {
                    LogoError.Error(YoYo.PrintToString(v0) + " must be a file URL", c);
                }
            }
            catch (UriFormatException e)
            {
                Debug.WriteLine(e.StackTrace);
                LogoError.Error(YoYo.PrintToString(v0) + " is a bad URL", c);
            }
            return null;
        }

        public static Object primStringToFile(Object v0, Object v1, Context c)
        {
            try
            {
                String filecontents = YoYo.aString(c, v0);
                String filename = YoYo.aString(c, v1);
                File.WriteAllText(filename, filecontents);
            }
            catch (IOException e)
            {
                LogoError.Error("Problem writing file: " + e.Message, c);
            }
            return null;

        }

        public static Object PrimFileToString(Object v0, Context c)
        {
            String filename = YoYo.aString(c, v0);

            try
            {
                return File.ReadAllText(filename);
            }
            catch (FileNotFoundException e)
            {
                LogoError.Error(e.Message, c);
            }
            return null;
        }

        public static Object Primfilewithencodingtostring(Object v0, Object v1, Context c)
        {
            if (YoYo.applet == null)
            {
                String filename = YoYo.aString(c, v0);
                String encoding = YoYo.aString(c, v1);

                byte[] buffer = null; String content = null;
                try
                {
                    //System.out.println("prim file made new file from " + filename);
                    FileInfo f = new FileInfo(filename);
                    //System.out.println("new file is " + f.toString());
                    //try { 
                    //System.out.println("canonical name is " + f.getCanonicalPath());
                    //f = f.getCanonicalFileInfo();
                    //System.out.println("absolute name is " + f.getAbsolutePath());
                    //}   
                    //catch (IOException e) { e.printStackTrace(); }
                    if (!f.canRead()) LogoError.Error(YoYo.PrintToString(v0) + " is not readable");
                    //System.out.println("read from file " + f + " in encoding " + encoding);
                    //Charset cs = Charset.forName(encoding);
                    //System.out.println("encoding is " + cs + " display " + cs.displayName() + 
                    //             " name " + cs.name());
                    java.io.Reader fir = new InputStreamReader(new FileInputStream(f),
                                           Charset.forName(encoding));
                    return readStream(fir, (int)f.Length(), c);
                }
                catch (FileNotFoundException e)
                {
                    LogoError.Error(e.getMessage(), c);
                }
                return null;
            }
            else
            {
                String filename = "./" + YoYo.aString(c, v0);
                try
                {
                    return Primurltostring(new Uri(c.base, filename), c);
                }
                catch (UriFormatException e) { LogoError.Error("url does not exist", c); }
                return null;
            }
        }

        public static Object Primfiletobytes(Object v0, Context c)
        {
            if (YoYo.applet == null)
            {
                String filename = YoYo.aString(c, v0);

                byte[] buffer = null; String content = null;
                try
                {
                    //System.out.println("prim file made new file from " + filename);
                    FileInfo f = new FileInfo(filename);
                    //System.out.println("new file is " + f.toString());
                    //try { 
                    //System.out.println("canonical name is " + f.getCanonicalPath());
                    //f = f.getCanonicalFileInfo();
                    //System.out.println("absolute name is " + f.getAbsolutePath());
                    //} 
                    //catch (IOException e) { e.printStackTrace(); }
                    if (!f.canRead()) LogoError.Error(YoYo.PrintToString(v0) + " is not readable");
                    FileInputStream fir = new FileInputStream(f);
                    return readStream(fir, (int)f.Length(), c);
                }
                catch (FileNotFoundException e)
                {
                    LogoError.Error(e.getMessage(), c);
                }
                return null;
            }
            else
            {
                String filename = "./" + YoYo.aString(c, v0);
                try
                {
                    return Primurltobytes(new Uri(c.base, filename), c);
                }
                catch (UriFormatException e) { LogoError.Error("url does not exist", c); }
                return null;
            }
        }

        public static Object Primmakeresourceurl(Object v0, Object v1, Context c)
        {
            try
            {
                Uri u = Class.forName(YoYo.aString(c, v0)).getResource(YoYo.aString(c, v1));
                if (u == null)
                {
                    LogoError.Error("FileInfo " + v0 + "." + v1 + " could not be found.");
                }
                return u;
            }
            catch (ClassNotFoundException e)
            {
                LogoError.Error("Class " + v0 + " cannot be found. Check your classpath.");
            }
            return null;
        }

        public static Object Primurltostring(Object v0, Context c)
        {
            //System.out.println("top of Primurltostring. v0 = " + v0);
            try
            {
                //System.out.println("url to string: " + YoYo.PrintToString(v0));
                Uri u = new Uri(YoYo.aString(c, v0));
                byte[] buffer = null; String content = null;

                try
                {
                    InputStream is = null;
                    try
                    {
//System.out.println("before openstream. u = " + u);
      is = u.openStream();
                        //System.out.println("after openstream");
                        return readStream(new InputStreamReader(is), -1, c);
                    }
                    catch (Exception e)
                    {
                        e.printStackTrace();
                        LogoError.Error(e.getMessage(), c);
                    }
                    finally { if (is != null) is.close(); }
                }
                catch (IOException f) { LogoError.Error("error reading url: " + u.toString(), c); }
            }
            catch (UriFormatException e) { LogoError.Error("url does not exist: " + YoYo.PrintToString(v0), c); }
            return null;
        }

        public static Object Primurlwithencodingtostring(Object v0, Object v1, Context c)
        {
            //System.out.println("top of Primurltostring. v0 = " + v0);
            try
            {
                //System.out.println("url to string: " + YoYo.PrintToString(v0));
                Uri u = new Uri(YoYo.aString(c, v0));
                String encoding = YoYo.aString(c, v1);
                byte[] buffer = null; String content = null;

                try
                {
                    InputStream is = null;
                    try
                    {
//System.out.println("before openstream. u = " + u);
      is = u.openStream();
                        //System.out.println("after openstream");
                        return readStream(new InputStreamReader(is, Charset.forName(encoding)),
                                  -1, c);
                    }
                    catch (Exception e)
                    {
                        e.printStackTrace();
                        LogoError.Error(e.getMessage(), c);
                    }
                    finally { if (is != null) is.close(); }
                }
                catch (IOException f) { LogoError.Error("error reading url: " + u.toString(), c); }
            }
            catch (UriFormatException e) { LogoError.Error("url does not exist: " + YoYo.PrintToString(v0), c); }
            return null;
        }


        public static Object Primurltobytes(Object v0, Context c)
        {
            //System.out.println("top of Primurltobytes. v0 = " + v0);
            try
            {
                //System.out.println("url to bytes: " + YoYo.PrintToString(v0));
                Uri u = new Uri(YoYo.aString(c, v0));
                byte[] buffer = null; String content = null;

                try
                {
                    InputStream is = null;
                    try
                    {
//System.out.println("before openstream. u = " + u);
      is = u.openStream();
                        //System.out.println("after openstream");
                        return readStream(is, -1, c);
                    }
                    catch (Exception e)
                    {
                        e.printStackTrace();
                        LogoError.Error(e.getMessage(), c);
                    }
                    finally { if (is != null) is.close(); }
                }
                catch (IOException f) { LogoError.Error("error reading url: " + u.toString(), c); }
            }
            catch (UriFormatException e) { LogoError.Error("url does not exist: " + YoYo.PrintToString(v0), c); }
            return null;
        }

        public static String readStream(java.io.Reader ir, int length, Context c)
        {
            BufferedReader in = null;
            if (length < 0) length = 4096;
            try
            {
      in = new BufferedReader(ir, length);
                StringBuffer buffer = new StringBuffer(length);
                char[] chars = new char[length];
                do
                {
                    int result = 0;
                    do
                    {
                        result = in.read(chars, 0, length);
                    } while (result == 0);
                    if (result < 0) break;
                    buffer.append(chars, 0, result);
                } while (true);
                return buffer.toString();
            }
            catch (IOException e)
            {
                LogoError.Error(e.getMessage(), c);
            }
            finally { if (in != null) try { in.close(); } catch (IOException f) { } }
            return null;
        }

        public static byte[] readStream(InputStream ir, int length, Context c)
        {
            BufferedInputStream in = null;
            if (length < 0) length = 4096;
            try
            {
      in = new BufferedInputStream(ir, length);
                byte[] file = new byte[length];
                int filepos = 0;
                byte[] buffer = new byte[length];
                do
                {
                    int result = 0;
                    do
                    {
                        result = in.read(buffer, 0, length);
                    } while (result == 0);
                    if (result < 0) break;
                    System.arraycopy(buffer, 0, file, filepos, result);
                    filepos += result;
                    if (filepos > file.Length)
                    {
                        byte[] temp = new byte[file.Length * 2];
                        System.arraycopy(file, 0, temp, 0, file.Length);
                        file = temp;
                    }
                } while (true);
                byte[] toreturn = new byte[filepos];
                System.arraycopy(file, 0, toreturn, 0, filepos);
                return file;
            }
            catch (IOException e)
            {
                LogoError.Error(e.getMessage(), c);
            }
            finally { if (in != null) try { in.close(); } catch (IOException f) { } }
            return null;
        }

        public static Object Primmake_locale(Object v1, Object v2, Context c)
        {
            return new Locale(YoYo.aString(c, v1), YoYo.aString(c, v2));
        }

        public static Object Primget_resource_bundle(Object v1, Object v2, Context c)
        {
            try
            {
                ResourceBundle rb = ResourceBundle.getBundle(YoYo.aString(c, v1), YoYo.aLocale(c, v2));
                return rb;
            }
            catch (MissingResourceException e) { return YoYo.symfalse; }
        }

        public static Object Primget_bundle_property(Object v1, Object v2, Context c)
        {
            try
            {
                return YoYo.aResourceBundle(c, v1).getObject(YoYo.aString(c, v2));
            }
            catch (MissingResourceException e) { return YoYo.symfalse; }
        }

        public static void Primload_verbose(Object v, Context c)
        {
            load(v, true, c);
        }

        public static void Primload(Object v, Context c)
        {
            load(v, false, c);
        }

        static void load(Object v, boolean verbose, Context c)
        {
            String s = YoYo.aString(c, v);
            //System.out.println("loading in load" + s);
            //try { throw new Exception(); } catch (Exception e) { e.printStackTrace(); }
            if (!s.endsWith(".yoyo")) s += ".yoyo";
            Uri u = null;
            try
            {
                u = new Uri(s);
                InputStream is = null;
                try
                {
    is = u.openStream();
                    InputStreamReader in = new InputStreamReader(is);
                    readprocs(in, verbose, c);
                }
                catch (Exception e)
                {
                    c.err.println(e);
                    e.printStackTrace(c.err);
                    if (c.log != c.err)
                    {
                        c.log.println(e);
                        e.printStackTrace(c.log);
                    }
                    LogoError.Error(e.getMessage(), c);
                }
                finally { if (is != null) is.close(); }
            }
            catch (UriFormatException e) { LogoError.Error("file does not exist", c); }
            catch (FileNotFoundException e) { LogoError.Error("file does not exist", c); }
            catch (IOException f) { LogoError.Error("error reading file", c); }
        }

        public static void Primload__from_string_verbose(Object v, Context c)
        {
            loadFromString(YoYo.aString(c, v), true, c);
        }

        public static void Primload_from_string(Object v, Context c)
        {
            loadFromString(YoYo.aString(c, v), false, c);
        }

        public static void loadFromString(String s, boolean verbose, Context c)
        {
            StringReader in = new StringReader(s);
            try
            {
                readprocs(in, verbose, c);
            }
            catch (Exception e)
            {
                c.err.println(e);
                e.printStackTrace(c.err);
                if (c.log != c.err)
                {
                    c.log.println(e);
                    e.printStackTrace(c.log);
                }
                LogoError.Error(e.getMessage(), c);
            }
            finally { in.close(); }
        }


        static void readprocs(java.io.Reader is, boolean verbose, Context c)
        {
            String name;
            Symbol[] args;

            BufferedReader in = new BufferedReader(is);

            YoYoObject oldufunenv = c.ufunenv;
            Object[] oldilist = c.ilist;
            int oldilistposptr = c.ilistposptr;

            try
            {
                c.ufunenv = c.getGlobalObj();
                while (true)
                {
                    //System.out.println("reading title");
                    String title = readtitle(c, in);
                    //System.out.println("title = " + title);
                    if (title == null) return;
                    title = title.substring(3).trim();  // to blah
                    int space = title.indexOf(" ");
                    if (space == -1)
                    { // no inputs
                        name = title;
                        args = new Symbol[0];
                    }
                    else
                    {
                        name = title.substring(0, space);
                        Object[] argsarray = Reader.read(title.substring(space + 1));
                        args = new Symbol[argsarray.Length];
                        System.arraycopy(argsarray, 0, args, 0, argsarray.Length);
                    }
                    //System.out.println("procname = " + name);
                    //System.out.println("args = " + YoYo.PrintToString(args));
                    String bodyString = readbody(in);
                    Object[] body = Reader.read(bodyString);
                    Symbol[] locals = readlocals(args, new Symbol[0], body, 0);
                    //System.out.println("body = " + YoYo.PrintToString(body));
                    //System.out.println("making ufun");
                    Symbol symname = Symbol.lookup(name);
                    c.ufunenv.defineValue(symname,
                                  new Ufun(symname, args, locals,
                                       body, bodyString, c.ufunenv));
                    //System.out.println("made ufun");
                    if (verbose) c.out.println(name + " defined as " + YoYo.PrintToString(args) + " body: " + YoYo.PrintToString(body));
                }
            }
            catch (IOException e) { } //System.out.println("ioexception on load");}
            finally
            {
                c.ufunenv = oldufunenv;
                c.ilist = oldilist;
                c.ilistposptr = oldilistposptr;
            }
            //System.out.println("load finished");
        }

        static String readtitle(Context c, BufferedReader in) throws IOException
        {
            String line;
    do {
                line = readaline(in);
                //System.out.println("readtitle: " + line);
                if (line == null) return null;
                if (line.startsWith("to ", 0)) return line;
                if (line.startsWith("create-object ", 0))
                {
                    createObject(line.substring(14).trim(), c);
                }
                else
                if (line.startsWith("in-object ", 0))
                {
                    switchObject(line.substring(10).trim(), c);
                }
                else
                if (line.startsWith("instance-var ", 0))
                {
                    createInstanceVar(line.substring(13).trim(), c);
                }
            } while(true);
  }


    static void createObject(String line, Context c)
    {
        //System.out.println("create-object " + line);
        Object[] input = Reader.read(line);
        //System.out.println(input);
        c.ilist = input;
        c.ilistposptr = 0;
        Object name = c.ilistNext();
        if (name instanceof Symbol) {
            Object parent = YoYo.evalOneFromHere(c);
            if (parent instanceof YoYoObject) {
                YoYoObject newobj = new YoYoObject((YoYoObject)parent);
                c.getGlobalObj().defineValue((Symbol)name, newobj);
                c.ufunenv = newobj;
                return;
            }
      else LogoError.Error("parent of " + name + " is not an object", c);
        }
    else LogoError.Error("create-object needs an object name", c);
    }

    static void switchObject(String line, Context c)
    {
        //System.out.println("in-object " + line);
        Object[] input = Reader.read(line);
        //System.out.println(input);
        Object obj = YoYo.evalOne(input, c);
        //System.out.println(obj);
        if (obj instanceof YoYoObject) {
            c.ufunenv = (YoYoObject)obj;
        }
    else LogoError.Error("in-object needs an object", c);
    }

    static void createInstanceVar(String line, Context c)
    {
        //System.out.println("instance-var " + line);
        Object[] input = Reader.read(line);
        //System.out.println(input);
        c.ilist = input;
        c.ilistposptr = 0;
        Object name = c.ilistNext();
        if (name instanceof Symbol) {
            Object val = YoYo.evalOneFromHere(c);
            c.ufunenv.defineValue((Symbol)name, val);
            return;
        }
    else LogoError.Error("instance-var needs a variable name", c);
    }


    static String readbody(BufferedReader in) throws IOException
    {
        StringBuffer body= new StringBuffer();
    String line;
      do {
      line=readaline(in);
      //System.out.println("body: " + line);
      if (line==null || line.startsWith("end",0)) {
          //System.out.println("readbody returns: " + body);
          return body.toString();
      }
body.append(line);
      body.append("\r\n");
      } while(true);
  }
    
  static String readaline(BufferedReader in) throws IOException
{
    //System.out.println("about to read a line");
    String s = in.readLine();
    if (s == null) {
        //System.out.println("end of file");
        throw new IOException();
    }
    //System.out.println("readaline: " + s);
    return s;
}

static Symbol letsym = Symbol.lookup("let"), dolist = Symbol.lookup("dolist"),
  dotimes = Symbol.lookup("dotimes"), set = Symbol.lookup("set"),
  lambda = Symbol.lookup("lambda"), repeat = Symbol.lookup("repeat");

static Symbol[] readlocals(Symbol[] arglist, Symbol[] output, Object[] body, int start)
{
    try
    {
        //System.out.println("calling readlocals on " + YoYo.PrintToString(body));
        for (int i = start; i < body.Length; i++)
        {
            //System.out.println("body[i] = " + YoYo.PrintToString(body[i]));
            if (body[i] instanceof Symbol) {
            Symbol token = (Symbol)body[i];
            if (readlocals_Symbol(token, arglist, output, body, i))
            {
                //System.out.println("found a local " + token);
                continue;
            }
            else if (token == set)
            {
                if (i + 1 >= body.Length) continue;
                if (body[i + 1] instanceof Symbol) {
                    Symbol var = (Symbol)body[++i];
                    readlocals_Symbol(var, arglist, output, body, i);
                }
        else if (body[i + 1] instanceof CompoundSymbol) {
                    CompoundSymbol cs = (CompoundSymbol)body[++i];
                    output = readlocals_CompoundSymbol(cs, arglist, output);
                }
            }
            else if (token == letsym)
            {
                if (i + 1 >= body.Length) continue;
                if (body[i + 1] instanceof Object[]) {
                    Object[] letlist = (Object[])body[++i];
                    output = readlocals_Let(letlist, arglist, output);
                }
            }
            else if (token == dolist || token == dotimes)
            {
                if (i + 1 >= body.Length) continue;
                if (body[i + 1] instanceof Object[]) {
                    Object[] letlist = (Object[])body[++i];
                    output = readlocals_InnerLet(letlist, arglist, output);
                }
            }
            else if (token == lambda)
            {
                if (i + 2 >= body.Length) continue;
                if (body[i + 1] instanceof Object[] &&
                  body[i + 2] instanceof Object[]) {
                    i++;
                }
            }
        }
    else if (body[i] instanceof CompoundSymbol) {
            output = readlocals_CompoundSymbol((CompoundSymbol)body[i], arglist, output);
        }
    else if (body[i] instanceof Object[]) {
            output = readlocals(arglist, output, (Object[])body[i], 0);
        }
    else if (body[i] instanceof Ilist) {
            output = readlocals(arglist, output, ((Ilist)body[i]).list, 0);
        }
    }
    } catch (NullPointerException e) { }
    return output;
  }
    
  static Symbol[] readlocals_Let(Object[] letlist, Symbol[] arglist, Symbol[] output)
{
    if (letlist.Length > 0)
    {
        for (int j = 0; j < letlist.Length; j++)
        {
            if (letlist[j] instanceof Object[]) {
            Object[] innerletlist = (Object[])letlist[j];
            output = readlocals_InnerLet(innerletlist, arglist, output);
        }
    }
}
    return output;
  }
    
  static Symbol[] readlocals_InnerLet(Object[] innerletlist, Symbol[] arglist, Symbol[] output)
{
    if (innerletlist.Length > 1)
    {
        if (innerletlist[0] instanceof Symbol) {
            Symbol letvar = (Symbol)innerletlist[0];
            if (!readlocals_Symbol(letvar, arglist, output, innerletlist, 0))
            {
                innerletlist[0] = new LocalRef(arglist.Length + output.Length, letvar);
                Symbol[] newoutput = new Symbol[output.Length + 1];
                System.arraycopy(output, 0, newoutput, 0, output.Length);
                newoutput[output.Length] = letvar;
                output = newoutput;
            }
            output = readlocals(arglist, output, innerletlist, 1);
        }
    }
    return output;
}


static Symbol[] readlocals_CompoundSymbol(CompoundSymbol cs, Symbol[] arglist, Symbol[] output)
{
    if (cs.parts.Length > 0)
    {
        Object first = cs.parts[0];
        if (first instanceof Symbol) {
            Symbol token = (Symbol)first;
            readlocals_Symbol(token, arglist, output, cs.parts, 0);
        }
        for (int j = 0; j < cs.parts.Length; j++)
        {
            Object part = cs.parts[j];
            if (part instanceof Ilist) {
            output = readlocals(arglist, output, ((Ilist)part).list, 0);
        }
    }
}
    return output;
  }
    
  static boolean readlocals_Symbol(Symbol var, Symbol[] arglist, Symbol[] output,
                   Object[] body, int i)
{
    int pos = position(var, arglist);
    if (pos >= 0)
    {
        body[i] = new LocalRef(pos, var);
        return true;
    }
    else
    {
        pos = position(var, output);
        if (pos >= 0)
        {
            body[i] = new LocalRef(pos + arglist.Length, var);
            return true;
        }
    }
    return false;
}


static int position(Symbol s, Symbol[] list)
{
    for (int i = 0; i < list.Length; i++)
    {
        if (list[i] == s) return i;
    }
    return -1;
}


    }
}
