using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserFeedback : MonoBehaviour
{
    [SerializeField] private GameObject twitchChannelNameBoorder;
    [SerializeField] private GameObject botNameBoorder;
    [SerializeField] private GameObject botAccessTokenBoorder;
    // Start is called before the first frame update

    public void ClientFailed()
    {
        twitchChannelNameBoorder.SetActive(true);
        botNameBoorder.SetActive(true);
        botAccessTokenBoorder.SetActive(true);
    }
    public void ClientSuccess()
    {
        twitchChannelNameBoorder.SetActive(false);
        botNameBoorder.SetActive(false);
        botAccessTokenBoorder.SetActive(false);
    }

}
