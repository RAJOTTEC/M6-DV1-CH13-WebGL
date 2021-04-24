using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Monetization;

public class PlayerShipBuild : MonoBehaviour
{
	//GameObject[] selection;
	[SerializeField]
	GameObject[] visualWeapons;
	GameObject textBoxPanel;
	GameObject bankObj;
	//GameObject target;
	GameObject buyButton;
	//GameObject tmpSelection;
	GameObject tmpSelection;
	int bank = 1100;
	string placementId_rewardedvideo = "rewardedVideo";
	string gameId = "1234567";
	bool purchaseMade = false;
	[SerializeField]
	SOActorModel defaultPlayerShip;
	GameObject playerShip;
	void Start()
	{
		purchaseMade = false;
		bankObj = GameObject.Find("bank");
		bankObj.GetComponentInChildren<TextMesh>().text = bank.ToString();
		textBoxPanel = GameObject.Find("textBoxPanel");
		buyButton = GameObject.Find("BUY?").gameObject;
		buyButton.SetActive(false);
		TurnOffPlayerShipVisuals();
		TurnOffSelectionHighlights();
		CheckPlatform();
		PreparePlayerShipForUpgrade();
	}

	void PreparePlayerShipForUpgrade()
	{
		playerShip = GameObject.Instantiate(Resources.Load("Prefab/Player/player_ship")) as GameObject;
		playerShip.GetComponent<Player>().enabled = false;
		playerShip.transform.position = new Vector3(0, 10000, 0);
		playerShip.GetComponent<Player>().ActorStats(defaultPlayerShip);
	}

	void CheckPlatform()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			gameId = "4063958";
		}
		else if (Application.platform == RuntimePlatform.Android)
		{
			gameId = "4063959";
		}
		Monetization.Initialize(gameId, false);
	}

	//REMOVED 01
	//
	//Raycast.
	//
	// GameObject ReturnClickedObject (out RaycastHit hit)
	// {
	// 	GameObject target = null;
	// 	Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
	// 	if (Physics.Raycast (ray.origin, ray.direction * 100, out hit)) 
	// 	{
	// 		target = hit.collider.gameObject;
	// 	}
	// 	return target;
	// }

	void ShowRewardedAds()
	{
		StartCoroutine(WaitForAd());
	}

	IEnumerator WaitForAd()
	{
		string placementId = placementId_rewardedvideo;
		while (!Monetization.IsReady(placementId))
		{
			yield return null;
		}

		ShowAdPlacementContent ad = null;
		ad = Monetization.GetPlacementContent(placementId) as ShowAdPlacementContent;
		if (ad != null)
		{
			ad.Show(AdFinished);
		}
	}

	void AdFinished(ShowResult result)
	{
		if (result == ShowResult.Finished)
		{
			bank += 300;
			bankObj.GetComponentInChildren<TextMesh>().text = bank.ToString();
			//TurnOffSelectionHighlights();
		}
	}
	void TurnOffPlayerShipVisuals()
	{
		for (int i = 0; i < visualWeapons.Length; i++)
		{
			visualWeapons[i].gameObject.SetActive(false);
		}
	}
	void TurnOffSelectionHighlights()
	{
		GameObject[] selections = GameObject.FindGameObjectsWithTag("Selection");
		for (int i = 0; i < selections.Length; i++)
		{
			if (selections[i].GetComponentInParent<ShopPiece>())
			{
				if (selections[i].GetComponentInParent<ShopPiece>().ShopSelection.iconName == "sold Out")
				{
					selections[i].SetActive(false);
				}
			}
		}
	}

	void UpdateDescriptionBox()
	{
		textBoxPanel.transform.Find("name").gameObject.GetComponent<TextMesh>().text = tmpSelection.GetComponent<ShopPiece>().ShopSelection.iconName;
		textBoxPanel.transform.Find("desc").gameObject.GetComponent<TextMesh>().text = tmpSelection.GetComponent<ShopPiece>().ShopSelection.description;
	}

	//REMOVED 02
	//
	//Target ray 3D game object.
	//
	// void Select()
	// {
	// 	tmpSelection = target.transform.Find("SelectionQuad").gameObject;
	// 	tmpSelection.SetActive(true);
	// }

	void LackOfCredits()
	{
		if (bank < System.Int32.Parse(tmpSelection.GetComponentInChildren<Text>().text))
		{
			Debug.Log("CAN'T BUY");
		}
	}
	void Affordable()
	{
		if (bank >= System.Int32.Parse(tmpSelection.GetComponentInChildren<Text>().text))
		{
			Debug.Log("CAN BUY");
			buyButton.SetActive(true);
		}
	}
	void SoldOut()
	{
		Debug.Log("SOLD OUT");
	}

	public void WatchAdvert()
	{
		if (Application.internetReachability != NetworkReachability.NotReachable)
		{
			ShowRewardedAds();
		}
	}
	public void BuyItem()
	{
		Debug.Log("PURCHASED");
		purchaseMade = true;
		buyButton.SetActive(false);
		textBoxPanel.transform.Find("desc").gameObject.GetComponent<TextMesh>().text = "";
		textBoxPanel.transform.Find("name").gameObject.GetComponent<TextMesh>().text = "";
		//		tmpSelection.SetActive(false);

		for (int i = 0; i < visualWeapons.Length; i++)
		{
			if (visualWeapons[i].name == tmpSelection.GetComponent<ShopPiece>().ShopSelection.iconName)
			{
				visualWeapons[i].SetActive(true);
			}
		}

		UpgradeToShip(tmpSelection.GetComponent<ShopPiece>().ShopSelection.iconName);

		bank = bank - System.Int16.Parse(tmpSelection.GetComponent<ShopPiece>().ShopSelection.cost);
		bankObj.transform.Find("bankText").GetComponent<TextMesh>().text = bank.ToString();
		tmpSelection.transform.Find("itemText").GetComponentInChildren<Text>().text = "SOLD";
	}

	void UpgradeToShip(string upgrade)
	{
		GameObject shipItem = GameObject.Instantiate(Resources.Load("Prefab/Player/" + upgrade)) as GameObject;
		shipItem.transform.SetParent(playerShip.transform);
		shipItem.transform.localPosition = Vector3.zero;
	}

	public void StartGame()
	{
		if (purchaseMade)
		{
			playerShip.name = "UpgradedShip";
			if (playerShip.transform.Find("energy +1(Clone)"))
			{
				playerShip.GetComponent<Player>().Health = 2;
			}
			DontDestroyOnLoad(playerShip);
		}
		GameManager.Instance.GetComponent<ScenesManager>().BeginGame(GameManager.gameLevelScene);
	}

	public void AttemptSelection(GameObject buttonName)
	{
		//REMOVED 03
		//
		// if (Input.GetMouseButtonDown (0)) 
		// {
		//RaycastHit hitInfo;
		//target = ReturnClickedObject (out hitInfo);

		// if (target != null)
		// {

		//if (target.transform.Find("itemText")))
		//{
		if (buttonName)
		{
			TurnOffSelectionHighlights();

			tmpSelection = buttonName;
			tmpSelection.transform.GetChild(1).gameObject.SetActive(true);

			//TurnOffSelectionHighlights();
			UpdateDescriptionBox();
			//Select();

			//NOT ALREADY SOLD
			if (buttonName.GetComponentInChildren<Text>().text != "SOLD")
			{
				//can afford
				Affordable();

				//can not afford
				LackOfCredits();
			}
			else if (buttonName.GetComponentInChildren<Text>().text == "SOLD")
			{
				SoldOut();
			}
		}

		//REMOVED 04
		//
		// else if (target.name == "WATCH AD")
		// {
		// 	WatchAdvert();
		// }
		// else if(currentSelection.name == "BUY ?")
		// {
		// 	BuyItem();
		// }
		// else if(target.name == "START")
		// {
		// 	StartGame();
		// }
		//}
		//}
	}

	//REMOVED 05
	//
	// void Update()
	// {
	// 	AttemptSelection();
	// }
}
