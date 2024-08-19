//using Modules.ModuleMng_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntry : MonoBehaviour
{
    public LogicBase modules;

    // *****************************
    // Start
    // *****************************
    private void Start()
    {
        /*
        var     moduleMng   = (modules as IModuleManager);
        bool    error       = moduleMng == null;
        if (error)
        {
            throw new System.Exception("Modules manager not assigned or does not inplement IModuleManager");
        }

        moduleMng.InitModule(null);
        */
    }
}
