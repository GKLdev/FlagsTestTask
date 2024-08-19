using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.WavingFlag_Public
{
    public interface IWavingFlag : IModuleInit, IModuleUpdate
    {
        /// <summary>
        /// Rebuilds or mesh using current config values
        /// </summary>
        public void RebuildFlagMesh();

        /// <summary>
        /// Define mesh size in vertices
        /// auto rebuilds mesh afterwards
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        public void SetSize(int _x, int _y);

        public void SetMode(FlagMode _mode);
    }

    // *****************************
    // FlagMode
    // *****************************
    public enum FlagMode
    {
        Undef = -1,
        CPU,
        GPU
    }
}