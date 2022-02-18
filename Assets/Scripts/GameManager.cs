using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    // Array of all possible words
    List<string> words = new List<string>();
    // Queue of past 30 words
    // Used to prevent repeat of words in relative succession
    FixedLengthQueue<string> pastWords = new FixedLengthQueue<string>(30);

    // List of clicked keyboard buttons to change color
    [SerializeField]
    List<Image> keyImages = new List<Image>(5);

    [Header("Screen GameObjects")]
    [Tooltip("Title Screen")]
    public GameObject titleScreen;
    [Tooltip("Game Screen")]
    public GameObject gameScreen;

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

    /// <summary>
    /// Choose a word based on the list of words available
    /// </summary>
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

        titleScreen.SetActive(false);
        gameScreen.SetActive(true);
    }

    /// <summary>
    /// Insert given letter into word and visual
    /// </summary>
    /// <param name="letter">Letter to insert</param>
    public void InsertLetter(string letter)
    {
        // Check if word is already at 5 letters
        if (lineWord.Length >= 5)
            return;

        // Add letter to line word
        lineWord += letter.ToLower();

        // get the current opening visual
        Transform currSpot = lines.transform.GetChild(lineCount).GetChild(letterCount).GetChild(0);
        // Increment letterCount
        letterCount++;

        // Show letter on screen
        currSpot.GetComponent<Text>().text = letter.ToUpper();

        // add keyboard button to list of keys
        keyImages.Add(EventSystem.current.currentSelectedGameObject.GetComponent<Image>());
    }

    /// <summary>
    /// Check for word Validity (Enter button)
    /// </summary>
    public void CheckValid()
    {
        // If Line is not finished, return
        // TODO: Play animation/sound/text to notify player they need to finish the word
        if (lineWord.Length != 5)
            return;

        // Check if lineWord is a valid word
        // TODO: Play animation/sound/text to notify player they need to type a valid word
        if (!words.Contains(lineWord))
            return;


        // Iterate through the letters and check them against the chosen word
        for (int i = 0; i < 5; i++)
        {
            // Current letter
            string currLetter = lineWord[i].ToString();

            // Current keyboard key
            Image currKey = keyImages[i];

            // Current Letter visual
            Image currVis = lines.transform.GetChild(lineCount).GetChild(i).GetComponent<Image>();

            
            // Right letter
            if (word.Contains(currLetter))
            {
                // Right spot
                if (word[i].ToString().Equals(currLetter))
                {
                    ChangeColor(currKey, Color.green);
                    ChangeColor(currVis, Color.green);
                }
                // Wrong spot
                else
                {
                    ChangeColor(currKey, Color.yellow);
                    ChangeColor(currVis, Color.yellow);
                }
            }
            // Wrong letter
            else
            {
                ChangeColor(currKey, Color.gray);
                ChangeColor(currVis, Color.gray);
            }
        }

        // Reset letterCount, lineWord, and keyImages and increment lineCount
        letterCount = 0;
        lineWord = "";
        keyImages = new List<Image>(5);
        lineCount++;

        // Call end game if last line, loser
        if (lineCount > 5)
            EndGame(false);

        // Call end game if right word, winner
        if (word.Equals(lineWord))
            EndGame(true);
    }

    /// <summary>
    /// Erase the previous letter from being typed
    /// </summary>
    public void BackSpace()
    {
        // Check that a letter has been typed
        if (lineWord.Length == 0)
            return;

        // Decrease letterCount
        letterCount--;

        // remove letter from line word
        lineWord = lineWord.Remove(letterCount);

        // get last visual
        Transform currSpot = lines.transform.GetChild(lineCount).GetChild(letterCount).GetChild(0);
        // Erase letter on screen
        currSpot.GetComponent<Text>().text = "";

        // Remove keyboard button from list
        keyImages.Remove(keyImages[letterCount]);
    }

    /// <summary>
    /// Change the color of a given image to a given color
    /// </summary>
    /// <param name="image">Image to change the color of</param>
    /// <param name="color">Color to change the image to</param>
    private void ChangeColor(Image image, Color color)
    {
        image.color = color;
    }

    /// <summary>
    /// Execute End Game conditions based on whether the player won or loss
    /// </summary>
    /// <param name="win">True if the player won, False if the player loss</param>
    private void EndGame(bool win)
    {
        if (win)
        {
            // do something spectacular
        }
        else
        {
            // do something embarassing
        }
    }
}
