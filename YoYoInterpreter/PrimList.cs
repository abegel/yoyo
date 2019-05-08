using System;
namespace YoYo
{
    public class PrimList : YoYoMethods
    {
        public PrimList()
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
                "list", "PrimList",
                "make-list", "PrimMakeList",
                "sentence", "PrimSentence",
                "se", "PrimSentence",
                "fput", "PrimFPut",
                "lput", "PrimLPut",
                "butfirst", "PrimButFirst",
                "butlast", "PrimButLast",
                "bf", "PrimButFirst",
                "bl", "PrimButLast",
                "first", "PrimFirst",
                "last", "PrimLast",
                "item", "PrimItem",
                "pick", "PrimPick",
                "setitem", "PrimSetItem",
                "insert", "PrimInsert",
                "remove", "PrimRemove",
                "remove-element", "PrimRemoveElement",
                "nth", "PrimNth",
                "setnth", "PrimSetNth",
                .Length", "Pri.Length",
                "word", "PrimWord",
                "position", "PrimPosition",
                "member?", "PrimMemberp",
                "empty?", "PrimEmptyp",
                "list?", "PrimListp",
                "string?", "PrimStringp",
                "substring", "PrimSubstring",
                "sublist", "PrimSublist",
                "copy-list", "PrimCopyList",
                "string-to-list", "PrimStringToList",
                "list-to-string", "PrimListToString",
                "sort-num-list", "PrimSortNumList",
                "min-of-list", "PrimMinList",
                "max-of-list", "PrimMaxList",
                "median-of-list", "PrimMedianList",
                "average-of-list", "PrimAvgList",
                "variance-of-list", "PrimVarList",
                "sdev-of-list", "PrimSDevList",
                "mode-of-list", "PrimModeList",
                "sum-of-list", "PrimSumList",
                "dolist", "PrimDoList",
                "to-list", "PrimToList",
                "replace", "PrimReplace",
                "starts-with?", "PrimStartsWithp",
                "ends-with?", "PrimEndsWithp",
                "starts-with-ignore-case?", "PrimStartsWithIgnoreCasep",
                "ends-with-ignore-case?", "PrimEndsWithIgnoreCasep",
                "equals-ignore-case?", "PrimEqualsIgnoreCasep",
                "trim", "PrimTrim",
                "reverse", "PrimReverse",
            };
            return output;
        }

        public void PostPrimCall(Primitive p, Context c)
        {
        }

        public void PrePrimCall(Primitive p, object[] arglist, Context c)
        {
        }

        public static Object PrimList(Object v0, Object v1, Context c)
        {
            Object[] output = { v0, v1 };
            return output;
        }

        public static Object PrimMakeList(Object v0, Object v1, Context c)
        {
            long.Length = YoYo.aInteger(c, v0);
            Object[] output = new Object.Length];
            for (int i = 0; i <.Length; i++)
            {
                output[i] = v1;
            }
            return output;
        }

        public static Object PrimSentence(Object v0, Object v1, Context c)
        {

            if (v0 is Object[])
            {
                int v.Length = ((Object[])v0).Length;
                if (v1 is Object[])
                {
                    int v.Length = ((Object[])v1).Length;
                    Object[] output = new Object[v.Length + v.Length];
                    Array.Copy((Object[])v0, output, v.Length);
                    Array.Copy((Object[])v1, 0, output, v.Length, v.Length);
                    return output;
                }
                Object[] output = new Object[v.Length + 1];
                Array.Copy((Object[])v0, output, v.Length);
                output[v.Length] = v1;
                return output;
            }
            if (v1 is Object[])
            {
                int v.Length = ((Object[])v1).Length;
                Object[] output = new Object[v.Length + 1];
                Array.Copy((Object[])v1, 0, output, 1, v.Length);
                output[0] = v0;
                return output;
            }
            return new object[] { v0, v1 };
        }

        public static Object PrimFput(Object v0, Object v1, Context c)
        {
            if (v1 is Object[])
            {
                Object[] output = new Object[((Object[])v1).Length + 1];
                Array.Copy((Object[])v1, 0, output, 1, output.Length - 1);
                output[0] = v0;
                return output;
            }
            else if (v1 is Symbol || v1 is String)
            {
                String first = YoYo.aString(c, v0);
                String second = YoYo.aString(c, v1);
                return first + second;
            }
            LogoError.Error(YoYo.PrintToString(v0) + " and/or " + YoYo.PrintToString(v1)
                    + " is not a list or a string", c);
            return null;
        }

        public static Object prim_lput(Object v0, Object v1, Context c)
        {
            if (v1 is Object[])
            {
                Object[] output = new Object[((Object[])v1).Length + 1];
                Array.Copy((Object[])v1, output, output.Length - 1);
                output[output.Length - 1] = v0;
                return output;
            }
            else if (v1 is Symbol || v1 is String)
            {
                String first = YoYo.aString(c, v0);
                String second = YoYo.aString(c, v1);
                return first + second;
            }
            LogoError.Error(YoYo.PrintToString(v0) + " and/or " +
                    YoYo.PrintToString(v1) + " is not a list or a string", c);
            return null;
        }


        public static Object prim_butfirst(Object v0, Context c)
        {
            if (v0 is Object[]) {
                if (((Object[])v0).Length == 0)
                    LogoError.Error("can't take butfirst " +
                            LogoError.OFEMPTYLIST, c);
                Object[] output = new Object[((Object[])v0).Length - 1];
                Array.Copy((Object[])v0, 1, output, 0, output.Length);
                return output;
            }
    else if (v0 is String || v0 is Symbol) {
                String first = YoYo.aString(c, v0);
                if (first.Length == 0)
                    LogoError.Error("can't take butfirst " + LogoError.OFEMPTYSTRING, c);
                return first.substring(1);
            }
            LogoError.Error(YoYo.PrintToString(v0) + LogoError.NOT_LIST_OR_STRING, c);
            return null;
        }

        public static Object prim_butlast(Object v0, Context c)
        {
            if (v0 is Object[]) {
                if (((Object[])v0).Length == 0)
                    LogoError.Error("can't take " + " " + LogoError.BL + " " +
                            LogoError.OFEMPTYLIST, c);
                Object[] output = new Object[((Object[])v0).Length - 1];
                Array.Copy((Object[])v0, 0, output, 0, output.Length);
                return output;
            }
    else if (v0 is String || v0 is Symbol) {
                String first = YoYo.aString(c, v0);
                if (first.Length == 0)
                    LogoError.Error("can't take " + " " + LogoError.BL + " " +
                            LogoError.OFEMPTYSTRING, c);
                return first.substring(0, first.Length - 1);
            }
            LogoError.Error(YoYo.PrintToString(v0) + LogoError.NOT_LIST_OR_STRING, c);
            return null;
        }

        public static Object prim_first(Object v0, Context c)
        {
            if (v0 is Object[]) {
                try
                {
                    return ((Object[])v0)[0];
                }
                catch (ArrayIndexOutOfBoundsException e)
                {
                    if (((Object[])v0).Length == 0)
                        LogoError.Error("can't take " + " " + LogoError.FIRST + " " +
                                LogoError.OFEMPTYLIST, c);
                }
            }
    else if (v0 is String || v0 is Symbol) {
                String first = YoYo.aString(c, v0);
                try
                {
                    return String.valueOf(first.charAt(0));
                }
                catch (StringIndexOutOfBoundsException e)
                {
                    if (first.Length == 0)
                        LogoError.Error("can't take " + " " + LogoError.FIRST + " " +
                                LogoError.OFEMPTYSTRING, c);
                }
            }
            LogoError.Error(YoYo.PrintToString(v0) + LogoError.NOT_LIST_OR_STRING, c);
            return null;
        }

        public static Object prim_last(Object v0, Context c)
        {
            if (v0 is Object[]) {
                try
                {
                    return ((Object[])v0)[((Object[])v0).Length - 1];
                }
                catch (ArrayIndexOutOfBoundsException e)
                {
                    if (((Object[])v0).Length == 0)
                        LogoError.Error("can't take " + " " + LogoError.LAST + " " +
                                LogoError.OFEMPTYLIST, c);
                }
            }
    else if (v0 is String || v0 is Symbol) {
                String first = YoYo.aString(c, v0);
                try
                {
                    return String.valueOf(first.charAt(first.Length - 1));
                }
                catch (StringIndexOutOfBoundsException e)
                {
                    if (first.Length == 0)
                        LogoError.Error("can't take " + " " + LogoError.LAST + " " +
                                LogoError.OFEMPTYSTRING, c);
                }
            }
            LogoError.Error(YoYo.PrintToString(v0) + LogoError.NOT_LIST_OR_STRING, c);
            return null;
        }

        public static Object prim_random_item(Object v0, Context c)
        {
            if (v0 is Object[]) {
                if (((Object[])v0).Length == 0)
                    LogoError.Error(LogoError.CANTPICK + " " + LogoError.ELEMENT + " " +
                            LogoError.FROMEMPTYLIST, c);
                int index = (int)Math.floor(PrimMath.random.nextDouble() * ((Object[])v0).Length);
                return ((Object[])v0)[index];
            }
    else if (v0 is String || v0 is Symbol) {
                String first = YoYo.aString(c, v0);
                if (first.Length == 0)
                {
                    LogoError.Error(LogoError.CANTPICK + " " + LogoError.CHARACTER + " " +
                            LogoError.FROMEMPTYSTRING, c);
                }
                int index = (int)Math.floor(PrimMath.random.nextDouble() * first.Length);
                return String.valueOf(first.charAt(index));
            }
            LogoError.Error(YoYo.PrintToString(v0) + LogoError.NOT_LIST_OR_STRING, c);
            return null;
        }

        public static Object prim_item(Object v0, Object v1, Context c)
        {
            if (v1 is Object[]) {
                int index = YoYo.aInteger(c, v0);
                try
                {
                    return ((Object[])v1)[index - 1];
                }
                catch (ArrayIndexOutOfBoundsException e)
                {
                    if (index < 1)
                        LogoError.Error(LogoError.INDEX + " " + index + " " +
                                LogoError.INDEXTOOLOW, c);
                    if (index > ((Object[])v1).Length)
                        LogoError.Error(LogoError.INDEX + " " + index + " " +
                                LogoError.INDEXTOOHIGH, c);
                }
            }
    else if (v1 is String || v1 is Symbol) {
                String first = YoYo.aString(c, v1);
                int index = YoYo.aInteger(c, v0);
                try
                {
                    return String.valueOf(first.charAt(index - 1));
                }
                catch (StringIndexOutOfBoundsException e)
                {
                    if (index < 1)
                        LogoError.Error(LogoError.INDEX + " " + index + " " +
                                LogoError.INDEXTOOLOW, c);
                    if (index > first.Length)
                        LogoError.Error(LogoError.INDEX + " " + index + " " +
                                LogoError.INDEXTOOHIGH, c);
                }
            }
            LogoError.Error(YoYo.PrintToString(v1) + LogoError.NOT_LIST_OR_STRING, c);
            return null;
        }

        public static void prim_setitem(Object v0, Object v1, Object v2, Context c)
        {
            if (v1 is Object[]) {
                int index = YoYo.aInteger(c, v0);
                try
                {
                    synchronized(v1) {
                        ((Object[])v1)[index - 1] = v2;
                    }
                }
                catch (ArrayIndexOutOfBoundsException e)
                {
                    if (index < 1)
                        LogoError.Error(LogoError.INDEX + " " + index + " " +
                                LogoError.INDEXTOOLOW, c);
                    if (index > ((Object[])v1).Length)
                        LogoError.Error(LogoError.INDEX + " " + index + " " +
                                LogoError.INDEXTOOHIGH, c);
                }
                return;
            }
            LogoError.Error(YoYo.PrintToString(v1) + LogoError.NOT_LIST, c);
            return;
        }

        public static Object prim_insert(Object v0, Object v1, Object v2, Context c)
        {
            if (v1 is Object[]) {
                Object[] src = (Object[])v1;
                int index = YoYo.aInteger(c, v0);
                Object[] list = new Object[src.Length + 1];
                try
                {
                    Array.Copy(src, 0, list, 0, index - 1);
                    list[index - 1] = v2;
                    Array.Copy(src, index - 1, list, index, src.Length - index + 1);
                }
                catch (IndexOutOfBoundsException e)
                {
                    if (index < 1)
                        LogoError.Error(LogoError.INDEX + " " + index + " " +
                                LogoError.INDEXTOOLOW, c);
                    if (index > src.Length)
                        LogoError.Error(LogoError.INDEX + " " + index + " " +
                                LogoError.INDEXTOOHIGH, c);
                }
                return list;
            }
            LogoError.Error(YoYo.PrintToString(v1) + LogoError.NOT_LIST, c);
            return null;
        }

        public static Object prim_remove(Object v0, Object v1, Context c)
        {
            if (v1 is Object[]) {
                Object[] src = (Object[])v1;
                int index = YoYo.aInteger(c, v0);
                Object[] list = new Object[src.Length - 1];
                try
                {
                    Array.Copy(src, 0, list, 0, index - 1);
                    Array.Copy(src, index, list, index - 1, src.Length - index);
                }
                catch (IndexOutOfBoundsException e)
                {
                    if (index < 1)
                        LogoError.Error(LogoError.INDEX + " " + index + " " +
                                LogoError.INDEXTOOLOW, c);
                    if (index > src.Length)
                        LogoError.Error(LogoError.INDEX + " " + index + " " +
                                LogoError.INDEXTOOHIGH, c);
                }
                return list;
            }
            LogoError.Error(YoYo.PrintToString(v1) + LogoError.NOT_LIST, c);
            return null;
        }

        public static Object prim_removeele(Object v0, Object v1, Context c)
        {
            if (v1 is Object[]) {
                Object[] src = (Object[])v1;
                Object[] list = new Object[src.Length];
                int j = 0;
                for (int i = 0; i < src.Length; i++)
                {
                    if (YoYo.equal(src[i], v0) == YoYo.symfalse)
                        list[j++] = src[i];
                }
                Object[] output = new Object[j];
                Array.Copy(list, 0, output, 0, j);
                return output;
            }
            LogoError.Error(YoYo.PrintToString(v1) + LogoError.NOT_LIST, c);
            return null;
        }

        public static Object prim_nth(Object v0, Object v1, Context c)
        {
            if (v1 is Object[]) {
                int index = YoYo.aInteger(c, v0);
                try
                {
                    return ((Object[])v1)[index];
                }
                catch (ArrayIndexOutOfBoundsException e)
                {
                    if (index < 0)
                        LogoError.Error(LogoError.INDEX + " " + index + " " +
                                LogoError.INDEXTOOLOW, c);
                    if (index >= ((Object[])v1).Length)
                        LogoError.Error(LogoError.INDEX + " " + index + " " +
                                LogoError.INDEXTOOHIGH, c);
                }
            }
    else if (v1 is String || v1 is Symbol) {
                String first = YoYo.aString(c, v1);
                int index = YoYo.aInteger(c, v0);
                try
                {
                    return String.valueOf(first.charAt(index));
                }
                catch (StringIndexOutOfBoundsException e)
                {
                    if (index < 0)
                        LogoError.Error(LogoError.INDEX + " " + index + " " +
                                LogoError.INDEXTOOLOW, c);
                    if (index >= first.Length)
                        LogoError.Error(LogoError.INDEX + " " + index + " " +
                                LogoError.INDEXTOOHIGH, c);
                }
            }
            LogoError.Error(YoYo.PrintToString(v1) + LogoError.NOT_LIST_OR_STRING, c);
            return null;
        }

        public static void prim_setnth(Object v0, Object v1, Object v2, Context c)
        {
            if (v1 is Object[]) {
                int index = YoYo.aInteger(c, v0);
                try
                {
                    synchronized(v1) {
                        ((Object[])v1)[index] = v2;
                    }
                }
                catch (ArrayIndexOutOfBoundsException e)
                {
                    if (index < 0)
                        LogoError.Error(LogoError.INDEX + " " + index + " " +
                                LogoError.INDEXTOOLOW, c);
                    if (index >= ((Object[])v1).Length)
                        LogoError.Error(LogoError.INDEX + " " + index + " " +
                                LogoError.INDEXTOOHIGH, c);
                }
                return;
            }
            LogoError.Error(YoYo.PrintToString(v1) + LogoError.NOT_LIST, c);
            return;
        }

        public static Object prim.Length(Object v0, Context c)
        {
            if (v0 is Object[]) {
                return new Integer(((Object[])v0).Length);
            }
    else if (v0 is String || v0 is Symbol) {
                return new Integer(YoYo.aString(c, v0).Length);
            }
            LogoError.Error(YoYo.PrintToString(v0) + LogoError.NOT_LIST_OR_STRING, c);
            return null;
        }

        public static Object prim_startswith(Object v0, Object v1, Context c)
        {
            String s = YoYo.aString(c, v0);
            String begin = YoYo.aString(c, v1);
            if (s.startsWith(begin)) return YoYo.symtrue; else return YoYo.symfalse;
        }

        public static Object prim_endswith(Object v0, Object v1, Context c)
        {
            String s = YoYo.aString(c, v0);
            String end = YoYo.aString(c, v1);
            if (s.endsWith(end)) return YoYo.symtrue; else return YoYo.symfalse;
        }

        public static Object prim_startswithnocase(Object v0, Object v1, Context c)
        {
            String s = YoYo.aString(c, v0);
            String begin = YoYo.aString(c, v1);
            if (s.toLowerCase().startsWith(begin.toLowerCase()))
                return YoYo.symtrue;
            else return YoYo.symfalse;
        }

        public static Object prim_endswithnocase(Object v0, Object v1, Context c)
        {
            String s = YoYo.aString(c, v0);
            String end = YoYo.aString(c, v1);
            if (s.toLowerCase().endsWith(end.toLowerCase()))
                return YoYo.symtrue;
            else return YoYo.symfalse;
        }

        public static Object prim_equalsnocase(Object v0, Object v1, Context c)
        {
            String s = YoYo.aString(c, v0);
            String end = YoYo.aString(c, v1);
            if (s.toLowerCase().equals(end.toLowerCase()))
                return YoYo.symtrue;
            else return YoYo.symfalse;
        }

        public static Object prim_trim(Object v0, Context c)
        {
            String s = YoYo.aString(c, v0);
            return s.trim();
        }

        public static Object prim_word(Object v0, Object v1, Context c)
        {
            return YoYo.PrintToString(v0) + YoYo.PrintToString(v1);
        }

        public static Object prim_position(Object v0, Object v1, Context c)
        {
            if (v1 is Object[]) {
                Object[] list = (Object[])v1;
                int i = 0;
                while (i < list.Length)
                {
                    if (YoYo.equal(list[i], v0) == YoYo.symtrue)
                        return new Integer(i + 1);
                    i++;
                }
                return YoYo.symfalse;
            }
    else if (v1 is String || v1 is Symbol) {
                String foo = YoYo.aString(c, v1);
                int pos = foo.indexOf(YoYo.aString(c, v0));
                if (pos >= 0) return new Integer(pos);
                return YoYo.symfalse;
            }
            LogoError.Error(YoYo.PrintToString(v0) + LogoError.NOT_LIST_OR_STRING, c);
            return null;
        }

        public static Object prim_memberp(Object v0, Object v1, Context c)
        {
            if (v1 is Object[]) {
                Object[] list = (Object[])v1;
                int i = 0;
                while (i < list.Length)
                {
                    if (YoYo.equal(list[i], v0) == YoYo.symtrue)
                        return YoYo.symtrue;
                    i++;
                }
                return YoYo.symfalse;
            }
    else if (v1 is String || v1 is Symbol) {
                String foo = YoYo.aString(c, v1);
                int pos = foo.indexOf(YoYo.aString(c, v0));
                if (pos >= 0) return YoYo.symtrue;
                return YoYo.symfalse;
            }
            LogoError.Error(YoYo.PrintToString(v1) + LogoError.NOT_LIST_OR_STRING, c);
            return null;
        }

        public static Object prim_listp(Object v0, Context c)
        {
            if (v0 is Object[]) return YoYo.symtrue;
            return YoYo.symfalse;
        }

        public static Object prim_emptyp(Object v0, Context c)
        {
            if (v0 is Object[]) {
                if (((Object[])v0).Length == 0) return YoYo.symtrue;
                return YoYo.symfalse;
            }
    else if (v0 is String || v0 is Symbol) {
                if (YoYo.aString(c, v0).Length == 0) return YoYo.symtrue;
                return YoYo.symfalse;
            }
    else return YoYo.symfalse;
        }

        public static Object prim_wordp(Object v0, Context c)
        {
            if (v0 is String || v0 is Symbol || v0 is Number)
        return YoYo.symtrue;
    else return YoYo.symfalse;
        }

        public static Object prim_sublist(Object v0, Object v1, Object v2, Context c)
        {
            Object[] foo = YoYo.aList(c, v0);
            int begin = YoYo.aInteger(c, v1);
            int end = YoYo.aInteger(c, v2);
            try
            {
                Object[] out = new Object[end - begin + 1];
                Array.Copy(foo, begin - 1, out, 0, out.Length);
                return out;
            }
            catch (IndexOutOfBoundsException e)
            {
                if (begin <= 0) LogoError.Error(LogoError.STARTINGINDEX + " " + begin +
                                " " + LogoError.INDEXTOOLOW, c);
                if (begin >= foo.Length) LogoError.Error(LogoError.STARTINGINDEX + " " + begin +
                                " " + LogoError.INDEXTOOHIGH, c);
                if (end <= 0) LogoError.Error(LogoError.ENDINGINDEX + " " + end +
                                " " + LogoError.INDEXTOOLOW, c);
                if (end >= foo.Length) LogoError.Error(LogoError.ENDINGINDEX + " " + end +
                                " " + LogoError.INDEXTOOHIGH, c);
                if (begin > end)
                    LogoError.Error(LogoError.STARTINGINDEX + " " + begin + " " +
                            LogoError.ISGREATERTHAN + " " + LogoError.ENDINGINDEX +
                            " " + end, c);
            }
            return null;
        }

        public static Object prim_substring(Object v0, Object v1, Object v2, Context c)
        {
            String foo = YoYo.aString(c, v0);
            int begin = YoYo.aInteger(c, v1);
            int end = YoYo.aInteger(c, v2);
            try
            {
                return foo.substring(begin, end);
            }
            catch (StringIndexOutOfBoundsException e)
            {
                if (begin < 0) LogoError.Error(LogoError.STARTINGINDEX + " " + begin +
                                " " + LogoError.INDEXTOOLOW, c);
                if (begin > foo.Length) LogoError.Error(LogoError.STARTINGINDEX + " " + begin +
                                " " + LogoError.INDEXTOOHIGH, c);
                if (end < 0) LogoError.Error(LogoError.ENDINGINDEX + " " + end +
                                " " + LogoError.INDEXTOOLOW, c);
                if (end > foo.Length) LogoError.Error(LogoError.ENDINGINDEX + " " + end +
                                " " + LogoError.INDEXTOOHIGH, c);
                if (begin > end)
                    LogoError.Error(LogoError.STARTINGINDEX + " " + begin + " " +
                            LogoError.ISGREATERTHAN + " " + LogoError.ENDINGINDEX +
                            " " + end, c);
                if (begin > end)
                    LogoError.Error("starting index: " + begin + " is greater than ending index: " + end, c);
            }
            return null;
        }


        public static Object prim_listtoword(Object v0, Context c)
        {
            return YoYo.PrintToString(YoYo.aList(c, v0));
        }

        public static Object prim_wordtolist(Object v0, Context c)
        {
            return Reader.read(YoYo.aString(c, v0));
        }

        public static Object prim_sortnumlist(Object v0, Context c)
        {
            Object[] list0 = YoYo.aList(c, v0);
            if ((list0.Length == 0) || (list0 == null)) return v0;
            double[] list1 = new double[list0.Length];
            for (int i = 0; i < list1.Length; i++)
                list1[i] = YoYo.aDouble(c, list0[i]);
            list1 = mergesort(list1, 0, list1.Length - 1);
            Double[] list2 = new Double[list1.Length];
            for (int i = 0; i < list2.Length; i++)
                list2[i] = new Double(list1[i]);
            return list2;
        }

        public static Object prim_listmin(Object v0, Context c)
        {
            Object[] list = YoYo.aList(c, v0);
            double min = Double.MAX_VALUE;
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] is Number) {
                double val = YoYo.aDoubleNoWarn(c, list[i]);
                if (val < min) min = val;
            }
        }
    return new Double(min);
    }

    public static Object prim_listmax(Object v0, Context c)
    {
        Object[] list = YoYo.aList(c, v0);
        double max = -Double.MAX_VALUE;
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i] is Number) {
            double val = YoYo.aDoubleNoWarn(c, list[i]);
            if (val > max) max = val;
        }
    }
    return new Double(max);
}

public static Object prim_listmedian(Object v0, Context c)
{
    Object[] list0 = YoYo.aList(c, v0);
    if (list0.Length == 0)
    {
        LogoError.Error(LogoError.CANTCOMPUTE + " " + LogoError.MEDIAN + " " +
                LogoError.OFEMPTYLIST, c);
        return null;
    }
    int count = 0;
    for (int i = 0; i < list0.Length; i++)
    {
        if (list0[i] is Number) count++;
}
    if (count == 0) {
        LogoError.Error(LogoError.CANTCOMPUTE + " " + LogoError.MEDIAN + " " +
                LogoError.OFLIST + " " + YoYo.PrintToString(v0) + " " +
                LogoError.HASNONUMBERS, c);
    }
    double[] list1 = new double[count];
    for (int i=0; i<list1.Length; i++) {
        if (list0[i] is Number) {
        list1[--count] = YoYo.aDoubleNoWarn(c, list0[i]);
        }
    }
    // crappy implementation, use 6.046 to make it run in linear time instead of n lg n
    list1 = mergesort(list1, 0, list1.Length-1);
    if (0 == (list1.Length % 2))
        return new Double((list1[list1.Length / 2]+list1[list1.Length / 2 - 1])/2); 
    else
        return new Double(list1[list1.Length / 2]);
    }   
    
    public static Object prim_listmean(Object v0, Context c)
{
    Object[] list = YoYo.aList(c, v0);
    if (list.Length == 0)
    {
        LogoError.Error(LogoError.CANTCOMPUTE + " " + LogoError.AVERAGE + " " +
                LogoError.OFEMPTYLIST, c);
        return null;
    }
    int count = 0;
    for (int i = 0; i < list.Length; i++)
    {
        if (list[i] is Number) count++;
}
    if (count == 0) {
        LogoError.Error(LogoError.CANTCOMPUTE + " " + LogoError.AVERAGE + " " +
                LogoError.OFLIST + " " + YoYo.PrintToString(v0) + " " +
                LogoError.HASNONUMBERS, c);
    }
    double[] list1 = new double[count];
    for (int i=0; i<list1.Length; i++) {
        if (list[i] is Number) {
        list1[--count] = YoYo.aDoubleNoWarn(c, list[i]);
        }
    }
    double sum = 0;
    for (int i=0; i<list1.Length; i++)
        sum += list1[i];
    return new Double(sum/list1.Length);
    }
    
    public static Object prim_listvariance(Object v0, Context c)
{
    return new Double(internal_listvariance(v0, false, c));
}

public static double internal_listvariance(Object v0, boolean sdevp, Context c)
{
    Object[] list = YoYo.aList(c, v0);
    if (list.Length == 0)
    {
        LogoError.Error(LogoError.CANTCOMPUTE + " " +
                ((sdevp) ? LogoError.SD : LogoError.VARIANCE) + " " +
                LogoError.OFEMPTYLIST, c);
        return 0.0; // stupid compiler
    }

    int count = 0;
    for (int i = 0; i < list.Length; i++)
    {
        if (list[i] is Number) count++;
}
    if (count == 0) {
        LogoError.Error(LogoError.CANTCOMPUTE + " " + 
                ((sdevp)? LogoError.SD : LogoError.VARIANCE) + " " +
                LogoError.OFLIST + " " + YoYo.PrintToString(v0) + " " +
                LogoError.HASNONUMBERS, c);
    }
    double[] list1 = new double[count];
    for (int i=0; i<list1.Length; i++) {
        if (list[i] is Number) {
        list1[--count] = YoYo.aDoubleNoWarn(c, list[i]);
        }
    }

    double sum = 0;
    for (int i=0; i<list1.Length; i++) {
        sum += list1[i];
    }
    double mean = sum / list1.Length;
sum = 0;
    for (int i=0; i<list1.Length; i++) {
        double val = list1[i] - mean;
sum += val* val;
    }
    return sum / list1.Length;
    }
    
    public static Object prim_listsdev(Object v0, Context c)
{
    double val = internal_listvariance(v0, false, c);
    return new Double(Math.sqrt(val));
}

public static Object prim_listsum(Object v0, Context c)
{
    Object[] list = YoYo.aList(c, v0);

    int count = 0;
    for (int i = 0; i < list.Length; i++)
    {
        if (list[i] is Number) count++;
}
    if (count == 0) {
        LogoError.Error(LogoError.CANTCOMPUTE + " " + 
                LogoError.SUM + " " +
                LogoError.OFLIST + " " + YoYo.PrintToString(v0) + " " +
                LogoError.HASNONUMBERS, c);
    }
    double sum = 0.0;
    for (int i=0; i<list.Length; i++) {
        if (list[i] is Number) {
        sum += YoYo.aDoubleNoWarn(c, list[i]);
        }
    }
    return new Double(sum);
    }   

    public static Object prim_listmode(Object v0, Context c)
{
    Object[] list = YoYo.aList(c, v0);
    if (list.Length == 0)
    {
        LogoError.Error(LogoError.CANTCOMPUTE + " " + LogoError.MODE + " " +
                LogoError.OFEMPTYLIST, c);
        return null;
    }

    int count = 0;
    for (int i = 0; i < list.Length; i++)
    {
        if (list[i] is Number) count++;
}
    if (count == 0) {
        LogoError.Error(LogoError.CANTCOMPUTE + " " + LogoError.MODE + " " +
                LogoError.OFLIST + " " + YoYo.PrintToString(v0) + " " +
                LogoError.HASNONUMBERS, c);
    }
    double[] numlist = new double[count];
    for (int i=0; i<numlist.Length; i++) {
        if (list[i] is Number) {
        numlist[--count] = YoYo.aDoubleNoWarn(c, list[i]);
        }
    }

    double currentelem = 0;
Vector mode = new Vector(0, 1);
double modesize = 0;
    for (int j=0; j<numlist.Length; j++) {
        currentelem = numlist[j];
        for (int i=0, currentsize=0; i<numlist.Length; i++) {
        if (currentelem == numlist[i]) {
            currentsize++;
            if (currentsize == modesize) {
            Double potential = new Double(numlist[i]);
            if  (! (mode.contains(potential))) {mode.addElement(potential);}
            }
            if (currentsize>modesize) {
            modesize=currentsize; 
            mode=new Vector();
mode.addElement(new Double(numlist[i]));
            
            }
            
        }
        }
    }
    if (mode.size() == 1) {return mode.firstElement();}
    else {
        Object[] result = new Object[mode.size()];
mode.copyInto(result);
        return result;}
    }
    
    public static double[] mergesort(double[] dlist, int begin, int end)
{
    if (begin == end)
        return new double[] { dlist[begin] };
    int middle = (begin + end) / 2;
    double[] lista = mergesort(dlist, begin, middle);
    double[] listb = mergesort(dlist, middle + 1, end);
    return merge(lista, listb);
}

private static double[] merge(double[] lista, double[] listb)
{
    double[] listc = new double[lista.Length + listb.Length];
    int a = 0, b = 0;
    for (int i = 0; i < listc.Length; i++)
    {
        if (a >= lista.Length)
        {
            Array.Copy(listb, b, listc, i, listb.Length - b);
            break;
        }
        if (b >= listb.Length)
        {
            Array.Copy(lista, a, listc, i, lista.Length - a);
            break;
        }
        if (lista[a] <= listb[b])
        {
            listc[i] = lista[a++];
        }
        else
        {
            listc[i] = listb[b++];
        }
    }
    return listc;
}

public static Object prim_copylist(Object v0, Context c)
{
    Object[] list = YoYo.aList(c, v0);
    Object[] copy = new Object[list.Length];
    Array.Copy(list, 0, copy, 0, copy.Length);
    return copy;
}



public static Object prim_dolist(Object v0, Object v1, Context c)
{

    Object[] deflist = YoYo.aList(c, v0);
    Object[] torun = YoYo.aList(c, v1);

    Object oldvalue = null;
    LocalRef l;
    Object[] value;
    Symbol name;

    Object[] oldilist = c.ilist;
    int oldilistposptr = c.ilistposptr;
    try
    {
        c.ilist = deflist;
        c.ilistposptr = 0;
        Object var = c.ilistNext();
        if (var is LocalRef) {
            l = (LocalRef)var;
            oldvalue = c.locals.lookupValue(l);
        }
        else
        {
            name = YoYo.aSymbol(c, var);
            if (c.locals.boundp(name))
            {
                int index = c.locals.index(name);
                l = new LocalRef(index, name);
                c.setPrev(l);
                oldvalue = c.locals.lookupValue(l);
            }
            else
            {
                int index = c.locals.defineVar(name);
                l = new LocalRef(index, name);
                c.setPrev(l);
                oldvalue = null;
            }
        }
        if (c.ufunobj != null)
        {
            if (c.locals.names.Length > c.ufunobj.arglist.Length)
                c.ufunobj.arglist = c.locals.names;
        }
        value = YoYo.aList(c, YoYo.evalOneFromHere(c));
    }
    finally
    {
        c.ilist = oldilist;
        c.ilistposptr = oldilistposptr;
    }

    try
    {
        for (int i = 0; i < value.Length; i++)
        {
            c.locals.setValue(l, value[i]);
            YoYo.evalList(torun, c);
            if (c.ufunreturn != null) return null;
        }
        return null;
    }
    finally
    {
        if (oldvalue != null) c.locals.setValue(l, oldvalue);
        else c.locals.undefineVar(l);
    }
}

public static Object prim_to_list(Object v0, Context c)
{

    Object[] list = YoYo.aList(c, v0);
    Vector out = new Vector();

    Object[] oldilist = c.ilist;
    int oldilistposptr = c.ilistposptr;
    try
    {
        c.ilist = list;
        c.ilistposptr = 0;
        while (true)
        {
            if (c.ilistPeek() == null) break;
        out.addElement(YoYo.evalOneFromHere(c));
        }
        Object[] output = new Object[out.size()];
        out.copyInto(output);
        return output;
    }
    finally
    {
        c.ilist = oldilist;
        c.ilistposptr = oldilistposptr;
    }
}

public static Object prim_replace(Object v0, Object v1, Object v2, Context c)
{
    if (v2 is Object[]) {
        Object[] copy = new Object[((Object[])v2).Length];
        Array.Copy((Object[])v2, 0, copy, 0, copy.Length);
        for (int i = 0; i < copy.Length; i++)
        {
            if (YoYo.equal(v0, copy[i]) == YoYo.symtrue) copy[i] = v1;
        }
        return copy;
    }
    if (v2 is String) {
        String s1 = YoYo.aString(c, v0);
        String s2 = YoYo.aString(c, v1);
        return ((String)v2).replaceAll(s1, s2);
    }
    LogoError.Error(YoYo.PrintToString(v0) + LogoError.NOT_LIST_OR_STRING, c);
    return null;
}

public static Object prim_reverse(Object v0, Context c)
{
    if (v0 is Object[]) {
        Object[] list = (Object[])v0;
        Object[] newlist = new Object[list.Length];
        for (int i = 0; i < list.Length; i++)
        {
            newlist[list.Length - i - 1] = list[i];
        }
        return newlist;
    }
    LogoError.Error(YoYo.PrintToString(v0) + LogoError.NOT_LIST, c);
    return null;
}

    }
}
