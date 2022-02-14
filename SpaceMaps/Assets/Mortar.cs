using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortar : MonoBehaviour
{
    public GameObject target;
    public Transform pivot;
    public Animator anim;
    public float delay;
    public float cooldown;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && delay <= 0)
        {
            Attack(target);
        }
        else
        {
            Idle();
            delay -= Time.deltaTime;
            anim.SetBool("Attack", false);
        }
    }
    public void Idle() {
        transform.localEulerAngles += Vector3.up * Time.deltaTime * 15f;
    }

    public void Attack(GameObject target) {
        transform.LookAt(target.transform);
        anim.SetBool("Attack", true);
        delay = cooldown;
    }
    private void OnTriggerEnter(Collider other)
    {
        target = other.gameObject;
        Debug.Log(target);
    }
}
