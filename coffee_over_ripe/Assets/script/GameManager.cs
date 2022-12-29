using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public GO_button GO_butt_script;

    public GameObject[] go_for_end;

    public float entrophy_prodution_rate;
    public const float entrophy_prodution_d = 0.2f;
    private float cur_entrophy_production;
    public int entrophy;
    public float entrophy_counter;
    public int[] entrophy_uses;

    public const float entrophy_slow = 0.2f;
    public float[] counters_change;
    public float year_counter;
    public float season_counter;
    public float coffee_counter;

    public void GO() {
        counters_change[0] = 0.25f * (1f - entrophy_uses[0] * entrophy_slow);
        counters_change[1] = 1f * (1f - entrophy_uses[1] * entrophy_slow);
        counters_change[2] = 2190f * (1f - entrophy_uses[2] * entrophy_slow);

        float temp_coffee_rot = 1f;
        float temp_lgap; float temp_rgap;
        if (((int)season_counter % 4 == 1) || (((int)(season_counter + counters_change[1])) % 4 == 1)) {
            temp_lgap = (season_counter / 4) % 1 - 0.25f; temp_rgap = ((season_counter + counters_change[1]) / 4) % 1 - 0.5f;
            if (temp_lgap * temp_rgap <= 0) {
                temp_coffee_rot = 2f;
            } else if ((temp_lgap > 0) && (temp_rgap > 0)) {
                temp_coffee_rot += (0.25f - temp_lgap) / ((counters_change[1] / 4) % 1);
            } else if ((temp_lgap < 0) && (temp_rgap < 0)) {
                temp_coffee_rot += (0.25f + temp_rgap) / ((counters_change[1] / 4) % 1);
            }
        } else if (((int)season_counter % 4 == 3) || (((int)(season_counter + counters_change[1])) % 4 == 3)) {
            temp_lgap = (season_counter / 4) % 1 - 0.75f; temp_rgap = (((season_counter + counters_change[1]) / 4) % 1 < 0.25f) ? (((season_counter + counters_change[1]) / 4) % 1) : (((season_counter + counters_change[1]) / 4) % 1 - 1f);
            if (temp_lgap * temp_rgap <= 0) {
                temp_coffee_rot = 0f;
            } else if ((temp_lgap > 0) && (temp_rgap > 0)) {
                temp_coffee_rot = temp_rgap / ((counters_change[1] / 4) % 1);
            } else if ((temp_lgap < 0) && (temp_rgap < 0)) { 
                temp_coffee_rot = -temp_lgap / ((counters_change[1] / 4) % 1);
            }
        }
        counters_change[2] *= temp_coffee_rot;

        cur_entrophy_production += entrophy_prodution_rate;
        entrophy_prodution_rate += entrophy_prodution_d * entrophy_uses[0];
        for (int i = 0; i < 3; i++) entrophy_uses[i] = 0;

        if (year_counter + counters_change[0] > 10f) {
            counters_change[1] /= 2;
            counters_change[2] /= 2;
        }
        year_counter += counters_change[0];
        season_counter += counters_change[1];
        coffee_counter += counters_change[2];

        entrophy_counter += 0.125f;
        if (entrophy_counter > 0.4f) {
            entrophy_counter = 0f;
        }
        if (cur_entrophy_production >= 1f) {
            int temp_ep = (int)cur_entrophy_production;
            entrophy += temp_ep;
            cur_entrophy_production -= temp_ep;
        }

        GraphicManager.gr.counter_update();
        GraphicManager.gr.season_update();
        GraphicManager.gr.entrophy_update(true);
        GraphicManager.gr.entrophy_use_update();
        StartCoroutine(GraphicManager.gr.coffee_update());

        if (year_counter >= 10f)
            StartCoroutine(end());
    }

    private IEnumerator end() {
        int temp_season_to_coffee = -1;

        GO_butt_script.deactivate();
        SoundManager.sm.end_play();
        yield return new WaitForSeconds(2f);
        year_counter = 5f;
        GraphicManager.gr.set_text(go_for_end[1], year_counter.ToString());
        GraphicManager.gr.set_text(go_for_end[3], season_counter.ToString());
        GraphicManager.gr.set_text(go_for_end[5], ((int)coffee_counter).ToString());
        GraphicManager.gr.movings_add(go_for_end[0], new Vector2(0f, 0f));
        yield return new WaitForSeconds(2f);
        GraphicManager.gr.set_text(go_for_end[2], "87600");
        GraphicManager.gr.movings_add(go_for_end[1], new Vector2(-1600f, 360f));
        GraphicManager.gr.movings_add(go_for_end[2], new Vector2(200f, 360f));
        yield return new WaitForSeconds(1f);
        temp_season_to_coffee = (int)(season_counter * 2190f);
        GraphicManager.gr.set_text(go_for_end[4], temp_season_to_coffee.ToString());
        GraphicManager.gr.movings_add(go_for_end[3], new Vector2(-1600f, 120f));
        GraphicManager.gr.movings_add(go_for_end[4], new Vector2(200f, 120f));
        yield return new WaitForSeconds(2f);
        GraphicManager.gr.set_text(go_for_end[6], ((int)(87600f + temp_season_to_coffee + coffee_counter)).ToString());
        GraphicManager.gr.movings_add(go_for_end[6], new Vector2(200f, -360f));

    }

    public void Awake() {
        if (gm == null) { gm = this; } else { Destroy(this); };
        entrophy_prodution_rate = entrophy_prodution_d;
        cur_entrophy_production = 0f;
        entrophy = 1;
        entrophy_counter = 0f;
        entrophy_uses = new int[3] { 0, 0, 0 };
        counters_change = new float[3] { 0f, 0f, 0f };
        year_counter = 0f;
        season_counter = 0f;
        coffee_counter = 0f;
    }
}
