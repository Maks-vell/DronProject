using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] public float speed = 0; 
  
    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0, Space.Self); 
    }
}
