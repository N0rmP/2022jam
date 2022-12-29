using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class button : MonoBehaviour
{
    public int code;
    public bool is_plus;

    public void on_click() {
        int temp_int = GameManager.gm.entrophy_uses[code];
        //string temp_str = "E";

        if (is_plus && (GameManager.gm.entrophy > 0) && (temp_int < 5)) {
            GameManager.gm.entrophy--;
            GameManager.gm.entrophy_uses[code]++;
            finish((++temp_int).ToString());
        } else if (!is_plus && (temp_int > 0)) {
            GameManager.gm.entrophy++;
            GameManager.gm.entrophy_uses[code]--;
            finish((--temp_int).ToString());
        }
    }

    private void finish(string temp_str) {
        GraphicManager.gr.entrophy_update(false);
        GraphicManager.gr.entrophy_uses3[code].text = temp_str;
    }
}
