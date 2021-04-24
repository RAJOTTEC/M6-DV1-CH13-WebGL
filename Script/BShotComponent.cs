using UnityEngine;

public class BShotComponent : MonoBehaviour 
{
	[SerializeField]
	GameObject bShotBullet;

	void Start()
	{
		if (GetComponentInParent<Player>())
		{
			GetComponentInParent<Player>().Fire = bShotBullet;	
		}
	}
}
