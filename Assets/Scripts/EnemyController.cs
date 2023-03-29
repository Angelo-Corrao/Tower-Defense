using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	public float speed = 0.5f;
    private CharacterController cc;

	private void Awake() {
		cc = GetComponent<CharacterController>();
	}

	void Update()
    {
		cc.Move(transform.forward * speed * Time.deltaTime);
    }
}
