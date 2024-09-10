using UnityEngine;

public class CinematicFrameManager : MonoBehaviour
{
    public SpriteRenderer topFrame;
    private SimpleMovement topFrameMovement;
    private AutoScale topFrameAutoScale;
    private AutoAlign topFrameAutoAlign;
    public SpriteRenderer bottomFrame;
    private SimpleMovement bottomFrameMovement;
    private AutoScale bottomFrameAutoScale;
    private AutoAlign bottomFrameAutoAlign;

    public void Start() {
        topFrameMovement = topFrame.GetComponent<SimpleMovement>();
        topFrameAutoScale = topFrame.GetComponent<AutoScale>();
        bottomFrameMovement = bottomFrame.GetComponent<SimpleMovement>();
        bottomFrameAutoScale = bottomFrame.GetComponent<AutoScale>();
        topFrameAutoAlign = topFrame.GetComponent<AutoAlign>();
        bottomFrameAutoAlign = bottomFrame.GetComponent<AutoAlign>();
    }
    public void ExitFrames() {
        topFrameMovement.SetMovementDelta(new Vector2(0, (topFrame.size.y * topFrame.transform.localScale.y)));
        topFrameAutoScale.scaleUpdateFrequency = ScaleUpdateFrequency.None;
        topFrameAutoAlign.continuouslyAdjustVertical = false;
        topFrameMovement.Move();
        bottomFrameMovement.SetMovementDelta(new Vector2(0, -(bottomFrame.size.y * bottomFrame.transform.localScale.y)));
        bottomFrameAutoScale.scaleUpdateFrequency = ScaleUpdateFrequency.None;
        bottomFrameAutoAlign.continuouslyAdjustVertical = false;
        bottomFrameMovement.Move();
    }

    public void EnterFrames() {
        topFrameMovement.SetMovementDelta(new Vector2(0, -(topFrame.size.y * topFrame.transform.localScale.y) + 0.1f));
        topFrameAutoScale.scaleUpdateFrequency = ScaleUpdateFrequency.None;
        topFrameAutoAlign.continuouslyAdjustVertical = false;
        topFrameMovement.Move();
        bottomFrameMovement.SetMovementDelta(new Vector2(0, (bottomFrame.size.y * bottomFrame.transform.localScale.y) - 0.1f));
        bottomFrameAutoScale.scaleUpdateFrequency = ScaleUpdateFrequency.None;
        bottomFrameAutoAlign.continuouslyAdjustVertical = false;
        bottomFrameMovement.Move();
    }

    public void SetFramesToFollowContinuously() {
        topFrameAutoAlign.continuouslyAdjustVertical = true;
        topFrameAutoAlign.continuouslyAdjustHorizontal = true;
        bottomFrameAutoAlign.continuouslyAdjustVertical = true;
        bottomFrameAutoAlign.continuouslyAdjustHorizontal = true;
    }

    public float GetMovementTime() {
        return topFrameMovement.movementTime;
    }
}
