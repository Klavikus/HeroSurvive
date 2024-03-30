using System;
using GameCore.Source.Domain.Enums;
using UnityEngine;

namespace GameCore.Source.Domain.Data
{
    [Serializable]
    public class TranslatableString
    {
        [field: SerializeField] public Language Language { get; private set; }
        [field: SerializeField] public string Text { get; private set; }

        public TranslatableString(Language language, string text)
        {
            Language = language;
            Text = text;
        }

        public override string ToString() =>
            $"{Language} {Text}";
    }
}