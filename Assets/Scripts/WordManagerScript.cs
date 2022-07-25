using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

[System.Serializable]
public class WordPack
{
    public string name;
    public string description;
    public string fileName;
    public float price; // Some wordpacks are gonna be pricy pricy spicy spicy
    public bool unlocked; // Unlocked, not bought. Some wordpacks can be unlocked through achievements.
    public bool enabled;
}

public class WordManagerScript : MonoBehaviour
{
    // name, description, fileName, enabled
    public WordPack[] Wordlist;
    // key : words
    public Dictionary<string, string[]> loadedWords = new Dictionary<string, string[]>();





    void Start() { LoadWordpacks(); }

    // Update is called once per frame
    void Update() { }


    void LoadWordpacks()
    {
        // root path
        string wordpackContainerPath = "Assets/Resources/Wordpacks/";

        foreach (WordPack wordpack in Wordlist)
        {
            string[] lines = System.IO.File.ReadAllLines(wordpackContainerPath + wordpack.fileName + ".txt");

            // Cleans up the entire wordlist so that it only contains spaces, and A-Z.
            for (int i = 0; i < lines.Length; i++) lines[i] = Regex.Replace(lines[i], "[^a-zA-Z ]", "").ToLower();

            // Add the words into the loaded words dictionary.
            loadedWords.Add(
                wordpack.name,
                lines
            );
        }
        /*
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        Debug.Log(reader.ReadToEnd());
        reader.Close();
        */
    }

    public WordPack[] GetWordpackMeta() { return Wordlist; }

    // very cool one liner
    public WordPack[] GetUsableWordpacks() { return Wordlist.Where(w => w.unlocked && w.enabled).ToArray(); }

    public string[] GetWordpack(string wordpackName) { return loadedWords[wordpackName]; }
}