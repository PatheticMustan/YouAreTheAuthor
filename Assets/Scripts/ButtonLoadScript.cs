using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLoadScript : MonoBehaviour {
	void Start () {}
	void Update () {}

    public void loadSceneThing () {
        SceneManager.LoadScene("Gameplay");
    }
}
