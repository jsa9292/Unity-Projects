using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mgrAnim : MonoBehaviour {

    public static mgrAnim instance;

    Animator anim;
    AnimatorStateInfo curAnim;

    public GameObject chr;
    [HideInInspector]public GameObject tmpChr = null;

    public Button[] btn;
    public Button[] sitBtn;

    public Material[] matChr = null;
    public Renderer matSwapChr = null;
    int matNum;


    public GameObject wall = null;
    public GameObject wallCenter = null;
    public GameObject WallSmall = null;

    bool disBtn;
    int randomFire;
    bool isState;

    public Button stateBtn = null;

    public int stdFireMNum;
    public int stdFireLNum;
    public int stdFireRNum;
    public int stdObserveNum;
    public int sitFireMNum;
    public int sitFireLNum;
    public int sitFireRNum;
    public int deathT;

    void Awake()
    {
        mgrAnim.instance = this;
        anim = chr.GetComponent<Animator>();
    }

    #region StateChoice (stand == !isState, sit == true)
    public void StateChoiceAct()
    {
        curAnim = anim.GetCurrentAnimatorStateInfo(0);

        if (!isState)
        {
            stateBtn.GetComponentInChildren<Text>().text = "Stand";

            for (int i = 0; i < btn.Length; i++)
            {
                btn[i].interactable = true;
            }
            for (int i = 0; i < sitBtn.Length; i++)
            {
                sitBtn[i].interactable = false;
            }

            if (curAnim.IsName("Base Layer.SitIdle"))
            {
                anim.SetBool("isSit", false);
            }

            isState = true;
        }

        else
        {
            stateBtn.GetComponentInChildren<Text>().text = "Sit";

            for (int i = 0; i < btn.Length; i++)
            {
                btn[i].interactable = false;
            }
            for (int i = 0; i < sitBtn.Length; i++)
            {
                sitBtn[i].interactable = true;
            }

            if (curAnim.IsName("Base Layer.StandIdle"))
            {
                anim.SetBool("isSit", true);
            }

            isState = false;
        }

        disBtn = false;
    }
    #endregion

    #region SwapMat
    public void SwapMat()
    {
        matSwapChr.material = matChr[matNum];
        matNum++;

        if (matNum == matChr.Length)
        {
            matNum = 0;
        }
    }
    #endregion

    #region Run 
    public void RunFAct()
    {
        curAnim = anim.GetCurrentAnimatorStateInfo(0);
        if (curAnim.IsName("Base Layer.StandIdle"))
        {
            anim.SetBool("isRun", true);
            disBtnAct();
        }
        StartCoroutine(RunToIdleAct());
    }
    IEnumerator RunToIdleAct()
    {
        yield return new WaitForSeconds(3);

        curAnim = anim.GetCurrentAnimatorStateInfo(0);
        if (curAnim.IsName("Base Layer.RunF"))
        {
            anim.SetBool("isRun", false);
            disBtnAct();
        }
    }
    #endregion

    #region Death
    public void DeathAct()
    {
        curAnim = anim.GetCurrentAnimatorStateInfo(0);
        if (curAnim.IsName("Base Layer.StandIdle"))
        {
            anim.SetBool("isDeath", true);
            disBtnAct();
        }
        StartCoroutine(DeathToIdleAct());
    }
    IEnumerator DeathToIdleAct()
    {
        yield return new WaitForSeconds(deathT);

        curAnim = anim.GetCurrentAnimatorStateInfo(0);
        if (curAnim.IsName("Base Layer.StandDeath"))
        {
            anim.SetBool("isDeath", false);
            disBtnAct();
        }
    }
    public void SitDeathAct()
    {
        curAnim = anim.GetCurrentAnimatorStateInfo(0);
        if (curAnim.IsName("Base Layer.SitIdle"))
        {
            anim.SetBool("isDeath", true);
            disBtnAct();
        }

        StartCoroutine(SitDearhToIdleAct());
    }

    IEnumerator SitDearhToIdleAct()
    {
        yield return new WaitForSeconds(2);

        curAnim = anim.GetCurrentAnimatorStateInfo(0);
        if (curAnim.IsName("Base Layer.SitDeath"))
        {
            anim.SetBool("isDeath", false);
            disBtnAct();
        }
    }
    #endregion

    #region Fire
    public void FireAct()
    {
        curAnim = anim.GetCurrentAnimatorStateInfo(0);
        randomFire = UnityEngine.Random.Range(0, stdFireMNum);

        if (curAnim.IsName("Base Layer.StandIdle"))
        {
            if (randomFire == 0)
            {
                anim.Play("StandFire0");
            }
            if (randomFire == 1)
            {
                anim.Play("StandFire1");
            }
            if (randomFire == 2)
            {
                anim.Play("StandFire2");
            }
            if (randomFire == 3)
            {
                anim.Play("StandFire3");
            }
            disBtnAct();
        }
    }
    public void FireLAct()
    {
        curAnim = anim.GetCurrentAnimatorStateInfo(0);
        randomFire = UnityEngine.Random.Range(0, stdFireLNum);

        if (curAnim.IsName("Base Layer.StandIdle"))
        {
            if (randomFire == 0)
            {
                anim.Play("StandLFire0");
            }
            if (randomFire == 1)
            {
                anim.Play("StandLFire1");
            }
            if (randomFire == 2)
            {
                anim.Play("StandLFire2");
            }
            if (randomFire == 3)
            {
                anim.Play("StandLFire3");
            }
            if (randomFire == 4)
            {
                anim.Play("StandLFire4");
            }
            wallCenter.SetActive(true);
            disBtnAct();
        }
    }
    public void FireRAct()
    {
        curAnim = anim.GetCurrentAnimatorStateInfo(0);
        randomFire = UnityEngine.Random.Range(0, stdFireRNum);

        if (curAnim.IsName("Base Layer.StandIdle"))
        {
            if (randomFire == 0)
            {
                anim.Play("StandRFire0");
            }
            if (randomFire == 1)
            {
                anim.Play("StandRFire1");
            }
            if (randomFire == 2)
            {
                anim.Play("StandRFire2");
            }
            if (randomFire == 3)
            {
                anim.Play("StandRFire3");
            }
            if (randomFire == 4)
            {
                anim.Play("StandRFire4");
            }
            wallCenter.SetActive(true);
            disBtnAct();
        }
    }
    public void SitFireAct()
    {
        curAnim = anim.GetCurrentAnimatorStateInfo(0);
        randomFire = UnityEngine.Random.Range(0, 2);

        if (randomFire == 0)
        {
            anim.Play("SitFire0");
        }
        if (randomFire == 1)
        {
            anim.Play("SitFire1");
        }
        disBtnAct();
    }
    public void SitFireLAct()
    {
        curAnim = anim.GetCurrentAnimatorStateInfo(0);
        randomFire = UnityEngine.Random.Range(0, 3);

        if (randomFire == 0)
        {
            anim.Play("SitLFire0");
        }
        if (randomFire == 1)
        {
            anim.Play("SitLFire1");
        }
        if (randomFire == 2)
        {
            anim.Play("SitLFire2");
        }
        WallSmall.SetActive(true);
        disBtnAct();
    }
    public void SitFireRAct()
    {
        curAnim = anim.GetCurrentAnimatorStateInfo(0);
        randomFire = UnityEngine.Random.Range(0, 3);

        if (randomFire == 0)
        {
            anim.Play("SitRFire0");
        }
        if (randomFire == 1)
        {
            anim.Play("SitRFire1");
        }
        if (randomFire == 2)
        {
            anim.Play("SitRFire2");
        }
        WallSmall.SetActive(true);
        disBtnAct();
    }
    public void SitFireUpAct()
    {
        curAnim = anim.GetCurrentAnimatorStateInfo(0);
        randomFire = UnityEngine.Random.Range(0, 3);

        if (randomFire == 0)
        {
            anim.Play("SitUpFire0");
        }
        if (randomFire == 1)
        {
            anim.Play("SitUpFire1");
        }
        if (randomFire == 2)
        {
            anim.Play("SitUpFire2");
        }
        WallSmall.SetActive(true);
        disBtnAct();
    }
    #endregion

    #region Jump
    public void JumpAct()
    {
        curAnim = anim.GetCurrentAnimatorStateInfo(0);

        if (curAnim.IsName("Base Layer.StandIdle"))
        {
            anim.SetBool("isJump", true);
        }
        else
        {
            anim.SetBool("isJump", false);
        }
        disBtnAct();
    }
    #endregion

    #region Observe
    public void StandObserveLAct()
    {
        curAnim = anim.GetCurrentAnimatorStateInfo(0);

        if (curAnim.IsName("Base Layer.StandIdle"))
        {
            anim.Play("Stand_ObserveL0");
        }
        wallCenter.SetActive(true);
        disBtnAct();
    }
    public void StandObserveL2Act()
    {
        curAnim = anim.GetCurrentAnimatorStateInfo(0);

        if (curAnim.IsName("Base Layer.StandIdle"))
        {
            anim.Play("Stand_ObserveL1");
        }

        wall.SetActive(true);
        disBtnAct();
    }
    public void StandObserveRAct()
    {
        curAnim = anim.GetCurrentAnimatorStateInfo(0);

        if (curAnim.IsName("Base Layer.StandIdle"))
        {
            anim.Play("Stand_ObserveR0");
        }

        wallCenter.SetActive(true);
        disBtnAct();
    }
    public void StandObserveR2Act()
    {
        curAnim = anim.GetCurrentAnimatorStateInfo(0);

        if (curAnim.IsName("Base Layer.StandIdle"))
        {
            anim.Play("Stand_ObserveR1");
        }

        wall.SetActive(true);
        disBtnAct();
    }
    public void SitObserveLAct()
    {
        curAnim = anim.GetCurrentAnimatorStateInfo(0);

        if (curAnim.IsName("Base Layer.SitIdle"))
        {
            anim.Play("Sit_ObserveL");
        }
        WallSmall.SetActive(true);
        disBtnAct();
    }

    public void SitObserveRAct()
    {
        curAnim = anim.GetCurrentAnimatorStateInfo(0);

        if (curAnim.IsName("Base Layer.SitIdle"))
        {
            anim.Play("Sit_ObserveR");
        }
        WallSmall.SetActive(true);
        disBtnAct();
    }
    #endregion

    #region Appear
    public void StandAppearLAct()
    {
        curAnim = anim.GetCurrentAnimatorStateInfo(0);

        if (curAnim.IsName("Base Layer.StandIdle"))
        {
            anim.Play("Stand_AppearL");
        }

        wall.SetActive(true);
        disBtnAct();
    }
    public void StandAppearRAct()
    {
        curAnim = anim.GetCurrentAnimatorStateInfo(0);

        if (curAnim.IsName("Base Layer.StandIdle"))
        {
            anim.Play("Stand_AppearR");
        }

        wall.SetActive(true);
        disBtnAct();
    }
    #endregion 

    #region Tumbling
    public void TumblingFAct()
    {
        curAnim = anim.GetCurrentAnimatorStateInfo(0);

        if (curAnim.IsName("Base Layer.StandIdle"))
        {
            anim.Play("Stand_TumblingF");
        }
        disBtnAct();
    }
    public void TumblingBAct()
    {
        curAnim = anim.GetCurrentAnimatorStateInfo(0);

        if (curAnim.IsName("Base Layer.StandIdle"))
        {
            anim.Play("Stand_TumblingB");
        }
        disBtnAct();
    }
    #endregion

    #region Btn Acttion
    public void disBtnAct()
    {
        if (!isState)
        {
            if (!disBtn)
            {
                for (int i = 0; i < sitBtn.Length; i++)
                {
                    sitBtn[i].interactable = false;
                }
                disBtn = true;
            }
            else
            {
                for (int i = 0; i < sitBtn.Length; i++)
                {
                    sitBtn[i].interactable = true;
                }
                WallDisableAct();
                disBtn = false;
            }
        }
        else
        {
            if (!disBtn)
            {
                for (int i = 0; i < btn.Length; i++)
                {
                    btn[i].interactable = false;
                }
                disBtn = true;
            }
            else
            {
                for (int i = 0; i < btn.Length; i++)
                {
                    btn[i].interactable = true;
                }
                WallDisableAct();
                disBtn = false;
            }
        }
    }
    #endregion

    #region TotalIdle
    public void TotalIdle()
    {
        anim.Play("StandIdle1");
    }
    #endregion

    #region Wall Action
    void WallDisableAct()
    {
        wall.SetActive(false);
        wallCenter.SetActive(false);
        WallSmall.SetActive(false);
    }
    #endregion
}
