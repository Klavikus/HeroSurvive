using UnityEngine;

namespace CodeBase.GameCore.Configs
{
    public static class GameConstants
    {
        public const string GameLoopScene = "GameLoop";
        public const string MainMenuScene = "MainMenu";
        public const string UpgradeModelPrefix = "Upgrade";
        public const string CurrencyDataKey = "Currency";
        public const string UserNameDataKey = "UserName";
        public const string BaseUserName = "Stranger";
        public const string StageTotalKillsLeaderBoardKey = "StageCompleted";
        public const string MusicVolume = "MusicVolume";

        public const int MaxNameLength = 15;
        public const float LeaderBoardPassiveUpdateDelay = 60f;
        public const float AIMinimumStaggerDelay = 0.01f;
        public const string InitialSceneName = "Initial";
        public static float RegenerationDelay = 1f;
        public static float DirectionTrackingDelay = 0.35f;
        public static float MinimumStopDistance = 0.2f;
        public static string BaseLanguage = "en";
        public static Vector4 SpawnRectangleRestriction = new Vector4(-61, 61, -61, 61);
        public static float RespawnInvincibleDuration = 2f;
        public static float BaseMusicVolume = 0.5f;
        public static bool MuteStatus = false;
    }
}