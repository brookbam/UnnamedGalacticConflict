using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathingSD : MonoBehaviour
{
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.2f;
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] int healthValue = 420;

    WaveConfig waveConfig;
    List<Transform> waypoints;
    int waypointIndex = 0;
    Player1 player1;
    

    // Start is called before the first frame update
    void Start()
    {
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].transform.position;
        player1 = FindObjectOfType<Player1>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Win();
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }

    private void Win()
    {
        if (transform.childCount == 0)
        {
            FindObjectOfType<Level>().LoadGameWin();
            Destroy(gameObject);
            GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
            Destroy(explosion, durationOfExplosion);
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
            if(player1.GetHealth() == 200)
            {
                FindObjectOfType<GameSession>().AddToScore(healthValue*4);
            }
            else if(player1.GetHealth() == 150)
            {
                FindObjectOfType<GameSession>().AddToScore(healthValue * 3);
            }
            else if (player1.GetHealth() == 100)
            {
                FindObjectOfType<GameSession>().AddToScore(healthValue * 2);
            }
            else if (player1.GetHealth() == 50)
            {
                FindObjectOfType<GameSession>().AddToScore(healthValue);
            }

        }

    }

    private void Move()
    {
        if (waypointIndex <= waypoints.Count - 1)
        {
            var targetPosition = waypoints[waypointIndex].transform.position;
            var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);
            if (transform.position == targetPosition)
            {
                waypointIndex++;
            }
            if(waypointIndex == waypoints.Count)
            {
                waypointIndex = 3;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
