// The Game Events used across the Game.
// Anytime there is a need for a new event, it should be added here.

using System;
using UnityEngine;

public static class GameEventsHandler
{
    public static readonly LevelInitializeEvent LevelInitializeEvent = new LevelInitializeEvent();
    public static readonly GameStartEvent GameStartEvent = new GameStartEvent();
    public static readonly GameOverEvent GameOverEvent = new GameOverEvent();
    public static readonly PlayerProgressEvent PlayerProgressEvent = new PlayerProgressEvent();
    public static readonly PlayerFinishCrossedEvent PlayerFinishCrossedEvent = new PlayerFinishCrossedEvent();
    public static readonly CoinCollectEvent CoinCollectEvent = new CoinCollectEvent();
    public static readonly PlayerHealthUpdateEvent PlayerHealthUpdateEvent = new PlayerHealthUpdateEvent();
    public static readonly PlayerStageHealthUpdateEvent PlayerStageHealthUpdateEvent = new PlayerStageHealthUpdateEvent();
    public static readonly PlayerStageHealthRecalculateRatioEvent PlayerStageHealthRecalculateRatioEvent = new PlayerStageHealthRecalculateRatioEvent();
    public static readonly PlayerStageChangeEvent PlayerStageChangeEvent = new PlayerStageChangeEvent();
    public static readonly PlayerApplyEffectEvent PlayerApplyEffectEvent = new PlayerApplyEffectEvent();
    public static readonly PlayerKilledMonsterEvent PlayerKilledMonsterEvent = new PlayerKilledMonsterEvent();
    public static readonly FinisherPlayerInPosition FinisherPlayerInPosition = new FinisherPlayerInPosition();
    public static readonly SwordHolderEnableEvent SwordHolderEnableEvent = new SwordHolderEnableEvent();
    public static readonly FinisherEndEvent FinisherEndEvent = new FinisherEndEvent();
    public static readonly DebugCallEvent DebugCallEvent = new DebugCallEvent();
    public static readonly PlayerRageEvent PlayerRageEvent = new PlayerRageEvent();
    public static readonly FinisherPlayerSlashEvent FinisherPlayerSlashEvent = new FinisherPlayerSlashEvent();
    public static readonly FinisherPlayerActualHitEvent FinisherPlayerActualHitEvent = new FinisherPlayerActualHitEvent();
    public static readonly FinisherBossAttackEvent FinisherBossAttackEvent = new FinisherBossAttackEvent();
    public static readonly TutorialShowEvent TutorialShowEvent = new TutorialShowEvent();
    public static readonly BossHealthDepletedEvent BossHealthDepletedEvent = new BossHealthDepletedEvent();
    public static readonly TutorialToggleEvent TutorialToggleEvent = new TutorialToggleEvent();
    
}

public class GameEvent {}

public class GameStartEvent : GameEvent
{
}

public class GameOverEvent : GameEvent
{
    public bool IsWin;
}

public class PlayerProgressEvent : GameEvent
{
    
}

public class PlayerFinishCrossedEvent : GameEvent
{
    public Transform PlayerTransform;
}

public class CoinCollectEvent : GameEvent
{
}

public class PlayerHealthUpdateEvent : GameEvent
{
    public float StageHealth;
    public float StageHealthNormalized;
}

public class PlayerKilledMonsterEvent : GameEvent
{
    
}

public class PlayerStageChangeEvent : GameEvent
{
    public int Stage;
    public float SetStageHealth;
}

public class LevelInitializeEvent : GameEvent
{
    public int LevelLength;
}

public class PlayerApplyEffectEvent : GameEvent
{
    public InteractiveType Type;
}

public class PlayerStageHealthUpdateEvent : GameEvent
{
    public float Difference;
}

public class PlayerStageHealthRecalculateRatioEvent : GameEvent
{
    public float A;
    public float B;
}
public class FinisherPlayerInPosition : GameEvent
{
    public int Stage;
    public bool Rage;
}

public class SwordHolderEnableEvent : GameEvent
{
    public Transform SwordHolderTransform;
}

public class FinisherEndEvent : GameEvent
{
    
}

public class DebugCallEvent : GameEvent
{
    public float Speed;
    public float Strafe;
    public float Stage1;
    public float Stage2;
    public float Stage3;
}

public class PlayerRageEvent : GameEvent
{
}

public class FinisherPlayerSlashEvent : GameEvent
{
    
}

public class FinisherPlayerActualHitEvent : GameEvent
{
    
}
public class  FinisherBossAttackEvent : GameEvent{}

public class TutorialShowEvent : GameEvent
{
}

public class BossHealthDepletedEvent : GameEvent
{
    
}

public class TutorialToggleEvent : GameEvent
{
    public bool Toggle;
}



