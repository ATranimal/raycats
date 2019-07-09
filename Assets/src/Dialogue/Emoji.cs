using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Emoji : MonoBehaviour
{
    ///////////////////
    // Public Variables

    ////////////////////
    // Private Variables
    // rend: ref to sprite renderer component
    SpriteRenderer rend;

    //////////////////////
    // Operation Variables
    // 
    void Awake() {
        rend = GetComponent<SpriteRenderer>();
    }

    public void setImage(string nameOfImage) {
        string[] splitPaths = nameOfImage.Split('_');
        string pathToImage = "Emoji";
        for (int index = 0; index < splitPaths.Length; index++) {
            pathToImage += "/" + splitPaths[index];
        }

        rend.sprite = Resources.Load<Sprite>(pathToImage);
    }
}
