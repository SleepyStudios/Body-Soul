using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    float tmrSpawn;

    void Update() {
        if (GameController.instance.lost) return;

        tmrSpawn += Time.deltaTime;
        float time = GameController.instance.score > 300 ? 0.5f : 1f;

        if(tmrSpawn >= time) {
            Vector3 pos = Vector3.zero;

            switch(Random.Range(0, 4)) {
                case 0:
                    pos = Camera.main.ViewportToWorldPoint(new Vector3(-1, 1, 1));
                    break;
                case 1:
                    pos = Camera.main.ViewportToWorldPoint(new Vector3(-1, -1, 1));
                    break;
                case 2:
                    pos = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 1));
                    break;
                case 3:
                    pos = Camera.main.ViewportToWorldPoint(new Vector3(1, -1, 1));
                    break;
            }

            Instantiate(Resources.Load("Enemy"), pos, Quaternion.identity);
            tmrSpawn = 0;
        }
    }
}
