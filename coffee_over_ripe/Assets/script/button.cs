using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class button : MonoBehaviour
{
    public int code;

    public void on_click() {
        bool temp_bool = GameManager.gm.is_entrophy_use[code];
        string temp_str = "E";

        if (temp_bool || GameManager.gm.entrophy > 0) {
            GameManager.gm.is_entrophy_use[code] = !GameManager.gm.is_entrophy_use[code];
            if (temp_bool) {
                temp_str = "X";
                GameManager.gm.entrophy++;
            } else {
                temp_str = "O";
                GameManager.gm.entrophy--;
            }
            GraphicManager.gr.entrophy_update(false);
            transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = temp_str;
        }
    }
}
