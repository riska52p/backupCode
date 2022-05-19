using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPCManager : MonoBehaviour
{
    PhotonView photonView;
    GameObject gb;

    // Start is called before the first frame update
    void Awake()
    {
        photonView = PhotonView.Get(this);
        gb = this.gameObject;
    }

    #region Play Sounds

    public void PlaySoundRPC()
    {
        photonView.RPC("playSound", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    private void playSound()
    {
        gb.GetComponent<AudioSource>().Play();
    }

    #endregion
}
