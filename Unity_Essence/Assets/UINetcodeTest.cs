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

    public Action<int> OnScoreCall; // 함수를 저장할 수 있는 변수.
    //public Func<int, int> MyFunc; // 첫번째 타입부터 매개변수, 마지막 매개변수가 리턴값
    //public Func<int, string> MyFunc2;
    //public void Func() { }
    //public void Func2(int val) { }
    //public int Func3(int val2) { return 3; }
    //public int Func4(int val2, int val3) { return 3; }
    //public string Func5(int val) { return ""; }

    // 네트워크 코드는 서버에서 실행하는지, 클라이언트에서 실행해야 하는 지 구분해야함
    // 조건문을 사용해서 서버인지, 서버가 아닌지 구분하고 각 상태에 맞는 함수를 연결하는 식으로 코드 구현

    public override void OnNetworkSpawn()
    {
        if (IsServer) 
        {
            OnScoreCall += HandleAddPoint;
            //MyFunc2 + ScoreValue.Value += 10; // 특별한 시점에서만 실행하고 싶다.
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
        // 모든 Client가 실행해야 하는 코드

        scoreText.SetText($"score : {newValue}");
    }

   

}
