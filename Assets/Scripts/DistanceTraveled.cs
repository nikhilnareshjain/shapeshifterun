using UnityEngine;
using UnityEngine.UI;

public class DistanceTraveled : MonoBehaviour
{
    int dist = 0;

    [SerializeField] Text distanceTraveled;

    // [SerializeField] AudioSource collectionSound;

    public void UpdateDistance(float distanceTraveledThisFrame, float speed) {
        
        // Update the total distance traveled
        // dist += Mathf.RoundToInt(distanceTraveledThisFrame * 1000) / 1000;
        Debug.Log("NJ: " + distanceTraveledThisFrame * 1000);
        dist += Mathf.RoundToInt(distanceTraveledThisFrame * 1000);
        distanceTraveled.text = "D:" + dist / 100;
        // Update the lastPosition to the current position for the next frame
    }

    public int getDistance() {
        return dist;
    }
}