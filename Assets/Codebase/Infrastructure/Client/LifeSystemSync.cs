using System;
using Codebase.Mechanics.Life_system;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Codebase.Infrastructure.Client
{
    public class LifeSystemSync : MonoBehaviourPunCallbacks
    {
        [SerializeField] 
        private Image _hpBar;
        
        public int HP { get; private set; }
        

        private void Update()
        {
            if (photonView.IsMine)
            {
                photonView.RPC(nameof(SyncHPBar), RpcTarget.All, GetComponent<BoatLifeComponent>().HP, GetComponent<BoatLifeComponent>().MaxHP);
            }

            _hpBar.enabled = GetComponent<BoatLifeComponent>().HP > 0;
        }

        [PunRPC]
        private void SyncHPBar(int HP, int MaxHP)
        {
            _hpBar.fillAmount = (float)HP / MaxHP;
        }
    }
}