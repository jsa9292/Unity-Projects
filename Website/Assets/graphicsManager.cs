using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class graphicsManager : MonoBehaviour
{
    public Transform graphicAssets;
    public ParticleControl pc;
    public Texture2D easter_egg;
    public List<GameObject> ui_Images;
    public List<GameObject> GOs;

    public void uiImagePosition(int target_i) {
        for (int i = 0; i < ui_Images.Count; i++) {
            ui_Images[i].SetActive(i == target_i);
        }
    }
    public void ActivateAsset(int target_i) {
        for (int i = 0; i < graphicAssets.childCount; i++)
        {
            graphicAssets.GetChild(i).gameObject.SetActive(i==target_i);
        }
    }
    public void ActivateGO(int target_i) {
        for (int i = 0; i < GOs.Count; i++) {
            if(i== target_i) GOs[i].SetActive(true);
        }
    }
    public void DeactivateGO(int target_i)
    {
        for (int i = 0; i < GOs.Count; i++)
        {
            if (i == target_i) GOs[i].SetActive(false);
        }
    }
    public void ParticleToggle(int t) {
        pc.particle_off = t==0;
    }
    public void EasterEgg() {
        pc.setTexture(easter_egg);
    }
}
