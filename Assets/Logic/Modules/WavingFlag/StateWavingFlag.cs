using Modules.WavingFlag_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.WavingFlag
{
    public class StateWavingFlag : LogicBase
    {
        [Header("Resources")]
        public WavingFlagConfig     config;
        public Transform            flagPivot;
        public Material             flagMat;
        public MeshFilter           meshFilter;
        public MeshRenderer         meshRenderer;

        [Header("Dynamic Data")]
        public DynamicData dynamicData = new DynamicData();

        // *****************************
        // DynamicData
        // *****************************
        //[System.Serializable]
        public class DynamicData
        {
            public bool initialised = false;

            public Vector3[]    verts;
            public Mesh         dynamicMesh;
            
            public FlagMode     mode = FlagMode.Undef;


            public VertexData[] vertexDataStatic;
            public int vSizeX;
            public int vSizeY;
        }
    }
}