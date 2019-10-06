using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOFLife
{
    static class FieldUtils
    {
        public static bool ContainsRecursion(List<bool[][]> History) {
            return ContainsRecursion(History, 30);
        }
        public static bool ContainsRecursion(List<bool[][]> History,int maxDepth) {

            if (maxDepth == 0)
            {
                maxDepth = History.Count;
            }
            for (int i = 2; i < maxDepth && i<History.Count;  i++)
            {
                string f= FieldToString(History[History.Count - i]);
                string g =FieldToString(History[History.Count - 1]);
                if (f==g ) //first Historyset that is equal to the current one
                {
                    for (int k = 0; k < i-1; k++)
                    {
                        if(FieldToString(History[History.Count - i-k])==FieldToString(History[History.Count - 1 - k]))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public static String FieldToString(bool[][] Gamefield)
        {
            int leadingZero=0;
            int leadingOne = 0;
            String s = "";
            foreach (bool[] row in Gamefield) {
                foreach (bool cell in row)
                {
                    if (cell)
                    {
                        s = s + "1";
                    }
                    else
                    {
                        s = s + "0";
                    }

                }
                s = s + "_";
            }
            return s;
        }

    }
}
