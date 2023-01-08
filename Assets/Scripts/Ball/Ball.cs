using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    private static float z;

    private float height = .58f;

    [SerializeField]float speed = 6;

    private bool move,isRising,gameOver;


    private SpriteRenderer splash;


    private static Color currentColor;
    private MeshRenderer meshRenderer;

    private float lerpAmount;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        splash = transform.GetChild(0).GetComponent<SpriteRenderer>();


    }

    void Start()
    {
        move = false;
        SetColor(GameController.instance.hitColor);

    }

    
    void Update()
    {
        

        if (Touch.isPressing()&&!gameOver)
        {
            move = true;
        }
        if (move)
        {
            Ball.z += speed * 0.025f;
        }
        transform.position = new Vector3(0, height, Ball.z);

        UpdateColor();
        
    }


    void UpdateColor()
    {
        meshRenderer.sharedMaterial.color = currentColor;

        if (isRising)
        {
            currentColor = Color.Lerp(meshRenderer.material.color,
                GameObject.FindGameObjectWithTag("ColorBump").GetComponent<ColorBump>().GetColor(),
                lerpAmount);
            lerpAmount += Time.deltaTime;
            if (lerpAmount >= 1)
                isRising = false;
        }
    }

    public static float GetZ()
    {
        return Ball.z;
    }

    public static Color SetColor(Color color)
    {
        return currentColor = color;
    }
    public static Color GetColor(Color color)
    {
        return currentColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Hit")
        {
            Destroy(other.transform.parent.gameObject);

        }
        if (other.tag=="ColorBump")
        {
            lerpAmount = 0;
            isRising = true;
        }

        if (other.tag=="Fail")
        {
            StartCoroutine(GameOver());
        }


        if (other.CompareTag("Finish"))
        {
            StartCoroutine(PlayNewLevel());

        }
    }
    IEnumerator GameOver()
    {
        gameOver = true;
        splash.color = currentColor;
        splash.transform.position = new Vector3(0, 0.7f, Ball.z-0.05f);
        splash.transform.eulerAngles = new Vector3(0, 0, Random.value * 360);
        splash.enabled = true;

        meshRenderer.enabled = false;
        GetComponent<SphereCollider>().enabled = false;
        move = false;

        yield return new WaitForSeconds(1.5f);
        gameOver = false;
        z = 0;
        GameController.instance.GenerateLevel();
        splash.enabled = false;
        meshRenderer.enabled = true;
        GetComponent<SphereCollider>().enabled = true;
    }

    IEnumerator PlayNewLevel()
    {
        Camera.main.GetComponent<CameraFollow>().enabled = false;
        yield return new WaitForSeconds(1.5f);
        move = false;

        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);

        Camera.main.GetComponent<CameraFollow>().enabled = true;
        Ball.z = 0;
        GameController.instance.GenerateLevel();


    }
}
