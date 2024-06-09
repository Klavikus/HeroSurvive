using System;
using Modules.DAL.Abstract.Data;

namespace GameCore.Source.Domain.Data.Dto
{
    [Serializable]
    public class CurrencyDto : IEntity
    {
        public int Gold;

        public static string DefaultId = nameof(CurrencyDto);

        public string Id => DefaultId;

        public object Clone() =>
            new CurrencyDto()
            {
                Gold = Gold
            };

        public bool Equals(IEntity other) =>
            Id == other?.Id;
    }
}