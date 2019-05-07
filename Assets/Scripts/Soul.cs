using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour {
    public bool healing;
    float tmrHeal;
    public float health = 100f;
    Vector3 nextPos = Vector3.zero;
    float tmrIdle;
    float nextFlip;

    private void Update() {
        if (health <= 0) {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.1f);
            transform.Rotate(Vector3.forward * -10);
        } else {
            transform.localScale = new Vector3(2f, 2f, 2f);
            transform.rotation = Quaternion.identity;
        }

        if (GameController.instance.lost) return;

        tmrIdle += Time.deltaTime;
        if (tmrIdle >= nextFlip) {
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
            nextFlip = Random.Range(0.25f, 0.75f);
            tmrIdle = 0;
        }

        if (healing) {
            if (GameObject.Find("Player").GetComponent<PlayerController>().transform.localScale.x >= 1.1f) {
                healing = false;
                return;
            }

            tmrHeal += Time.deltaTime;
            if (tmrHeal >= 0.1f) {
                GameObject.Find("Player").GetComponent<PlayerController>().health = 
                    Mathf.Clamp(GameObject.Find("Player").GetComponent<PlayerController>().health + 1f, 0f, 100f);
                tmrHeal = 0f;
            }
        }

        if(Vector3.Distance(transform.position, nextPos) <= 0.1f) {
            float min = 0.1f;
            float max = 0.9f;
            nextPos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(min, max), Random.Range(min, max), 1f));
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPos, 0.075f);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.name == "Player") {
            healing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.name == "Player") {
            healing = false;
        }
    }
}
