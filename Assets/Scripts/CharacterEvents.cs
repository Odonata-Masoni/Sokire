using System;
using UnityEngine;

public static class CharacterEvents
{
    public static Action<GameObject, float> characterDamaged;
    public static Action<GameObject, float> characterHealed;
}
