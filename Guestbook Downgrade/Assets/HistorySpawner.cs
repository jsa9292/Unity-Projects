using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HistorySpawner : MonoBehaviour
{
    public GameObject cell_prefab;

    // Start is called before the first frame update
    void OnApplicationPause()
    {
        Invoke("LoadHistory",0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public string[] dataString;
    void LoadHistory() {
        StreamReader sr = new StreamReader(RuntimeText.filePath);
        while (sr.Peek() > 0) {
            dataString = sr.ReadLine().Replace('(',' ').Replace(')',' ').Split(',');
            float[] data = new float[dataString.Length];
            //Debug.Log(System.DateTime.Parse(dataString[0]));
            for (int i = 1; i < dataString.Length; i++) {
                data[i] = float.Parse(dataString[i]);
            }
            Vector3 pos = new Vector3(data[1], data[2], data[3]);
            Vector3 rand_dir = new Vector3(data[4], data[5], data[6]);

            GameObject go = Instantiate(cell_prefab);
            CellCtrl cc = go.GetComponent<CellCtrl>();
            cc.Setup_Cell(data);
            cc.ReadyForSpawning();

            go.transform.position = pos + rand_dir;

            
        }
    }
}
