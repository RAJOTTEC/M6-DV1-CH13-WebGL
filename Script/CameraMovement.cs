using UnityEngine;

public class CameraMovement : MonoBehaviour 
{	
	bool beginMoving = false;
	float camSpeed;
	public float CamSpeed
    {
        get {return camSpeed;}
        set {camSpeed = value;}
    }

	void Start() 
	{
		Invoke("DelayStart",7f);
	}

	void DelayStart()
	{
		beginMoving = true;
		if (GameObject.Find("Player"))
		{
			GameObject.Find("Player").GetComponent<Player>().CamTravelSpeed = camSpeed;
		}
	
	}

	void Update()
	{
		if (beginMoving)
		{
			if (transform.position.x < 5350)
			{
				transform.Translate(Vector3.right * Time.deltaTime * camSpeed);	
			}
			else
			{
				beginMoving = false;
				GameObject.Find("PlayerSpawner").GetComponentInChildren<Player>().CamTravelSpeed = 0;
			}	
		}
	}
}
