using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

// NetWork ���� ������ ������ ����߳�
// ��Ʈ��ũ���� Spawn �Ǿ��� �� ����
// ������ ��ư�� Host�϶��� Ȱ��ȭ�ǵ��� �ڵ� �ۼ�

public class HostButton : NetworkBehaviour
{
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }  

    

}
