using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.WavingFlag
{
    public static class CompMeshAnimCPU
    {
        // *****************************
        // OnUpdate
        // *****************************
        public static void OnUpdate(StateWavingFlag _state)
        {
            CompException.ExeptionIfNotinitialised(_state);

            AnimateTexture(_state, -_state.config.p_txtTranslationSpdHor, _state.config.p_txtTranslationSpdVert, _state.config.p_texNameMain);
            AnimateTexture(_state, -_state.config.p_waveSpdHor, _state.config.p_waveSpdVert, _state.config.p_texNameHeightMap);
            AnimateMesh(_state);
        }


        // *****************************
        // AnimateTexture
        // *****************************
        static void AnimateTexture(StateWavingFlag _state, float _spdHor, float _spdVert, string _texName)
        {
            float horFactor     = _spdHor * Time.deltaTime;
            float vertFactor    = _spdVert * Time.deltaTime;

            var currentOffset   = _state.flagMat.GetTextureOffset(_texName);
            currentOffset       += new Vector2(horFactor, vertFactor);

           _state.flagMat.SetTextureOffset(_texName, currentOffset);
        }

        // *****************************
        // AnimateMesh
        // *****************************
        static void AnimateMesh(StateWavingFlag _state)
        {
            // prepare data //
            var     mesh            = _state.dynamicData.dynamicMesh;
            var     vData           = _state.dynamicData.vertexDataStatic;
            Vector2 heightmapOffset = _state.flagMat.GetTextureOffset(_state.config.p_texNameHeightMap);

            var appData = new AppData() {
                mapOffset           = heightmapOffset,
                offsetFactor        = _state.config.p_waveHeightFactor,
                pinThreshold        = _state.config.p_pinThresholdPercent,
                pinSmoothless       = _state.config.p_pinThresholdSmoothless,
                cpuWaveAmplitude    = _state.config.p_waveAmplitudeCPU
            };

            // make calculations //
            for (int v = 0; v < vData.Length; v++)
            {
               ProcessVertex(ref vData[v], ref appData);
            }

            // apply to mesh //
            var verts = mesh.vertices;
            for (int i = 0; i < mesh.vertexCount; i++)
            {
                verts[i]  = vData[i].posOut;
            }

            mesh.vertices = verts;
        }

        // *****************************
        // ProcessVertex
        // *****************************
        static void ProcessVertex(ref VertexData _input, ref AppData _appData) {

            float waveMovementH = _appData.mapOffset.x * 10f;
            float waveMovementV = _appData.mapOffset.y * 10f;
            float functionX         = _input.uv.x * _appData.cpuWaveAmplitude.x + waveMovementH;
            float functionY         = _input.uv.y * _appData.cpuWaveAmplitude.y + waveMovementV;

            float wavesCombined = functionX + -functionY;
            float offset        = Mathf.Sin(wavesCombined) * _appData.offsetFactor;

            ApplyPosAndPin(ref _input, ref _appData, offset);
        }

        // *****************************
        // ApplyPosAndPin
        // *****************************
        static void ApplyPosAndPin(ref VertexData _input, ref AppData _appData, float _displaceFactor)
        {
            // this logic copied the same piece from the shader //
            float pinThresholdNormalized    = _appData.pinThreshold / 100f;
            bool  pinEnabled                 = pinThresholdNormalized > 0f;
            bool  shouldDisplaceVetex        = !pinEnabled || pinEnabled && _input.uv.x > pinThresholdNormalized;

            // pin or displace //
            if (shouldDisplaceVetex)
            {
                float pinInfluenceFactor = 0f; // 1.0 means zero effect on offset, 0.0 - maximum possible effect i. e. no offset, 1-0 will be values which are decreases offset

                // make calculations //
                float   texcoordNormalised  = _input.uv.x - pinThresholdNormalized;
                bool    ignoreSmoothing     = !pinEnabled || _appData.pinSmoothless < 0f;

                float smoothFactor = _appData.pinSmoothless < 0.001f ? 1.0f : Mathf.Clamp(texcoordNormalised / _appData.pinSmoothless, 0f, 1f);
                pinInfluenceFactor = ignoreSmoothing ? 1f : smoothFactor;

                // displace //
                _input.posOut = _input.pos + _input.normal * _displaceFactor * pinInfluenceFactor;
            } else
            {
                _input.posOut = _input.pos;
            }
        }

        // *****************************
        // AppData
        // *****************************
        public struct AppData
        {
            public Vector2      mapOffset;
            public Vector2      cpuWaveAmplitude;
            public float offsetFactor;
            public float pinThreshold;
            public float pinSmoothless;
        }
    }


    // *****************************
    // VertexData
    // *****************************
    public struct VertexData
    {
        public Vector3 pos;
        public Vector2 uv;
        public Vector3 normal;

        public Vector3 posOut;
        //public Vector2 uvOut;
        //public Vector3 normalOut;
    }
}