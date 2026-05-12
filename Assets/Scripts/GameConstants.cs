using UnityEngine;

public static class GameConfig
{
    // Player settings
    public const float PlayerStrafeSpeed = 3f;
    public const int PlayerInitialLives = 3;
    public const int PlayerInitialHealth = 100;
    public const float PlayerXAxisLimit = 5f;
    public const float PlayerCollisionResetY = 0f;
    public const float PlayerForwardSpeedMultiplier = 4f / 3f;
    public const float PlayerDefaultSpeedMultiplier = 3f / 4f;
    public const float PowerupEffectDuration = 1.5f;
    public const float BarrelEffectDuration = 2f;
    public const float BarrelRotationFactor = 360f;

    // Environment settings
    public const float EnvironmentMoveSpeed = 1.5f;
    public const float EnvironmentResetPositionZ = 15f;
    public const float EnvironmentFloorThresholdZ = -9f;
    public const float EnvironmentWallThresholdZ = -15f;

    // Spawn settings
    public const float SpawnZ = 14f;
    public const float SpawnXRange = 5f;
    public const float SpawnEnemyY = 0.65f;
    public const float SpawnPowerupY = 1f;
    public const float SpawnEnemyTime = 0.75f;
    public const float SpawnPowerupTime = 10f;
    public const float SpawnStartDelay = 1f;
    public const float MinEnemySpawnTime = 0.2f;
    public const float SpawnTimeAdjustment = -0.1f;

    // Barrel movement
    public const float MoveDownDestroyZ = -10f;
    public const float PowerupDestroyZ = -10f;
    public const float GravityForce = 50f;
}

public static class InputConfig
{
    public static readonly KeyCode MoveForward = KeyCode.UpArrow;
    public static readonly KeyCode MoveLeft = KeyCode.LeftArrow;
    public static readonly KeyCode MoveRight = KeyCode.RightArrow;
}

public static class TagNames
{
    public const string Player = "Player";
    public const string Ground = "Ground";
    public const string Powerup = "Powerup";
    public const string Power1 = "Power1";
    public const string RustyBarrel = "RustyBarrel";
    public const string GasBarrel = "GasBarrel";
    public const string ToxicBarrel = "ToxicBarrel";
    public const string StripedBarrel = "StripedBarrel";
}

public static class AnimatorHashes
{
    public static readonly int Speed = Animator.StringToHash("Speed");
}
