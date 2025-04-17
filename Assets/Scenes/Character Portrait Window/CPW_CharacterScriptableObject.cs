using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CPW_CharacterScriptableObject", menuName = "Scriptable Objects/CPW_CharacterScriptableObject")]
public class CPW_CharacterScriptableObject : ScriptableObject
{
    public event EventHandler OnLevelUpEvent;
    public event EventHandler OnExpChangeEvent;

    [SerializeField] private int level;
    [SerializeField] private string objName;
    [SerializeField] private float exp;
    

    [Header("需要初始化的屬性")]
    [SerializeField] private float maxExp;
    [SerializeField] private int hp;
    [SerializeField] private int maxHp;
    [SerializeField] private int attack;
    [SerializeField] private int defence;

    [Header("成長屬性")]
    [SerializeField] private int growHp;
    [SerializeField] private int growAttack;
    [SerializeField] private int growDefence;

    public Transform TF { get; set; }

    public void Initialize(int maxExp , int maxHp , int attack , int defence)
    {
        level = 1;
        exp = 0;
        this.maxExp = maxExp;
        this.maxHp = maxHp;
        hp = maxHp;
        
        this.attack = attack;
        this.defence = defence;

    }

    public string 名字
    {
        get => objName;
    }

    public int Level
    {
        get => level;
    }

    public int 生命
    {
        get => hp;
    }
    public int 攻擊
    {
        get => attack;
    }
    public int 防禦
    {
        get => defence;
    }
    public void AddExp(int exp)
    {
        this.exp += exp;
        OnExpChangeEvent?.Invoke(this, EventArgs.Empty);

        if (this.exp >= maxExp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level += 1;

        hp += growHp;
        maxHp += growHp;
        attack += growAttack;
        defence += growDefence;

        exp = 0;
        maxExp *= 1.1f;

        OnLevelUpEvent?.Invoke(this, EventArgs.Empty);
    }

    public float GetHPNormalized()
    {
        return (float)hp / maxHp;
    }

    public float GetEXPNormalized()
    {
        return exp / maxExp;
    }
}
