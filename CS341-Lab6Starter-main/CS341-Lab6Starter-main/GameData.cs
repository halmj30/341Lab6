using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab6Starter
{
    class GameData
    {
        public GameData(Player weiner, String Tiem)
        {
            switch (weiner)
            {
                case Player.O:
                    Winner = "O";
                    break;
                case Player.X:
                    Winner = "X";
                    break;
                case Player.Both:
                    Winner = "Nobody";
                    break;
            }
            Time = Tiem;
        }
        public String Winner { get; set; }
        public String Time { get; set; }

        public String DataString
        {
            get { return String.Format("{0} won in {1}", Winner, Time); }
        }
    }
}
