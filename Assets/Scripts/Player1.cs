using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    //config params
    [Header("Player Movement")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 0.5f;

    [Header("Health and Sound")]
    [SerializeField] int health = 200;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.2f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.05f;
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] AudioClip hitSound;
    [SerializeField] [Range(0, 1)] float hitSoundVolume = 0.2f;
    [SerializeField] AudioClip r2Death;
    [SerializeField] [Range(0, 1)] float r2SoundVolume = 0.2f;
    [SerializeField] GameObject hitVFX;
    [SerializeField] float durationOfHit = 1f;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.2f;

    Coroutine firingCoroutine;
    bool isFiring = false;
    PauseControl pauseControl;
    float mid;
    

    float xMin;
    float xMax;
    //float yMin;
    //float yMax;
    Vector3 xWingOffset = new Vector3(0.4f, 0, 0);
    Vector3 worldPosition;

    // Start is called before the first frame update
    void Start()
    {
        pauseControl = FindObjectOfType<PauseControl>();
        SetUpMoveBoundaries();
        mid = Screen.width / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        mouseMove();
        Fire();
    }

    private void Fire()
    {
        if (!PauseControl.gameIsPaused)
        {
            if (((Input.GetButton("Fire1")) || (Input.GetAxis("Fire1") == 1) || (Input.GetButton("Fire2")) || (Input.GetMouseButton(0))) && !isFiring)
            {
                firingCoroutine = StartCoroutine(FireContinuously());
            }
        }
    }


    IEnumerator FireContinuously()
    {
            isFiring = true;
            GameObject laser1 = Instantiate(laserPrefab, transform.position + xWingOffset, Quaternion.identity) as GameObject;
            GameObject laser2 = Instantiate(laserPrefab, transform.position - xWingOffset, Quaternion.identity) as GameObject;
            laser1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            laser2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
            isFiring = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer)
        {
            return;
        }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
        else
        {
            AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, hitSoundVolume);
            GameObject explosion = Instantiate(hitVFX, transform.position, transform.rotation);
            Destroy(explosion, durationOfHit);
        }
    }

    private void Die()
    {
        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
        AudioSource.PlayClipAtPoint(r2Death, Camera.main.transform.position, r2SoundVolume);
    }

    public int GetHealth()
    {
        return health;
    }

    private void mouseMove()
    {
        float cur = transform.position.x;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        mousePos.y = Camera.main.nearClipPlane;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        if (Input.GetMouseButton(0))
        {
            if(worldPosition.x < cur-.15)
            {
                var deltaX = Time.deltaTime * moveSpeed;
                var newXPos = Mathf.Clamp(transform.position.x - deltaX, xMin, xMax);
                transform.position = new Vector2(newXPos, transform.position.y);
            }
            else if(worldPosition.x > cur+.15)
            {
                var deltaX = Time.deltaTime * moveSpeed;
                var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
                transform.position = new Vector2(newXPos, transform.position.y);
            }
            //else if(worldPosition.x < cur + .15 && worldPosition.x > cur -.15)
            //{
             //   transform.position = new Vector2(transform.position.x, transform.position.y);
            //}

            
        }
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        //var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        //var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, transform.position.y);
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        //yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        //yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }
}
