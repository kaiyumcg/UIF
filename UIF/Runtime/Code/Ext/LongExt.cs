using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIF
{
    public static class LongExt
    {
        public static string GetCoinUIString(this long coinNum)
        {
            string coinStr = "";
            if (coinNum <= 999L)
            {
                coinStr = "" + coinNum.ToString();
            }
            else if (coinNum > 999L)
            {
                double ln = ((double)coinNum / (double)1000L);
                coinStr = ln.ToString("0.0") + "K";
            }
            else if (coinNum > (1000L * 100L * 10L - 1))
            {
                double ln = ((double)coinNum / (double)(1000L * 100L * 10L));
                coinStr = ln.ToString("0.0") + "M";
            }
            else if (coinNum > (1000L * 100L * 10L * 1000L - 1))
            {
                double ln = ((double)coinNum / (double)(1000L * 100L * 10L * 1000L));
                coinStr = ln.ToString("0.0") + "B";
            }
            else if (coinNum > (1000L * 100L * 10L * 1000L * 1000L - 1L))
            {
                double ln = ((double)coinNum / (double)(1000L * 100L * 10L * 1000L * 1000L));
                coinStr = ln.ToString("0.0") + "T";
            }
            return coinStr;
        }
    }
}