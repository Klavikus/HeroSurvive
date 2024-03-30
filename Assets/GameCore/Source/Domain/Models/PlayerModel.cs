using GameCore.Source.Domain.Abilities;

namespace GameCore.Source.Domain.Models
{
    public class PlayerModel
    {
        public bool IsFreeSlotAvailable { get; set; }
        public AbilityContainer AbilityContainer { get; set; }
    }
}