﻿using UnityEngine;

namespace CodeBase.Configs
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

        public const int MaxNameLength = 15;
        public const float LeaderBoardPassiveUpdateDelay = 60f;
        public const float AIMinimumStaggerDelay = 0.01f;
        public static float RegenerationDelay = 1f;
        public static float DirectionTrackingDelay = 0.35f;
        public static float MinimumStopDistance = 0.2f;
        public static string BaseLanguage = "en";
        public static Vector4 SpawnRectangleRestriction = new Vector4(-61, 61, -61, 61);
    }
}