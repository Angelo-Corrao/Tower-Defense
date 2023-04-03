using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject projectile;
    public Transform projSpawnPoint;
    public float fireRate = 1f;
	public float projectileSpeed = 10f;
	public float rotationSpeed = 5f;
	private EnemyController targettedEnemy;
	[HideInInspector]
    public float rotationCounterTimer = 1f;
    private Quaternion targetRotation;
	[HideInInspector]
    public bool isTargettingEnemy = false;
    private bool canFire = true;

	private void OnEnable() {
		EnemyController.died += EnemyKilled;
	}

	private void OnDisable() {
		EnemyController.died -= EnemyKilled;
	}

	void Update()
    {
        if (rotationCounterTimer < 1) {
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationCounterTimer);
			rotationCounterTimer += Time.deltaTime * rotationSpeed;
		}
		else {
			if (isTargettingEnemy) {
				if (canFire)
					StartCoroutine(Shoot());
			}
		}
	}

    public void RotatePlayer(EnemyController ec) {
		if (targettedEnemy != null) {
			Vector3 eDirection = ec.transform.position - transform.position;
			float eLength = eDirection.magnitude;

			Vector3 teDirection = targettedEnemy.transform.position - transform.position;
			float teLength = teDirection.magnitude;

			if (eLength < teLength) {
			    targettedEnemy = ec;
				Vector3 direction = targettedEnemy.transform.position - transform.position;
				direction.y = 0f;
				targetRotation = Quaternion.LookRotation(direction);
				rotationCounterTimer = 0f;
			}
        }
        else {
			targettedEnemy = ec;
			Vector3 direction = targettedEnemy.transform.position - transform.position;
			direction.y = 0f;
			targetRotation = Quaternion.LookRotation(direction);
			rotationCounterTimer = 0f;
		}
        isTargettingEnemy = true;
    }

    public IEnumerator Shoot() {
        GameObject proj = Instantiate(projectile, projSpawnPoint.position, projSpawnPoint.rotation);
        proj.GetComponent<Rigidbody>().AddForce(projSpawnPoint.forward * projectileSpeed, ForceMode.Impulse);
		canFire = false;
		yield return new WaitForSeconds(fireRate);
        canFire = true;
	}

	public void EnemyKilled() {
		isTargettingEnemy = false;
		if (Physics.CheckSphere(Vector3.zero, 15f, LayerMask.GetMask("Enemy"))) {
			foreach (EnemyController ec in EnemyManager.Instance.enemies) {
				Vector3 distance = ec.transform.position - Vector3.zero;
				if (distance.magnitude < 15f)
					RotatePlayer(ec);
			}
		}
	}
}
