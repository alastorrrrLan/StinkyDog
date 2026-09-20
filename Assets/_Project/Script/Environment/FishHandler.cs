using UnityEngine;

public class FishHandler : MonoBehaviour
{
    public int maxFishCount, currentFishCount;

    public void Start()
    {
        currentFishCount = maxFishCount;
        UpdateVisual();
    }
    public void UpdateVisual()
    {
        // Update the visual representation of the fish count here
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        for (int i = 0; i < currentFishCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void RemoveFish()
    {
        currentFishCount--;
        UpdateVisual();
    }
}
