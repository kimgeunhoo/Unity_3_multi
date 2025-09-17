using UnityEngine;

public class LifeTime : MonoBehaviour
{
    [SerializeField] private float time = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, time);

    }

    // ObjectPooling 최적화 기법 - 개선

}
