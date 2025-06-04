using System.Collections;
using UnityEngine;

public class BombEffect : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(EffectDestory());
    }

    IEnumerator EffectDestory()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
