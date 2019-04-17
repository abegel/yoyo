using System;
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
                //"primitives_show", "PrimPrint",
                //"primitives_type", "PrimType",
                //"primitives_resett", "PrimResett",
                //"primitives_timer", "PrimTimer",
                //"primitives_freememory", "PrimFree<emory",
                //"primitives_not", "PrimNot",
                //"primitives_ignore", "PrimIgnore",
                //"primitives_system_quit", "PrimSystemQuit",
                //"primitives_symbolp", "PrimSymbolp",
                //"primitives_booleanp", "PrimBooleanp",
                //"primitives_colon", "PrimDelay",
                //"primitives_lambda", "PrimLambda",
                //"primitives_to_string", "PrimToString",
                //"primitives_unquote", "PrimUnquote",
                //"primitives_ilistp", "PrimIlistp",
                //"primitives_ilist_to_list", "PrimIlisttoList",
                //"primitives_list_to_ilist", "PrimListtoIlist",
                //"primitives_ascii_to_integer", "PrimA2i",
                //"primitives_integer_to_ascii", "PrimI2a",
                //"primitives_trace", "PrimTrace",
                //"primitives_untrace", "PrimUntrace",
                //"primitives_get_string", "PrimGetString",
                //"primitives_untrace_all", "PrimUntrace_all",
                //"primitives_load_sound", "PrimLoadSound",
                //"primitives_setup_sound", "PrimSetupSound",
                //"primitives_play_sound", "PrimPlaySound",
                //"primitives_stop_sound", "PrimStopSound",
                //"primitives_system_os", "PrimSystemOS",
                //"primitives_system_language", "PrimSystemLanguage",
                //"primitives_system_locale", "PrimSystemLocale",
                //"primitives_system_print", "PrimSystemPrint",
                //"primitives_system_type", "PrimSystemType"
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



    }
}
