using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Character/AttributeInfo")]
[System.Serializable]
public class TDCharacterAttribute : ScriptableObject
{
    [Header("---Basic---")]
    public float maxHP = 100;
    public float maxAP = 100;
    public float def;
    public float healrate;
    public float moveSpeed;

    [Header("---Damager---")]
    public float originalPlayerDamage;
    public float weaponDamageAdder;
    public List<float> buffDamageAdders;
    public float criticalDamageMultipler;
    public float flashDamageMultipler;
    public float finalWeaponDamage;
    public float finalFlashDamage;
    public LayerMask damageMask = 1 << 9;

    [Header("---Heal---")]
    public float healMultipler;
}
