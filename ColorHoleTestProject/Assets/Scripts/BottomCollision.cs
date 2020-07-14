using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class BottomCollision : MonoBehaviour
{

    [SerializeField] FirstHoleMovement firstHoleMovement;
    [SerializeField] SecondHoleMovement secondHoleMovement;

    private void OnTriggerEnter(Collider other)
    {
        if (!DataStorage.isGameover)
        {

            string tag = other.tag;

            if (tag.Equals("Object"))
            {
                LevelManager.Instance.objectsInScene--;
                UIManager.Instance.UpdateLevelProgress();

                Magnet.Instance.RemoveFromMagneticField(other.attachedRigidbody);

                Destroy(other.gameObject);

                if(LevelManager.Instance.objectsInScene == 0)
                {
                    if (DataStorage.firstPhase)
                    {
                        firstHoleMovement.FirstPhaseCompleted();
                        secondHoleMovement.InitiateSecondPhase();
                    }
                    else
                    {
                        DataStorage.secondPhase = false;

                        UIManager.Instance.ShowLevelCompletedUI();

                        LevelManager.Instance.PlayWinVFX();
                    }
                }
            }

            if (tag.Equals("Obstacle"))
            {
                DataStorage.isGameover = true;

                Camera.main.transform.DOShakePosition(2f, 0.1f, 5, 45f).
                    OnComplete(()=> LevelManager.Instance.RestartLevel());

                Destroy(other.gameObject);
            }
        }

    }


}
