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

using Lab6Starter;
using System.Collections.ObjectModel;
using Npgsql;


/// <summary>
/// The MainPage, this is a 1-screen app
/// </summary>
public partial class MainPage : ContentPage
{
    TicTacToeGame ticTacToe; // model class
    Button[,] grid;          // stores the buttons
    Boolean randomColor = false; 
    ObservableCollection<GameData> games = new(); //A Collection of GameData which is the outcomes of previous games



    /// <summary>
    /// initializes the component
    /// </summary>
    public MainPage()
    {
        InitializeComponent(); //loads the page on the screen
        ticTacToe = new TicTacToeGame(); //resets the game
        getGamesFromDB(); //loads games with GameData(s) from the DataBase
        GamesListView.ItemsSource = games; //loads the ListView with the games
        grid = new Button[TicTacToeGame.GRID_SIZE, TicTacToeGame.GRID_SIZE] { { Tile00, Tile01, Tile02 }, { Tile10, Tile11, Tile12 }, { Tile20, Tile21, Tile22 } };
    }

    /// <summary>
    /// Load games with GameData(s) from the DataBase
    /// </summary>
    void getGamesFromDB()
    {
        var bitHost = "db.bit.io";
        var bitApiKey = "v2_3vPwz_a5t2YWH7XNN59ycUFWTxAaZ";
        var bitUser = "halmj30";
        var bitDbName = "halmj30/Lab8";
        var cs = $"Host={bitHost};Username={bitUser};Password={bitApiKey};Database={bitDbName}";
        try
        {
            using var con = new NpgsqlConnection(cs);
            con.Open();

            var query = "SELECT * FROM \"gamedata\" limit 10;";
            using var cmd = new NpgsqlCommand(query, con);
            using NpgsqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())//for each item
            {
                var gameBeingAdded = new GameData(null, reader[1] as String);//make a gameData from online
                gameBeingAdded.Winner = reader[0] as String;//add the winner
                games.Add(gameBeingAdded);//add gameBeingadded to the listview
            }
            con.Close();
        } catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    /// <summary>
    /// Switch the buttons colors
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void OnToggled(object sender, ToggledEventArgs e)
    {
        randomColor = !randomColor; //toggle the random color
        if (randomColor)
        {
            //set buttons colors randomly
            Random rand = new();
            Color[] randomcolors = { Colors.Green, Colors.Blue, Colors.Red };//create list of random colors
            Tile00.BackgroundColor = randomcolors[rand.Next(0, 3)]; //set each tile to a random one of the colors
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
        addDataToDB(game);

        ResetGame();
    }

    /// <summary>
    /// Add a GameData to the DataBase
    /// </summary>
    /// <param name="game"></param>
    private void addDataToDB(GameData game)
    {
        var bitHost = "db.bit.io";
        var bitApiKey = "v2_3vPwz_a5t2YWH7XNN59ycUFWTxAaZ";
        var bitUser = "halmj30";
        var bitDbName = "halmj30/Lab8";
        var cs = $"Host={bitHost};Username={bitUser};Password={bitApiKey};Database={bitDbName}";
        try
        {
            using var con = new NpgsqlConnection(cs);
            con.Open();

            //var query = String.Format("INSERT INTO \"gamedata\" VALUES({0},{1})", game.Winner, game.Time);
            var query = "INSERT INTO \"gamedata\" VALUES(@winner, @time)";
            using var cmd = new NpgsqlCommand(query, con);
            cmd.Parameters.AddWithValue("winner", game.Winner);//add the winner
            cmd.Parameters.AddWithValue("time", game.Time); //add the time
            using NpgsqlDataReader reader = cmd.ExecuteReader(); //add the data
            
            con.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
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



