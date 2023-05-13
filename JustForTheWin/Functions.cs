using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustForTheWin
{
    public static class Functions
    {
        public static int ReturnToPlayer(int wonCredits, int playingCredits)
        {
            double rtp = ((wonCredits) / ((double)playingCredits)) * 100;
            return (int)(rtp);
        }

        public static int RoundNum(int num)
        {
            int rem = num % 10;
            return rem >= 5 ? (num - rem + 10) : (num - rem);
        }
    }
}
