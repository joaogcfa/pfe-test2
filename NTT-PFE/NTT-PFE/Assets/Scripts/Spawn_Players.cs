using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Spawn_Players : MonoBehaviour
{
    public GameObject AR_player;
    public GameObject PC_player;
    private bool done1,done2;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        #if UNITY_STANDALONE_WIN

            if(!done1){
                PhotonNetwork.Instantiate( PC_player.name , new Vector3(5f,-2f,5f), transform.rotation * Quaternion.Euler (-90f, 0f, 0f));
                print("spawn");
            }
            done1 = true;

        #endif

        #if UNITY_LUMIN

            if(!done2){
                PhotonNetwork.Instantiate( AR_player.name, new Vector3(0f,0f,0f), Quaternion.identity);
                print("spawnLumin");
            }
            done2 = true;

        #endif
    }
}
