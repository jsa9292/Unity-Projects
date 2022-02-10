using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ctrlAnimState : MonoBehaviour {

    public Animator anim;
    public Vector3 targetPos;
    public float speed;
    public float range;
    public bool isRunning;
    public bool isAttacking;
    public bool isDead;
    private void Start()
    {
        targetPos = transform.parent.position;
        transform.LookAt(targetPos);
    }
    private void Update()
    {
        if(isRunning) transform.position += transform.forward * speed * Time.deltaTime;
    }
    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position+Vector3.up, transform.forward, out hit, range))
        {
            Debug.DrawRay(transform.position + Vector3.up, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            isRunning = false;
            isAttacking = true;
        }
        else {
            isRunning = true;
            isAttacking = false;
        }
        anim.SetBool("isRun", isRunning);
        anim.SetBool("isAttack", isAttacking);
        anim.SetBool("isDeath", isDead);
    }
    public void StateAct()
    {
        //mgrAnim.instance.disBtnAct();
    }

    public void RetrunSitIdleAct()
    {
        //mgrAnim.instance.SitDeathAct();
    }

    public void RetrunIdleAct()
    {
        //mgrAnim.instance.JumpAct();
    }

}
