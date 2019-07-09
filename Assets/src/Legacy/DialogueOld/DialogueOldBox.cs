using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using System.IO;

public class DialogueOldBox : MonoBehaviour
{
    public GameObject emojiPrefab;

    TextAsset text;
    string pathToText;
    
    string[] parsedDialogue;

    float counter;
    int index;
    int emojiCount;
    float currentSpeed;
    GameObject speaker;
    
    public void Initialize(string pathToText, GameObject speaker)
    {  
        emojiCount = 0;
        this.speaker = speaker;
        this.setPathToText(pathToText);
        this.parseDialogue();
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

        setPosition();
    }

    private void setPosition() {
        Vector3 dialogueBoxPosition = Camera.main.WorldToViewportPoint(speaker.transform.position);
        // float speakerHeight = speaker.GetComponent<SpriteRenderer>().
        float speakerHeight = 10;

        this.transform.position = new Vector3(
            dialogueBoxPosition.x * Screen.width,
            dialogueBoxPosition.y * Screen.height + speakerHeight,
            0
        );
    }

    private void setPathToText(string pathToText) {
        this.pathToText = pathToText;
    }

    private void parseDialogue() {
        text = Resources.Load<TextAsset>(pathToText);
        try {
            parsedDialogue = text.text.Split(
                new string[] { "\r\n", "\n"},
                StringSplitOptions.None
            );
        } catch {
            Debug.LogError("No path to text given");
            GameObject.Destroy(this);
        }
    }

    private void setInitialSpeed() {
        try {
            currentSpeed = float.Parse(parsedDialogue[0]);
            index = 1;
        } catch {
            Debug.LogError("text file " + pathToText + " does not start with a float");
        }
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
