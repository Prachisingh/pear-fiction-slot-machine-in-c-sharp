using System;
using System.Collections;
using System.Collections.Generic;

namespace SlotMachin
{
    internal class Program
    {
        static void Main(string[] args)
        {
           

            List<string[]> bgReelsA = new List<string[]>(5);
            List<int> stopPosition = new List<int>();

            bgReelsA.Add(new string[] { "sym2", "sym7", "sym7", "sym1", "sym1", "sym5", "sym1", "sym4", "sym5", "sym3", "sym2", "sym3", "sym8", "sym4", "sym5", "sym2", "sym8", "sym5", "sym7", "sym2" });
            bgReelsA.Add(new string[] { "sym1", "sym6", "sym7", "sym6", "sym5", "sym5", "sym8", "sym5", "sym5", "sym4", "sym7", "sym2", "sym5", "sym7", "sym1", "sym5", "sym6", "sym8", "sym7", "sym6", "sym3", "sym3", "sym6", "sym7", "sym3" });
            bgReelsA.Add(new string[] { "sym5", "sym2", "sym7", "sym8", "sym3", "sym2", "sym6", "sym2", "sym2", "sym5", "sym3", "sym5", "sym1", "sym6", "sym3", "sym2", "sym4", "sym1", "sym6", "sym8", "sym6", "sym3", "sym4", "sym4", "sym8", "sym1", "sym7", "sym6", "sym1", "sym6" });
            bgReelsA.Add(new string[] { "sym2", "sym6", "sym3", "sym6", "sym8", "sym8", "sym3", "sym6", "sym8", "sym1", "sym5", "sym1", "sym6", "sym3", "sym6", "sym7", "sym2", "sym5", "sym3", "sym6", "sym8", "sym4", "sym1", "sym5", "sym7" });
            bgReelsA.Add(new string[] { "sym7", "sym8", "sym2", "sym3", "sym4", "sym1", "sym3", "sym2", "sym2", "sym4", "sym4", "sym2", "sym6", "sym4", "sym1", "sym6", "sym1", "sym6", "sym4", "sym8" });

            int stake = 1;
            int boardHeight = 3;
            int boardWidth = 5;

            Random rng = new Random();

            List<String[]> slotFace = new List<string[]>(5);

            int stopPos;
            foreach (string[] reel in bgReelsA)
            {
                stopPos = rng.Next(reel.Length); //
                string[] slotFaceReel = selectReels(3, reel, stopPos);
                stopPosition.Add(stopPos);
                slotFace.Add(slotFaceReel);
            }
            Console.Write("Stop Positions:");  
            for (int i = 0; i < stopPosition.Count; i++)
            {
                Console.Write(stopPosition[i]+",");
            }
            Console.WriteLine();
            Console.WriteLine("Screen:");
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 5; col++)
                {

                   Console.Write(" " + slotFace[col][row]);
                }
                Console.WriteLine();
            }
            calculateWin(slotFace, stake, boardHeight, boardWidth);

        }

        private static string[] selectReels(int boardHeight, string[] reel, int position) {
       
        string[] boardReel = new string[boardHeight];
        for(int i = 0; i < boardHeight; i++){
            boardReel[i] = reel[(position + i) % reel.Length];
        }
        return boardReel;
    }


        private static void calculateWin(List<string[]> slotFace, int stake, int boardHeight, int boardWidth)
        {
            float totalWin = 0f;
            List<WinData> winDataList = new List<WinData>();
            
            for (int row = 0; row < boardHeight; row++)
            {

                string symToCompare = slotFace[0][row]; // only first column elements need to be compared.
                bool exists = false; //exit if the symbol is already compared
                foreach (WinData symbol in winDataList)
                {
                    if(symbol.symbolName == symToCompare)
                    {
                        exists = true;
                    }

                }
                
                if (winDataList.Count !=0 && exists)
                {
                    continue;
                }

                WinData winData = checkForWinCombination(symToCompare, boardHeight, boardWidth, slotFace);
                populateWin(winData, winDataList, stake);
                if (winData.winAmount != 0)
                {
                    totalWin += winData.winAmount;                    
                }
            }
            Console.WriteLine("Total wins:" + totalWin);

            foreach (WinData win in winDataList)
            {

                Console.Write("- Ways win "); 

                for (int i = 0; i < win.posList.Count; i++)
                {
                    Console.Write(win.posList[i] + "-");
                }
                Console.Write(" " + win.symbolName + " X" + win.symCountOnEachCol.Count + ", " + win.winAmount + ", Ways: " + win.ways+" ");
            }




        }


        private static WinData checkForWinCombination(string symToCompare, int boardHeight, int boardWidth, List<string[]> slotFace)
        {

            WinData winData = new WinData();
            List<int> posList = new List<int>();
            Hashtable symCountPerColMap = new Hashtable();
            int currentCol = 0;

            for (int col = 0; col < boardWidth; col++)
            {
                int symCountPerColumn = 0;
                int pos = col;
                if (col - currentCol > 1)
                    break;
                for (int row = 0; row < boardHeight; row++)
                {
                    String currentSym = slotFace[col][row];

                    if (symToCompare == currentSym)
                    {

                        symCountPerColumn++;
                        if (symCountPerColMap.ContainsKey(col))
                        {
                            symCountPerColMap.Remove(col);
                            symCountPerColMap.Add(col, symCountPerColumn);
                        }
                        else
                        {
                            symCountPerColMap.Add(col, symCountPerColumn);
                        }
                       

                        posList.Add(pos);

                        currentCol = col;
                    }
                    pos += 5;
                }
            }
            winData.posList = posList;
            winData.symCountOnEachCol = symCountPerColMap;
            winData.symbolName = symToCompare;
            return winData;
        }

        private static void populateWin(WinData winData, List<WinData> winDataList, int stake)
        {
            SlotSymbolWaysPayConfig payOut = (SlotSymbolWaysPayConfig)getPayout()[winData.symbolName];
            float symbolWin;
            int ways;
            if (payOut != null && winData.symCountOnEachCol.Count >= payOut.minimumMatch)
            {
                symbolWin = payOut.getWinAmount(winData.symCountOnEachCol.Count);

                ways = 1;

                for (int i = 0; i<winData.symCountOnEachCol.Count; i++) {
                    ways *= (int)winData.symCountOnEachCol[i];
                }

                float finalWin = symbolWin * ways;
                winData.winAmount = finalWin * stake;
                winData.ways = ways;
                winData.basePayout = symbolWin;
                winDataList.Add(winData);
            }
        }

        private static Hashtable getPayout()
        {

            Hashtable payTable = new Hashtable();

            payTable.Add("sym1", new SlotSymbolWaysPayConfig(3, new List<float> { 1, 2, 3 }));
            payTable.Add("sym2", new SlotSymbolWaysPayConfig(3, new List<float> { 1, 2, 3 }));

            payTable.Add("sym3", new SlotSymbolWaysPayConfig(3, new List<float> { 1, 2, 5 }));

            payTable.Add("sym4", new SlotSymbolWaysPayConfig(3, new List<float> { 2, 5, 10 }));

            payTable.Add("sym5", new SlotSymbolWaysPayConfig(3, new List<float> { 5, 10, 15 }));
            payTable.Add("sym6", new SlotSymbolWaysPayConfig(3, new List<float> { 5, 10, 15 }));

            payTable.Add("sym7", new SlotSymbolWaysPayConfig(3, new List<float> { 5, 10, 20 }));
            payTable.Add("sym8", new SlotSymbolWaysPayConfig(3, new List<float> { 10, 20, 50 }));

            return payTable;
        }
    }
}
