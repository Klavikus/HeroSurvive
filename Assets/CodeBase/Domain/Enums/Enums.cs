namespace CodeBase.Domain.Enums
{
    public enum BaseProperty
    {
        MaxHealth,
        HealthRegen,
        MoveSpeed,
        Damage,
        Cooldown,
        Duration,
        Amount,
        Area,
        ProjectileSpeed,
    }
    
    public enum PlayerStat
    {
        ProjectilesCountModifier,
        DamageModifier,
        SpeedModifier,
        SizeModifier,
        DurationModifier,
        CooldownModifier,
    }

    public enum EnemyType
    {
        Ghost,
        Skeleton,
        Hound,
        Imp,
        Bird,
        Bear,
        FireKnight,
        Dragon,
        Dwarf,
        Orc
    }

    public enum MoveState
    {
        Idle,
        Run,
    }
    
    public enum AttackType
    {
        Continuous,
        Periodical,
        Single,
    }

    public enum MoveType
    {
        MoveUp,
        Orbital,
    }

    public enum SpawnType
    {
        Point,
        Circle,
        Arc
    }

    public enum TargetingType
    {
        ByDirection,
        ToClosest,
        RandomTarget
    }
}