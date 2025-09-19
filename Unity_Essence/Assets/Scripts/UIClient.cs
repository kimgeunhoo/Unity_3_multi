using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIClient : NetworkBehaviour
{
    private UINetcodeTest netcodeTest;

    // A��ǻ��, �ڱ� ��ǻ�Ϳ��� ������ ������Ʈ�� ����
    // B��ǻ��, B��ǻ�Ϳ��� ������ ������Ʈ�� ����
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

        // fkey ������ ������? ������ �ƴ�
        // Bus<IScoreEvent>.Raise
        // ���Ͱ� óġ�Ǿ��� �� ������ ������.

        if (Keyboard.current.fKey.isPressed)
        {
            // ���� Host ����, Client ����
            AddPointServerRpc(); // ?. : null�� �ƴҶ��� ����
        }
    }

    /*
        Rpc : Remote Procedual Call
        ��Ʈ��ũ�� ����Ǿ� �ִ� ��ǻ�Ͱ� �ٸ� ��ǻ��, �������� �����϶� ��� ����� �ϱ� ���� ����
        ������ ���� ������ �� �־�� �Ѵ�. ������ �ƴ� ��ǻ�Ϳ��� ���� �����ϰ� ���� ��쿡
        �������� ���, �ٸ� ��ǻ�Ϳ� �ִ� �Լ��� ȣ���� �� �ִ� ���
        RequireOwnership = false : ������ ȣ���� �� �ִ� �Լ�
        Owner�� �ִ� ����� ȣ���Ѵ�.
     */
    [Rpc(SendTo.Server, RequireOwnership = false)]
    private void AddPointServerRpc()
    {
        netcodeTest.OnScoreCall?.Invoke(1);
    }
}
