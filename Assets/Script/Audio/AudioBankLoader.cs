using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBankLoader : MonoBehaviour
{
    [FMODUnity.BankRef]
    public string bank;
    private void Awake()
    {
        FMODUnity.RuntimeManager.LoadBank(bank);
    }
}
