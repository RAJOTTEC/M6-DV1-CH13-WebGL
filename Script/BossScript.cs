using UnityEngine;

public class BossScript : MonoBehaviour 
{
	    void OnTriggerEnter(Collider other)
    {
        // if the player or their bullet hits you....
        if (other.tag == "Player")
        {
			Die(other.gameObject);
        }
    }

	public void Die(GameObject other)
    {
        Destroy(other);
    }
}
