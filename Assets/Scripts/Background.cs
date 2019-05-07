using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {
    float tarScale = 1.25f;
    bool scaleReverse;

    void Update() {
        transform.Rotate(Vector3.forward * 0.25f);

       if(scaleReverse) {
            tarScale = 1.25f;
            if (transform.localScale.x <= tarScale + 0.1f) {
                scaleReverse = false;
            }
        } else {
            tarScale = 3f;
            if(transform.localScale.x >= tarScale - 0.1f) {
                scaleReverse = true;
            }
        }

        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(tarScale, tarScale, tarScale), 0.005f);
    }
}
