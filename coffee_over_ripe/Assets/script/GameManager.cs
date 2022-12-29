using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public GO_button GO_butt_script;

    public GameObject[] go_for_end;

    public float entrophy_prodution_rate;
    public const float entrophy_prodution_d = 0.125f;
    private float cur_entrophy_production;
    public int entrophy;
    public float entrophy_counter;
    public bool[] is_entrophy_use;

    public const float entrophy_slow = 0.5f;
    public float[] counters_change;
    public float year_counter;
    public float season_counter;
    public float coffee_counter;

    public void GO() {
        float temp_coffee_rot = 1f;
        switch ((int)season_counter % 4) {
            case 1:
                temp_coffee_rot = 2f; break;
            case 3:
                temp_coffee_rot = 0.5f; break;
        }
        counters_change[0] = 0.25f * ((is_entrophy_use[0]) ? entrophy_slow : 1);
        counters_change[1] = 1f * ((is_entrophy_use[1]) ? entrophy_slow : 1);
        counters_change[2] = 2190f * ((is_entrophy_use[2]) ? entrophy_slow : 1) * temp_coffee_rot;
        for (int i = 0; i < 3; i++) is_entrophy_use[i] = false;

        year_counter += counters_change[0];
        season_counter += counters_change[1];
        coffee_counter += counters_change[2];

        entrophy_counter += 0.125f;
        entrophy_prodution_rate += entrophy_prodution_d;
        if (entrophy_counter > 0.4f) {
            entrophy_counter = 0f;
        }
        cur_entrophy_production += entrophy_prodution_rate;
        if (cur_entrophy_production > 1f) {
            int temp_ep = (int)cur_entrophy_production;
            entrophy += temp_ep;
            cur_entrophy_production -= temp_ep;
        }

        GraphicManager.gr.counter_update();
        GraphicManager.gr.season_update();
        GraphicManager.gr.entrophy_update(true);
        GraphicManager.gr.entrophy_use_update();
        Debug.Log("cc" + coffee_counter);
        StartCoroutine(GraphicManager.gr.coffee_update());

        if (year_counter >= 5f)
            StartCoroutine(end());
    }

    private IEnumerator end() {
        Debug.Log("end begins");
        int temp_season_to_coffee = -1;

        GO_butt_script.deactivate();
        yield return new WaitForSeconds(2f);
        year_counter = 5f;
        GraphicManager.gr.set_text(go_for_end[1], year_counter.ToString());
        GraphicManager.gr.set_text(go_for_end[3], season_counter.ToString());
        GraphicManager.gr.set_text(go_for_end[5], coffee_counter.ToString());
        GraphicManager.gr.movings_add(go_for_end[0], new Vector2(0f, 0f));
        yield return new WaitForSeconds(2f);
        GraphicManager.gr.set_text(go_for_end[2], "175200");
        GraphicManager.gr.movings_add(go_for_end[1], new Vector2(-1600f, 360f));
        GraphicManager.gr.movings_add(go_for_end[2], new Vector2(100f, 360f));
        yield return new WaitForSeconds(1f);
        temp_season_to_coffee = (int)(season_counter * 2190f);
        GraphicManager.gr.set_text(go_for_end[4], temp_season_to_coffee.ToString());
        GraphicManager.gr.movings_add(go_for_end[3], new Vector2(-1600f, 120f));
        GraphicManager.gr.movings_add(go_for_end[4], new Vector2(100f, 120f));
        yield return new WaitForSeconds(2f);
        GraphicManager.gr.set_text(go_for_end[6], (175200f + temp_season_to_coffee + coffee_counter).ToString());
        GraphicManager.gr.movings_add(go_for_end[6], new Vector2(200f, -360f));

    }

    public void Awake() {
        if (gm == null) { gm = this; } else { Destroy(this); };
        entrophy_prodution_rate = 0.125f;
        cur_entrophy_production = 0f;
        entrophy = 1;
        entrophy_counter = 0f;
        is_entrophy_use = new bool[3] { false, false, false };
        counters_change = new float[3] { 0f, 0f, 0f };
        year_counter = 4f;
        season_counter = 0f;
        coffee_counter = 0f;
    }
}
