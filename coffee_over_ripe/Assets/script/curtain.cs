using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class curtain : MonoBehaviour
{
    private bool beginning = true;

    public void FixedUpdate() {
        if (beginning) {
            gameObject.GetComponent<Image>().color += new Color(0f, 0f, 0f, -0.03f);
            if (gameObject.GetComponent<Image>().color.a < 0.05f) {
                gameObject.GetComponent<Image>().color -= new Color(0f, 0f, 0f, 1f);
                gameObject.GetComponent<RectTransform>().localPosition = new Vector3(-3000f, 0f);
                beginning = false;
            }
        }
    }
}
