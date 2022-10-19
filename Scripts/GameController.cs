using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject bullet;
    public GameObject[] RandomObstacles;
    public Vector2 bulletSpawnPos;
    public Vector2 obstacleSpawnPos;

    public float myscore;
    public UI_Controller ui;
    
    // Start is called before the first frame update
    void Start()
    {
        myscore = 0;
        ui.SetScoreText("Score: " + myscore);
        spawnBullet();    
    }

    // Update is called once per frame
    void Update()
    {
        GameObject tmp_obs = GameObject.FindGameObjectWithTag("Obstacle");
        if (tmp_obs == null)
        {
            RandomSpawnObstacle();
        }
        GameObject tmp_ball = GameObject.FindGameObjectWithTag("Bullet");
        if (tmp_ball == null)
        {
            spawnBullet();
        }
    }

    public void spawnBullet()
    {
        if (bullet)
        {
            Instantiate(bullet, bulletSpawnPos, Quaternion.identity);
        }
    }

    void RandomSpawnObstacle()
    {
        int n = Random.Range(0, 3);
        GameObject Obstacle = RandomObstacles[n];
        obstacleSpawnPos = new Vector2(Random.Range(6, 9), 7.5f);
        if (Obstacle)
        {
            Instantiate(Obstacle, obstacleSpawnPos, Quaternion.identity);
        }
    }

    public void IncreaseScore()
    {
        myscore += 100;
        ui.SetScoreText("Score: " + myscore);
    }


}
