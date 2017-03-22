using UnityEngine;
using System.Collections;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataHooks : MonoBehaviour {

	// Use this for initialization
	void Start () {

        FileStream fs = File.Open(Application.dataPath + "/data.txt", FileMode.Create);

        fs.Close();
	}

    public void WriteLine(string line) {

        FileStream fs = File.Open(Application.dataPath + "/data.txt", FileMode.Append);

            StreamWriter sw = new StreamWriter(fs);

            sw.WriteLine(line);

            sw.Flush();

        fs.Close();
    }
}