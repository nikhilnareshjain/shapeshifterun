using UnityEngine;

public class Obstacle : MonoBehaviour {
    [SerializeField] GameObject woodDestroyEffect;

    public void DestroyWood() {
        woodDestroyEffect.SetActive(true);
        woodDestroyEffect.GetComponent<ParticleSystem>().Play();
    }
}