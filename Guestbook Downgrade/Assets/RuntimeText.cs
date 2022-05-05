using UnityEngine;
using System.IO;
public class RuntimeText : MonoBehaviour
{
    public static RuntimeText instance;
    public static string filename;
    public static string filePath;

    private void Start()
    {
        instance = this;
        filePath = Application.streamingAssetsPath + "/data_dump.txt";

    }

    public static void WriteString(string content)
    {
        StreamWriter writer = new StreamWriter(filePath,true);
        writer.WriteLine(content);
        writer.Close();
    }

    public void ReadString()
    {
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(filePath);
        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }
}