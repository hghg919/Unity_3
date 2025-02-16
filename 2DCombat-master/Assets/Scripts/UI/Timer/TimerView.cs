using CustomUtility;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerView : MonoBehaviour
{
    [SerializeField] Image cooldownBG;
    [SerializeField] TextMeshProUGUI cooldownText;

    public float duration;
    private Tween cooldownTween;

    public virtual void Awake()
    {
        // �⺻ ���� Ű(ctrl) �Է����� �� �Լ��� ����
        InputUser.Instance.BaseAttack += AnimateCooldown;      
    }

    public void AnimateCooldown()
    {
        if (cooldownTween == null)
        {
            // BG �ִϸ��̼�
            cooldownBG.fillAmount = 1;
            cooldownTween = cooldownBG.DOFillAmount(0, duration)
                .OnComplete(
                () => cooldownTween = null
                );

            // Text �ִϸ��̼�
            StartCoroutine(TextCoolTime()); 
        }
    }

    public IEnumerator TextCoolTime()
    {
        float remain = duration;

        cooldownText.gameObject.SetActive(true);
        while(remain > 0)
        {
             cooldownText.text = remain.ToString("F0");
             yield return new WaitForSeconds(1f);
             remain--;
        }
        cooldownText.gameObject.SetActive(false);
    }
}
