using Modules.DAL.Abstract.Data;

namespace GameCore.Source.Domain.Data.Dto
{
    public class AccountDto : IEntity
    {
        public static string DefaultId = nameof(AccountDto);

        public int TotalRunCompleted;
        public int TotalWavesCleared;

        public object Clone()
        {
            return new AccountDto()
            {
                TotalRunCompleted = TotalRunCompleted,
                TotalWavesCleared = TotalWavesCleared,
            };
        }

        public bool Equals(IEntity other) =>
            other?.Id == Id;

        public string Id => DefaultId;
    }
}