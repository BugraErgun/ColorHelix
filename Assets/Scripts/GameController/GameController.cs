using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public Color[] colors;

    [HideInInspector]
    public Color hitColor, failColor;

    private int wallSpawn = 11;
    private float z = 7;

    public GameObject finishLine;

    private bool colorBump;

    private GameObject[] walls2;

    

    private void Awake()
    {
        instance = this;
        GenerateColor();
        PlayerPrefs.GetInt("Level", 1);

    }
    private void Start()
    {
        
        GenerateLevel();
    }

    void Update()
    {
        
    }

    void GenerateColor()
    {
        hitColor = colors[Random.Range(0, colors.Length)];

        failColor = colors[Random.Range(0, colors.Length)];

        while (failColor==hitColor)
        {
            failColor = colors[Random.Range(0, colors.Length)];
        }
        Ball.SetColor(hitColor);
    }

    public void GenerateLevel()
    {
        GenerateColor();

        if (PlayerPrefs.GetInt("Level") >= 1 && PlayerPrefs.GetInt("Level") <= 4)
            wallSpawn = 12;
        else if (PlayerPrefs.GetInt("Level") >= 5 && PlayerPrefs.GetInt("Level") <= 10)
            wallSpawn = 13;
        else
            wallSpawn = 15;

        z = 7;
        DeleteWalls();
        colorBump = false;
        SpawnWalls();
    }

    void DeleteWalls()
    {
        walls2 = GameObject.FindGameObjectsWithTag("Fail");

        if (walls2.Length>1)
        {
            for (int i = 0; i < walls2.Length; i++)
            {
                Destroy(walls2[i].transform.parent.gameObject);
            }

        }

        Destroy(GameObject.FindGameObjectWithTag("ColorBump"));
    }

    void SpawnWalls()
    {
        for (int i = 0; i < wallSpawn; i++)
        {
            GameObject wall;

            if (Random.value <=0.2f && !colorBump && PlayerPrefs.GetInt("Level")>=3)
            {
                colorBump = true;
                wall = Instantiate(Resources.Load("ColorBump") as GameObject, transform.position, Quaternion.identity);
            }

            else if (Random.value <=0.2f && PlayerPrefs.GetInt("Level") >= 6)
            {
                wall = Instantiate(Resources.Load("Walls") as GameObject, transform.position, Quaternion.identity);
            }
            else if (i>= wallSpawn -1 && !colorBump && PlayerPrefs.GetInt("Level") >= 3)
            {
                colorBump = true;
                wall = Instantiate(Resources.Load("ColorBump") as GameObject, transform.position, Quaternion.identity);
            }

            else
            {
                wall = Instantiate(Resources.Load("Wall") as GameObject, transform.position, Quaternion.identity);
            }

            

            wall.transform.SetParent(GameObject.Find("Helix").transform);
            wall.transform.localPosition = new Vector3(0, 0, z);

            float randomRotation = Random.Range(0, 360);
            wall.transform.localRotation =  Quaternion.Euler(new Vector3(0, 0, randomRotation));
            z += 7;


            if (i <= wallSpawn)
            {
                finishLine.transform.position = new Vector3(0, 0.01f, z);
            }
        }
    }
}
