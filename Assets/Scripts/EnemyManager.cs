using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
	public static EnemyManager Instance { get; set; }
	public GameObject enemy;
	public List<EnemyController> activeEnemies = new List<EnemyController>();
	public UnityEvent<EnemyController> inRange;

	void Awake() {
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	private void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.z = 19.35f;
			mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
			Vector3 direction = transform.position - mousePosition;
			Quaternion enemyRotation = Quaternion.LookRotation(direction);
			enemyRotation.x = 0f;
			enemyRotation.z = 0f;
			GameObject spawnedEnemy = Instantiate(enemy, mousePosition, enemyRotation);
			EnemyController ec = spawnedEnemy.GetComponent<EnemyController>();
			Add(ec);
		}

		if (Physics.CheckSphere(Vector3.zero, 15f, LayerMask.GetMask("Default"))) {
			foreach (EnemyController ec in activeEnemies) {
				Vector3 distance = ec.transform.position - Vector3.zero;
				if (distance.magnitude < 15f)
					inRange?.Invoke(ec);
			}
		}

		/*foreach (EnemyController ec in activeEnemies) {
			Vector3 distance = ec.transform.position - Vector3.zero;
			if (distance.magnitude < 15f)
				inRange?.Invoke(ec);
			if (Vector3.Distance(ec.transform.position, Vector3.zero) < 15f) {
				inRange?.Invoke(ec);
			}
		}*/
	}

	public void Add(EnemyController enemy) {
		activeEnemies.Add(enemy);
	}

    public void Remove(EnemyController enemy) {
        activeEnemies.Remove(enemy);
    }
}
