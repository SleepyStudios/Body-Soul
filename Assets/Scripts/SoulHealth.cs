using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulHealth : MonoBehaviour {
    float shownHealth = 1f;
    void Update() {
        shownHealth = Mathf.Lerp(shownHealth, GameObject.Find("Soul").GetComponent<Soul>().health / 100f, 0.1f);
        GetComponent<Image>().fillAmount = shownHealth;
    }
}
