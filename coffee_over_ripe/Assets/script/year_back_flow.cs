using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class year_back_flow : MonoBehaviour
{
    RectTransform rt;

    public void FixedUpdate() {
        rt.localPosition += new Vector3(0f, 0.1f, 0f);
        if (rt.localPosition.y > 940f)
            rt.localPosition -= new Vector3(0f, 2040f, 0f);
    }

    public void Awake() {
        rt = gameObject.GetComponent<RectTransform>();
    }
}
