using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
	public static EnemyManager Instance { get; set; }
	public GameObject enemy;
	public GameObject spawnArea;
	public List<EnemyController> enemies = new List<EnemyController>();
	public Collider[] enemiesInRangeColliders;
	public UnityEvent<EnemyController> inRange;
	public bool canCheck = true;

	void Awake() {
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	private void Update() {
		if (Input.GetMouseButtonDown(0) && !GameManager.Instance.anyUIActive) {
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.z = 19.35f;
			mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

			Vector3 spawnAreaSize = spawnArea.GetComponent<BoxCollider>().size;
			if (mousePosition.x < spawnArea.transform.position.x - spawnAreaSize.x / 2 || 
				mousePosition.x > spawnArea.transform.position.x + spawnAreaSize.x / 2 ||
				mousePosition.z < spawnArea.transform.position.z - spawnAreaSize.z / 2 || 
				mousePosition.z > spawnArea.transform.position.z + spawnAreaSize.z / 2) {
					Vector3 direction = transform.position - mousePosition;
					Quaternion enemyRotation = Quaternion.LookRotation(direction);
					enemyRotation.x = 0f;
					enemyRotation.z = 0f;
					GameObject spawnedEnemy = Instantiate(enemy, mousePosition, enemyRotation);
					EnemyController ec = spawnedEnemy.GetComponent<EnemyController>();
					Add(ec);
			}
		}

		if (canCheck)
			StartCoroutine(CheckEnemiesInRange(0.1f));
	}

	public void Add(EnemyController enemy) {
		enemies.Add(enemy);
	}

    public void Remove(EnemyController enemy) {
        enemies.Remove(enemy);
    }

	public IEnumerator CheckEnemiesInRange(float seconds) {
		canCheck = false;
		yield return new WaitForSeconds(seconds);
		enemiesInRangeColliders = Physics.OverlapSphere(Vector3.zero, 15f, LayerMask.GetMask("Enemy"));
		foreach (Collider c in enemiesInRangeColliders)
			inRange?.Invoke(c.gameObject.GetComponent<EnemyController>());
		canCheck = true;
	}
}
