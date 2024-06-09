using System;
using GameCore.Source.Domain.Data.Dto;

namespace GameCore.Source.Domain.Models
{
    public class AccountModel
    {
        public event Action<int> TotalWavesClearChanged;

        public AccountModel(AccountDto accountDto)
        {
            TotalWavesCleared = accountDto.TotalWavesCleared;
            TotalRunCompleted = accountDto.TotalRunCompleted;
        }

        public int TotalWavesCleared { get; private set; }
        public int TotalRunCompleted { get; private set; }

        public void HandleWaveClearing()
        {
            TotalWavesCleared++;
            TotalWavesClearChanged?.Invoke(TotalWavesCleared);
        }
    }
}