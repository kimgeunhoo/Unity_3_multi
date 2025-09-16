using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIClient : NetworkBehaviour
{
    private UINetcodeTest netcodeTest;

    // A컴퓨터, 자기 컴퓨터에서 생성된 오브젝트를 참조
    // B컴퓨터, B컴퓨터에서 생성된 오브젝트를 참조
    private void Awake()
    {
        netcodeTest = FindAnyObjectByType<UINetcodeTest>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) { return; }
    }
    public override void OnNetworkDespawn()
    {
        if (!IsOwner) { return; }
    }

    private void Update()
    {
        if (!IsOwner) { return; }

        // fkey 점수가 오른다? 실제론 아님
        // Bus<IScoreEvent>.Raise
        // 몬스터가 처치되었을 때 점수가 오른다.

        if (Keyboard.current.fKey.isPressed)
        {
            // 서버 Host 실행, Client 실행
            AddPointServerRpc(); // ?. : null이 아닐때만 실행
        }
    }

    /*
        Rpc : Remote Procedual Call
        네트워크에 연결되어 있는 컴퓨터가 다른 컴퓨터, 서버에서 실행하라 라는 명령을 하기 위한 문법
        서버만 값을 변경할 수 있어야 한다. 서버가 아닌 컴퓨터에서 값을 변경하고 싶은 경우에
        서버에게 명령, 다른 컴퓨터에 있는 함수를 호출할 수 있는 기능
        RequireOwnership = false : 누구나 호출할 수 있는 함수
        Owner가 있는 사람만 호출한다.
     */
    [Rpc(SendTo.Server, RequireOwnership = false)]
    private void AddPointServerRpc()
    {
        netcodeTest.OnScoreCall?.Invoke(1);
    }
}
