using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager sm;

    public AudioSource back_as;
    public AudioSource common_as;
    public AudioSource[] ding_as = new AudioSource[3];

    public AudioClip retro_time;
    public AudioClip wheel;
    public AudioClip ding;
    public AudioClip bell;
    public AudioClip good;
    public AudioClip bad;

    private bool back_fade = false;
    public bool back_fade_ { get; set; }

    public IEnumerator coffee_towering(int num) {
        float temp_pitch = 1 + GraphicManager.gr.cur_coffee_num_ * 0.04f;
        int cur_index = 0;
        for (int i = 0; i < num; i++) {
            ding_as[cur_index].pitch = temp_pitch;
            ding_as[cur_index].Play();
            temp_pitch += 0.2f;
            cur_index = (cur_index == 2) ? 0 : (cur_index + 1);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void end_play() {
        back_fade = true;
        common_as.clip = good;
        common_as.Play();
    }

    public void FixedUpdate() {
        if (back_fade) back_as.volume -= 0.03f;
    }

    public void Awake() {
        if (sm == null) { sm = this; } else { Destroy(this); }
    }
}
