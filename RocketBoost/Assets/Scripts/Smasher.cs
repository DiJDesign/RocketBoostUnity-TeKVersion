using UnityEngine;

public class Smasher : MonoBehaviour
{

    Vector3 origPos;
    float timer = 0.0f;
    [SerializeField]float duration = 10.0f;
    [SerializeField]float height = 3.5f;
    [SerializeField] int speed = 10;
    void Start()
    {
        origPos = transform.position;   
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer < duration / 2)
        {
            transform.position = Vector3.Lerp(transform.position, origPos + Vector3.up * height, timer / speed); // 上
        }
        else
        {
            transform.position = Vector3.Lerp(origPos + Vector3.up * height, origPos, timer - duration / 2); // 下
        }
        if(timer >= duration)
        {
            timer = 0;
        }
        
    }
}
