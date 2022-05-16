using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LogVisualizer : MonoBehaviour
{
    public Text t;
    public Text t1;
    public List<string> texts;
    public string output;
    public static LogVisualizer instance;
    private void Start()
    {
        instance = this;
    }
    public void BottomLog(string logString) {
        output = logString;
        texts.Insert(0, output);
        t.text = "";
        for (int i = 0; i < texts.Count; i++) {
            t.text += texts[i] + "\n";
        }
    }
    public void TopLog() {
        t1.text = System.DateTime.Now.ToString() + "\n";
        t1.text += "Unique Cell Count: " + CellCtrl.cell_count.ToString();
    }
}
