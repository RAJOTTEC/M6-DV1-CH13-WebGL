using UnityEngine;
using UnityEngine.UI;

public class ShopPiece : MonoBehaviour
{
	[SerializeField]
	SOShopSelection shopSelection;
	public SOShopSelection ShopSelection
	{
		get { return shopSelection; }
		set { shopSelection = value; }
	}

	void Awake()
	{
		if (transform.GetChild(3).GetComponent<Image>() != null)
		{
			transform.GetChild(3).GetComponent<Image>().sprite = shopSelection.icon;
		}
		if (transform.Find("itemText"))
		{
			GetComponentInChildren<Text>().text = shopSelection.cost.ToString();
		}
	}
}