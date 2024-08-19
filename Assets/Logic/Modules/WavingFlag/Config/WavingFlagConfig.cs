using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.WavingFlag
{

    [CreateAssetMenu(fileName = "WavingFlagConfig", menuName = "ReferenceDb/Configs/WavingFlagConfig")]
    public class WavingFlagConfig : ScriptableObject
    {

        [Header("Size")]
        [SerializeField]
        int vertexSizeX;

        [SerializeField]
        int vertexSizeY;

        [SerializeField]
        Vector2 wSpaceSize;

        [Header("Waves")]
        [SerializeField]
        float waveSpdHor;

        [SerializeField]
        float waveSpdVert;

        [SerializeField]
        [Range(-2, 2)]
        float waveHeightFactor;

        [SerializeField]
        Vector2 waveAmplitudeCPU = new Vector2(8f, 8f);

        [Header("Texture")]
        [SerializeField]
        float textureTranslationSpdHor;

        [SerializeField]
        float textureTranslationSpdVert;
        
        [Header("Pin threshold")]
        [SerializeField]
        [Range(-0.05f, 100f)]
        float pinThresholdPercent;

        [SerializeField]
        [Range(-0.05f, 1f)]
        float pinThresholdSmoothless;

        [Header("Technical")]
        [SerializeField]
        int vertsLimit = 65535;

        [SerializeField]
        string propNameWaveHorSpd;

        [SerializeField]
        string propNameWaveVertSpd;

        [SerializeField]
        string propNameWaveHeight;

        [SerializeField]
        string propNameTextureHorSpd;

        [SerializeField]
        string propNameTextureVertSpd;

        [SerializeField]
        string propNamePinThreshold;

        [SerializeField]
        string propNamePinSmoothless;

        [SerializeField]
        string texNameHeightMap;

        [SerializeField]
        string texNameMain;

        public int      p_vertexSizeX      => vertexSizeX;
        public int      p_vertexSizeY      => vertexSizeY;
        public float    p_waveSpdHor        => waveSpdHor;
        public float    p_waveSpdVert       => waveSpdVert;
        public Vector2  p_wSpaceSize       => wSpaceSize;
        public float    p_waveHeightFactor => waveHeightFactor;
        public int      p_vertsLimit       => vertsLimit;

        public float    p_txtTranslationSpdHor     => textureTranslationSpdHor;
        public float    p_txtTranslationSpdVert    => textureTranslationSpdVert;
        public float    p_pinThresholdPercent      => pinThresholdPercent;
        public float    p_pinThresholdSmoothless   => pinThresholdSmoothless;
        public Vector2  p_waveAmplitudeCPU          => waveAmplitudeCPU;

        public string p_propNameWaveHorSpd  => propNameWaveHorSpd;
        public string p_propNameWaveVertSpd => propNameWaveVertSpd;
        public string p_propNameWaveHeight  => propNameWaveHeight;

        public string p_propNameTextureHorSpd   => propNameTextureHorSpd;
        public string p_propNameTextureVertSpd  => propNameTextureVertSpd;
        public string p_propNamePinThreshold    => propNamePinThreshold;
        public string p_propNamePinSmoothless   => propNamePinSmoothless;
        public string p_texNameHeightMap        => texNameHeightMap;
        public string p_texNameMain             => texNameMain;
        //texNameMain
    }
}