using System;
using UnityEngine;

namespace GameCore.Source.Domain.Data
{
    [Serializable]
    public struct LevelViewData
    {
        public string Title;
        public int Id;
        public int Order;
        public Sprite Icon;
        public StageVariantData[] StageVariantsData;

        public Level ToModel() => new Level(Id, StageVariantsData);
    }
}