using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public interface IWall
{
    public VisualEffect Effect { get; set; }
    public GameObject EffectGameObjectPrefab { get; set; }
}
