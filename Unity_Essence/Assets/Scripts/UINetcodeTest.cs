using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class UINetcodeTest : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;

    NetworkVariable<int> ScoreValue = new NetworkVariable<int>(10,
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server);

    public Action<int> OnScoreCall; // �Լ��� ������ �� �ִ� ����.
    //public Func<int, int> MyFunc; // ù��° Ÿ�Ժ��� �Ű�����, ������ �Ű������� ���ϰ�
    //public Func<int, string> MyFunc2;
    //public void Func() { }
    //public void Func2(int val) { }
    //public int Func3(int val2) { return 3; }
    //public int Func4(int val2, int val3) { return 3; }
    //public string Func5(int val) { return ""; }

    // ��Ʈ��ũ �ڵ�� �������� �����ϴ���, Ŭ���̾�Ʈ���� �����ؾ� �ϴ� �� �����ؾ���
    // ���ǹ��� ����ؼ� ��������, ������ �ƴ��� �����ϰ� �� ���¿� �´� �Լ��� �����ϴ� ������ �ڵ� ����

    public override void OnNetworkSpawn()
    {
        if (IsServer) 
        {
            OnScoreCall += HandleAddPoint;
            //MyFunc2 + ScoreValue.Value += 10; // Ư���� ���������� �����ϰ� �ʹ�.
        }

        ScoreValue.OnValueChanged += OnScoreValueChanged;
    }

    private void HandleAddPoint(int value)
    {
        ScoreValue.Value += value;
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer) 
        { 
            OnScoreCall -= HandleAddPoint;
        }

        ScoreValue.OnValueChanged -= OnScoreValueChanged;
    }

    private void OnScoreValueChanged(int previousValue, int newValue)
    {
        // ��� Client�� �����ؾ� �ϴ� �ڵ�

        scoreText.SetText($"score : {newValue}");
    }

   

}
