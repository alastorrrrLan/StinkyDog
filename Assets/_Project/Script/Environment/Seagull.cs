using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Seagull : MonoBehaviour
{
    public bool fishCollected;
    public float speed, delay, barkSensitivity;
    public GameObject fishVisual;
    public Transform fish;
    public DogHandler dogHandler;

    public PauseManager loseManager;

    public AudioClip scream, steal;

    public void Start()
    {
        StartCoroutine(StartTimer());
    }

    public Vector2 GetStartPos()
    {
        return new Vector3(Random.Range(-8f, 8f), 10f, 0f);
    }

    private IEnumerator StartTimer()
    {
        if (fish.GetComponent<FishHandler>().currentFishCount == 0)
        {
            // stop the seagull from diving

            if (!dogHandler.isStinky)
            {
                // you lose
                loseManager.Lose();
            }
            yield break;
        }

        fishVisual.SetActive(false);
        yield return new WaitForSeconds(delay);
        StartCoroutine(StartDive());
    }

    private IEnumerator StartDive()
    {
        SoundManager.Instance.PlayRandomPitch(scream);

        Vector2 start = GetStartPos();
        transform.position = start;

        Vector2 fishPos = new(fish.position.x, fish.position.y);
        Vector2 end = -start;

        // Rotate to face fish
        Vector2 direction = start - fishPos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);


        // Move to fish
        float duration = 8f;
        float elapsedTime = 0f;
        float t;

        while (elapsedTime < duration)
        {
            t = elapsedTime / duration;

            t = 1f - Mathf.Pow(1f - t, 2f);

            transform.position = Vector2.Lerp(start, fishPos, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = fishPos;

        // Rotate to face end position
        direction = fishPos - end;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);


        SoundManager.Instance.PlayRandomPitch(steal);

        fish.GetComponent<FishHandler>().RemoveFish();
        CollectFish();

        // Move to end position
        elapsedTime = 0f;
        duration /= 2f;

        while (elapsedTime < duration)
        {
            t = elapsedTime / duration;

            t = 1f - Mathf.Pow(1f - t, 3f);

            transform.position = Vector2.Lerp(fishPos, end, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = end;

        StartCoroutine(StartTimer());
    }

    private IEnumerator Escape()
    {
        SoundManager.Instance.PlayRandomPitch(scream);

        Vector2 end = GetStartPos();
        Vector2 start = transform.position;

        Vector2 direction = start - end;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // escape to end position
        float t;
        float duration = 3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            t = elapsedTime / duration;

            t = 1f - Mathf.Pow(1f - t, 3f);

            transform.position = Vector2.Lerp(start, end, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = end;

        StartCoroutine(StartTimer());
    }

    public void ReactToBark(Vector2 dogPos)
    {
        float distance = Vector2.Distance(transform.position, dogPos);
        if (distance < barkSensitivity)
        {
            StopAllCoroutines();
            StartCoroutine(Escape());
        }
    }

    public void CollectFish()
    {
        fishCollected = true;
        fishVisual.SetActive(true);
    }
}
