using Fusion;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/New Tag", order = 3)]
public class TagScriptableObject : ScriptableObject
{
    [Header("Tag type")]
    public BuildingsScriptableObject.BuildingType TagType;

    [Header("Tag Sprite")]
    [SerializeField]
    public Sprite Sprite;

}