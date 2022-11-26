using System;
using CodeBase.Configs;
using UnityEngine;

namespace CodeBase.MVVM.Models
{
    [Serializable]
    public class UserModel
    {
        [field: SerializeField] public String Name { get; private set; }

        public event Action<string> NameChanged;

        public UserModel() => Name = GameConstants.BaseUserName;

        public void SetName(string newName)
        {
            if (string.IsNullOrEmpty(newName))
                throw new ArgumentException($"Incorrect {nameof(newName)} handled value: \'{newName}\'");

            Name = newName;
            NameChanged?.Invoke(Name);
        }
    }
}