using Modules.WavingFlag_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_WavingFlag : MonoBehaviour
{
    public LogicBase flagModule;
    public FlagMode mode = FlagMode.GPU;

    IWavingFlag iflagModule;

    // *****************************
    // Start
    // *****************************
    void Start()
    {
        iflagModule = flagModule as IWavingFlag;
        BuildFlag();
    }

    // *****************************
    // Update
    // *****************************
    void Update()
    {
        ErrorOnNullModule();
        iflagModule.OnUpdate();
    }

    // *****************************
    // ErrorOnNullModule
    // *****************************
    void ErrorOnNullModule()
    {
        if (iflagModule == null)
        {
            throw new System.Exception(" flagModule is NULL or does not implement 'IWavingFlag'!");
        }
    }

    // *****************************
    // BuildFlag
    // *****************************
    void BuildFlag()
    {
        ErrorOnNullModule();
        iflagModule.InitModule(null);
        iflagModule.SetMode(mode);
    }
}
