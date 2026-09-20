using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DogHandler : MonoBehaviour
{
    public float speed, barkCooldown, maxStink, stinkProgress;
    public bool isStinky, stinkyInProgress, canBark;
    public GameObject human, seagull, stink, bark;
    public Transform tail;
    public float wagSpeed, wagAngle;

    public AudioClip barkSound, stinkSound;

    private Vector2 movement;
    private Rigidbody2D rb;
    private Quaternion tailStartRotation;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stink.SetActive(false);
        isStinky = false;
        tailStartRotation = tail.localRotation;
    }

    // movement here:
    private void Update()
    {
        movement = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
            movement.y += 1;

        if (Keyboard.current.sKey.isPressed)
            movement.y -= 1;

        if (Keyboard.current.aKey.isPressed)
            movement.x -= 1;

        if (Keyboard.current.dKey.isPressed)
            movement.x += 1;

        movement = movement.normalized;

        // Rotate to face movement direction
        if (movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            angle -= 90f; // Adjust for sprite orientation, might be a better way but idk
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        // Just barking lol
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Bark();
        }

        if (stinkyInProgress)
        {
            stink.SetActive(true);
            stinkProgress += Time.deltaTime;

            // alpha the stink visual based on stinkProgress
            float a = Mathf.Clamp01(stinkProgress / maxStink);
            Color c = stink.GetComponent<SpriteRenderer>().color;

            if (stinkProgress >= maxStink)
            {
                isStinky = true;
                stinkyInProgress = false;
                a = 2f;

                // sound effect for stink here
                SoundManager.Instance.PlayRandomPitch(stinkSound);
            }

            a /= 10f; // make it less opaque

            c.a = a;
            stink.GetComponent<SpriteRenderer>().color = c;

            
        }
        else if (!isStinky)
        {
            stinkProgress -= Time.deltaTime;

            if (stinkProgress <= 0)
            {
                stinkProgress = 0;
                stink.SetActive(false);
            }
            else
            {
                // alpha the stink visual based on stinkProgress
                float a = Mathf.Clamp01(stinkProgress / maxStink) / 20;
                Color c = stink.GetComponent<SpriteRenderer>().color;
                c.a = a;
                stink.GetComponent<SpriteRenderer>().color = c;
            }
        }

        if (stinkyInProgress) // this one is AI, idk what it does but it makes the tail wag when stinkyInProgress is true
        {
            float angle = Mathf.Sin(Time.time * wagSpeed) * wagAngle;

            tail.localRotation = tailStartRotation * Quaternion.Euler(0, 0, angle);
        }
        else
        {
            // Return tail to its normal position
            tail.localRotation = Quaternion.Lerp(
                tail.localRotation,
                tailStartRotation,
                10f * Time.deltaTime
            );
        }
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * movement);
    }

    public void Bark()
    {
        if (!canBark)
            return;
        canBark = false;
        human.GetComponent<Enemy>().MoveToSound(transform.position);

        seagull.GetComponent<Seagull>().ReactToBark(transform.position);


        StartCoroutine(BarkCooldown());

        // Add bark sound effect here
        SoundManager.Instance.PlayRandomPitch(barkSound);
    }

    private IEnumerator BarkCooldown()
    {
        bark.SetActive(true);
        yield return new WaitForSeconds(barkCooldown / 2);
        bark.SetActive(false);
        yield return new WaitForSeconds(barkCooldown / 2);
        canBark = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
            stinkyInProgress = true;
            stink.SetActive(true);
        }
        else if (collision.gameObject.CompareTag("Exit") && isStinky)
        {
            Debug.Log("You win!");
            MenuManager.Instance.LoadNextLevel();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
            stinkyInProgress = false;
        }
    }
}
