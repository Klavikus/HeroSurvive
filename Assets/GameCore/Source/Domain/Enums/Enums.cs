namespace GameCore.Source.Domain.Enums
{
    public enum Language
    {
        en,
        ru,
        tr,
    }

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

    public enum AttackType
    {
        Continuous,
        Periodical,
        Single,
    }

    public enum SizeType
    {
        Constant,
        OverLifetime,
        OverLifetimeFixed,
    }

    public enum MoveType
    {
        MoveUp,
        Orbital,
        OrbitalMovePoint,
        Follow
    }

    public enum SpawnType
    {
        Point,
        PivotPoint,
        Circle,
        Arc
    }

    public enum TargetingType
    {
        ByDirection,
        ToClosest,
        RandomTarget
    }

    public enum AudioSourceType
    {
        Main,
        Secondary,
    }
}