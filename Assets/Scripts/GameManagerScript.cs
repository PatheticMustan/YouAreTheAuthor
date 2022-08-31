using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Made by Kevin Wang! :D
 * https://github.com/PatheticMustan
*/

public class GameManagerScript : MonoBehaviour {
    public Image BottomText;
    // Main
    public InputField mainTextBox;
    public Text newWordText;
    public Text announcementText;
    // Header
    public Text moneyText;
    public Text roundText;
    public Text timeText;
    // Footer
    public Text genreText;
    public Text wordsCompletedText;
    public Text billsText;
    public Text charactersTypedText;

    private string[] words;

    // Timers
    public float timeLeft;
    private float timeGreenFlash;
    private float startSetGoTimer;

    // Progression.
    private int wordsLeft;

    private bool roundOver;
    private bool playing;

    // Shorten references to Upgrade Manager Script
    //public UpgradeManagerScript ugm;
    public SummaryManagerScript scm;
    public WordManagerScript wms;


    void Start() {
        // Timers
        timeGreenFlash = 1.0f;
        startSetGoTimer = 0.0f;

        // Progression
        roundOver = false;

        // Start the first round.
        Debug.Log("Start finished!");
        RoundStart();
    }


    void Update() {
        if (newWordText.text.StartsWith(mainTextBox.text) == false) {
            mainTextBox.GetComponent<InputField>().image.color = new Color32(0xFF, 0xB6, 0xB6, 0xFF);
        } else {
            // Green flash! :D
            timeGreenFlash += Time.deltaTime * 2; // The higher the multiplier is, the faster the flash is.
            mainTextBox.image.color = Color.Lerp(Color.green, Color.white, Mathf.Min(timeGreenFlash, 1));
        }

        // This value is impossible under normal circumstances.
        // If this is true, it means I do not want StartSetGo to be run until I say so.
        if (startSetGoTimer > -9001.0f) { //IT'S UNDER 9001
            startSetGoTimer += Time.deltaTime;
            StartSetGo(startSetGoTimer);
        }


        if (playing == true) {
            //float startSetGoTimer = 0.0f;

            //Constantly selects the textbox.
            mainTextBox.ActivateInputField();
            mainTextBox.Select();

            timeLeft -= Time.deltaTime;

            if (timeLeft < 0) {
                RoundEnd();
            }

            /*
             * Don't touch this!!!
             * Minutes = t / 60
             * Seconds = t % 60
             * Hundo Seconds = t * 100 % 100
            */

            timeText.text =
                        Mathf.Max(0, (Mathf.Floor(timeLeft / 60))).ToString().PadLeft(2, '0') + ':' + // Minutes
                        Mathf.Max(0, (Mathf.Floor(timeLeft % 60))).ToString().PadLeft(2, '0') + '.' + // Seconds
                        Mathf.Max(0, (Mathf.Floor(timeLeft * 100 % 100))).ToString().PadLeft(2, '0'); // Hundredths of Seconds.

            if (mainTextBox.text.ToLower() == newWordText.text.ToLower()) {
                DataScript.charactersTyped += newWordText.text.Length;
                charactersTypedText.text = "Characters typed: " + DataScript.charactersTyped;
                timeGreenFlash = 0.0f;
                // Award money.
                DataScript.money += newWordText.text.Length * DataScript.moneyPerChar; // * ((bool)ugm.upgradeValues["moneyMultiplier"] ? 2 : 1);
                // Display new cash amount.
                moneyText.text = DataScript.money.ToString("c2");
                // Update completed words.
                DataScript.wordsCompleted += 1;
                wordsCompletedText.text = DataScript.wordsCompleted + " words completed.";

                // bottom text green if past rent
                if (DataScript.money >= DataScript.billsCost) {
                    BottomText.color = new Color32(0x72, 0xE5, 0x5C, 0xFF);
                }

                if (wordsLeft != 1) {
                    wordsLeft -= 1;
                    // Pick and display a new word.
                    newWordText.text = words[Random.Range(0, words.Length)];
                    mainTextBox.transform.Find("Placeholder").GetComponent<Text>().text = newWordText.text;
                } else {
                    // There are no more words
                    RoundEnd();
                }
                // Clear the textbox.
                mainTextBox.text = "";
            }
        } else {
            mainTextBox.text = "";
        }
    }

    void RoundStart() {
        DataScript.wordsCompleted = 0;
        wordsCompletedText.text = DataScript.wordsCompleted + " words completed.";
        DataScript.currentDay += 1;
        wordsLeft = 100;
        // CHANGE THIS!!!
        timeLeft = 60.0f; // + ((bool)ugm.upgradeValues["timeIncrease"] ? 30.0f : 0.0f);
        // should modify bills to increase difficulty?
        DataScript.billsCost = 25.0f;

        WordPack[] usableWordpacks = wms.GetUsableWordpacks();
        int wordpackIndex = Random.Range(0, usableWordpacks.Length); // pick random wordpack
        string wordpackName = usableWordpacks[wordpackIndex].name;

        // Sets up the words.
        words = wms.GetWordpack(wordpackName);
        newWordText.text = words[Random.Range(0, words.Length)];
        mainTextBox.transform.Find("Placeholder").GetComponent<Text>().text = newWordText.text;

        //Sets the genre.
        genreText.text =
            "Genre " + (1 + wordpackIndex) + "/" + usableWordpacks.Length + ": " + wordpackName + " (" + words.Length + " words)";

        timeText.text =
                    (Mathf.FloorToInt(timeLeft / 60)).ToString().PadLeft(2, '0') + ':' + // Minutes
                    (Mathf.FloorToInt(timeLeft % 60)).ToString().PadLeft(2, '0') + '.' + // Seconds
                    (Mathf.FloorToInt(timeLeft * 100 % 100)).ToString().PadLeft(2, '0'); // Hundredths of Seconds.

        scm.previousTotal = DataScript.money;
        billsText.text = "Bills: " + Mathf.Abs(DataScript.billsCost).ToString("c2");
        roundText.text = "Round " + DataScript.currentDay.ToString();
        DataScript.charactersTyped = 0;
        charactersTypedText.text = "Characters Typed: 0";

        roundOver = false;

        if (DataScript.money >= DataScript.billsCost) {
            BottomText.color = new Color32(0x72, 0xE5, 0x5C, 0xFF);
        } else {
            BottomText.color = new Color32(0xFF, 0x86, 0x86, 0xFF);
        }
    }



    void RoundEnd() {
        playing = false;
        startSetGoTimer = -3.0f;

        // Reset upgrades.
        DataScript.money -= DataScript.billsCost;

        moneyText.text = DataScript.money.ToString("c2");

        Debug.Log("End round!");
    }



    void StartSetGo(float currentSSGTimer) {
        switch (Mathf.FloorToInt(currentSSGTimer)) {
            case -3:
                announcementText.color = new Color32(0x93, 0x13, 0x13, 0xFF);
                announcementText.text = "FINISH";
                announcementText.GetComponent<CanvasGroup>().alpha = 1;

                roundOver = true;
                break;

            case 0:
                if (roundOver == true) {
                    scm.ShowSummary();
                    startSetGoTimer = -9001.0f;
                } else {
                    // Ready, show text.
                    announcementText.color = new Color32(0x00, 0x73, 0x06, 0xFF);
                    announcementText.GetComponent<CanvasGroup>().alpha = 1;
                    announcementText.text = "READY";
                }
                break;
            case 1:
                // Set
                announcementText.text = "SET";
                break;

            case 2:
                // Go
                announcementText.text = "GO";
                break;

            case 3:
                // Hide Go
                announcementText.GetComponent<CanvasGroup>().alpha = 0;
                playing = true;
                break;

            default:
                // Do nothing.
                break;
        }
    }
}