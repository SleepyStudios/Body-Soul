using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {
    float shownHealth = 1f;
    void Update() {
        shownHealth = Mathf.Lerp(shownHealth, GameObject.Find("Player").GetComponent<PlayerController>().health / 100f, 0.1f);
        GetComponent<Image>().fillAmount = shownHealth; 
    }
}
