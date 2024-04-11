using Microsoft.Maui.Controls.PlatformConfiguration;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;
using Windows.System;
using Windows.UI.WindowManagement;

namespace final_work;

public partial class MainPage : ContentPage
{
    public int playerTurnCount { get; set; }
    public int playerTurn { get; set; }
    public bool WinLoseDrawCheck { get; set; }
    public string marker { get; set; }
    public bool AIPlacedMarker { get; set; }
    public DateTime startTime { get; set; }

    public MainPage()
    {
        InitializeComponent();

        playerTurnEntry.Text = "Start Player 1 (X)...";
        //clearing json file 
        string filePath = AppDomain.CurrentDomain.BaseDirectory;
        string fileCombine = Path.Combine(filePath, "playerdata.json");

        if (!File.Exists(fileCombine))
        {
            // Create a new empty list of userInfo objects
            List<userInfo> users = new List<userInfo>();

            // Serialize the list to JSON and write it to the file
            string emptyJson = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileCombine, emptyJson);
        }
        else
        {
            // Read the existing data
            string json = File.ReadAllText(fileCombine);
            List<userInfo> users = JsonSerializer.Deserialize<List<userInfo>>(json);
            users.Clear();
            string emptyJson = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileCombine, emptyJson);
        }

    }
    public void AIButton_Clicked(object sender, EventArgs e)
    {
        //reset match to start AI match
        resetButton_Clicked(sender, e);

    }
    public void AIPutsMarker(Button button)
    {
        
        if (!WinLoseDrawCheck)
        {
            checkForWins();
            playerTurnEntry.Text = $"Player 2 turn (O)...";
            Button selectedButton = null;
            AIPlacedMarker = false;
            //random delay between 0.5-2 sec
            Random random = new Random();
            double minValue = 0.5;
            double maxValue = 2.1;
            
            double range = maxValue - minValue;
            double randomDouble = random.NextDouble() * range + minValue;
                        
            //adds delay
            this.Dispatcher.StartTimer(TimeSpan.FromSeconds(randomDouble), () =>
            {
                while ((!AIPlacedMarker && !WinLoseDrawCheck) && playerTurnCount <= 9)
                {
                    AIPlacedMarker = false;
                    Debug.Write("while || ");
                    //generating random button to place marker on
                    Random rnd = new Random();
                    int buttonNumber = rnd.Next(1, 10);

                    switch (buttonNumber)
                    {
                        case 1:
                            selectedButton = button1;
                            break;
                        case 2:
                            selectedButton = button2;
                            break;
                        case 3:
                            selectedButton = button3;
                            break;
                        case 4:
                            selectedButton = button4;
                            break;
                        case 5:
                            selectedButton = button5;
                            break;
                        case 6:
                            selectedButton = button6;
                            break;
                        case 7:
                            selectedButton = button7;
                            break;
                        case 8:
                            selectedButton = button8;
                            break;
                        case 9:
                            selectedButton = button9;
                            break;
                    }
                    
                    if (!WinLoseDrawCheck && selectedButton.Text != "X" && selectedButton.Text != "O")
                    {
                        
                        //AI places marker
                        marker = "O";
                        selectedButton.Text = marker;
                        AIPlacedMarker = true;



                        if (!WinLoseDrawCheck)
                        {
                            playerTurnEntry.Text = $"Player 1 turn (X)...";
                            playerTurn = 1;
                            playerTurnCount += 1;
                        }
                        
                    }
                    checkForWins();
                    if (WinLoseDrawCheck)
                    {
                        break;
                    }


                }
                
                return false;
                
            });    
        }

    }

    public void Button_Clicked(object sender, EventArgs e)
    {
        //start timer
        if (playerTurnCount == 0)
        startTime = DateTime.Now;

        //player turns
        playerTurn = 0;
        if (playerTurnCount % 2 != 0)
        {
            playerTurn = 1;
        }
        
        //figure which X or O to put on board based on player turn
        var button = sender as Button;
        marker = "";
        //player turn text
        if (playerTurn == 0)
        {
            marker = "X";
            //text to say its opponents turn
            if (button.Text != "X" && button.Text != "O" && !playerTurnEntry.Text.Contains("Wins"))
            {
                playerTurnEntry.Text = $"Player 2 turn (O)...";
            }
        }

        else if (playerTurn == 1)
        {
            //remain player 1 marker as X if AIButton is ON 
            if (AIButton.IsChecked == true)
            {
                marker = "X";
            }
            else
            {
                marker = "O";
            }
            //text to say its opponents turn
            if (button.Text != "X" && button.Text != "O" && !playerTurnEntry.Text.Contains("Wins"))
            {
                playerTurnEntry.Text = $"Player 1 turn (X)...";
            }

        }

        //make AI place markers if button is checked and its right turn
        if (AIButton.IsChecked == true && playerTurn % 2 == 0)
        {
            AIPutsMarker(button);
        }

        //dont let player place markers if game is win, lose or draw 
        //dont let user place marker on already filled square
        if (button.Text != "X" && button.Text != "O" && !WinLoseDrawCheck)
        {
            button.Text = marker;

            checkForWins();
        }
        //add to counter
        playerTurnCount += 1;
    }
    public void checkForWins()
    {
        //for save file purposes
        bool win = false;
        bool draw = false;

        string winnerText = "temp";
        //checking all win conditions one by one of the same markers 3 lines
        //checking horizontal lines
        if (button1.Text == marker && button2.Text == marker && button3.Text == marker)
        {
            winnerText = $"Player {playerTurn + 1} Wins";
            playerTurnEntry.Text = winnerText;
            win = true;

        }
        else if (button4.Text == marker && button5.Text == marker && button6.Text == marker)
        {
            winnerText = $"Player {playerTurn + 1} Wins";
            playerTurnEntry.Text = winnerText;
            win = true;
        }
        else if (button7.Text == marker && button8.Text == marker && button9.Text == marker)
        {
            winnerText = $"Player {playerTurn + 1} Wins";
            playerTurnEntry.Text = winnerText;
            win = true;
        }
        //vertical lines
        else if (button1.Text == marker && button4.Text == marker && button7.Text == marker)
        {
            winnerText = $"Player {playerTurn + 1} Wins";
            playerTurnEntry.Text = winnerText;
            win = true;
        }
        else if (button2.Text == marker && button5.Text == marker && button8.Text == marker)
        {
            winnerText = $"Player {playerTurn + 1} Wins";
            playerTurnEntry.Text = winnerText;
            win = true;
        }
        else if (button3.Text == marker && button6.Text == marker && button9.Text == marker)
        {
            winnerText = $"Player {playerTurn + 1} Wins";
            playerTurnEntry.Text = winnerText;
            win = true;
        }
        //cross lines
        else if (button1.Text == marker && button5.Text == marker && button9.Text == marker)
        {
            winnerText = $"Player {playerTurn + 1} Wins";
            playerTurnEntry.Text = winnerText;
            win = true;
        }
        else if (button7.Text == marker && button5.Text == marker && button3.Text == marker)
        {
            winnerText = $"Player {playerTurn + 1} Wins";
            playerTurnEntry.Text = winnerText;
            win = true;
        }
        else
        {
            if (playerTurnCount == 9 && !WinLoseDrawCheck)
            {
                winnerText = "Draw";
                playerTurnEntry.Text = winnerText;
                draw = true;
            }
        }
        //change WinLoseDrawCheck condition if any conditions are met
        if (winnerText != "temp")
        {
            WinLoseDrawCheck = true;

            //end timer
            DateTime endTime = DateTime.Now;
            TimeSpan duration = endTime - startTime;
            Debug.Write("DURATION:: " + duration);
            double durationInSeconds = Math.Round(duration.TotalSeconds, 2);
            
            
            //save win,lose or draw to file
            UpdateUserWins(win, draw, durationInSeconds);
        }

    }
    public void resetButton_Clicked(object sender, EventArgs e)
    {
        //clear all buttons
        button1.Text = "";
        button2.Text = "";
        button3.Text = "";
        button4.Text = "";
        button5.Text = "";
        button6.Text = "";
        button7.Text = "";
        button8.Text = "";
        button9.Text = "";

        WinLoseDrawCheck = false;
        playerTurnEntry.Text = "Start Player 1 (X)...";

        playerTurnCount = 0;
        playerTurn = 0;
    }

    public async void SaveButton_Clicked(object sender, EventArgs e)
    {
        List<userInfo> tietue = new List<userInfo>();
        string filePath = AppDomain.CurrentDomain.BaseDirectory;
        string fileCombine = Path.Combine(filePath, "playerdata.json");
        //make sure user given info is in right format
        try
        {
            userInfo userInfo2 = new userInfo();
            //adding all info that is needed for jsonfile 
            tietue.Add(new userInfo() { userName = NameEntry.Text, userLastName = LastNameEntry.Text, userBirthYear = int.Parse(BirthYearEntry.Text), userWins = 0,userLoses = 0, userDraws = 0, userTotalGameLenght = 0 });

            //save userName for updating right json file info
            userInfo2.userName = NameEntry.Text;

            //empty entrys
            NameEntry.Text = null;
            LastNameEntry.Text = null;
            BirthYearEntry.Text = null;
            
        }
        catch (Exception ex)
        {
            await DisplayAlert("Virhe", "Syötit tiedot väärin", "OK");
            Debug.WriteLine(ex);
        }
        //writing to file
        string json = JsonSerializer.Serialize(tietue, new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(fileCombine, json);
    }

    public async void UpdateUserWins(bool win, bool draw, double durationInSeconds)
    {
        string filePath = AppDomain.CurrentDomain.BaseDirectory;
        string fileCombine = Path.Combine(filePath, "playerdata.json");

        Debug.Write("turncount: " + playerTurnCount + ", ");
        Debug.Write("playerturn:" + playerTurn + ", ");
        Debug.Write("winlosedraw:" + WinLoseDrawCheck + " || ");
        try
        {
            // Read the existing data
            string json = File.ReadAllText(fileCombine);
            List<userInfo> users = JsonSerializer.Deserialize<List<userInfo>>(json);

            //update stats 
            if (users != null && users.Any() && AIButton.IsChecked == true)
            {
                if (win && playerTurn == 0)
                {
                    users.Last().userWins++;
                }
                else if (win && playerTurn == 1)
                {
                    users.Last().userLoses++;
                }
                else if (draw)
                {
                    users.Last().userDraws++;
                }
                //update time
                users.Last().userTotalGameLenght += durationInSeconds;
            }
            else
            {
                if (AIButton.IsChecked == true)
                {
                    await DisplayAlert("No Users Found", "Enter user information", "OK");
                }
                return;
            }

            // Serialize the list back to JSON and write it to the file
            string updatedJson = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileCombine, updatedJson);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "An error occurred: " + ex.Message, "OK");
            Debug.WriteLine(ex);
        }
    }

}
public class userInfo
{

    // etunimi, sukunimi, syntymävuosi
    public string userName { get; set; }
    public string userLastName { get; set; }
    public int userBirthYear { get; set; }

    // voitot, tappiot, tasapelit, pelattujen pelien yhteiskesto
    public int userWins { get; set; }
    public int userLoses { get; set; }
    public int userDraws { get; set; }
    public double userTotalGameLenght { get; set; }

}
