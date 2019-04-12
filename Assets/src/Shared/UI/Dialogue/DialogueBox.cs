using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using System.IO;

public class DialogueBox : MonoBehaviour
{
    public GameObject emojiPrefab;

    TextAsset text;
    string pathToText = "Dialogue/Test/test";  

    string[] parsedDialogue;

    float counter;
    int index;
    int emojiCount;
    float currentSpeed;
    
    void Start()
    {  
        parseDialogue();
        emojiCount = 0;

        try {  
            currentSpeed = float.Parse(parsedDialogue[0]);
            index = 1;
        } catch {
            Debug.LogError("text file " + pathToText + " does not start with a float");
        }
    }

    void Update()
    {
        float newSpeed;
        bool timeBetweenEmojiHasElapsed = counter > currentSpeed;

        if (index < parsedDialogue.Length) {
            if (float.TryParse(parsedDialogue[index], out newSpeed)) {
                currentSpeed = newSpeed;
                index += 1;
            } else {
                if (timeBetweenEmojiHasElapsed) {
                    createEmoji(parsedDialogue[index]);
                    counter = 0;
                    index += 1;
                }
            }
        }
        else {
            // wait for input to destroy box? or set timeout for box to disappear on its own.
        }
        
        counter += Time.deltaTime;
    }

    private void parseDialogue() {
        text = Resources.Load<TextAsset>(pathToText);
        parsedDialogue = text.text.Split(
            new string[] { "\r\n", "\n"},
            StringSplitOptions.None
        );
    }

    private void createEmoji(string nameOfEmoji) {
        Vector3 newPosition = this.transform.position + new Vector3(emojiCount * 50, 0, 0);
        emojiCount += 1;

        GameObject newEmoji = Instantiate(
            emojiPrefab,
            newPosition,
            Quaternion.Euler(0, 0, 0)
        );
        newEmoji.transform.SetParent(this.transform.parent);
        newEmoji.GetComponent<Emoji>().setImage(nameOfEmoji);
    }
}
