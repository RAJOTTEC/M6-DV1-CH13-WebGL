using UnityEngine;
using UnityEngine.EventSystems;
public class Player : MonoBehaviour, IActorTemplate
{
	int travelSpeed;
	int health;
	int hitPower;
	GameObject actor;
	GameObject fire;
	GameObject _Player;
	float camTravelSpeed;
	public float CamTravelSpeed
    {
		get { return camTravelSpeed; }
		set { camTravelSpeed = value; }
    }

	float movingScreen;

	public int Health
	{
		get { return health; }
		set { health = value; }
	}

	public GameObject Fire
	{
		get { return fire; }
		set { fire = value; }
	}

	//float width;
	//float height;

	Vector3 direction;
	Rigidbody rb;
	public static bool mobile = false;
	GameObject[] screenPoints = new GameObject[2];

	void Start()
	{
		//height = 1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).y - .5f);
		//width = 1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).x - .5f);
		//movingScreen = width;
		_Player = GameObject.Find("_Player");

		mobile = false;

		#if UNITY_ANDROID && !UNITY_EDITOR
            mobile = true; 
            InvokeRepeating("Attack",0,0.3f); 
            rb = GetComponent<Rigidbody>();
		#endif

		CalculateBoundaries();
	}

	void Update()
	{
		if (Time.timeScale == 1)
		{
			PlayersSpeedWithCamera();

			if (mobile)
			{
				MobileControls();
			}
			else
			{
				Movement();
				Attack();
			}
		}
	}

	void CalculateBoundaries()
	{
		screenPoints[0] = new GameObject("p1");
		screenPoints[1] = new GameObject("p2");
		Vector3 v1 = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 300));
		Vector3 v2 = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 300));
		screenPoints[0].transform.position = v1;
		screenPoints[1].transform.position = v2;
		screenPoints[0].transform.SetParent(this.transform.parent);
		screenPoints[1].transform.SetParent(this.transform.parent);
		movingScreen = screenPoints[1].transform.position.x;
	}

	void MobileControls()
	{
		if (Input.touchCount > 0 && EventSystem.current.currentSelectedGameObject == null)
		{
			Touch touch = Input.GetTouch(0);
			Vector3 touchPosition = Camera.main.ScreenToWorldPoint
									(new Vector3(touch.position.x, touch.position.y, 300));
			touchPosition.z = 0;
			direction = (touchPosition - transform.position);
			rb.velocity = new Vector3(direction.x, direction.y, 0) * 5;
			direction.x += movingScreen;

			if (touch.phase == TouchPhase.Ended)
			{
				rb.velocity = Vector3.zero;
			}
		}
	}

		public void ActorStats(SOActorModel actorModel)
		{
			health = actorModel.health;
			travelSpeed = actorModel.speed;
			hitPower = actorModel.hitPower;
			fire = actorModel.actorsBullets;
		}

		void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Enemy")
		{
			if (health >= 1)
			{
				if (transform.Find("energy +1(Clone)"))
				{
					Destroy(transform.Find("energy +1(Clone)").gameObject);
					health -= other.GetComponent<IActorTemplate>().SendDamage();
				}
				else
				{
					health -= 1;
				}
			}

			if (health <= 0)
			{
				Die();
			}
		}
	}

	public void TakeDamage(int incomingDamage)
	{
		health -= incomingDamage;
	}

	public int SendDamage()
	{
		return hitPower;
	}

	void PlayersSpeedWithCamera()
    {
		if (camTravelSpeed > 1)
		{
			transform.position += Vector3.right * Time.deltaTime * camTravelSpeed;
			movingScreen += Time.deltaTime * camTravelSpeed;
		}
        else
        {
			movingScreen = 0;
        }
	}

	void Movement()
	{
		if (Input.GetAxisRaw("Horizontal") > 0)
		{
			if (transform.localPosition.x < (screenPoints[1].transform.localPosition.x - screenPoints[1].transform.localPosition.x / 30f) + movingScreen)
			{
				transform.localPosition += new Vector3(Input.GetAxisRaw("Horizontal") * Time.deltaTime * travelSpeed, 0, 0);
			}
		}

		if (Input.GetAxisRaw("Horizontal") < 0)
		{
			if (transform.localPosition.x > (screenPoints[0].transform.localPosition.x + screenPoints[0].transform.localPosition.x / 30f) + movingScreen)
			{
				transform.localPosition += new Vector3(Input.GetAxisRaw("Horizontal") * Time.deltaTime * travelSpeed, 0, 0);
			}
		}

		if (Input.GetAxisRaw("Vertical") < 0)
		{
			if (transform.localPosition.y > (screenPoints[1].transform.localPosition.y - screenPoints[1].transform.localPosition.y / 3f))
			{
				transform.localPosition += new Vector3(0, Input.GetAxisRaw("Vertical") * Time.deltaTime * travelSpeed, 0);
			}
		}

		if (Input.GetAxisRaw("Vertical") > 0)
		{
			if (transform.localPosition.y < (screenPoints[0].transform.localPosition.y - screenPoints[0].transform.localPosition.y / 5))
			{
				transform.localPosition += new Vector3(0, Input.GetAxisRaw("Vertical") * Time.deltaTime * travelSpeed, 0);
			}
		}
	}

	public void Die()
	{
		GameObject explode = GameObject.Instantiate(Resources.Load("Prefab/explode")) as GameObject;
		explode.transform.position = this.gameObject.transform.position;
		GameManager.Instance.LifeLost();
		Destroy(this.gameObject);
	}

	public void Attack()
	{
		if (Input.GetButtonDown("Fire1") || mobile)
		{
			GameObject bullet = GameObject.Instantiate(fire, transform.position, Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;
			bullet.transform.SetParent(_Player.transform);
			bullet.transform.localScale = new Vector3(7, 7, 7);
		}
	}
}