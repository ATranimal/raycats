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
    int emojiCount = 0;
    // totalNumberOfEmoji: how many emoji are in the current dialogue, for calculating size
    int totalNumberOfEmoji;
    // maxEmojiPerLine: the width of the dialogue box in Emojis
    int maxEmojisPerLine = 5;
    // numberOfLines: the amount of lines in dialogue;
    int numberOfLines;

    public void Initialize(string pathToText, GameObject speaker)
    {
        emojiCount = 0;
        this.speaker = speaker;
        this.pathToText = pathToText;

        this.parseDialogue();
        this.numberOfLines = (this.totalNumberOfEmoji - 1) / this.maxEmojisPerLine + 1;
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

    // setDialogueBoxSize(): set dialogue box size
    private void setDialogueBoxSize() {
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        float dialogueBoxWidth = this.totalNumberOfEmoji > this.maxEmojisPerLine
            ? this.maxEmojisPerLine
            : this.totalNumberOfEmoji;
        float dialogueBoxHeight = this.numberOfLines;
        rend.size = new Vector2(dialogueBoxWidth * 0.55f, dialogueBoxHeight * 0.64f);
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
        float xOffset = (this.maxEmojisPerLine / 2) * 0.5f - (((this.maxEmojisPerLine + 1) % 2) * 0.25f);
        float emojiX = ((emojiCount % this.maxEmojisPerLine) * -0.5f) + xOffset;

        float yOffset = (this.numberOfLines - 1) * 0.25f;
        float emojiY = yOffset - ((emojiCount / this.maxEmojisPerLine) * 0.5f);

        Vector3 newPosition = new Vector3(emojiX, emojiY, 0.5f);

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
