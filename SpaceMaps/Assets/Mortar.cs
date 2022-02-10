using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortar : MonoBehaviour
{
    public GameObject target;
    public Transform pivot;
    public Animation attack_anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Idle();
        }
        else {
            //Attack(target);
        }
    }
    public void Idle() {
        transform.localEulerAngles += Vector3.up * Time.deltaTime * 15f;
    }

    public void Attack(GameObject target) {
        attack_anim.Play();
    }
    private void OnTriggerEnter(Collider other)
    {
        target = other.gameObject;
        Debug.Log(target);
    }
}
