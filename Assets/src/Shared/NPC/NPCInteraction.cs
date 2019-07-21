using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCInteraction : MonoBehaviour
{
    // Public Variables
    public Interaction interactionType;
    public GameObject dialogueBoxPrefab;
    public string dialoguePath;

    // Private Variables
    
    // Operation Variables

    private void OnTriggerStay(Collider other) {
        if (Input.GetButtonDown("Interact") && other.tag == "Player") {
            switch (interactionType)
            {
                case Interaction.COOKING:
                    this.cookingInteraction();
                    break;
                case Interaction.DIALOGUE:
                    this.dialogueInteraction();
                    break;
            }
        }
    }

    public void cookingInteraction() {
        SceneManager.LoadSceneAsync("TestCooking");
    }

    public void dialogueInteraction() {
        GameObject newDialogueBox = Instantiate(
            dialogueBoxPrefab,
            new Vector3(0, 1, 0),
            Quaternion.Euler(0, 0, 0)
        );

        newDialogueBox.transform.SetParent(this.transform, false);
        newDialogueBox.GetComponent<DialogueBox>().Initialize(dialoguePath, this.gameObject);
    }
}
