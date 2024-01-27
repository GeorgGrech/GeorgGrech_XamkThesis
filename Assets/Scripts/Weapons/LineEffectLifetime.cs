using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineEffectLifetime : MonoBehaviour
{
    [SerializeField] private float lineEffectDuration;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Lifetime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(lineEffectDuration);
        Destroy(gameObject);
    }
}
