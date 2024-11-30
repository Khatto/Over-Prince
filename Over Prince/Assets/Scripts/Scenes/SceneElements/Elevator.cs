using UnityEngine;

public class Elevator : MonoBehaviour
{
    public ElevatorStyle style = ElevatorStyle.Angled;

    public GameObject leftDoor;
    public SimpleMovement leftDoorMovement;
    public SpriteMask leftDoorMask;
    public GameObject rightDoor;
    public SimpleMovement rightDoorMovement;
    public SpriteMask rightDoorMask;
    public Fade signBoxDeltaUpFade;
    public Fade signBoxDeltaDownFade;

    public static class ElevatorConstants {
        public static Vector2 angledLeftDoorCloseDisplacement = new Vector2(1.10661f, -0.90354f); // Because of the angle of display, the displacement is not perfectly exact.  We could use an offset variable coupled with Constants.GetInvertedVector
        public static Vector2 angledRightDoorCloseDisplacement = new Vector2(-1.089f, 0.882f); // .787673f or 1.34531f
        public const float doorOpenCloseDuration = 0.5f;
        public const float signBoxDeltaFlashDuration = 1.5f;
        public const float disappearFadeTime = 1.0f;
    }

    void Start() {
        leftDoorMovement = leftDoor.GetComponent<SimpleMovement>();
        rightDoorMovement = rightDoor.GetComponent<SimpleMovement>();
    }

    public void CloseDoors() {
        leftDoor.SetActive(true);
        rightDoor.SetActive(true);
        leftDoorMask.gameObject.SetActive(true);
        rightDoorMask.gameObject.SetActive(true);
        if (style == ElevatorStyle.Angled) {
            leftDoorMovement.SetMovementValues(ElevatorConstants.doorOpenCloseDuration, ElevatorConstants.angledLeftDoorCloseDisplacement, EasingFuncs.Linear);
            rightDoorMovement.SetMovementValues(ElevatorConstants.doorOpenCloseDuration, ElevatorConstants.angledRightDoorCloseDisplacement, EasingFuncs.Linear);
        } else {

        }
        leftDoorMovement.Move();
        rightDoorMovement.Move();
    }

    public void FlashSignBoxDeltas() {
        signBoxDeltaUpFade.gameObject.SetActive(true);
        signBoxDeltaDownFade.gameObject.SetActive(true);
        signBoxDeltaUpFade.StartFadeWithTime(FadeType.FadeOut, ElevatorConstants.signBoxDeltaFlashDuration);
        signBoxDeltaDownFade.StartFadeWithTime(FadeType.FadeOut, ElevatorConstants.signBoxDeltaFlashDuration);
    }
}

public enum ElevatorStyle {
    Angled,
    Horizontal
}
