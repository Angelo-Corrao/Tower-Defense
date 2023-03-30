using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject projectile;
    public Transform projSpawnPoint;
    public float fireRate = 1f;
	public float rotationSpeed = 5f;
	private EnemyController targettedEnemy;
    private float rotationCounterTimer = 1f;
    private Quaternion targetRotation;
    private bool isTargettingEnemy = false;
    private bool canFire = true;
    private Vector3 bho;
	private Vector3 bho2;

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
        /*foreach (EnemyController e in EnemyManager.Instance.activeEnemies) {
            Vector3 eDirection = e.transform.position - transform.position;
            float length = eDirection.magnitude;

            if (length < precedentLength) {
                //Debug.Log("ok");
                targettedEnemy = e;
            }

            precedentLength = length;
        }
        precedentLength = 0f;*/

		if (targettedEnemy != null) {
			Vector3 eDirection = ec.transform.position - transform.position;
            bho2 = eDirection;
			float eLength = eDirection.magnitude;

			Vector3 teDirection = targettedEnemy.transform.position - transform.position;
			float teLength = teDirection.magnitude;

			if (eLength < teLength) {
			    targettedEnemy = ec;
				Vector3 direction = targettedEnemy.transform.position - transform.position;
				bho = direction;
				direction.y = 0f;
				targetRotation = Quaternion.LookRotation(direction);
				rotationCounterTimer = 0f;
			}
        }
        else {
			targettedEnemy = ec;
			Vector3 direction = targettedEnemy.transform.position - transform.position;
			bho = direction;
			direction.y = 0f;
			targetRotation = Quaternion.LookRotation(direction);
			rotationCounterTimer = 0f;
		}
        isTargettingEnemy = true;
    }

    public IEnumerator Shoot() {
        GameObject proj = Instantiate(projectile, projSpawnPoint.position, projSpawnPoint.rotation);
        proj.GetComponent<Rigidbody>().AddForce(projSpawnPoint.forward * 5f, ForceMode.Impulse);
		canFire = false;
		yield return new WaitForSeconds(fireRate);
        canFire = true;
	}

	public void EnemyKilled() {
		isTargettingEnemy = false;
		if (Physics.CheckSphere(Vector3.zero, 15f, LayerMask.GetMask("Default"))) {
			foreach (EnemyController ec in EnemyManager.Instance.activeEnemies) {
				Vector3 distance = ec.transform.position - Vector3.zero;
				if (distance.magnitude < 15f)
					RotatePlayer(ec);
			}
		}
	}

    /*private void OnDrawGizmos() {
		Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, bho);
		Gizmos.DrawLine(transform.position, bho2);
        Gizmos.DrawSphere(Vector3.zero, 15f);
	}*/
}
