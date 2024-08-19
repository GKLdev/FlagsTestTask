using Modules.WavingFlag_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.WavingFlag
{
    public static class CompOnUpdate
    {
        // *****************************
        // OnUpdate
        // *****************************
        public static void OnUpdate(StateWavingFlag _state)
        {
            CompException.ExeptionIfNotinitialised(_state);
            CompException.ExceptionIfUndefMode(_state);

            bool isCPUMode = _state.dynamicData.mode == FlagMode.CPU;
            if (isCPUMode)
            {
                CompMeshAnimCPU.OnUpdate(_state);
            }
        }

    }
}