using System;
using Modules.DAL.Abstract.Data;

namespace GameCore.Source.Domain.Data.Dto
{
    [Serializable]
    public class CurrencyData : IEntity
    {
        public int Gold;

        private static string s_id = nameof(CurrencyData);

        public string Id => s_id;

        public object Clone() =>
            new CurrencyData()
            {
                Gold = Gold
            };

        public bool Equals(IEntity other) =>
            Id == other?.Id;
    }
}