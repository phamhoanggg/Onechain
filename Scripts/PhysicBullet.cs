using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PhysicBullet : MonoBehaviour
{
    public bool isPressed;
    public Rigidbody2D myrb;
    public float releaseTime;
    public Transform HookPoint;

    public GameController gc;
    public LineRenderer TrajectoryLineRenderer;
    public float ThrowSpeed;
    private bool flying;

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
            myrb.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float distance = Vector2.Distance(HookPoint.position, myrb.position);
            if (distance < 1.5f)
            {
                TrajectoryLineRenderer.enabled = false;
            }
            else
            {
                DisplayTrajectoryLineRenderer(distance);
            }
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
            myrb.isKinematic = false;
            TrajectoryLineRenderer.enabled = false;
            float distance = Vector3.Distance(HookPoint.position, transform.position);
            ThrowBird(distance);
            StartCoroutine(DestroyCo());
            StartCoroutine(ReleaseCo());
        }
    }


    private void ThrowBird(float distance)
    {

        Vector3 velocity = HookPoint.position - transform.position;

        myrb.velocity = new Vector2(velocity.x, velocity.y) * ThrowSpeed * distance;
    }

    private IEnumerator ReleaseCo()
    {
        yield return new WaitForSeconds(releaseTime);
        GetComponent<Collider2D>().isTrigger = false;
        flying = true;
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

        // The first line point is wherever the player's cannon, etc is
        segments[0] = myrb.position;

        // The initial velocity
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
