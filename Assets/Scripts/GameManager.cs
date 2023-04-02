using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
	public Canvas gameOver;
	public PlayerController player;
	public bool anyUIActive = false;

	private void Awake() {
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	public void GameOver() {
		Time.timeScale = 0;
		gameOver.gameObject.SetActive(true);
		anyUIActive = true;
	}

	public void Restart() {
		foreach (EnemyController ec in EnemyManager.Instance.activeEnemies) {
			Destroy(ec.gameObject);
		}
		EnemyManager.Instance.activeEnemies.Clear();
		EnemyManager.Instance.activeEnemiesInRange.Clear();
		player.isTargettingEnemy = false;
		player.rotationCounterTimer = 1;
		player.transform.rotation = Quaternion.Euler(0, 0, 0);
		gameOver.gameObject.SetActive(false);
		anyUIActive = false;
		Time.timeScale = 1;
	}

	public void QuitGame() {
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}
}
