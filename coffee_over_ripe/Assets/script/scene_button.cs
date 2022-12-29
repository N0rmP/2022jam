using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class scene_button : MonoBehaviour
{
    public GameObject curtain;
    private int acting = -1;

    public void to_menu() {
        acting = 0;
    }

    public void to_game() {
        acting = 1;
    }

    public void FixedUpdate() {
        if (acting != -1) {
            curtain.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f);
            curtain.GetComponent<Image>().color += new Color(0f, 0f, 0f, 0.05f);

            if (curtain.GetComponent<Image>().color.a > 0.9f) {
                switch (acting) {
                    case 0:
                        SceneManager.LoadScene("menu"); break;
                    case 1:
                        SceneManager.LoadScene("game"); break;
                }
            }
        }
    }
}
