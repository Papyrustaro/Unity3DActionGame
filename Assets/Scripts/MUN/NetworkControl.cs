using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class NetworkControl : MonobitEngine.MonoBehaviour
{
    private string roomName = "";

    private GameObject playerPrefab = null;

    void Start()
    {
        //デフォルトロビーへの自動入室を許可する
        MonobitNetwork.autoJoinLobby = true;

        //MUNサーバーに接続
        MonobitNetwork.ConnectServer("ActionSample_v1.0");
    }

    private void Update()
    {
        // MUNサーバに接続しており、かつルームに入室している場合
        if (MonobitNetwork.isConnect && MonobitNetwork.inRoom)
        {
            // プレイヤーキャラクタが未登場の場合に登場させる
            if (playerPrefab == null)
            {
                playerPrefab = MonobitNetwork.Instantiate(
                                "Player/Player",
                                Vector3.zero,
                                Quaternion.identity,
                                0);
            }
        }
    }

    private void OnGUI()
    {
        // デフォルトのボタンと被らないように、段下げを行なう。
        GUILayout.Space(24);

        // MUNサーバに接続している場合
        if (MonobitNetwork.isConnect)
        {
            // ボタン入力でサーバから切断＆シーンリセット
            if (GUILayout.Button("Disconnect", GUILayout.Width(150)))
            {
                // サーバから切断する
                MonobitNetwork.DisconnectServer();

                // シーンをリロードする
#if UNITY_5_3_OR_NEWER || UNITY_5_3
                string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
#else
                Application.LoadLevel(Application.loadedLevelName);
#endif
            }

            // ルームに入室している場合
            if (MonobitNetwork.inRoom)
            {
                // ボタン入力でルームから退室
                if (GUILayout.Button("Leave Room", GUILayout.Width(150)))
                {
                    MonobitNetwork.LeaveRoom();
                }
            }

            // ルームに入室していない場合
            if (!MonobitNetwork.inRoom)
            {
                GUILayout.BeginHorizontal();

                // ルーム名の入力
                GUILayout.Label("RoomName : ");
                roomName = GUILayout.TextField(roomName, GUILayout.Width(200));

                // ボタン入力でルーム作成
                if (GUILayout.Button("Create Room", GUILayout.Width(150)))
                {
                    MonobitNetwork.CreateRoom(roomName);
                }

                GUILayout.EndHorizontal();

                // 現在存在するルームからランダムに入室する
                if (GUILayout.Button("Join Random Room", GUILayout.Width(200)))
                {
                    MonobitNetwork.JoinRandomRoom();
                }

                // ルーム一覧から選択式で入室する
                foreach (RoomData room in MonobitNetwork.GetRoomData())
                {
                    string strRoomInfo =
                        string.Format("{0}({1}/{2})",
                                      room.name,
                                      room.playerCount,
                                      (room.maxPlayers == 0) ? "-" : room.maxPlayers.ToString());

                    if (GUILayout.Button("Enter Room : " + strRoomInfo))
                    {
                        MonobitNetwork.JoinRoom(room.name);
                    }
                }
            }
        }
    }
}
