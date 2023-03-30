using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
	public float speed = 0.5f;
	public static event Action died;
    private CharacterController cc;

	private void Awake() {
		cc = GetComponent<CharacterController>();
	}

	void Update()
    {
		cc.Move(transform.forward * speed * Time.deltaTime);
    }

	private void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Projectile") {
			Destroy(collision.gameObject);
			Destroy(gameObject);
			EnemyManager.Instance.Remove(this);
			died?.Invoke();
		}
	}
}
