using UnityEngine;
using Unity.Netcode.Components;

// server�� ��������� ������ �� ������ �Ѵ�.

public class ClinetNetworkAnimator : NetworkAnimator
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
