using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeMovement : MonoBehaviour
{
    public int lives = 0;
    public int startlives = 0;
    public int score;
    public Transform tr;
    private float speed = 0.24f;
    public Material FourLives;
    public Material ThreeLives;
    public Material TwoLives;
    public Material oneLife;
    public Material fireBall;
    public Material lockAndLaunch;
    public Material lifeBlock;
    public Material ballInc;
    bool isLifeBlock, isBallIncrease, isFireBall, isLaunch;


    public GameObject destroyParticle;
    private GameObject gameHandler;


    private int diffculty = 0;

    void Start()
    {
        gameHandler = GameObject.Find("GameHandler");
        gameHandler.GetComponent<GameHandler>().blockCount += 1;
        diffculty = gameHandler.GetComponent<GameHandler>().getlevel() + 3;
        if (diffculty > 5)
            diffculty -= 2;
        if (diffculty/3 >= 1000)
            diffculty = 1000;

        int Special = Random.Range(1, 90);
        if (Special > 3 && Special < 9)
        {
            isFireBall = true;
            GetComponent<Renderer>().material = fireBall;
        }
        else if (Special > 10 && Special < 14)
        {
            isLaunch
     = true;
            GetComponent<Renderer>().material = lockAndLaunch;
        }
        else if (Special == 2)
        {
            isLifeBlock = true;
            GetComponent<Renderer>().material = lifeBlock;
        }
        else if (Special > 15 && Special < 20)
        {
            isBallIncrease = true;
            GetComponent<Renderer>().material = ballInc;
        }


        if (isLifeBlock || isBallIncrease || isFireBall || isLaunch
)
            lives = 1;
        else
        {
            lives = Random.Range(1, 1000);
            if (lives < 30 + diffculty && lives > 0)
                lives = score = 4;
            else if (lives >= 30 + diffculty && lives < 100 + diffculty / 2 + diffculty)
                lives = score = 3;
            else if (lives >= 100 + +diffculty + diffculty /2 && lives < 350 + diffculty + diffculty /2 + diffculty / 3)
                lives = score = 2;
            else if (lives >= 350 + diffculty / 3 + diffculty /2 + diffculty && lives <= 1000)
                lives = score = 1;

            switch (lives)
            {
                case 1:
                    GetComponent<Renderer>().material = oneLife;
                    break;
                case 2:
                    GetComponent<Renderer>().material = TwoLives;
                    break;
                case 3:
                    GetComponent<Renderer>().material = ThreeLives;
                    break;
                case 4:
                    GetComponent<Renderer>().material = FourLives;
                    break;
            }
        }
        startlives = lives;
    }
    void Update()
    {
        tr.position = new Vector3(tr.position.x , tr.position.y, tr.position.z - speed * Time.deltaTime);

        if (isLifeBlock || isBallIncrease || isFireBall || isLaunch
){}
            // Debug.Log(lives); 
        else
        switch (lives)
        {
            case 1:
                GetComponent<Renderer>().material = oneLife;
                break;
            case 2:
                GetComponent<Renderer>().material = TwoLives;
                break;
            case 3:
                GetComponent<Renderer>().material = ThreeLives;
                break;
            case 4:
                GetComponent<Renderer>().material = FourLives;
                break;
        }

        if (transform.position.z < -9.5){
            BlockDestroyerScript.init();
            gameHandler.GetComponent<GameHandler>().loseLife(gameHandler.GetComponent<GameHandler>().ball);
            Destroy(this.gameObject);
        }
    }

    public void ChangeLife() 
    {
        lives--;
        if (lives <= 0){
            gameHandler.GetComponent<GameHandler>().addScore(score);
            if (isLifeBlock){
                gameHandler.GetComponent<GameHandler>().addLife();
            }
            else if (isFireBall){
                gameHandler.GetComponent<GameHandler>().fireballPowerUp();
            }
            else if (isBallIncrease){
                Instantiate(GameObject.FindGameObjectWithTag("Ball"), transform.position, Quaternion.identity);
                gameHandler.GetComponent<GameHandler>().addBall();
            }
            else if (isLaunch){
                BallMovement.lockAndLaunch = true;
            }

            Destroy(this.gameObject);
        }
    }

    void OnDestroy(){
        Instantiate(destroyParticle, transform.position, Quaternion.identity);
        gameHandler.GetComponent<GameHandler>().addXP(Random.Range(23,57) * startlives * diffculty/5);
        gameHandler.GetComponent<GameHandler>().StartCoroutine(gameHandler.GetComponent<GameHandler>().Shake(0.1f, 0.025f));
    }
}
