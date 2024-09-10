using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotMachin
{
    internal class SlotSymbolWaysPayConfig
    {

        public int minimumMatch { get; set; }

        public List<float> winAmounts { get; set; }

        public SlotSymbolWaysPayConfig(int minimumMatch, List<float> winAmounts)
        {
            this.minimumMatch = minimumMatch;
            this.winAmounts = winAmounts;
        }

        public float getWinAmount(int matchedColumnsCount)
        {
            if (matchedColumnsCount < minimumMatch) return 0f;

            if (matchedColumnsCount - minimumMatch == 0)
            {
                return winAmounts[0];
            }
            else if (matchedColumnsCount - minimumMatch == 1)
            {
                return winAmounts[1];
            }
            else
            {
                return winAmounts[2];
            }
        }
    }


   
}
