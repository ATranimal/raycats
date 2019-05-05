using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public GameObject dialogueBoxPrefab;
    public string dialoguePath;

    private void OnTriggerStay(Collider other) {
        if (Input.GetButtonDown("A") && other.tag == "Player") {
            this.interaction();
        }
    }
    public void interaction() {
        Vector3 dialogueBoxPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        print(dialogueBoxPosition);
        GameObject newDialogueBox = Instantiate(
            dialogueBoxPrefab,
            dialogueBoxPosition,
            Quaternion.Euler(0, 0, 0)
        );

        newDialogueBox.transform.SetParent(GameObject.Find("Canvas").transform, false);
        newDialogueBox.GetComponent<DialogueBox>().Initialize(dialoguePath);
    }
}
