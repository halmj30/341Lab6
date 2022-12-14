//using Javax.Security.Auth;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6Starter;

/**
 * 
 * Name: Jack Halm and Nick Miller
 * Date: 11/7/2022
 * Description: A TicTacToe Game
 * Bugs: None
 * Reflection: The Git part was easy, Features 2 and 3 were 30 seconds of work, 5 and 6 took a couple minutes of work. 1 and 4 were the hardest.
 * 
 */

/// <summary>
/// The model class for TicTacToe
/// </summary>
internal class TicTacToeGame
{
    internal const int GRID_SIZE = 3;
    Player[,] grid = new Player[GRID_SIZE, GRID_SIZE];
    int[] scores = { 0, 0 }; // O score, X score
    LinkedList<String> times = new(); //List of times
    private Stopwatch _start;
    Boolean isFirstTurn = true;

    /// <summary>
    /// The player about to make a move
    /// </summary>
    public Player CurrentPlayer
    {
        get;
        set;
    }

    // can access TicTacToeGame instance using [ , ]
    public Player this[int row, int col]
    {
        get => grid[row, col];
        set
        {
            grid[row, col] = value;

        }
    }

    /// <summary>
    /// Access to X's score
    /// </summary>
    public int XScore
    {
        get
        {
            return scores[(int)Player.X];
        }
    }

    /// <summary>
    /// Access to Y's score
    /// </summary>
    public int OScore
    {
        get
        {
            return scores[(int)Player.O];
        }
    }

    /// <summary>
    /// get the latestgamestime
    /// </summary>
    /// <returns></returns>
    public string lastgamestime() { 
        return times.Last.Value;
    }

    /// <summary>
    /// Resets the game
    /// </summary>
    public TicTacToeGame()
    {
        
        ResetGame();
    }


    /// <summary>
    /// Processes the current turn - returns true if there is a victor, false otherwise
    /// </summary>
    /// <param name="row">clicked row</param>
    /// <param name="col">clicked column</param>
    /// <param name="victor">whoever won or Player.Nobody if nobody's won</param>
    /// <returns>true if there is a victor</returns>
    public Boolean ProcessTurn(int row, int col, out Player victor)
    {
        if (isFirstTurn)
        {
            _start = Stopwatch.StartNew(); //get the initial time
            isFirstTurn = false;
        }
       
        if (grid[row, col] == Player.X || grid[row, col] == Player.O) // already occupied, so ignore
        {
            victor = Player.Nobody;
            return false;
        }

        grid[row, col] = CurrentPlayer; // record the entry

        victor = IsThereAWinner();
        if (victor == Player.Nobody)
        {
            ToggleCurrentPlayer();
            return false;
        }

        isFirstTurn = true;
        times.AddLast(_start.ElapsedMilliseconds/1000+"");//add the latest time to the list of times

        if (victor != Player.Both) //if the game is still going on and not a tie
        {
            scores[(int)victor]+=1;//update the score
        }
        return true;
    }

    /// <summary>
    /// Returns Player.X or Player.O if there is a winner, Player.Nobody if nobody's won, Player.Both if there's a tie
    /// This method is too long --  refactor it to make it more compact.
    /// </summary>
    /// <returns></returns>
    public Player IsThereAWinner()
    {
        int rowSum;
        int colSum;
        int diagonalSum;
        Boolean gridFilled = true;                  // if grid is filled, and no winner, there's a tie

        for (int r = 0; r < GRID_SIZE; r++)         // check the row sums
        {
            rowSum = 0;
            for (int c = 0; c < GRID_SIZE; c++)
            {
                rowSum += (int)grid[r, c];
                if (grid[r, c] == Player.Nobody)
                {
                    gridFilled = false;
                }
            }
            if (rowSum == 0)
            {
                return Player.O;
            }
            else if (rowSum == GRID_SIZE)
            {
                return Player.X;
            }
        }

        for (int c = 0; c < GRID_SIZE; c++)         // check the column sums
        {
            colSum = 0;
            for (int r = 0; r < GRID_SIZE; r++)
            {
                colSum += (int)grid[r, c];
            }
            if (colSum == 0)
            {
                return Player.O;
            }
            else if (colSum == GRID_SIZE)
            {
                return Player.X;
            }
        }

        diagonalSum = 0;                            // check the diagonal sums

        for (int r = 0; r < GRID_SIZE; r++)
        {
            diagonalSum += (int)grid[r, r];
        }

        if (diagonalSum == 0)
        {
            return Player.O;
        }
        else if (diagonalSum == GRID_SIZE)
        {
            return Player.X;
        }

        diagonalSum = 0;                               // check off-diagonal sums here

        for (int r = 0; r < GRID_SIZE; r++)
        {
            diagonalSum += (int)grid[r, GRID_SIZE - 1 - r];
        }

        if (diagonalSum == 0)
        {
            return Player.O;
        }
        else if (diagonalSum == GRID_SIZE)
        {
            return Player.X;
        }

        return gridFilled ? Player.Both : Player.Nobody;
    }

    /// <summary>
    /// Resets the grid and sets the current player to X
    /// </summary>
    public void ResetGame()
    {
        for (int r = 0; r < GRID_SIZE; r++)
        {
            for (int c = 0; c < GRID_SIZE; c++)
            {
                grid[r, c] = Player.Nobody;
            }
        }
        CurrentPlayer = Player.X; // X always goes first
       
    }

    /// <summary>
    /// Toggles the current Player: X => O, O => X
    /// If Empty, ignore it
    /// </summary>
    private void ToggleCurrentPlayer()
    {
        CurrentPlayer = (CurrentPlayer == Player.X) ? Player.O : Player.X;
    }
}

