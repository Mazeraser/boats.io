using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Codebase.Infrastructure.Server
{
    /// <summary>
    /// Реализует корректное отображение единок на арене
    /// </summary>
    public class FoodSync : MonoBehaviourPunCallbacks, IPunObservable
    {
        
        Vector3 _latestPos;
        Quaternion _latestRot;
        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
            else
            {
                _latestPos = (Vector3)stream.ReceiveNext();
                _latestRot = (Quaternion)stream.ReceiveNext();
            }
        }
        
        void Update()
        {
            if (!photonView.IsMine)
            {
                transform.position = _latestPos;
                transform.rotation = _latestRot;
            }
        }
    }
}