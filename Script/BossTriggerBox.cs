using UnityEngine;
using UnityEngine.Playables;

public class BossTriggerBox : MonoBehaviour 
{
	GameObject timeLine;
	void Start()
	{
		if (GameObject.Find("Timeline"))
		{
			timeLine = GameObject.Find("Timeline");
		}
	}

	void OnTriggerEnter(Collider other)
	{
		PlayableDirector pd = timeLine.GetComponent<PlayableDirector>();
		pd.Play();
	}
}
