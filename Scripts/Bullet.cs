using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float releaseTime;
    public float Gravity;
    public float ThrowSpeed;

    private bool flying;
    private bool isPressed;
    private float TimeSinceThown;
    private float birdVelocX, birdVelocY;
    private Vector3 ReleasePos;
    private Transform HookPoint;
    private Rigidbody2D myrb;
    private GameController gc;
    private LineRenderer TrajectoryLineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        isPressed = false;
        flying = false;
        myrb = GetComponent<Rigidbody2D>();
        HookPoint = GameObject.FindGameObjectWithTag("HookPoint").transform;
        gc = FindObjectOfType<GameController>();
        TrajectoryLineRenderer = FindObjectOfType<LineRenderer>();
        TrajectoryLineRenderer.transform.position = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPressed)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position += new Vector3(0, 0, 10);
            float distance = Vector2.Distance(HookPoint.position, transform.position);
            if (distance < 1.5f)
            {
                TrajectoryLineRenderer.enabled = false;
            }
            else
            {
                DisplayTrajectoryLineRenderer(distance);
            }
            ReleasePos = transform.position;

        }
    }

    private void FixedUpdate()
    {
        if (flying)
        {
            TimeSinceThown += Time.fixedDeltaTime;

            float posX = ReleasePos.x + TimeSinceThown * birdVelocX;
            float posY = ReleasePos.y + TimeSinceThown * birdVelocY - 0.5f * Gravity * Mathf.Pow(TimeSinceThown, 2);
            transform.position = new Vector3(posX, posY);
        }
    }

    private void OnMouseDown()
    {
        if (!flying)
        {
            isPressed = true;
            myrb.isKinematic = true;
        }
    }

    private void OnMouseUp()
    {
        if (!flying)
        {
            isPressed = false;
            //            myrb.isKinematic = false;
            TrajectoryLineRenderer.enabled = false;
            flying = true;
            TimeSinceThown = 0;
            float distance = Vector2.Distance(HookPoint.position, transform.position);
            ThrowBird(distance);
            StartCoroutine(DestroyCo());
            StartCoroutine(ReleaseCo());
        }
    }


    private void ThrowBird(float distance)
    {

        Vector2 velocity = HookPoint.position - transform.position;

        Vector2 birdVeloc = velocity * ThrowSpeed * distance;
        float alpha = Mathf.Atan2(birdVeloc.y, birdVeloc.x);
        birdVelocX = birdVeloc.magnitude * Mathf.Cos(alpha);
        birdVelocY = birdVeloc.magnitude * Mathf.Sin(alpha);

    }

    private IEnumerator ReleaseCo()
    {
        yield return new WaitForSeconds(releaseTime);
        GetComponent<Collider2D>().isTrigger = false;

    }

    private IEnumerator DestroyCo()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            gc.IncreaseScore();
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            if (flying)
            {
                Destroy(gameObject);
            }
        }
    }

    void DisplayTrajectoryLineRenderer(float distance)
    {
        TrajectoryLineRenderer.enabled = true;
        Vector2 v2 = new Vector2(HookPoint.position.x, HookPoint.position.y) - myrb.position;
        int segmentCount = 15;
        Vector2[] segments = new Vector2[segmentCount];

        segments[0] = transform.position;

        Vector2 segVelocity = v2 * ThrowSpeed * distance;
        for (int i = 1; i < segmentCount; i++)
        {
            float time = i * Time.fixedDeltaTime * 3;
            segments[i] = segments[0] + segVelocity * time + 0.5f * Physics2D.gravity * Mathf.Pow(time, 2);
        }

        TrajectoryLineRenderer.positionCount = segmentCount;
        for (int i = 0; i < segmentCount; i++)
            TrajectoryLineRenderer.SetPosition(i, segments[i]);
    }



}
