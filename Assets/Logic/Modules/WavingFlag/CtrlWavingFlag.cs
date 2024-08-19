using Modules.WavingFlag_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.WavingFlag
{
    [RequireComponent(typeof(StateWavingFlag))]
    public class CtrlWavingFlag : LogicBase, IWavingFlag
    {
        [SerializeField]
        StateWavingFlag state;

        // *****************************
        // InitModule
        // *****************************
        public void InitModule(DependencyContainer _dep)
        {
            CompInit.OnInit(state);
        }

        // *****************************
        // OnUpdate
        // *****************************
        public void OnUpdate()
        {
            CompOnUpdate.OnUpdate(state);
        }

        // *****************************
        // RebuildFlagMesh
        // *****************************
        public void RebuildFlagMesh()
        {
            CompMeshBuilder.BuildAndAssignMesh(state);
        }

        // *****************************
        // SetMode
        // *****************************
        public void SetMode(FlagMode _mode)
        {
            CompSetMode.SetMode(state, _mode);
        }

        // *****************************
        // SetSize
        // *****************************
        public void SetSize(int _x, int _y)
        {
            CompSetSize.SetMeshSizeVerts(state, _x, _y, true);
        }
    }
}
