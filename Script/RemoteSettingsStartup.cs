using UnityEngine;

public class RemoteSettingsStartUp : MonoBehaviour
{
    void Awake()
    {
        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork ||
          Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {

            RemoteSettings.Updated += () =>
            {
                GameManager.playerLives = RemoteSettings.GetInt("PlayersStartUpLives", GameManager.playerLives);
            };

        }
    }
}