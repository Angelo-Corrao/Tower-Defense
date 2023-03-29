using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private EnemyController targettedEnemy;
    private float rotationSpeed = 5f;
    private float rotationCounterTimer = 0f;
    private Quaternion targetRotation;
    private Vector3 bho;
	private Vector3 bho2;
	void Update()
    {
        if (rotationCounterTimer > 0) {
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
		}
        else {
            rotationCounterTimer -= Time.deltaTime * rotationSpeed;
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
                Debug.Log("ok");
			    targettedEnemy = ec;
		    }
        }
        else {
			targettedEnemy = ec;
		}

        Vector3 direction = targettedEnemy.transform.position - transform.position;
        bho = direction;
        direction.y = 0f;
        targetRotation = Quaternion.LookRotation(direction);
        rotationCounterTimer = 1f;
    }

	private void OnDrawGizmos() {
		Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, bho);
		Gizmos.DrawLine(transform.position, bho2);
	}
}
