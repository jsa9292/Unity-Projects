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
        filename = System.DateTime.Today.ToShortDateString().Replace('/','_');
        filePath = Application.streamingAssetsPath + "/" + filename + ".txt";

        DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(filePath));
        if (!directoryInfo.Exists) directoryInfo.Create();
        
        Debug.Log(filename);
    }

    public void WriteString(string content)
    {
        FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
        StreamWriter writer = new StreamWriter(fileStream, System.Text.Encoding.Unicode);
        writer.WriteLine(content);
        writer.Close();
    }

    public void ReadString()
    {
        string path = Application.streamingAssetsPath + "/" + filename + ".txt";
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }
}