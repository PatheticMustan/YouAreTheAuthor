using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SummaryManagerScript : MonoBehaviour {
    public GameObject summaryPanel;
    public GameObject endScreen;

    public UpgradeManagerScript ums;

    public Text summaryRoundText;
    public Text previousTotalText;
    public Text billsText;
    public Text moneyEarnedText;
    public Text moneyTotalText;
    public Text wordsCompletedText;

    [HideInInspector]
    public float previousTotal;

    void Start () {
        previousTotal = 0.0f;
    }

    void Update () { }

    public void ShowSummary () {
        summaryRoundText.text = "Round " + DataScript.currentDay + " Finished!";
        previousTotalText.text = previousTotal.ToString("c2");
        FormatNumber(-DataScript.billsCost, billsText);
        FormatNumber(DataScript.money - previousTotal, moneyEarnedText);
        FormatNumber(DataScript.money, moneyTotalText);
        moneyTotalText.color = new Color32(0x32, 0x32, 0x32, 0xFF);
        wordsCompletedText.text = DataScript.wordsCompleted + " words completed!";

        summaryPanel.GetComponent<CanvasGroup>().alpha = 1;
        summaryPanel.GetComponent<CanvasGroup>().interactable = true;
        summaryPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void HideSummary () {
        summaryPanel.GetComponent<CanvasGroup>().alpha = 0;
        summaryPanel.GetComponent<CanvasGroup>().interactable = false;
        summaryPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

        if (DataScript.money < 0)
        {
            // Lose.
            endScreen.GetComponent<CanvasGroup>().alpha = 1;
            endScreen.GetComponent<CanvasGroup>().interactable = true;
            endScreen.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else {
            SceneManager.LoadScene("Upgrade");
        }
    }

    public void FormatNumber (float num, Text textValue) {
        if (num > 0) {
            textValue.text = ("+ " + num.ToString("c2"));
            textValue.color = new Color32(0x00, 0x72, 0x13, 0xFF);
        } else if (num < 0) {
            textValue.text = ("- " + Mathf.Abs(num).ToString("c2"));
            textValue.color = new Color32(0x93, 0x13, 0x13, 0xFF);
        } else {
            textValue.text = (num.ToString("c2"));
            textValue.color = new Color32(0x32, 0x32, 0x32, 0xFF);
        }
    }
}