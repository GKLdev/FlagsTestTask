using Modules.WavingFlag_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.WavingFlag
{
    public static class CompMeshBuilder
    {
        // *****************************
        // BuildAndAssignMesh
        // *****************************
        public static void BuildAndAssignMesh(StateWavingFlag _state)
        {
            CompException.ExeptionIfNotinitialised(_state);
            CompException.ExceptionIfUndefMode(_state);

            var interData       = PrepareInterData(_state);
            var generatedMesh   = ConstructMesh(_state, interData);

            BuildVertexData(_state, generatedMesh);
            AssignMesh(_state, generatedMesh);
        }

        // *****************************
        // PrepareInterData
        // *****************************
        static IntermediateData PrepareInterData(StateWavingFlag _state)
        {
            // mesh size in verts //
            int meshSizeX = _state.dynamicData.vSizeX;
            int meshSizeY = _state.dynamicData.vSizeY;

            bool error = meshSizeX <= 1 || meshSizeY <= 1;
            if (error)
            {
                throw new System.Exception(" 'meshX' or 'meshY' config parameters are invalid!");
            }

            // world size //
            Vector2 wspaceSize = _state.config.p_wSpaceSize;

            bool meshSizeTooSmall = Mathf.Approximately(wspaceSize.x, 0f) || Mathf.Approximately(wspaceSize.y, 0f);
            if (meshSizeTooSmall)
            {
                throw new System.Exception("Mesh should not be zero sized! Please setup 'wspaceSize' properly. ");
            }

            // verts count //
            int vertsCount = (meshSizeY) * meshSizeX * 6; // 6 is here because we are crateing a quad for each X vert which is 6 verts

            bool vertsCountError = vertsCount > _state.config.p_vertsLimit;
            if (vertsCountError)
            {
                throw new System.Exception($"Vertex limit of {_state.config.p_vertsLimit} exceeded! Current vertex count is: {vertsCount} \n Please, consider decreasing verts count at config.");
            }

            Vector2 vertexStepWSpace = new Vector2(wspaceSize.x / (float)meshSizeX, wspaceSize.y / (float)meshSizeY);

            // intermediate data //
            IntermediateData interData = new IntermediateData()
            {
                vertexStepWSpace    = vertexStepWSpace,
                wSpaceSize          = wspaceSize,
                vertsCount          = vertsCount,
                currentVertexId     = 0,
                currentUVId         = 0,
                meshSizeX           = meshSizeX,
                meshSizeY           = meshSizeY
            };

            return interData;
        }

        // *****************************
        // VertexIntermediateData
        // *****************************
        class IntermediateData
        {
            public Vector2 vertexStepWSpace;
            public Vector2 wSpaceSize;
            public int vertsCount;
            public int currentVertexId;
            public int currentUVId;
            public int meshSizeX;
            public int meshSizeY;
        }


        // *****************************
        // ConstructMesh
        // *****************************
        static Mesh ConstructMesh(StateWavingFlag _state, IntermediateData _interData)
        {
            // create mesh //
            var mesh = _state.dynamicData.dynamicMesh = new Mesh();

            Vector3[]   verts   = new Vector3[_interData.vertsCount];
            Vector2[]   uvs     = new Vector2[_interData.vertsCount];
            int[]       tris    = new int[_interData.vertsCount];

            #region About
            // Here we're filling our array in clockwize manner to have triangles 

            // vertex packing:
            // 0 1
            // 2 3

            // 0 1 2 - upper triangle
            // 1 3 2 - lower triangle

            #endregion

            // temp data for mesh construction //
            int         quadVertsCount  = 4;
            Vector3[]   quadTris        = new Vector3[quadVertsCount];
            Vector3[]   quadUvs         = new Vector3[quadVertsCount];
            int[]       vertsPassOrder  = new int[] { 0, 1, 2, 1, 3, 2 };

            // construct verts //
            for (int y = 0; y <= _interData.meshSizeY - 1; y++)
            {
                for (int x = 0; x <= _interData.meshSizeX - 1; x++)
                {                  
                    
                    // Get verts and UVs //
                    for (int i = 0; i < quadVertsCount; i++)
                    {
                        int xId = (i + 1) % 2 == 0 ? x + 1 : x;
                        int yId = i >= 2 ? y + 1 : y;

                        quadTris[i] = GetVertexPosAndUV(xId, yId, _interData, out Vector2 uv);
                        quadUvs[i] = uv;
                    }

                    // Add Verts and UVs //
                    for (int i = 0; i < vertsPassOrder.Length; i++)
                    {
                        int posAtArray = _interData.currentVertexId + i;
                        int quadVertexId = vertsPassOrder[i];

                        verts[posAtArray] = quadTris[quadVertexId];
                        uvs[posAtArray] = quadUvs[quadVertexId];
                    }

                    _interData.currentVertexId += vertsPassOrder.Length;
                }
            }

            // construct triangles //
            for (int i = 0; i < _interData.vertsCount; i++)
            {
                tris[i] = i;
            }

            // form mesh //
            mesh.MarkDynamic();
            mesh.vertices       = verts;
            mesh.triangles      = tris;
            mesh.uv             = uvs;
            mesh.RecalculateNormals();

            return mesh;
        }


        // *****************************
        // AssignMesh
        // *****************************
        static void AssignMesh(StateWavingFlag _state, Mesh _mesh)
        {
            var mat = _state.flagMat;

            bool shaderBasedAnim = _state.dynamicData.mode == FlagMode.GPU;
            if (shaderBasedAnim)
            {
                mat.SetFloat(_state.config.p_propNameWaveHeight, _state.config.p_waveHeightFactor);
                mat.SetFloat(_state.config.p_propNameWaveHorSpd, _state.config.p_waveSpdHor);
                mat.SetFloat(_state.config.p_propNameWaveVertSpd, _state.config.p_waveSpdVert);
                mat.SetFloat(_state.config.p_propNameTextureHorSpd, _state.config.p_txtTranslationSpdHor);
                mat.SetFloat(_state.config.p_propNameTextureVertSpd, _state.config.p_txtTranslationSpdVert);
                mat.SetFloat(_state.config.p_propNamePinThreshold, _state.config.p_pinThresholdPercent);
                mat.SetFloat(_state.config.p_propNamePinSmoothless, _state.config.p_pinThresholdSmoothless);
            }

            // assign mesh //
            _state.meshFilter.mesh          = _mesh;
            _state.meshRenderer.material    = mat;

            _state.meshFilter.gameObject.transform.position = _state.flagPivot.transform.position;
        }

        // *****************************
        // GetVertexPos
        // *****************************
        static Vector3 GetVertexPosAndUV(int _offsetX, int _offsetY, IntermediateData _interData, out Vector2 _uv)
        {
            // position //
            float offsetHor     = _interData.vertexStepWSpace.x * (float)_offsetX;
            float offsetVert    = _interData.vertexStepWSpace.y * (float)_offsetY;

            Vector3 result = Vector3.right * offsetHor + -Vector3.up * offsetVert;

            // UV //
            _uv = new Vector2(offsetHor / _interData.wSpaceSize.x, (-offsetVert) / _interData.wSpaceSize.y);

            return result;
        }

        // *****************************
        // BuildVertexData
        // *****************************
        static void BuildVertexData(StateWavingFlag _state, Mesh _mesh)
        {
            int vCount = _mesh.vertexCount;

            _state.dynamicData.vertexDataStatic     = new VertexData[vCount];

            bool casheVertsData = _state.dynamicData.mode == FlagMode.CPU;
            if (!casheVertsData)
            {
                return;
            }

            // Creating vertexdata cashe for original mesh - this will allow "CPU shader" work fast in CPU mode
            // Cashing arrays first, otherwise we would have perfomance drop in this case
            var verts   = _mesh.vertices;
            var normals = _mesh.normals;
            var uvs     = _mesh.uv;

            for (int i = 0; i < vCount; i++)
            {
                VertexData data = new VertexData();
                data.pos        = verts[i];
                data.normal     = normals[i];
                data.uv         = uvs[i];

                data.posOut         = data.pos;
                //data.normalOut      = data.normal;
                //data.uvOut          = data.uv;

                _state.dynamicData.vertexDataStatic[i] = data;

            }
        }
    }
}