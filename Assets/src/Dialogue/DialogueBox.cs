using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System;

public class DialogueBox : MonoBehaviour
{
    ///////////////////
    // Public Variables
    public GameObject emojiPrefab;

    ////////////////////
    // Private Variables
    string pathToText;
    GameObject speaker;

    //////////////////////
    // Operation Variables
    //  text: actual text object that dialogue will be parsed from
    TextAsset text;
    // counterForNextEmoji: increments up from 0 by Time.deltaTime
    float counterForNextEmoji;
    // intervalBetweenEmoji: how long elapses between each emoji
    float intervalBetweenEmoji;
    // indexOfCurrentEmoji: index for going through parsedDialogue
    int indexOfCurrentEmoji;
    // parsedDialogue: the string parsed from the text object.
    string[] parsedDialogue;
    // emojiCount: used to determine emojiCount for position in box
    // *note differs from index because doesn't include speed changes
    int emojiCount;
    // totalNumberOfEmoji: how many emoji are in the current dialogue, for calculating size
    int totalNumberOfEmoji;

    public void Initialize(string pathToText, GameObject speaker)
    {
        emojiCount = 0;
        this.speaker = speaker;
        this.pathToText = pathToText;
        this.parseDialogue();
        this.setDialogueBoxSize();
    }

    // parseDialgoue(): read text from resources using pathToText
    private void parseDialogue() {
        text = Resources.Load<TextAsset>(pathToText);
        try {
            parsedDialogue = text.text.Split(
                new string[] { "\r\n", "\n"},
                StringSplitOptions.None
            );
            float temp;
            foreach (var line in parsedDialogue) {
                if (!float.TryParse(line, out temp)) {
                    this.totalNumberOfEmoji++;
                }
            }
        } catch {
            Debug.LogError("No path to text given");
            GameObject.Destroy(this);
        }
    }

    private void setDialogueBoxSize() {
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        float dialogueBoxWidth = this.totalNumberOfEmoji > 6
            ? 6
            : this.totalNumberOfEmoji;
        float dialogueBoxHeight = (this.totalNumberOfEmoji - 1) / 6 + 1;
        rend.size = new Vector2(dialogueBoxWidth * 0.64f, dialogueBoxHeight * 0.64f);
    }

    void Update()
    {
        float newSpeed;
        bool timeBetweenEmojiHasElapsed = counterForNextEmoji > intervalBetweenEmoji;

        if (indexOfCurrentEmoji < parsedDialogue.Length){
            // changeSpeedOfText: If next index is a float, set the interval to be that value
            if (float.TryParse(parsedDialogue[indexOfCurrentEmoji], out newSpeed)) {
                intervalBetweenEmoji = newSpeed;
                indexOfCurrentEmoji += 1;
            } else {
                // instantiateEmojiAndResetCounter: Create emoji and reset the loop
                if (timeBetweenEmojiHasElapsed) {
                    createEmoji(parsedDialogue[indexOfCurrentEmoji]);
                    counterForNextEmoji = 0;
                    indexOfCurrentEmoji += 1;
                }
            }
        } else {
            // TODO: wait for input to destory box, or set timeout for box to disapper (might not need with the new dialogue system)
        }

        counterForNextEmoji += Time.deltaTime;
    }

    private void createEmoji(string nameOfEmoji) {
        // Set position! 
        Vector3 newPosition = new Vector3(emojiCount * -0.5f, 0, 0.2f);

        GameObject newEmoji = Instantiate(
            emojiPrefab,
            newPosition,
            Quaternion.Euler(0, 0, 0)
        );
        newEmoji.transform.SetParent(this.transform, false);
        newEmoji.GetComponent<Emoji>().setImage(nameOfEmoji);

        emojiCount++;
    }
}
