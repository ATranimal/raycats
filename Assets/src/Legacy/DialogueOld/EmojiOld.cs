using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EmojiOld : MonoBehaviour
{
    Image img;

    float counter;
    float fadeInTime;

    public void setImage(string nameOfImage) {
        string[] splitPaths = nameOfImage.Split('_');
        string pathToimage = "Emoji/" + splitPaths[0];
        for (int index = 1; index < splitPaths.Length; index++) {
            pathToimage += "/" + splitPaths[index];
        }

        img.sprite = Resources.Load<Sprite>(pathToimage);
    }

    void Awake()
    {
        img = GetComponent<Image>();

        counter = 0;
        fadeInTime = 0f;    
    }

    void Update()
    {
        // float transparencyValue = counter / fadeInTime;
        img.color = new Color(
            img.color.r,
            img.color.g,
            img.color.b,
            1
        );

        if (counter > fadeInTime) {
            counter += Time.deltaTime;
        }
    }   
}
