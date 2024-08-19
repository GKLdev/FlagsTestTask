using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.WavingFlag
{
    public static class CompInit
    {
        // *****************************
        // OnInit
        // *****************************
        public static void OnInit(StateWavingFlag _state)
        {
            _state.flagMat  = GameObject.Instantiate(_state.flagMat);
            _state.config   = GameObject.Instantiate(_state.config);

            _state.dynamicData.vSizeX =_state.config.p_vertexSizeX;
            _state.dynamicData.vSizeY = _state.config.p_vertexSizeY;

            ResetMaterial(_state);

            _state.dynamicData.initialised = true;
        }

        // *****************************
        // ResetMaterial
        // *****************************
        static void ResetMaterial(StateWavingFlag _state)
        {
            var mat = _state.flagMat;

            mat.SetFloat(_state.config.p_propNameWaveHeight, 0);
            mat.SetFloat(_state.config.p_propNameWaveHorSpd, 0);
            mat.SetFloat(_state.config.p_propNameWaveVertSpd, 0);
            mat.SetFloat(_state.config.p_propNameTextureHorSpd, 0);
            mat.SetFloat(_state.config.p_propNameTextureVertSpd, 0);
            mat.SetFloat(_state.config.p_propNamePinThreshold, -0.05f);
            mat.SetFloat(_state.config.p_propNamePinSmoothless, -0.05f);
        }
    }
}