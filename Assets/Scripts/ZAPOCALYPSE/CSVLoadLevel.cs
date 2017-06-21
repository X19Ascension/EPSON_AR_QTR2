using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System;

public class CSVLoadLevel : MonoBehaviour
{

    public TextAsset csvFile; // Reference of CSV file
    public int[,] loadedMap;
    public int xwaveno;
    public int yspawnpt;
    private char lineSeperator = '\n'; // It defines line seperate character
    private char fieldSeperator = ','; // It defines field seperate chracter
    //public LevelGenerate LevelLoad;

    // Use this for initialization
    void Awake()
    {
        string[] records = csvFile.text.Split(lineSeperator);
        int i = 0;
        int j = 0;
        foreach (string record in records)
        {
            if (record == "")
            {
                break;
            }
            string[] fields = record.Split(fieldSeperator);
            foreach (string field in fields)
            {
                i++;
            }
            xwaveno = i;
            i = 0;
            j++;
        }
        //newxsize = i;
        yspawnpt = j;
        Debug.Log(xwaveno + " " + yspawnpt);

        loadedMap = new int[xwaveno, yspawnpt];
        readData();

        //LevelLoad.generateLoadMap(loadedMap, newxsize, newysize);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void readData()
    {
        string[] records = csvFile.text.Split(lineSeperator);
        int i = 0;
        int j = 0;
        foreach (string record in records)
        {
            if (record == "")
            {
                break;
            }

            string[] fields = record.Split(fieldSeperator);
            foreach (string field in fields)
            {
                //Debug.Log(j);
                loadedMap[i, j] = Convert.ToInt32(field);
                i++;
            }

            i = 0;
            j++;
        }
    }
}