using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConfigs

{
   public const int MININMUM_PLAYERS=2;
   public const int MAXIMUM_PLAYERS=4;
   public const string Temp="";
   public const string DefenderBots = "DefenderBots";

      public const string EscaperBots = "EscaperBots";

   public const string TeamSelection="team";
   public const string GameLevelName="Game Level";

   public static Color GetColor(int colorChoice)
        {
            switch (colorChoice)
            {
                case 0: return Color.red;
                case 1: return Color.green;
                case 2: return Color.blue;
                case 3: return Color.yellow;
                case 4: return Color.cyan;
                case 5: return Color.grey;
                case 6: return Color.magenta;
                case 7: return Color.white;
            }

            return Color.black;
        }

}
