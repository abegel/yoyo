using System;
using System.Collections.Generic;

namespace YoYo
{
    public class PrimLists : YoYoMethods
    {
        public PrimLists()
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
                "length", "PrimLength",
                "word", "PrimWord",
                "position", "PrimPosition",
                "member?", "PrimMemberp",
                "empty?", "PrimEmptyp",
                "list?", "PrimListp",
                "string?", "PrimStringp",
                "substring", "PrimSubString",
                "sublist", "PrimSubList",
                "copy-list", "PrimCopyList",
                "string-to-list", "PrimStringToList",
                "list-to-string", "PrimListToString",
                "sort-num-list", "PrimSortNumList",
                "min-of-list", "PrimListMin",
                "max-of-list", "PrimListMax",
                "median-of-list", "PrimListMedian",
                "average-of-list", "PrimListMean",
                "variance-of-list", "PrimListVariance",
                "sdev-of-list", "PrimListSDev",
                "mode-of-list", "PrimListMode",
                "sum-of-list", "PrimListSum",
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
            long length = YoYo.aInteger(c, v0);
            Object[] output = new Object[length];
            for (int i = 0; i < length; i++)
            {
                output[i] = v1;
            }
            return output;
        }

        public static Object PrimSentence(Object v0, Object v1, Context c)
        {
            Object[] output;
            int vLength;
            if (v0 is Object[])
            {
                vLength = ((Object[])v0).Length;
                if (v1 is Object[])
                {
                    vLength = ((object[])v1).Length;
                    output = new Object[vLength + vLength];
                    Array.Copy((Object[])v0, output, vLength);
                    Array.Copy((Object[])v1, 0, output, vLength, vLength);
                    return output;
                }
                output = new Object[vLength + 1];
                Array.Copy((Object[])v0, output, vLength);
                output[vLength] = v1;
                return output;
            }
            if (v1 is Object[])
            {
                vLength = ((Object[])v1).Length;
                output = new Object[vLength + 1];
                Array.Copy((Object[])v1, 0, output, 1, vLength);
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

        public static Object PrimLput(Object v0, Object v1, Context c)
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


        public static Object PrimButFirst(Object v0, Context c)
        {
            if (v0 is Object[])
            {
                if (((Object[])v0).Length == 0)
                    LogoError.Error("can't take butfirst " +
                            "of empty list", c);
                Object[] output = new Object[((Object[])v0).Length - 1];
                Array.Copy((Object[])v0, 1, output, 0, output.Length);
                return output;
            }
            else if (v0 is String || v0 is Symbol)
            {
                String first = YoYo.aString(c, v0);
                if (first.Length == 0)
                    LogoError.Error("can't take butfirst " + "of empty string", c);
                return first.Substring(1);
            }
            LogoError.Error(YoYo.PrintToString(v0) + "is not a list or string", c);
            return null;
        }

        public static Object PrimButLast(Object v0, Context c)
        {
            if (v0 is Object[])
            {
                if (((Object[])v0).Length == 0)
                    LogoError.Error("can't take " + " " + "butlast" + " " +
                            "of empty list", c);
                Object[] output = new Object[((Object[])v0).Length - 1];
                Array.Copy((Object[])v0, 0, output, 0, output.Length);
                return output;
            }
            else if (v0 is String || v0 is Symbol)
            {
                String first = YoYo.aString(c, v0);
                if (first.Length == 0)
                    LogoError.Error("can't take " + " " + "butlast" + " " +
                            "of empty string", c);
                return first.Substring(0, first.Length - 1);
            }
            LogoError.Error(YoYo.PrintToString(v0) + "is not a list or string", c);
            return null;
        }

        public static Object PrimFirst(Object v0, Context c)
        {
            if (v0 is Object[])
            {
                try
                {
                    return ((Object[])v0)[0];
                }
                catch (IndexOutOfRangeException)
                {
                    if (((Object[])v0).Length == 0)
                        LogoError.Error("can't take " + " " + "first" + " " +
                                "of empty list", c);
                }
            }
            else if (v0 is String || v0 is Symbol)
            {
                String first = YoYo.aString(c, v0);
                try
                {
                    return first[0];
                }
                catch (IndexOutOfRangeException)
                {
                    if (first.Length == 0)
                        LogoError.Error("can't take " + " " + "first" + " " +
                                "of empty string", c);
                }
            }
            LogoError.Error(YoYo.PrintToString(v0) + "is not a list or string", c);
            return null;
        }

        public static Object PrimLast(Object v0, Context c)
        {
            if (v0 is Object[])
            {
                try
                {
                    return ((Object[])v0)[((Object[])v0).Length - 1];
                }
                catch (IndexOutOfRangeException)
                {
                    if (((Object[])v0).Length == 0)
                        LogoError.Error("can't take " + " " + "last" + " " +
                                "of empty list", c);
                }
            }
            else if (v0 is String || v0 is Symbol)
            {
                String first = YoYo.aString(c, v0);
                try
                {
                    return first[first.Length - 1];
                }
                catch (IndexOutOfRangeException)
                {
                    if (first.Length == 0)
                        LogoError.Error("can't take " + " " + "last" + " " +
                                "of empty string", c);
                }
            }
            LogoError.Error(YoYo.PrintToString(v0) + "is not a list or string", c);
            return null;
        }

        public static Object PrimPick(Object v0, Context c)
        {
            if (v0 is Object[])
            {
                if (((Object[])v0).Length == 0)
                    LogoError.Error("can't pick" + " " + "element" + " " +
                            "from an empty list", c);
                int index = (int)Math.Floor(PrimMath.rand.NextDouble() * ((Object[])v0).Length);
                return ((Object[])v0)[index];
            }
            else if (v0 is String || v0 is Symbol)
            {
                String first = YoYo.aString(c, v0);
                if (first.Length == 0)
                {
                    LogoError.Error("can't pick" + " " + "a character" + " " +
                            "from an empty string", c);
                }
                int index = (int)Math.Floor(PrimMath.rand.NextDouble() * first.Length);
                return first[index];
            }
            LogoError.Error(YoYo.PrintToString(v0) + "is not a list or string", c);
            return null;
        }

        public static Object PrimItem(Object v0, Object v1, Context c)
        {
            if (v1 is Object[])
            {
                long index = YoYo.aInteger(c, v0);
                try
                {
                    return ((Object[])v1)[index - 1];
                }
                catch (IndexOutOfRangeException)
                {
                    if (index < 1)
                        LogoError.Error("index" + " " + index + " " +
                                "is too low", c);
                    if (index > ((Object[])v1).Length)
                        LogoError.Error("index" + " " + index + " " +
                                "is too high", c);
                }
            }
            else if (v1 is String || v1 is Symbol)
            {
                String first = YoYo.aString(c, v1);
                long index = YoYo.aInteger(c, v0);
                try
                {
                    return first[(int)index - 1];
                }
                catch (IndexOutOfRangeException)
                {
                    if (index < 1)
                        LogoError.Error("index" + " " + index + " " +
                                "is too low", c);
                    if (index > first.Length)
                        LogoError.Error("index" + " " + index + " " +
                                "is too high", c);
                }
            }
            LogoError.Error(YoYo.PrintToString(v1) + "is not a list or string", c);
            return null;
        }

        public static void PrimSetItem(Object v0, Object v1, Object v2, Context c)
        {
            if (v1 is Object[])
            {
                long index = YoYo.aInteger(c, v0);
                try
                {
                    lock (v1)
                    {
                        ((Object[])v1)[index - 1] = v2;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    if (index < 1)
                        LogoError.Error("index" + " " + index + " " +
                                "is too low", c);
                    if (index > ((Object[])v1).Length)
                        LogoError.Error("index" + " " + index + " " +
                                "is too high", c);
                }
                return;
            }
            LogoError.Error(YoYo.PrintToString(v1) + "is not a list", c);
            return;
        }

        public static Object PrimInsert(Object v0, Object v1, Object v2, Context c)
        {
            if (v1 is Object[])
            {
                Object[] src = (Object[])v1;
                long index = YoYo.aInteger(c, v0);
                Object[] list = new Object[src.Length + 1];
                try
                {
                    Array.Copy(src, 0, list, 0, index - 1);
                    list[index - 1] = v2;
                    Array.Copy(src, index - 1, list, index, src.Length - index + 1);
                }
                catch (IndexOutOfRangeException)
                {
                    if (index < 1)
                        LogoError.Error("index" + " " + index + " " +
                                "is too low", c);
                    if (index > src.Length)
                        LogoError.Error("index" + " " + index + " " +
                                "is too high", c);
                }
                return list;
            }
            LogoError.Error(YoYo.PrintToString(v1) + "is not a list", c);
            return null;
        }

        public static Object PrimRemove(Object v0, Object v1, Context c)
        {
            if (v1 is Object[])
            {
                Object[] src = (Object[])v1;
                long index = YoYo.aInteger(c, v0);
                Object[] list = new Object[src.Length - 1];
                try
                {
                    Array.Copy(src, 0, list, 0, index - 1);
                    Array.Copy(src, index, list, index - 1, src.Length - index);
                }
                catch (IndexOutOfRangeException)
                {
                    if (index < 1)
                        LogoError.Error("index" + " " + index + " " +
                                "is too low", c);
                    if (index > src.Length)
                        LogoError.Error("index" + " " + index + " " +
                                "is too high", c);
                }
                return list;
            }
            LogoError.Error(YoYo.PrintToString(v1) + "is not a list", c);
            return null;
        }

        public static Object PrimRemoveElt(Object v0, Object v1, Context c)
        {
            if (v1 is Object[])
            {
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
            LogoError.Error(YoYo.PrintToString(v1) + "is not a list", c);
            return null;
        }

        public static Object PrimNth(Object v0, Object v1, Context c)
        {
            if (v1 is Object[])
            {
                long index = YoYo.aInteger(c, v0);
                try
                {
                    return ((Object[])v1)[index];
                }
                catch (IndexOutOfRangeException)
                {
                    if (index < 0)
                        LogoError.Error("index" + " " + index + " " +
                                "is too low", c);
                    if (index >= ((Object[])v1).Length)
                        LogoError.Error("index" + " " + index + " " +
                                "is too high", c);
                }
            }
            else if (v1 is String || v1 is Symbol)
            {
                String first = YoYo.aString(c, v1);
                long index = YoYo.aInteger(c, v0);
                try
                {
                    return first[(int)index];
                }
                catch (IndexOutOfRangeException)
                {
                    if (index < 0)
                        LogoError.Error("index" + " " + index + " " +
                                "is too low", c);
                    if (index >= first.Length)
                        LogoError.Error("index" + " " + index + " " +
                                "is too high", c);
                }
            }
            LogoError.Error(YoYo.PrintToString(v1) + "is not a list or string", c);
            return null;
        }

        public static void PrimSetNth(Object v0, Object v1, Object v2, Context c)
        {
            if (v1 is Object[])
            {
                long index = YoYo.aInteger(c, v0);
                try
                {
                    lock (v1)
                    {
                        ((Object[])v1)[index] = v2;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    if (index < 0)
                        LogoError.Error("index" + " " + index + " " +
                                "is too low", c);
                    if (index >= ((Object[])v1).Length)
                        LogoError.Error("index" + " " + index + " " +
                                "is too high", c);
                }
                return;
            }
            LogoError.Error(YoYo.PrintToString(v1) + "is not a list", c);
            return;
        }

        public static Object PrimLength(Object v0, Context c)
        {
            if (v0 is Object[])
            {
                return ((Object[])v0).Length;
            }
            else if (v0 is String || v0 is Symbol)
            {
                return YoYo.aString(c, v0).Length;
            }
            LogoError.Error(YoYo.PrintToString(v0) + "is not a list or string", c);
            return null;
        }

        public static Object PrimStartsWith(Object v0, Object v1, Context c)
        {
            String s = YoYo.aString(c, v0);
            String begin = YoYo.aString(c, v1);
            if (s.StartsWith(begin, StringComparison.CurrentCulture)) return YoYo.symtrue; else return YoYo.symfalse;
        }

        public static Object PrimEndsWith(Object v0, Object v1, Context c)
        {
            String s = YoYo.aString(c, v0);
            String end = YoYo.aString(c, v1);
            if (s.EndsWith(end, StringComparison.CurrentCulture)) return YoYo.symtrue; else return YoYo.symfalse;
        }

        public static Object PrimStartsWithIgnoreCase(Object v0, Object v1, Context c)
        {
            String s = YoYo.aString(c, v0);
            String begin = YoYo.aString(c, v1);
            if (s.StartsWith(begin, StringComparison.CurrentCultureIgnoreCase))
                return YoYo.symtrue;
            else return YoYo.symfalse;
        }

        public static Object PrimEndsWithIgnoreCase(Object v0, Object v1, Context c)
        {
            String s = YoYo.aString(c, v0);
            String end = YoYo.aString(c, v1);
            if (s.EndsWith(end, StringComparison.CurrentCultureIgnoreCase))
                return YoYo.symtrue;
            else return YoYo.symfalse;
        }

        public static Object PrimEqualsIgnoreCase(Object v0, Object v1, Context c)
        {
            String s = YoYo.aString(c, v0);
            String end = YoYo.aString(c, v1);
            if (s.Equals(end, StringComparison.CurrentCultureIgnoreCase))
                return YoYo.symtrue;
            else return YoYo.symfalse;
        }

        public static Object PrimTrim(Object v0, Context c)
        {
            String s = YoYo.aString(c, v0);
            return s.Trim();
        }

        public static Object PrimWord(Object v0, Object v1, Context c)
        {
            return YoYo.PrintToString(v0) + YoYo.PrintToString(v1);
        }

        public static Object PrimPosition(Object v0, Object v1, Context c)
        {
            if (v1 is Object[])
            {
                Object[] list = (Object[])v1;
                int i = 0;
                while (i < list.Length)
                {
                    if (YoYo.equal(list[i], v0) == YoYo.symtrue)
                        return i + 1;
                    i++;
                }
                return YoYo.symfalse;
            }
            else if (v1 is String || v1 is Symbol)
            {
                String foo = YoYo.aString(c, v1);
                int pos = foo.IndexOf(YoYo.aString(c, v0), StringComparison.CurrentCulture);
                if (pos >= 0) return pos;
                return YoYo.symfalse;
            }
            LogoError.Error(YoYo.PrintToString(v0) + "is not a list or string", c);
            return null;
        }

        public static Object PrimMemberp(Object v0, Object v1, Context c)
        {
            if (v1 is Object[])
            {
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
            else if (v1 is String || v1 is Symbol)
            {
                String foo = YoYo.aString(c, v1);
                int pos = foo.IndexOf(YoYo.aString(c, v0), StringComparison.CurrentCulture);
                if (pos >= 0) return YoYo.symtrue;
                return YoYo.symfalse;
            }
            LogoError.Error(YoYo.PrintToString(v1) + "is not a list or string", c);
            return null;
        }

        public static Object PrimListp(Object v0, Context c)
        {
            if (v0 is Object[]) return YoYo.symtrue;
            return YoYo.symfalse;
        }

        public static Object PrimEmptyp(Object v0, Context c)
        {
            if (v0 is Object[])
            {
                if (((Object[])v0).Length == 0) return YoYo.symtrue;
                return YoYo.symfalse;
            }
            else if (v0 is String || v0 is Symbol)
            {
                if (YoYo.aString(c, v0).Length == 0) return YoYo.symtrue;
                return YoYo.symfalse;
            }
            else return YoYo.symfalse;
        }

        public static Object PrimWordp(Object v0, Context c)
        {
            if (v0 is String || v0 is Symbol || v0 is long || v0 is double)
                return YoYo.symtrue;
            else return YoYo.symfalse;
        }

        public static Object PrimSubList(Object v0, Object v1, Object v2, Context c)
        {
            Object[] foo = YoYo.aList(c, v0);
            long begin = YoYo.aInteger(c, v1);
            long end = YoYo.aInteger(c, v2);
            try
            {
                Object[] output = new Object[end - begin + 1];
                Array.Copy(foo, begin - 1, output, 0, output.Length);
                return output;
            }
            catch (IndexOutOfRangeException)
            {
                if (begin <= 0) LogoError.Error("starting index" + " " + begin +
                                " " + "is too low", c);
                if (begin >= foo.Length) LogoError.Error("starting index" + " " + begin +
                                " " + "is too high", c);
                if (end <= 0) LogoError.Error("ending index" + " " + end +
                                " " + "is too low", c);
                if (end >= foo.Length) LogoError.Error("ending index" + " " + end +
                                " " + "is too high", c);
                if (begin > end)
                    LogoError.Error("starting index" + " " + begin + " " +
                            "is greater than" + " " + "ending index" +
                            " " + end, c);
            }
            return null;
        }

        public static Object PrimSubString(Object v0, Object v1, Object v2, Context c)
        {
            String foo = YoYo.aString(c, v0);
            long begin = YoYo.aInteger(c, v1);
            long end = YoYo.aInteger(c, v2);
            try
            {
                return foo.Substring((int)begin, (int)end);
            }
            catch (IndexOutOfRangeException)
            {
                if (begin < 0) LogoError.Error("starting index" + " " + begin +
                                " " + "is too low", c);
                if (begin > foo.Length) LogoError.Error("starting index" + " " + begin +
                                " " + "is too high", c);
                if (end < 0) LogoError.Error("ending index" + " " + end +
                                " " + "is too low", c);
                if (end > foo.Length) LogoError.Error("ending index" + " " + end +
                                " " + "is too high", c);
                if (begin > end)
                    LogoError.Error("starting index" + " " + begin + " " +
                            "is greater than" + " " + "ending index" +
                            " " + end, c);
                if (begin > end)
                    LogoError.Error("starting index: " + begin + " is greater than ending index: " + end, c);
            }
            return null;
        }


        public static Object PrimListToWord(Object v0, Context c)
        {
            return YoYo.PrintToString(YoYo.aList(c, v0));
        }

        public static Object PrimWordToList(Object v0, Context c)
        {
            return Reader.Read(YoYo.aString(c, v0));
        }

        public static Object PrimSortNumList(Object v0, Context c)
        {
            Object[] list0 = YoYo.aList(c, v0);
            if ((list0.Length == 0) || (list0 == null)) return v0;
            double[] list1 = new double[list0.Length];
            for (int i = 0; i < list1.Length; i++)
                list1[i] = YoYo.aDouble(c, list0[i]);
            list1 = mergesort(list1, 0, list1.Length - 1);
            Double[] list2 = new Double[list1.Length];
            for (int i = 0; i < list2.Length; i++)
                list2[i] = list1[i];
            return list2;
        }

        public static Object PrimListMin(Object v0, Context c)
        {
            Object[] list = YoYo.aList(c, v0);
            double min = Double.MaxValue;
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] is long || list[i] is double)
                {
                    double val = YoYo.aDoubleNoWarn(c, list[i]);
                    if (val < min) min = val;
                }
            }
            return min;
        }

        public static Object PrimListMax(Object v0, Context c)
        {
            Object[] list = YoYo.aList(c, v0);
            double max = -Double.MaxValue;
            for (int i = 0; i < list.Length; i++)
            {
                if (YoYo.isNumber(list[i]))
                {
                    double val = YoYo.aDoubleNoWarn(c, list[i]);
                    if (val > max) max = val;
                }
            }
            return max;
        }

        public static Object PrimListMedian(Object v0, Context c)
        {
            Object[] list0 = YoYo.aList(c, v0);
            if (list0.Length == 0)
            {
                LogoError.Error("cannot compute" + " " + "median" + " " +
                        "of empty list", c);
                return null;
            }
            int count = 0;
            for (int i = 0; i < list0.Length; i++)
            {
                if (YoYo.isNumber(list0[i])) count++;
            }
            if (count == 0)
            {
                LogoError.Error("cannot compute" + " " + "median" + " " +
                        "of list" + " " + YoYo.PrintToString(v0) + " " +
                        "which has no numbers", c);
            }
            double[] list1 = new double[count];
            for (int i = 0; i < list1.Length; i++)
            {
                if (YoYo.isNumber(list0[i]))
                {
                    list1[--count] = YoYo.aDoubleNoWarn(c, list0[i]);
                }
            }
            // crappy implementation, use 6.046 to make it run in linear time instead of n lg n
            list1 = mergesort(list1, 0, list1.Length - 1);
            if (0 == (list1.Length % 2))
                return (list1[list1.Length / 2] + list1[list1.Length / 2 - 1]) / 2;
            else
                return list1[list1.Length / 2];
        }

        public static Object PrimListMean(Object v0, Context c)
        {
            Object[] list = YoYo.aList(c, v0);
            if (list.Length == 0)
            {
                LogoError.Error("cannot compute" + " " + "average" + " " +
                        "of empty list", c);
                return null;
            }
            int count = 0;
            for (int i = 0; i < list.Length; i++)
            {
                if (YoYo.isNumber(list[i])) count++;
            }
            if (count == 0)
            {
                LogoError.Error("cannot compute" + " " + "average" + " " +
                        "of list" + " " + YoYo.PrintToString(v0) + " " +
                        "which has no numbers", c);
            }
            double[] list1 = new double[count];
            for (int i = 0; i < list1.Length; i++)
            {
                if (YoYo.isNumber(list[i]))
                {
                    list1[--count] = YoYo.aDoubleNoWarn(c, list[i]);
                }
            }
            double sum = 0;
            for (int i = 0; i < list1.Length; i++)
                sum += list1[i];
            return sum / list1.Length;
        }

        public static Object PrimListVariance(Object v0, Context c)
        {
            return internal_listvariance(v0, false, c);
        }

        public static double internal_listvariance(Object v0, Boolean sdevp, Context c)
        {
            Object[] list = YoYo.aList(c, v0);
            if (list.Length == 0)
            {
                LogoError.Error("cannot compute" + " " +
                        ((sdevp) ? "sd" : "variance") + " " +
                        "of empty list", c);
                return 0.0; // stupid compiler
            }

            int count = 0;
            for (int i = 0; i < list.Length; i++)
            {
                if (YoYo.isNumber(list[i])) count++;
            }
            if (count == 0)
            {
                LogoError.Error("cannot compute" + " " +
                        ((sdevp) ? "sd" : "variance") + " " +
                        "of list" + " " + YoYo.PrintToString(v0) + " " +
                        "which has no numbers", c);
            }
            double[] list1 = new double[count];
            for (int i = 0; i < list1.Length; i++)
            {
                if (YoYo.isNumber(list[i]))
                {
                    list1[--count] = YoYo.aDoubleNoWarn(c, list[i]);
                }
            }

            double sum = 0;
            for (int i = 0; i < list1.Length; i++)
            {
                sum += list1[i];
            }
            double mean = sum / list1.Length;
            sum = 0;
            for (int i = 0; i < list1.Length; i++)
            {
                double val = list1[i] - mean;
                sum += val * val;
            }
            return sum / list1.Length;
        }

        public static Object PrimListSDev(Object v0, Context c)
        {
            double val = internal_listvariance(v0, false, c);
            return Math.Sqrt(val);
        }

        public static Object PrimListSum(Object v0, Context c)
        {
            Object[] list = YoYo.aList(c, v0);

            int count = 0;
            for (int i = 0; i < list.Length; i++)
            {
                if (YoYo.isNumber(list[i])) count++;
            }
            if (count == 0)
            {
                LogoError.Error("cannot compute" + " " +
                        "sum" + " " +
                        "of list" + " " + YoYo.PrintToString(v0) + " " +
                        "which has no numbers", c);
            }
            double sum = 0.0;
            for (int i = 0; i < list.Length; i++)
            {
                if (YoYo.isNumber(list[i]))
                {
                    sum += YoYo.aDoubleNoWarn(c, list[i]);
                }
            }
            return sum;
        }

        public static Object PrimListMode(Object v0, Context c)
        {
            Object[] list = YoYo.aList(c, v0);
            if (list.Length == 0)
            {
                LogoError.Error("cannot compute" + " " + "mode" + " " +
                        "of empty list", c);
                return null;
            }

            int count = 0;
            for (int i = 0; i < list.Length; i++)
            {
                if (YoYo.isNumber(list[i])) count++;
            }
            if (count == 0)
            {
                LogoError.Error("cannot compute" + " " + "mode" + " " +
                        "of list" + " " + YoYo.PrintToString(v0) + " " +
                        "which has no numbers", c);
            }
            double[] numlist = new double[count];
            for (int i = 0; i < numlist.Length; i++)
            {
                if (YoYo.isNumber(list[i]))
                {
                    numlist[--count] = YoYo.aDoubleNoWarn(c, list[i]);
                }
            }

            double currentelem = 0;
            List<double> mode = new List<double>(1);
            double modesize = 0;
            for (int j = 0; j < numlist.Length; j++)
            {
                currentelem = numlist[j];
                for (int i = 0, currentsize = 0; i < numlist.Length; i++)
                {
                    if (currentelem == numlist[i])
                    {
                        currentsize++;
                        if (currentsize == modesize)
                        {
                            Double potential = numlist[i];
                            if (!(mode.Contains(potential))) { mode.Add(potential); }
                        }
                        if (currentsize > modesize)
                        {
                            modesize = currentsize;
                            mode = new List<double>();
                            mode.Add(numlist[i]);

                        }

                    }
                }
            }
            if (mode.Count == 1) { return mode[0]; }
            else
            {
                Object[] result = new Object[mode.Count];
                for (int i = 0; i < mode.Count; i++)
                {
                    result[i] = mode[i];
                }
                return result;
            }
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

        public static Object PrimCopyList(Object v0, Context c)
        {
            Object[] list = YoYo.aList(c, v0);
            Object[] copy = new Object[list.Length];
            Array.Copy(list, 0, copy, 0, copy.Length);
            return copy;
        }



        public static Object PrimDoList(Object v0, Object v1, Context c)
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
                if (var is LocalRef)
                {
                    l = (LocalRef)var;
                    oldvalue = c.locals.LookupValue(l);
                }
                else
                {
                    name = YoYo.aSymbol(c, var);
                    if (c.locals.boundp(name))
                    {
                        int index = c.locals.Index(name);
                        l = new LocalRef(index, name);
                        c.SetPrev(l);
                        oldvalue = c.locals.LookupValue(l);
                    }
                    else
                    {
                        int index = c.locals.DefineVar(name);
                        l = new LocalRef(index, name);
                        c.SetPrev(l);
                        oldvalue = null;
                    }
                }
                if (c.ufunObj != null)
                {
                    if (c.locals.names.Length > c.ufunObj.ArgList.Length)
                        c.ufunObj.ArgList = c.locals.names;
                }
                value = YoYo.aList(c, YoYo.EvalOneFromHere(c));
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
                    c.locals.SetValue(l, value[i]);
                    YoYo.EvalList(torun, c);
                    if (c.ufunreturn != null) return null;
                }
                return null;
            }
            finally
            {
                if (oldvalue != null) c.locals.SetValue(l, oldvalue);
                else c.locals.UndefineVar(l);
            }
        }

        public static Object PrimToList(Object v0, Context c)
        {

            Object[] list = YoYo.aList(c, v0);
            List<object> output = new List<object>();

            Object[] oldilist = c.ilist;
            int oldilistposptr = c.ilistposptr;
            try
            {
                c.ilist = list;
                c.ilistposptr = 0;
                while (true)
                {
                    if (c.ilistPeek() == null) break;
                    output.Add(YoYo.EvalOneFromHere(c));
                }
                Object[] output0 = new Object[output.Count];
                output.CopyTo(output0);
                return output0;
            }
            finally
            {
                c.ilist = oldilist;
                c.ilistposptr = oldilistposptr;
            }
        }

        public static Object PrimReplace(Object v0, Object v1, Object v2, Context c)
        {
            if (v2 is Object[])
            {
                Object[] copy = new Object[((Object[])v2).Length];
                Array.Copy((Object[])v2, 0, copy, 0, copy.Length);
                for (int i = 0; i < copy.Length; i++)
                {
                    if (YoYo.equal(v0, copy[i]) == YoYo.symtrue) copy[i] = v1;
                }
                return copy;
            }
            if (v2 is String)
            {
                String s1 = YoYo.aString(c, v0);
                String s2 = YoYo.aString(c, v1);
                return ((String)v2).Replace(s1, s2);
            }
            LogoError.Error(YoYo.PrintToString(v0) + "is not a list or string", c);
            return null;
        }

        public static Object PrimReverse(Object v0, Context c)
        {
            if (v0 is Object[])
            {
                Object[] list = (Object[])v0;
                Object[] newlist = new Object[list.Length];
                for (int i = 0; i < list.Length; i++)
                {
                    newlist[list.Length - i - 1] = list[i];
                }
                return newlist;
            }
            LogoError.Error(YoYo.PrintToString(v0) + "is not a list", c);
            return null;
        }

    }
}
