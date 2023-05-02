using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using PathOfHero.Utilities;
using System.Collections;

public class HUD : Singleton<HUD>
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider manaSlider;
    [SerializeField] private Slider xpSlider;

    [SerializeField] private Text profileNameText;
    [SerializeField] private Text healthText;
    [SerializeField] private Text manaText;
    [SerializeField] private Text xpText;
    [SerializeField] private Text mobCountText;

    [SerializeField] private GameObject hugeMessage;
    [SerializeField] private Text hugeMessageTitleText;
    [SerializeField] private Text hugeMessageSubtitleText;
    [SerializeField] private Text regionText;
    [SerializeField] private Slider[] abilitySliders;
    [SerializeField] private Image[] abilityImages;
    [SerializeField] private GameObject[] abilityCooldownText;
    [SerializeField] private Text[] abilityKeyHintText;

    [SerializeField] private GameObject objectivePanel;
    [SerializeField] private CanvasGroup objectivePanelCanvasGroup;
    [SerializeField] private Text objectiveText;

    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject mainPanel;

    private int m_CurrentHealth;
    private int m_CurrentMana;
    private int m_CurrentXP;

    // Start is called before the first frame update
    void Start()
    {
        regionText.text = ""; // temporary?
        profileNameText.text = SaveSystem.Instance.ProfileName;
    }

    // Update is called once per frame
    void Update()
    {
        //mobCountText.text = "MOB COUNT: " + GameObject.FindGameObjectsWithTag("Mob").Length.ToString("n0");
    }

    // public void SetupHealth(float health, float maxHealth)
    // {
    //     healthSlider.maxValue = maxHealth;
    //     UpdateHealth(health);
    // }

    // public void SetupMana(float mana, float maxMana)
    // {
    //     this.maxMana = maxMana;
    //     manaSlider.maxValue = maxMana;
    //     UpdateMana(mana);
    // }

    public void SetupAbility(int slotNumber, Sprite icon, float cooldownTime, string hintText)
    {
        if (slotNumber >= 0 && slotNumber < abilitySliders.Length)
        {
            abilityImages[slotNumber].sprite = icon;
            abilitySliders[slotNumber].maxValue = cooldownTime;
            abilityKeyHintText[slotNumber].text = hintText;
        }
    }

    public void SetupXP(int level, int maxXP)
    {
        xpSlider.maxValue = maxXP;
        xpSlider.value = maxXP;
        xpText.text = "Level " + level.ToString("n0") + " (" + maxXP.ToString("n0") + "/" + maxXP.ToString("n0") + " XP)";
        m_CurrentXP = maxXP;
    }

    // public void UpdateHealth(float health)
    // {
    //     healthSlider.DOValue(health, 0.25f).SetEase(Ease.OutQuart);
    //     healthText.text = ((int)health).ToString("n0") + " HP";
    // }

    public void UpdateHealth(float health, float upgradedMaxHealth)
    {
        bool forceUpdate = false;
        if (healthSlider.maxValue != upgradedMaxHealth)
        {
            healthSlider.maxValue = upgradedMaxHealth;
            forceUpdate = true;
        }

        if (forceUpdate || m_CurrentHealth != (int)health)
        {
            healthSlider.DOValue(health, 0.25f).SetEase(Ease.OutQuart);
            m_CurrentHealth = (int)health;
        }

        // healthText.text = $"{(int)health} HP";
        healthText.text = $"{health.ToString("n0")} HP";
    }

    // public void UpdateMana(float mana)
    // {
    //     manaSlider.DOValue(mana, 0.25f).SetEase(Ease.OutQuart);
    //     manaText.text = ((int)mana).ToString("n0") + "/" + maxMana.ToString("n0") + " MP";
    // }

    public void UpdateMana(float mana, float upgradedMaxMana)
    {
        bool forceUpdate = false;
        if (manaSlider.maxValue != upgradedMaxMana)
        {
            manaSlider.maxValue = upgradedMaxMana;
            forceUpdate = true;
        }

        if (forceUpdate || m_CurrentMana != (int)mana)
        {
            manaSlider.DOValue(mana, 0.25f).SetEase(Ease.OutQuart);
            m_CurrentMana = (int)mana;
        }

        // manaText.text = $"{(int)mana} / {(int)manaSlider.maxValue} MP";
        manaText.text = $"{mana.ToString("n0")} / {manaSlider.maxValue.ToString("n0")} MP";
    }

    public void UpdateAbility(int slotNumber, float remainingCooldownTime)
    {
        if (0 > slotNumber || slotNumber >= abilitySliders.Length)
            return;
        
        var slider = abilitySliders[slotNumber];
        var text = abilityCooldownText[slotNumber].GetComponent<Text>();
        if (remainingCooldownTime <= 0)
        {
            slider.value = 0;
            text.text = string.Empty;
        }
        else if (slider.value != remainingCooldownTime)
        {
            slider.value = remainingCooldownTime;
            text.text = remainingCooldownTime.ToString("0.0");
        }
    }

    public void UpdateXP(int level, int storedExp, int requiredExp)
    {
        bool forceUpdate = false;
        if (xpSlider.maxValue != requiredExp)
        {
            xpSlider.maxValue = requiredExp;
            forceUpdate = true;
        }

        if (forceUpdate || m_CurrentXP != storedExp)
        {
            xpSlider.DOValue(storedExp, 0.25f).SetEase(Ease.OutQuart);
            m_CurrentXP = storedExp;
        }

        xpText.text = string.Format("Level {0} ({1}/{2} XP)", level.ToString("n0"), storedExp.ToString("n0"), requiredExp.ToString("n0"));
    }

    public void UpdateMobCount(int value)
    {
        mobCountText.text = $"Mob Count: {value.ToString("n0")}";
    }

    #region HugeMessage
    public Task ShowHugeMessageAsync(string title, Color titleColor, float duration = 1.5f) // duration = seconds
        => ShowHugeMessageAsync(title, titleColor, "", Color.white, duration);

    public Task ShowHugeMessageAsync(string title, float duration = 1.5f)
        => ShowHugeMessageAsync(title, new Color32(150, 255, 150, 255), "", Color.white, duration);

    public Task ShowHugeMessageAsync(string title, string subtitle, float duration = 1.5f)
        => ShowHugeMessageAsync(title, new Color32(150, 255, 150, 255), subtitle, Color.white, duration);

    public IEnumerator ShowHugeMessage(string title, Color titleColor, float duration = 1.5f) // duration = seconds
    {
        var task = ShowHugeMessageAsync(title, titleColor, "", Color.white, duration);
        yield return new WaitUntil(() => task.IsCompleted);
    }

    public IEnumerator ShowHugeMessage(string title, float duration = 1.5f)
    {
        var task = ShowHugeMessageAsync(title, new Color32(150, 255, 150, 255), "", Color.white, duration);
        yield return new WaitUntil(() => task.IsCompleted);
    }

    public IEnumerator ShowHugeMessage(string title, string subtitle, float duration = 1.5f)
    {
        var task = ShowHugeMessageAsync(title, new Color32(150, 255, 150, 255), subtitle, Color.white, duration);
        yield return new WaitUntil(() => task.IsCompleted);
    }

    public async Task ShowHugeMessageAsync(string title, Color titleColor, string subtitle, Color subtitleColor, float duration = 1.5f)
    {
        hugeMessage.transform.localScale = new Vector2(0, 1);

        hugeMessageTitleText.text = title;
        hugeMessageTitleText.color = titleColor;

        if (subtitle.Length > 0 || subtitle == null)
        {
            hugeMessageSubtitleText.gameObject.SetActive(true);
            hugeMessageSubtitleText.text = subtitle;
            hugeMessageSubtitleText.color = subtitleColor;
        }
        else
        {
            hugeMessageSubtitleText.gameObject.SetActive(false);
        }

        hugeMessage.SetActive(true);
        await hugeMessage.transform.DOScaleX(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        await Task.Delay((int)(duration * 1000));
        await hugeMessage.transform.DOScaleX(0, 0.25f).SetEase(Ease.InQuart).AsyncWaitForCompletion();
        hugeMessage.SetActive(false);
    }
    #endregion

    public void ShowObjective(string objective)
    {
        objectiveText.text = objective;

        objectivePanel.SetActive(true);
        objectivePanelCanvasGroup.DOFade(1, 0.25f).SetEase(Ease.OutQuart);
    }

    public async void HideObjective()
    {
        await objectivePanelCanvasGroup.DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        objectivePanel.SetActive(false);
    }

    public async void ShowHUD()
    {
        hudPanel.SetActive(true);
        await hudPanel.GetComponent<CanvasGroup>().DOFade(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        hudPanel.GetComponent<CanvasGroup>().alpha = 1f;
    }

    public async void HideHUD()
    {
        await hudPanel.GetComponent<CanvasGroup>().DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        hudPanel.GetComponent<CanvasGroup>().alpha = 0f;
        hudPanel.SetActive(false);
    }

    public async void ShowHUDMain()
    {
        mainPanel.SetActive(true);
        await mainPanel.GetComponent<CanvasGroup>().DOFade(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        mainPanel.GetComponent<CanvasGroup>().alpha = 1f;
    }

    public async void HideHUDMain()
    {
        await mainPanel.GetComponent<CanvasGroup>().DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        mainPanel.GetComponent<CanvasGroup>().alpha = 0f;
        mainPanel.SetActive(false);   
    }
}