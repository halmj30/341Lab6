namespace Lab6Starter;

//using AndroidX.Annotations;
//using Java.Security.Cert;
/**
 * 
 * Name: Jack Halm
 * Date: 11/7/2022
 * Description: A TicTacToe Game
 * Bugs:
 * Reflection:
 * 
 */

using Lab6Starter;
using System.Collections.ObjectModel;

//using static Android.InputMethodServices.Keyboard;


/// <summary>
/// The MainPage, this is a 1-screen app
/// </summary>
public partial class MainPage : ContentPage
{
    TicTacToeGame ticTacToe; // model class
    Button[,] grid;          // stores the buttons
    Boolean randomColor = false;
    ObservableCollection<GameData> games = new();



    /// <summary>
    /// initializes the component
    /// </summary>
    public MainPage()
    {
        InitializeComponent();
        ticTacToe = new TicTacToeGame();
        GamesListView.ItemsSource = games;
        grid = new Button[TicTacToeGame.GRID_SIZE, TicTacToeGame.GRID_SIZE] { { Tile00, Tile01, Tile02 }, { Tile10, Tile11, Tile12 }, { Tile20, Tile21, Tile22 } };
    }

    void OnToggled(object sender, ToggledEventArgs e)
    {
        randomColor = !randomColor;
        if (randomColor)
        {
            //set buttons colors randomly
            Random rand = new();
            Color[] randomcolors = { Colors.Green, Colors.Blue, Colors.Red };
            Tile00.BackgroundColor = randomcolors[rand.Next(0,3)];
            Tile01.BackgroundColor = randomcolors[rand.Next(0, 3)];
            Tile02.BackgroundColor = randomcolors[rand.Next(0, 3)];
            Tile10.BackgroundColor = randomcolors[rand.Next(0, 3)];
            Tile11.BackgroundColor = randomcolors[rand.Next(0, 3)];
            Tile12.BackgroundColor = randomcolors[rand.Next(0, 3)];
            Tile20.BackgroundColor = randomcolors[rand.Next(0, 3)];
            Tile21.BackgroundColor = randomcolors[rand.Next(0, 3)];
            Tile22.BackgroundColor = randomcolors[rand.Next(0, 3)];
        } else
        {
            //reset colors
            Tile00.BackgroundColor = Colors.Red;
            Tile01.BackgroundColor = Colors.Blue;
            Tile02.BackgroundColor = Colors.Green;
            Tile10.BackgroundColor = Colors.Blue;
            Tile11.BackgroundColor = Colors.Green;
            Tile12.BackgroundColor = Colors.Red;
            Tile20.BackgroundColor = Colors.Green;
            Tile21.BackgroundColor = Colors.Red;
            Tile22.BackgroundColor = Colors.Blue;
        }
    }

    /// <summary>
    /// Handles button clicks - changes the button to an X or O (depending on whose turn it is)
    /// Checks to see if there is a victory - if so, invoke 
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HandleButtonClick(object sender, EventArgs e)
    {
        Player victor;
        Player currentPlayer = ticTacToe.CurrentPlayer;

        Button button = (Button)sender;
        int row;
        int col;

        FindTappedButtonRowCol(button, out row, out col);
        if (button.Text.ToString() != "")
        { // if the button has an X or O, bail
            DisplayAlert("Illegal move", "Try again, chump", "Darn it");
            return;
        }
        button.Text = currentPlayer.ToString();
        Boolean gameOver = ticTacToe.ProcessTurn(row, col, out victor);

        if (gameOver)
        {
            CelebrateVictory(victor);

        }
    }

    /// <summary>
    /// Returns the row and col of the clicked row
    /// There used to be an easier way to do this ...
    /// </summary>
    /// <param name="button"></param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    private void FindTappedButtonRowCol(Button button, out int row, out int col)
    {
        row = -1;
        col = -1;

        for (int r = 0; r < TicTacToeGame.GRID_SIZE; r++)
        {
            for (int c = 0; c < TicTacToeGame.GRID_SIZE; c++)
            {
                if(button == grid[r, c])
                {
                    row = r;
                    col = c;
                    return;
                }
            }
        }
        
    }


    /// <summary>
    /// Celebrates victory, displaying a message box and resetting the game
    /// </summary>
    private void CelebrateVictory(Player victor)
    {
        //MessageBox.Show(Application.Current.MainWindow, String.Format("Congratulations, {0}, you're the big winner today", victor.ToString()));
        XScoreLBL.Text = String.Format("X's Score: {0}", ticTacToe.XScore);
        OScoreLBL.Text = String.Format("O's Score: {0}", ticTacToe.OScore);
        TimerLBL.Text = String.Format("Last Game took {0} seconds", ticTacToe.lastgamestime());

        //update list view
        GameData game = new GameData(victor, ticTacToe.lastgamestime());
        games.Add(game);

        //send to bit.io


        ResetGame();
    }

    /// <summary>
    /// Resets the grid buttons so their content is all ""
    /// </summary>
    private void ResetGame()
    {
        
        for (int r = 0; r < TicTacToeGame.GRID_SIZE; r++)
        {
            for (int c = 0; c < TicTacToeGame.GRID_SIZE; c++)
            {
                grid[r, c].Text = "";
            }
        }
        ticTacToe.ResetGame();
    }

}



