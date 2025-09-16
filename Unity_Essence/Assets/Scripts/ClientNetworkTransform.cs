using UnityEngine;
using Unity.Netcode.Components;

/// <summary>
/// 이 클래스를 부착한 네트워크 오브젝트는 클라이언트를 통해서만 이동할 수 있다.
/// </summary>
public class ClientNetworkTransform : NetworkTransform
{
    protected override bool OnIsServerAuthoritative()
    {
         return false;
    }
}
