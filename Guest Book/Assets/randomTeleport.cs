using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomTeleport : MonoBehaviour
{
    private float timer = 0;
    public float interval;
    public float distance;
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > interval) {
            timer -= interval;
            Teleport();
        }
    }
    void Teleport() {
        Vector3 newDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 1);
        transform.localPosition = newDir.normalized * distance;
    }
}
