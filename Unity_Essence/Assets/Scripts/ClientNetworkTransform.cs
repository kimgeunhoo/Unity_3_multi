using UnityEngine;
using Unity.Netcode.Components;

/// <summary>
/// �� Ŭ������ ������ ��Ʈ��ũ ������Ʈ�� Ŭ���̾�Ʈ�� ���ؼ��� �̵��� �� �ִ�.
/// </summary>
public class ClientNetworkTransform : NetworkTransform
{
    protected override bool OnIsServerAuthoritative()
    {
         return false;
    }
}
