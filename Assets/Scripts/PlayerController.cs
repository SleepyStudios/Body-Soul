using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float health = 100f;
    float maxScale = 10f;

    void Update() {
        if(health <= 0) {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.1f);
            transform.Rotate(Vector3.forward * -10);
        } else {
            transform.rotation = Quaternion.identity;
        }

        if (GameController.instance.lost) return;

        float speed = 12f * (1f / transform.localScale.x);

        // Horizontal movement
        int xUnitSpeed = (int)Input.GetAxisRaw("Horizontal");
        float xSpeed = xUnitSpeed * speed * Time.deltaTime;
        GetComponent<SpriteRenderer>().flipX = xSpeed > 0;

        // Vertical movement
        int yUnitSpeed = (int)Input.GetAxisRaw("Vertical");
        float ySpeed = yUnitSpeed * speed * Time.deltaTime;

        // Magnitude to make diagonal movements the same speed as vertical and horizontal
        float magnitude = 1.0f;
        if (!(xUnitSpeed == 0 && yUnitSpeed == 0)) {
            magnitude = Mathf.Sqrt(Mathf.Pow(xUnitSpeed, 2) + Mathf.Pow(yUnitSpeed, 2));
        } else {
            magnitude = 1.0f;
        }

        transform.Translate(xSpeed / magnitude, ySpeed / magnitude, 0.0f);

        Vector3 minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minScreenBounds.x + 1, maxScreenBounds.x - 1), Mathf.Clamp(transform.position.y, minScreenBounds.y + 1, maxScreenBounds.y - 1), transform.position.z);

        if(Input.GetKeyDown(KeyCode.Space)) {
            GameController.instance.PlaySound("Enlarge" + Random.Range(0, 6));
        }

        if (Input.GetKey(KeyCode.Space)) {
            float bigScale = maxScale;
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(bigScale, bigScale), 0.2f);
        } else {
            transform.localScale = Vector2.Lerp(transform.localScale, Vector2.one, 0.2f);
        }

        transform.Find("HealingSprite").GetComponent<SpriteRenderer>().color = Color.Lerp(
            transform.Find("HealingSprite").GetComponent<SpriteRenderer>().color,
            new Color(1, 1, 1, GameObject.Find("Soul").GetComponent<Soul>().healing ? 1 : 0),
            0.1f
        );
    }
}
