using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public GameObject player;
    public Maze mazePrefab;

	private Maze mazeInstance;

	private void Start () {
		BeginGame();
	}
	
	private void Update () {
		if (Input.GetKeyDown(KeyCode.P)) {
			RestartGame();
		}
	}

	private void BeginGame () {
		mazeInstance = Instantiate(mazePrefab) as Maze;
        mazeInstance.player = player;
        mazeInstance.gameManager = this.gameObject;
        StartCoroutine(mazeInstance.Generate());

	}

	private void RestartGame () {
		StopAllCoroutines();
		Destroy(mazeInstance.gameObject);
		BeginGame();
	}
}