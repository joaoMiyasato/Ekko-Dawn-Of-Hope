using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_camera : MonoBehaviour
{
    public static scr_camera instance;
    public Transform player;
    private BoxCollider2D cameraBox;
    public float maskx,masky,maskz = -50f;
    public float damp = 0.1f;
    private Vector3 pos;
    private Vector3 velocity = Vector3.zero;
    private float shakeTimeRemaining, shakePower, shakeFadeTime, shakeRotation, rotationMultiplier;
    public float pow,tim,rot; //teste
    private bool diferenciado;
    private float RatioX, RatioY;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        cameraBox = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        AspectRatioBoxChange();

        //teste key
        if(Input.GetKeyDown(KeyCode.K))
        {
            StartShake(pow,tim,rot); //0.1, 0.1, 2
        }
    }
    private void FixedUpdate()
    {
        FollowPlayer();
    }

    private void LateUpdate()
    {
        if(!GameManager.instance.isPaused)
        {
            if(shakeTimeRemaining > 0)
            {
                shakeTimeRemaining -= Time.deltaTime;

                float xAmount = Random.Range(-1f,1f) * shakePower;
                float yAmount = Random.Range(-1f,1f) * shakePower;

                transform.position += new Vector3(xAmount,yAmount,0f);

                shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);

                shakeRotation = Mathf.MoveTowards(shakeRotation, 0f, shakeFadeTime * rotationMultiplier * Time.deltaTime);
            }

            transform.rotation = Quaternion.Euler(0f,0f,shakeRotation * Random.Range(-1f,1f));
        }
    }

    public void StartShake(float power, float lenght, float rotation)
    {
        shakeTimeRemaining = lenght;
        shakePower = power;
        rotationMultiplier = rotation;

        shakeFadeTime = power  / lenght;

        shakeRotation = power * rotationMultiplier;
    }

    private void AspectRatioBoxChange()
    {
        if(Camera.main.aspect >= 1.25f && Camera.main.aspect < 1.3f)
        {
            RatioX = 35f; RatioY = 28f;
        }
        if(Camera.main.aspect >= 1.3f && Camera.main.aspect < 1.4f)
        {
            RatioX = 37f; RatioY = 28f;
        }
        if(Camera.main.aspect >= 1.5f && Camera.main.aspect < 1.6f)
        {
            RatioX = 42f; RatioY = 28f;
        }
        if(Camera.main.aspect >= 1.6f && Camera.main.aspect < 1.7f)
        {
            RatioX = 45f; RatioY = 28f;
        }
        if(Camera.main.aspect >= 1.7f && Camera.main.aspect <= 1.8f)
        {
            RatioX = 50f; RatioY = 28f;
        }
        if(Camera.main.aspect > 1.8f)
        {
            RatioX = 67f; RatioY = 28f;
        }

        cameraBox.size = new Vector2(RatioX,RatioY);
    }

    private void FollowPlayer()
    {
        if(GameObject.Find("Boundary"))
        {
            pos = new Vector3(Mathf.Clamp(player.position.x + maskx, GameObject.Find("Boundary").GetComponent<BoxCollider2D>().bounds.min.x + cameraBox.size.x /2, GameObject.Find("Boundary").GetComponent<BoxCollider2D>().bounds.max.x - cameraBox.size.x /2),
                            Mathf.Clamp(player.position.y + masky, GameObject.Find("Boundary").GetComponent<BoxCollider2D>().bounds.min.y + cameraBox.size.y /2, GameObject.Find("Boundary").GetComponent<BoxCollider2D>().bounds.max.y - cameraBox.size.y /2),
                            maskz);
            transform.position = Vector3.SmoothDamp(gameObject.transform.position, pos, ref velocity, damp);
        }
    }
}
