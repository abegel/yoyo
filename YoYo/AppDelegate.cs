using System;
using System.IO;
using System.Text;
using AppKit;
using Foundation;

namespace YoYo
{
    [Register("AppDelegate")]
    public class AppDelegate : NSApplicationDelegate
    {
        public static string bootfile = null;
        public static string listtorun = null;
        public static UberContext ub = null;
        public static Uri baseURL = null;
        public Context c;


        public AppDelegate()
        {
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            // Insert code here to initialize your application
            string[] args = NSProcessInfo.ProcessInfo.Arguments;
            InitArgs(args);
            baseURL = NSFileManager.DefaultManager.GetHomeDirectoryForCurrentUser();

            ub = new UberContext(baseURL);
            c = new Context(null, baseURL, Console.Out, null, ub);

            if (bootfile != null && bootfile.Length > 0)
            {
                LoadBootFile();
                Run();
            }
        }

        public void InitArgs(String[] args)
        {
            if (args.Length > 2) bootfile = args[2];
            StringBuilder sb = new StringBuilder();
            if (args.Length > 3) sb.Append(args[3]);
            for (int i = 4; i < args.Length; i++)
            {
                sb.Append(" ");
                sb.Append(args[i]);
            }
            listtorun = sb.ToString();
        }

        public void LoadBootFile()
        {
            Object[] ilist = Reader.Read("load \"" + bootfile + "\"");
            c.RunList(ilist);
            c.WaitUntilDone();
        }

        public void Run()
        {
            if (listtorun != null && listtorun.Length > 0)
            {
                Object[] ilist = Reader.Read(listtorun);
                c.RunList(ilist);
            }
        }


        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}
