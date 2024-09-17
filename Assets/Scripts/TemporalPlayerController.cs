using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalPlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.D)) {
            transform.Translate(Vector2.right * 3 * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A)) {
            transform.Translate(Vector2.right * -3 * Time.deltaTime);
        }
    }
}
