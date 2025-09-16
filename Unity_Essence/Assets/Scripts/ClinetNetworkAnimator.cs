using UnityEngine;
using Unity.Netcode.Components;

// server가 변경사항이 생겼을 때 수정을 한다.

public class ClinetNetworkAnimator : NetworkAnimator
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
