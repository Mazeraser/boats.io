using System;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using Codebase.Infrastructure.Client;

namespace Codebase.Infrastructure.Server
{
    [Serializable]
    public struct ObjectInfo
    {
        public string prefabName;
        public int posX;
        public int posY;
    }

    public static class ObjectInfoData
    {
        public static object Deserialize(byte[] data)
        {
            var stream = new System.IO.MemoryStream(data);
            var reader = new System.IO.BinaryReader(stream);

            ObjectInfo obj = new ObjectInfo
            {
                prefabName = reader.ReadString(),
                posX = reader.ReadByte(),
                posY = reader.ReadByte()
            };

            return obj;
        }

        public static byte[] Serialize(object customObject)
        {
            ObjectInfo obj = (ObjectInfo)customObject;
            var stream = new System.IO.MemoryStream();
            var writer = new System.IO.BinaryWriter(stream);

            writer.Write(obj.prefabName);
            writer.Write(obj.posX);
            writer.Write(obj.posY);

            return stream.ToArray();
        }
    }
    public class ServerMemento : MonoBehaviourPun, IOnEventCallback
    {
        private List<ObjectInfo> _environment = new List<ObjectInfo>();
        
        public ObjectInfo[] GetEnvironment(Vector3 position, Vector2 size)
        {
            List<ObjectInfo> info = new List<ObjectInfo>();
            foreach (var obj in Physics2D.OverlapBoxAll(position, size, 0))
            {
                if (!obj.CompareTag("Player"))
                {
                    ObjectInfo newObj;
                    newObj.prefabName = obj.gameObject.name.Replace("(Clone)", "");
                    newObj.posX = (int)obj.gameObject.transform.position.x;
                    newObj.posY = (int)obj.gameObject.transform.position.y;
                    info.Add(newObj);
                }
            }
            return info.ToArray();
        }

        private void Awake()
        {
            PhotonPeer.RegisterType(typeof(ObjectInfo), (byte)'O', ObjectInfoData.Serialize, ObjectInfoData.Deserialize);
        }

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }
        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code == PlayerSync.SendEnvironmentToPlayerCode)
            {
                Debug.Log("Setting environment middle...");
                object[] data = (object[])photonEvent.CustomData;

                float posX = (float)data[0];
                float posY = (float)data[1];
                float posZ = (float)data[2];
                float sizeX = (float)data[3];
                float sizeY = (float)data[4];
                int viewID = (int)data[5];

                Vector3 targetPosition = new Vector3(posX, posY, posZ);
                Vector2 size = new Vector2(sizeX, sizeY);
                PhotonView playerView = PhotonView.Find(viewID);

                if (playerView == null)
                {
                    Debug.LogError("Ошибка: `PhotonView` не найден! ViewID: " + viewID);
                    return;
                }

                ObjectInfo[] env = GetEnvironment(targetPosition, size);
                if (env.Length == 0)
                {
                    Debug.Log("Нет объектов для отправки.");
                    return;
                }

                // Подготавливаем данные
                List<object> sendData = new List<object> { viewID }; // ID игрока

                foreach (var obj in env)
                {
                    sendData.Add(obj.prefabName);
                    sendData.Add(obj.posX);
                    sendData.Add(obj.posY);
                }

                PhotonNetwork.RaiseEvent(PlayerSync.SetEnvironmentCode, sendData.ToArray(), 
                    new RaiseEventOptions { Receivers = ReceiverGroup.All }, 
                    new SendOptions { Reliability = true });

                Debug.Log("Event raised");
            }
        }
    }
}