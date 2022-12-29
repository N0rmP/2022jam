using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GO_button : MonoBehaviour
{
    public void on_click() {
        GameManager.gm.GO();
    }

    public void deactivate() {
        gameObject.GetComponent<Button>().interactable = false;
    }
}
