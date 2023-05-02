using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelObjectType
{
    Monster,
    MonsterPatrol,
    Wall,
    Fire,
    Coin,
    ArtifactGood,
    ArtifactBad,
    GateGL,
    GateBL,
    Gate2G,
    TutorialDetector
}

public enum ChunkType
{
    Simple,
    Hill,
    BridgeLeft,
    BridgeRight,
    RampLeft,
    RampRight,
    HoleLeft,
    HoleRight
}
[Serializable]
public class LevelObject
{
    public LevelObjectType type;
    public Vector2 position;
}

[Serializable]
public class ChunkTemplate
{
    public LevelObject[] objects;
    public ChunkType chunkType;
}
[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/LevelScriptableObject", order = 1)]
public class LevelTemplate : ScriptableObject
{
    public ChunkTemplate[] chunks;
}
