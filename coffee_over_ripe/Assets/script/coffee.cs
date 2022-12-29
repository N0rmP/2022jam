using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coffee : MonoBehaviour
{
    public Sprite[] coffee_sprites = new Sprite[4];

    public void Start() {
        GraphicManager.gr.set_image(gameObject, coffee_sprites[(int)Random.Range(0f, 4f)]);
    }
}
