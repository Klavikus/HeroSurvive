namespace CodeBase.Domain
{
    public enum Language
    {
        en,
        ru,
        tr,
    }
    public enum LeanTranslationPath
    {
        MainMenu_Start,
        MainMenu_Upgrades,
        MainMenu_Leaderboard,
        MainMenu_Options,
        
        Common_Back,
        Common_Continue,
        Common_Apply,
        Common_Reset,
        Common_Buy,
        Common_MaxLevel,
        Common_BaseAbility,
        
        Leaderboard_TopPlayers,
        
        Upgrades_Health,
        Upgrades_Regeneration,
        Upgrades_MoveSpeed,
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