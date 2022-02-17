using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    // Array of all possible words
    List<string> words = new List<string>();
    // Queue of past 30 words
    // Used to prevent repeat of words in relative succession
    FixedLengthQueue<string> pastWords = new FixedLengthQueue<string>(30);

    [Tooltip("Word chosen for this play")]
    public string word;
    [Header("Line and Character information")]
    [Tooltip("Line currently on")]
    public int lineCount = 0;

    [Tooltip("What the current line says")]
    public string lineWord = "";

    [Tooltip("Letter currently on")]
    public int letterCount = 0;

    [Tooltip("Line Container")]
    public GameObject lines;
    
    // Start is called before the first frame update
    void Start()
    {
        // Load list of words into array
        StreamReader reader = new StreamReader("./Assets/wordList.txt");
        string line = null;
        while ((line = reader.ReadLine()) != null)
        {
            string[] lineWords = line.Split();
            foreach (string word in lineWords)
            {
                words.Add(word);
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Choose a word based on the list of words available
    public void WordPicker()
    {
        // Pick number in range of words array
        int num = Random.Range(0, words.Count - 1);

        // Get that word
        word = words[num];

        // while word is in past word queue, get new word
        while (pastWords.Contains(word))
        {
            // Pick number in range of words array
            num = Random.Range(0, words.Count - 1);

            // Get that word
            word = words[num];
        }

        // Add fresh word into queue, knocking out oldest word if applicable
        pastWords.Enqueue(word);

    }

    // Insert given letter into word and visual
    public void InsertLetter(string letter)
    {

        // Add letter to line word
        lineWord += letter;

        // get the current opening visual
        Transform currSpot = lines.transform.GetChild(lineCount).GetChild(letterCount).GetChild(0);
        // Increment letterCount
        letterCount++;

        // Show letter on screen
        currSpot.GetComponent<Text>().text = letter.ToUpper();
    }

    // Check for word Validity (Enter button)
    public void CheckValid()
    {
        // Iterate through the letters and check them against the chosen word
        for (int i = 0; i < 5; i++)
        {
            // Current letter
            string curr = lineWord[i].ToString();
            // Right letter
            if (word.Contains(curr))
            {

            }
        }

        // Reset letterCount and lineWord and increment lineCount
        letterCount = 0;
        lineWord = "";
        lineCount++;

        // Call end game if last line
        if (lineCount > 5)
        {
            EndGame();
        }
    }

    private void ChangeColor(GameObject square)
    {

    }

    private void EndGame()
    {

    }
}
