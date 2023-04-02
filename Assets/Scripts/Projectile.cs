using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	void Start()
    {
        StartCoroutine(DestroyProj());
		Physics.IgnoreLayerCollision(6, 6, true);
	}

    private IEnumerator DestroyProj() {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
