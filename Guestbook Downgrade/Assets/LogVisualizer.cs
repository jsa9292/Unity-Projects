using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LogVisualizer : MonoBehaviour
{
    public Text t;
    public List<string> texts;
    public string output;
    // Start is called before the first frame update
    void Start()
    {
        Application.logMessageReceived += HandleLog;
        texts = new List<string>(5);
    }

    void HandleLog(string logString, string stackTrace, LogType type) {
        output = logString;
        texts.Insert(0, output);
        t.text = "";
        for (int i = 0; i < texts.Count; i++) {
            t.text += texts[i] + "\n";
        }
    }
}
