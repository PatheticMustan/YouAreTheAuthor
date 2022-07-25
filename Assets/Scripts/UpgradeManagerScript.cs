using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UpgradeManagerScript : MonoBehaviour {
    public GameObject upgradePrefab;
    public Transform content;

    public Text moneyText;

    public GameObject[] upgradeGameObjects;

    public Dictionary<string, object> upgradeValues;



    void Start() {
        upgradeGameObjects = new GameObject[DataScript.upgradesInfo.Length];

        for (int i = 0; i < DataScript.upgradesInfo.Length; i++) {
            Debug.Log(DataScript.upgradesInfo[i].ToString());
            // Create upgrade.
            GameObject currentUpgrade = Instantiate(upgradePrefab);
            currentUpgrade.transform.SetParent(content);

            currentUpgrade.GetComponent<UpgradeItemScript>().uid = i;
            currentUpgrade.GetComponent<CanvasGroup>().interactable = true;
            currentUpgrade.GetComponent<UpgradeItemScript>().bought = false;

            upgradeGameObjects[i] = currentUpgrade;
            DataScript.upgradesInfo[i].value = 0;
        }

        UpdateMoneyButtons();
    }

    void Update() { }

    public void UpdateMoneyButtons() {
        // Update
        moneyText.text = DataScript.money.ToString("c2");
        foreach (GameObject go in upgradeGameObjects) {
            go.GetComponent<UpgradeItemScript>().updateButton();
        }
    }

    public void NextRound() {
        SceneManager.LoadScene("Gameplay");
    }
}