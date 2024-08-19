using Modules.WavingFlag_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.WavingFlag
{
    public static class CompSetMode
    {
        // *****************************
        // SetMode
        // *****************************
        public static void SetMode(StateWavingFlag _state, FlagMode _mode)
        {
            CompException.ExeptionIfNotinitialised(_state);
            _state.dynamicData.mode = _mode;
            CompMeshBuilder.BuildAndAssignMesh(_state);
        }
    }
}