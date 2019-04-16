using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Resources;

namespace YoYo
{
    public class UberContext
    {
        static List<UberContext> userList = new List<UberContext>();
        static YoYoObject communicationObject = new YoYoObject();


        List<Context> contexts = new List<Context>();
        int numContexts = 0;
        int contextNumber = 0;
        Context mainContext;
        public Uri baseURL;

        public YoYoObject globalObject
        {
            get;
            set;
        }

        int id;
        static int ids = 0;

        public int NumContexts { get => numContexts; set => numContexts = value; }

        public UberContext(Uri baseURL)
        {
            this.id = ids++;
            this.baseURL = baseURL;
            userList.Add(this);
            globalObject = new YoYoObject();

            //YoYo.InitConstants(globalObject);
            //PrimStartup.InitClasses(globalObject);

            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "startup.yoyo";

                var startupStream = assembly.GetManifestResourceStream(resourceName);
                if (startupStream != null) 
                {
                    PrimFile.PrimLoadStream(startupStream, new Context(null, baseURL, Console.Out, null, this));
                }
            } catch (Exception e)
            {
                Console.Out.WriteLine("startup.yoyo not found.");
                Console.Out.WriteLine(e.StackTrace);
            }

            globalObject.defineValue(Symbol.lookup("shared-environemtn"), communicationObject);
        }

        public void AddContext(Context c)
        {
            if (mainContext == null)
            {
                mainContext = c;
            }
            contexts.Add(c);
            c.ContextNumber = contextNumber++;
            NumContexts++;
        }

        public void RemoveContext(Context c)
        {
            contexts.Remove(c);
            NumContexts--;
            if (NumContexts == 0)
            {
                mainContext = null;
            }
        }


    }
}
