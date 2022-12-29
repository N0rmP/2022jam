using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using System;
using Newtonsoft.Json;
using System.IO;
//using UnityEngine.UI.Extensions;

public class GraphicManager : MonoBehaviour {
    private float time;

    public static GraphicManager gr;
    public GameObject canvas;
    public GameObject coffee_side;

    public GameObject season_back;
    public Sprite[] season_back_sprites = new Sprite[4];

    public Image[] sliders = new Image[2];
    public TextMeshProUGUI[] counters = new TextMeshProUGUI[3];

    public TextMeshProUGUI[] entrophy_uses3 = new TextMeshProUGUI[3];
    public Image entrophy_slider;
    public TextMeshProUGUI entrophy_production_t;
    public TextMeshProUGUI entrophy_t;

    private const int coffee_limit = 1000;
    public GameObject coffee_obj;
    private GameObject[] coffee_bucket = new GameObject[1000];
    private int cur_coffee_num = 0;

    private Dictionary<GameObject, bool> glowings;
    private Dictionary<GameObject, Vector3> movings;
    private Dictionary<TextMeshProUGUI, float> fadings_text;
    private Dictionary<Image, float> fadings_image;
    private Dictionary<TextMeshProUGUI, int> countings;
    private Dictionary<Image, float> slidings;
    private readonly Color glow_lighten = new Color(0.01f, 0.01f, 0.01f, 0f);
    private readonly Color fade_emerge = new Color(0f, 0f, 0f, 0.1f);
    private const float slide_rate = 0.005f;

    #region general
    public void set_text(GameObject tar, string con) {
        tar.GetComponent<TextMeshProUGUI>().text = con;
    }

    public void set_image_color(GameObject tar, Color cor) {
        //glow precedes other color change
        foreach (GameObject g in glowings.Keys)
            if (g == tar)
                return;
        tar.GetComponent<Image>().color = cor;
    }

    public void set_image(GameObject tar, Sprite spr) {
        tar.GetComponent<Image>().sprite = spr;
    }



    #region glow
    private void glow() {
        float temp_r = -1;
        GameObject[] temp_g = new GameObject[glowings.Keys.Count];
        glowings.Keys.CopyTo(temp_g, 0);
        foreach (GameObject g in temp_g) {
            if (glowings[g])
                g.GetComponent<Image>().color += glow_lighten;
            else
                g.GetComponent<Image>().color -= glow_lighten;

            temp_r = g.GetComponent<Image>().color.r;
            if (temp_r > 0.99f)
                glowings[g] = false;
            else if (temp_r < 0.01f)
                glowings[g] = true;
        }
    }

    public void glowings_add(GameObject g) {
        glowings[g] = true;
    }

    public void glowings_remove(GameObject g) {
        glowings.Remove(g);
    }

    public void glowings_clear() {
        glowings.Clear();
    }
    #endregion glow
    #region move
    private void move() {
        GameObject[] temp_g = new GameObject[movings.Keys.Count];
        movings.Keys.CopyTo(temp_g, 0);
        foreach (GameObject g in temp_g) {
            if (movings[g].magnitude < 1) {
                g.GetComponent<RectTransform>().localPosition += movings[g];
                movings.Remove(g);
            } else {
                g.GetComponent<RectTransform>().localPosition += movings[g] * 0.1f;
                movings[g] *= 0.9f;
            }
        }
    }

    public void movings_add(GameObject g, Vector2 goal) {
        if (movings.ContainsKey(g))
            movings_remove(g);
        Vector3 temp_v = (Vector3)goal - g.GetComponent<RectTransform>().localPosition;
        movings[g] = temp_v;
    }

    public void movings_remove(GameObject g) {
        movings.Remove(g);
    }
    public void movings_clear() {
        movings.Clear();
    }
    #endregion move
    #region fade
    private void fade() {
        TextMeshProUGUI[] temp_t = new TextMeshProUGUI[fadings_text.Keys.Count];
        Image[] temp_i = new Image[fadings_image.Count];
        fadings_text.Keys.CopyTo(temp_t, 0);
        fadings_image.Keys.CopyTo(temp_i, 0);

        //text
        foreach (TextMeshProUGUI t in temp_t) {
            if (fadings_text[t] > 0) {
                //emerge
                if (t.color.a > 0.95f) {
                    t.color += new Color(0f, 0f, 0f, 1f);
                    fadings_text.Remove(t);
                } else
                    t.color += fade_emerge * fadings_text[t];
            } else {
                //disapper
                if (t.color.a < 0.05f) {
                    t.color -= new Color(0f, 0f, 0f, 1f);
                    fadings_text.Remove(t);
                } else
                    t.color += fade_emerge * fadings_text[t];
            }
        }

        //image
        foreach (Image i in temp_i) {
            if (fadings_image[i] > 0) {
                //emerge
                if (i.color.a > 0.95f) {
                    i.color += new Color(0f, 0f, 0f, 1f);
                    fadings_remove(i);
                } else
                    i.color += fade_emerge * fadings_image[i];
            } else {
                //disappear
                if (i.color.a < 0.05f) {
                    i.color -= new Color(0f, 0f, 0f, 1f);
                    fadings_image.Remove(i);
                } else
                    i.color += fade_emerge * fadings_image[i];
            }
        }
    }

    public void fadings_text_add(GameObject tar, float emerge_rate) {
        fadings_text[tar.GetComponent<TextMeshProUGUI>()] = emerge_rate;
    }
    public void fadings_add(TextMeshProUGUI tar, float emerge_rate) {
        fadings_text[tar] = emerge_rate;
    }
    public void fadings_text_remove(TextMeshProUGUI tar) {
        fadings_text.Remove(tar);
    }

    public void fadings_image_add(GameObject tar, float emerge_rate) {
        fadings_image[tar.GetComponent<Image>()] = emerge_rate;
    }
    public void fadings_add(Image tar, float emerge_rate) {
        fadings_image[tar] = emerge_rate;
    }
    public void fadings_remove(Image tar) {
        fadings_image.Remove(tar);
    }
    public void fadiings_clear() {
        fadings_image.Clear();
        fadings_text.Clear();
    }
    #endregion fade
    #region count
    private void count() {
        TextMeshProUGUI[] temp_t = new TextMeshProUGUI[countings.Count];
        countings.Keys.CopyTo(temp_t, 0);

        //★이걸 구현하려면 정말 코루틴뿐인 걸까?
        int temp_i = -1;
        foreach (TextMeshProUGUI t in temp_t) {
            temp_i = int.Parse(t.text);
            if (temp_i > countings[t]) {
                temp_i--;
                t.text = temp_i.ToString();
            } else if (int.Parse(t.text) < countings[t]) {
                if (countings[t] - 100 > temp_i) { temp_i += 61; } else { temp_i++; }
                t.text = temp_i.ToString();
            } else {
                countings_remove(t);
            }

        }
    }
    public void countings_add(TextMeshProUGUI tar, int goal) {
        if (countings.ContainsKey(tar))
            countings.Remove(tar);
        countings[tar] = goal;
    }
    public void countings_remove(TextMeshProUGUI tar) {
        countings.Remove(tar);
    }
    public void countings_clear() {
        countings.Clear();
    }
    #endregion count
    #region slide
    private void slide() {
        Image[] temp_t = new Image[slidings.Keys.Count];
        slidings.Keys.CopyTo(temp_t, 0);
        foreach (Image tar in temp_t) {
            if (slidings[tar] > 0.1f) {
                tar.fillAmount += slide_rate * 3;
                slidings[tar] -= slide_rate * 3;
            } else {
                tar.fillAmount += slide_rate;
                slidings[tar] -= slide_rate;
            }
            if (tar.fillAmount > 0.996f) tar.fillAmount = 0f;
            if (slidings[tar] < 0.004f) slidings.Remove(tar);
        }
    }

    public void slidings_add(Image tar, float val) {
        slidings[tar] = val;
    }

    public void slidings_remove(Image tar) {
        slidings.Remove(tar);
    }

    public void slidings_clear() {
        slidings.Clear();
    }
    #endregion slider
    #endregion general

    public void counter_update() {
        countings_add(counters[0], (int)GameManager.gm.year_counter);
        countings_add(counters[1], (int)GameManager.gm.season_counter);
        countings_add(counters[2], (int)GameManager.gm.coffee_counter);
        slidings_add(sliders[0].GetComponent<Image>(), GameManager.gm.counters_change[0]);
        slidings_add(sliders[1].GetComponent<Image>(), GameManager.gm.counters_change[1]);
    }

    public void entrophy_use_update() {
        for (int i = 0; i < 3; i++) {
            entrophy_uses3[i].text = (GameManager.gm.is_entrophy_use[i]) ? "O" : "X";
        }
    }

    public void entrophy_update(bool is_GO) {
        if (is_GO) {
            slidings_add(entrophy_slider, 0.125f);
        }
        entrophy_t.text = GameManager.gm.entrophy.ToString();
        entrophy_production_t.text = GameManager.gm.entrophy_prodution_rate.ToString();
    }

    public void season_update() {
        int temp_season = (int)GameManager.gm.season_counter % 4;
        fadings_image_add(season_back, -1);
        StartCoroutine(season_back_recover(temp_season));
    }

    public IEnumerator coffee_update() {
        int temp_loop_num = (int)GameManager.gm.coffee_counter / 1000;
        for (; cur_coffee_num < temp_loop_num; cur_coffee_num++) {
            movings_add(coffee_bucket[cur_coffee_num],
                coffee_bucket[cur_coffee_num].GetComponent<RectTransform>().localPosition - new Vector3(0f, 1000f, 0f)
                );
            yield return new WaitForSeconds(0.4f);
        }
    }

    private IEnumerator season_back_recover(int temp_season) {
        yield return new WaitForSeconds(0.3f);
        season_back.GetComponent<Image>().sprite = season_back_sprites[temp_season];
        fadings_image_add(season_back, 1);
    }

    public void FixedUpdate() {
        glow();
        move();
        fade();
        slide();

        time += Time.deltaTime;
        if (time > 0.001f) {
            count();
            time = 0f;
        }

        if (entrophy_slider.fillAmount > 0.494f) {
            slidings.Remove(entrophy_slider);
            entrophy_slider.fillAmount = 0f;
        }
    }

    void Awake() {
        if (gr == null) { gr = this; } else { Destroy(this.gameObject); }
    }

    public void Start() {
        glowings = new Dictionary<GameObject, bool>();
        movings = new Dictionary<GameObject, Vector3>();
        fadings_text = new Dictionary<TextMeshProUGUI, float>();
        fadings_image = new Dictionary<Image, float>();
        countings = new Dictionary<TextMeshProUGUI, int>();
        slidings = new Dictionary<Image, float>();

        for (int i = 0; i < 1000; i++) {
            coffee_bucket[i] = Instantiate(coffee_obj, new Vector2(0f, 0f), Quaternion.Euler(0f, 0f, (float)Random.Range(0, 360)));
            coffee_bucket[i].transform.SetParent(coffee_side.transform);
            coffee_bucket[i].GetComponent<RectTransform>().localPosition = new Vector2(80f + i % 10 * 20, 620f + i / 10 * 30);
        }

        //entrophy_update();
    }
}