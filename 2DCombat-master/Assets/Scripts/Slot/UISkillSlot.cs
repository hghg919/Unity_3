using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillSlot : UISlot
{
    public Action OnSkillUnlock;

    private bool unlocked;
    public bool Unlocked => unlocked;
    [Tooltip("shouldBeUnlocked�� ��� true���� �ر��� �����ϴ�.")]
    [SerializeField] private UISkillSlot[] shouldBeUnlocked;
    [SerializeField] private UISkillSlot[] shouldBelocked;

    [SerializeField] private Item item;
    private Button _button;

    private void Start()
    {
        itemImage = GetComponent<Image>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(UnlockSkillSlot);

        if(!unlocked)
            itemImage.color = Color.red;
    }

    private void OnValidate()
    {
        gameObject.name = $"SkillTreeSlot_UI - {item.itemName}";
    }

    private void UnlockSkillSlot()
    {
        if (unlocked) return;
        // ShouldBeUnlocked �ϳ��� lock�̸� return
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if(shouldBeUnlocked[i].Unlocked == false)
            {
                return;
            }
        }

        // ShouldBelocked �ϳ��� unlock�̸� return
        for (int i = 0; i < shouldBelocked.Length; i++)
        {
            if (shouldBelocked[i].Unlocked == true)
            {
                return;
            }
        }

        unlocked = true;
        itemImage.color = Color.white;
        OnSkillUnlock?.Invoke();
    }
}
