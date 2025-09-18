using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

// NetWork 연동 했을때 무엇을 상속했나
// 네트워크에서 Spawn 되었을 때 조건
// 참조한 버튼을 Host일때만 활성화되도록 코드 작성

public class HostButton : NetworkBehaviour
{
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public override void OnNetworkSpawn()
    {
        if(IsHost) // Host 일때만 버튼 활성화
        {
            button.gameObject.SetActive(true);
        }
        else
        {
            button.gameObject.SetActive(false);
        }
    }

    public override void OnNetworkDespawn()
    {

    }

}
