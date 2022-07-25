using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItemScript : MonoBehaviour {
    // Upgrade ID
    public int uid;
    // References
    private UpgradeManagerScript ums;

    // Values
    public string title;
    public string description;
    public float cost;
    public float increaseAmount;
    public string id;
    public bool stackable;

    // Set?
    private bool set;
    public bool bought;

	void Start () {
        gameObject.GetComponent<CanvasGroup>().alpha = 1;
	}
	


	void Update () {
        // SPINNY LOCK OwO
        if (set == false) {
            if (uid != -1) {
                // Setup stuff I guess
                set = true;
                Upgrade data = DataScript.upgradesInfo[uid];

                // Set data.
                title          = (string) data.name;
                description    = (string) data.description;
                cost           = (float ) data.cost;
                increaseAmount = (float ) data.incrementPerPurchase;
                id             = (string) data.id;
                stackable      = (bool  ) data.stackable;

                gameObject.name = title.ToString();
                gameObject.transform.Find("Title").GetComponent<Text>().text = title.ToString();
                gameObject.transform.Find("Description").GetComponent<Text>().text = description.ToString();
                gameObject.transform.Find("Buy")
                          .transform.Find("Text").GetComponent<Text>().text = cost.ToString("c2");

                gameObject.transform.Find("Buy").gameObject.GetComponent<Button>().onClick.AddListener(localBuyItem);
            }
        }
	}

    public void updateButton () {
        // If you don't have enough money, or it's not stackable and you already bought it, prevent from clicking
        if (DataScript.money < cost || (bought && !stackable)) {
            gameObject.GetComponent<CanvasGroup>().interactable = false;
        }
    }

    void localBuyItem () {
        // If the player can click the button we already know they have enough money,
        // so we don't need to check
        DataScript.upgradesInfo[uid].value += 1;

        Debug.Log(
            "Bought a " + DataScript.upgradesInfo[uid].name +
            "(" + DataScript.upgradesInfo[uid].value + ") " +
            "for " + DataScript.upgradesInfo[uid].cost.ToString("c2") + "!"
        );

        DataScript.money -= cost;
        cost += increaseAmount;
        bought = true;

        // Updates both the money text, the money button texts, and interactive/not.
        ums.UpdateMoneyButtons();
    }
}
