using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class UIManager : MonoBehaviour
{

    public static UIManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    [Header("Level Progress UI")]
    [SerializeField] int sceneOffset;
    [SerializeField] TMP_Text nextLevelText;
    [SerializeField] TMP_Text currentLevelText;
    [SerializeField] Image progressFillImageFirstPhase;
    [SerializeField] Image progressFillImageSecondPhase;

    [Space]
    [SerializeField] TMP_Text levelCompletedText;


    private void Start()
    {
        progressFillImageFirstPhase.fillAmount = 0;
        progressFillImageSecondPhase.fillAmount = 0;
    }

    public void UpdateLevelProgress()
    {
        float val = 1f - ((float)LevelManager.Instance.objectsInScene / LevelManager.Instance.totalObjects);

        if (DataStorage.firstPhase)
        {
            progressFillImageFirstPhase.DOFillAmount(val, 0.4f);
        }
        else
        {
            progressFillImageSecondPhase.DOFillAmount(val, 0.4f);
        }
    }

    public void ShowLevelCompletedUI()
    {
        levelCompletedText.DOFade(1f, 0.6f).From(0f);
    }

}
