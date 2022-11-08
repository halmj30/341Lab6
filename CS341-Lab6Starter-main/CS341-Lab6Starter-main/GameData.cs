using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab6Starter
/**
* 
* Name: Jack Halm and Nick Miller
* Date: 11/7/2022
* Description: A TicTacToe Game
* Bugs: None
* Reflection: The Git part was easy, Features 2 and 3 were 30 seconds of work, 5 and 6 took a couple minutes of work. 1 and 4 were the hardest.
* 
*/
{
    class GameData
    {
        public GameData(Player? playerWinner, String Tiem)
        {
            if (playerWinner != null)
            {
                switch (playerWinner) //add the correct winner name
                {
                    case Player.O:
                        Winner = "O";
                        break;
                    case Player.X:
                        Winner = "X";
                        break;
                    case Player.Both:
                        Winner = "Tie";
                        break;
                }
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
