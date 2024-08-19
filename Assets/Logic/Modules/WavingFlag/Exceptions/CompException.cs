using Modules.WavingFlag_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.WavingFlag
{
    public static class CompException
    {
        // *****************************
        // ExeptionIfNotinitialised
        // *****************************
        public static void ExeptionIfNotinitialised(StateWavingFlag _state)
        {
            if (!_state.dynamicData.initialised)
            {
                throw new System.Exception("Module WavingFlag is MUST be initialied before usage!");
            }
        }

        // *****************************
        // ExceptionIfUndefMode
        // *****************************
        public static void ExceptionIfUndefMode(StateWavingFlag _state)
        {
            if (_state.dynamicData.mode == FlagMode.Undef)
            {
                throw new System.Exception("FlagMode MUST be defined! Now Undef");
            }
        }
    }
}