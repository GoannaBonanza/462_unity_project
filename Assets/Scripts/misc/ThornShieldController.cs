using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornShieldController : MonoBehaviour
{
    public float persistence_time = 4.0f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(kill());
    }
    void FixedUpdate()
    {
        
    }
    private IEnumerator kill()
    {
        yield return new WaitForSeconds(persistence_time);
        Destroy(gameObject, 0.5f);
    } 
}
