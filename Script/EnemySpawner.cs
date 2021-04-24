using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField]
	SOActorModel actorModel;
	[SerializeField]
	float spawnRate;
	[SerializeField]
	[Range(0, 10)]
	int quantity;
	GameObject enemies;

	void Awake()
	{
		enemies = GameObject.Find("_Enemies");
		StartCoroutine(FireEnemy(quantity, spawnRate));
	}

	IEnumerator FireEnemy(int qty, float spwnRte)
	{
		for (int i = 0; i < qty; i++)
		{
			GameObject enemyUnit = CreateEnemy();
			enemyUnit.gameObject.transform.SetParent(this.transform);
			enemyUnit.transform.position = transform.position;
			yield return new WaitForSeconds(spwnRte);
		}
		yield return null;
	}

	GameObject CreateEnemy()
	{
		GameObject enemy = GameObject.Instantiate(actorModel.actor) as GameObject;
		enemy.GetComponent<IActorTemplate>().ActorStats(actorModel);
		enemy.name = actorModel.actorName.ToString();
		return enemy;
	}
}