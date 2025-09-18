using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class UINextStage : NetworkBehaviour
{
    [Header("Next Stage")]
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private int remainCount = 3;
    [SerializeField] private string loadSceneName = "Game2";

    private void Start()
    {
        uiPanel.SetActive(false);
    }

    public override void OnNetworkSpawn()
    {
        if(!IsServer) { return; }
        Bus<IStageClearEvent>.OnEvent += HandleStageClear;
    }
    public override void OnNetworkDespawn()
    {
        if (!IsServer) { return; }
        Bus<IStageClearEvent>.OnEvent -= HandleStageClear;
    }

    private void HandleStageClear(IStageClearEvent evt)
    {
        StageClearClientRpc();
    }

    [Rpc(SendTo.Everyone)]
    private void StageClearClientRpc()
    {
        uiPanel.SetActive(true);
        StartCoroutine(UICountDown());
    }

    IEnumerator UICountDown()
    {
        int loopCount = remainCount;
        for (int i = 0; i < loopCount; i++)
        {
            countDownText.SetText(remainCount.ToString());
            remainCount--;
            yield return new WaitForSeconds(1);
        }

        NetworkManager.SceneManager.LoadScene(loadSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
