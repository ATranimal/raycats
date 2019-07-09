using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    // Public Variables
    public GameObject dialogueBoxPrefab;
    public string dialoguePath;

    private void OnTriggerStay(Collider other) {
        if (Input.GetButtonDown("A") && other.tag == "Player") {
            this.interaction();
        }
    }
    public void interaction() {
        GameObject newDialogueBox = Instantiate(
            dialogueBoxPrefab,
            new Vector3(0, 1, 0),
            Quaternion.Euler(0, 0, 0)
        );

        newDialogueBox.transform.SetParent(this.transform, false);
        newDialogueBox.GetComponent<DialogueBox>().Initialize(dialoguePath, this.gameObject);
    }
}
