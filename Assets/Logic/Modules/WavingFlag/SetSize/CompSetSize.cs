using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.WavingFlag
{
    public static class CompSetSize
    {

        // *****************************
        // SetMeshSizeVerts
        // *****************************
        public static void SetMeshSizeVerts(StateWavingFlag _state, int _x, int _y, bool _forceRebuild)
        {
            CompException.ExeptionIfNotinitialised(_state);
            CompException.ExceptionIfUndefMode(_state);

            _state.dynamicData.vSizeX = _x;
            _state.dynamicData.vSizeY = _y;

            if (_forceRebuild)
            {
                CompMeshBuilder.BuildAndAssignMesh(_state);
            }
        }
    }
}
