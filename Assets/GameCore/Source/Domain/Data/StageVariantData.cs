using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Source.Domain.Data
{
    [Serializable]
    public struct StageVariantData
    {
        [FormerlySerializedAs("Sprite")] public Sprite Icon;
        public float SizeMultiplayer;
        public SlicerStageInfo slicerStageInfo;
    }
}