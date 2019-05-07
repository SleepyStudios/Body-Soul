using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public static GameController instance;
    public bool lost;
    public float shownScore, score;
    int instructionStage = -1;
    float tmrInstructions;
    bool instructionsFade;
    int highscore;
    float tmrLogo;

    private void Awake() {
        instance = this;
    }

    void Update() {
        shownScore = Mathf.Lerp(shownScore, score, 0.1f);
        GameObject.Find("ScoreText").GetComponent<Text>().text = Mathf.Ceil(shownScore).ToString();

        if(lost && Input.GetKeyDown(KeyCode.Space)) {
            OnRestart();
        }

        tmrInstructions += Time.deltaTime;
        if(tmrInstructions >= 3.5f) {
            instructionsFade = true;
            tmrInstructions = 0;
        }

        if(instructionsFade) {
            GameObject.Find("InstructionsText").GetComponent<Text>().color = Color.Lerp(GameObject.Find("InstructionsText").GetComponent<Text>().color, new Color(0, 0, 0, 0), 0.1f);
            if (GameObject.Find("InstructionsText").GetComponent<Text>().color.a <= 0.1f && instructionStage < 3) {
                instructionStage++;
                instructionsFade = false;
            }
        } else {
            GameObject.Find("InstructionsText").GetComponent<Text>().color = Color.Lerp(GameObject.Find("InstructionsText").GetComponent<Text>().color, new Color(1, 1, 1, 1), 0.1f);
        }

        if(instructionStage == -1) {
            GameObject.Find("InstructionsText").GetComponent<Text>().text = "";
        } else if (instructionStage == 0) {
            GameObject.Find("InstructionsText").GetComponent<Text>().text = "Move with arrow keys/WASD, hold SPACE to enlarge";
        } else if (instructionStage == 1) {
            GameObject.Find("InstructionsText").GetComponent<Text>().text = "Touching enemies trades your health for their allegiance";
        } else if (instructionStage == 2) {
            GameObject.Find("InstructionsText").GetComponent<Text>().text = "Staying with your soul heals you over time";
        } else if (instructionStage == 3) {
            GameObject.Find("InstructionsText").GetComponent<Text>().text = "Protect your soul from enemies";
        }

        if(GameObject.Find("Logo").GetComponent<Image>().color.a < 0.9f) {
            GameObject.Find("Logo").GetComponent<Image>().color = Color.Lerp(GameObject.Find("Logo").GetComponent<Image>().color, new Color(1, 1, 1, 1), 0.1f);
        } else {
            tmrLogo += Time.deltaTime;
            if(tmrLogo >= 2f) {
                GameObject.Find("Logo").transform.localScale = Vector3.Lerp(GameObject.Find("Logo").transform.localScale, Vector3.zero, 0.1f);
                GameObject.Find("Logo").transform.Rotate(Vector3.forward * -10);
            }
        }
    }

    public void OnLose() {
        if (score > highscore) highscore = (int)score;
        GameObject.Find("LoseText").GetComponent<Text>().enabled = true;
        GameObject.Find("LoseOverlay").GetComponent<Image>().enabled = true;
        GameObject.Find("HighscoreText").GetComponent<Text>().enabled = true;
        GameObject.Find("HighscoreText").GetComponent<Text>().text = "Your highscore is " + highscore;

        PlaySound("Lose");

        lost = true;
    }

    public void OnRestart() {
        GameObject.Find("LoseText").GetComponent<Text>().enabled = false;
        GameObject.Find("LoseOverlay").GetComponent<Image>().enabled = false;
        GameObject.Find("HighscoreText").GetComponent<Text>().enabled = false;

        score = 0;
        shownScore = 0;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
            Destroy(enemy);
        }
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Ally")) {
            Destroy(enemy);
        }

        GameObject.Find("Player").GetComponent<PlayerController>().health = 100f;
        GameObject.Find("Player").transform.position = Vector3.zero;
        GameObject.Find("Soul").GetComponent<Soul>().health = 100f;
        GameObject.Find("Soul").transform.position = Vector3.zero;

        tmrInstructions = 0;
        instructionsFade = false;
        instructionStage = -1;

        tmrLogo = 0;
        GameObject.Find("Logo").GetComponent<Image>().color = new Color(1, 1, 1, 0);
        GameObject.Find("Logo").transform.localScale = Vector3.one;
        GameObject.Find("Logo").transform.rotation = Quaternion.identity;

        lost = false;
    }

    public void PlaySound(string sound) {
        GetComponent<AudioSource>().PlayOneShot(Resources.Load("Sounds/" + sound) as AudioClip, 1f);
    }
}
