using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    float speed;
    GameObject target;
    float tarScale = 1f;
    bool dead;
    float tmrIdle;
    Vector3 nextPos = Vector3.zero;
    float nextFlip;

    private void Start() {
        speed = Random.Range(3f, 6f);
    }

    private void Update() {
        if (GameController.instance.lost) return;

        tmrIdle += Time.deltaTime;
        if (tmrIdle >= nextFlip) {
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
            nextFlip = Random.Range(0.25f, 0.75f);
            tmrIdle = 0;
        }

        if (tag != "Ally") {
            GameObject soul = GameObject.Find("Soul");
            transform.position = Vector2.MoveTowards(transform.position, soul.transform.position, speed * Time.deltaTime);
        } else {
            GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, new Color(0.2f, 0.8f, 0.2f), 0.1f);

            if(target == null || target.tag == "Ally") {
                foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
                    if (Vector3.Distance(enemy.transform.position, transform.position) <= 10f) target = enemy;
                }

                if (Vector3.Distance(transform.position, nextPos) <= 0.25f || nextPos == Vector3.zero) {
                    float min = 0.2f;
                    float max = 0.8f;
                    nextPos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(min, max), Random.Range(min, max), 1f));
                }

                transform.position = Vector3.MoveTowards(transform.position, nextPos, 0.1f);
            } else {
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            }
        }

        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(tarScale, tarScale, tarScale), 0.1f);
        if (dead && transform.localScale.x <= 0.1f) Destroy(gameObject);
        if (dead) {
            transform.Rotate(Vector3.forward * -10);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (dead) return;

        if (collision.gameObject.name == "Soul" && tag != "Ally") {
            collision.gameObject.GetComponent<Soul>().health -= 10f;
            GameController.instance.PlaySound("Damage" + Random.Range(0, 6));

            if(collision.gameObject.GetComponent<Soul>().health <= 0f) {
                GameController.instance.OnLose();
            }

            Destroy(gameObject);
        } else if(collision.gameObject.tag == "Enemy" && tag == "Ally" && !collision.gameObject.GetComponent<Enemy>().dead) {
            collision.gameObject.GetComponent<Enemy>().OnDeath();

            tarScale = Mathf.Clamp(tarScale - 1f, 0f, 1f);
            if (tarScale <= 0f) OnDeath();

            GameController.instance.score += 5f;
            GameController.instance.PlaySound("AllyHit" + Random.Range(0, 6));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (dead) return;

        if (collision.gameObject.name == "Player" && tag != "Ally") {
            collision.gameObject.GetComponent<PlayerController>().health -= 10f;

            if(collision.gameObject.GetComponent<PlayerController>().health <= 0f) {
                GameController.instance.OnLose();
                return;
            }

            gameObject.tag = "Ally";
            speed += 1f;
            tarScale = 2f;

            GameController.instance.score += 10f;
            GameController.instance.PlaySound("HitGhost" + Random.Range(0, 6));
        }
    }

    public void OnDeath() {
        tarScale = 0f;
        dead = true;
    }
}
