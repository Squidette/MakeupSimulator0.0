using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 모바일 펜 인풋
/// 닿았는지 여부 + 스크린상 좌표 + 세기
/// </summary>
public class PenInput : MonoBehaviour
{
    /// <summary>
    /// 펜이 스크린에 닿아 있는지
    /// </summary>
    public bool IsPenDown { get; private set; }

    /// <summary>
    /// 펜이 닿은 위치
    /// </summary>
    public Vector2 PenPosition { get; private set; }

    /// <summary>
    /// 필압
    /// </summary>
    public float Pressure { get; private set; }

    void Update()
    {
#if UNITY_EDITOR
        EmulateStylusPenInput();
#else
        GetStylusPenInput();
#endif
    }

    /// <summary>
    /// 스타일러스 펜 인풋 받기
    /// </summary>
    void GetStylusPenInput()
    {
        Pen currentPen = Pen.current;
        if (currentPen == null)
        {
            IsPenDown = false;
            Pressure = 0.0F;
            return;
        }

        // 펜이 스크린에 닿았는지 여부 인식
        if (currentPen.tip != null)
        {
            IsPenDown = currentPen.tip.isPressed;
        }
        else
        {
            IsPenDown = false;
            Pressure = 0.0F;
            return;
        }

        // 펜 위치 인식
        if (currentPen.position != null)
        {
            PenPosition = currentPen.position.ReadValue();
        }
        else
        {
            IsPenDown = false;
            Pressure = 0.0F;
            return;
        }

        // 필압 인식
        if (currentPen.pressure != null)
        {
            Pressure = currentPen.pressure.ReadValue();
        }
        else Pressure = IsPenDown ? 1.0F : 0.0F;
    }

#if UNITY_EDITOR
    /// <summary>
    /// 에디터 환경에서 마우스로 대체하기
    /// </summary>
    void EmulateStylusPenInput()
    {
        if (Mouse.current != null)
        {
            IsPenDown = Mouse.current.leftButton.isPressed;
            PenPosition = Mouse.current.position.ReadValue();
            Pressure = IsPenDown ? 1.0F : 0.0F;
        }
        else
        {
            IsPenDown = false;
            PenPosition = Vector2.zero;
            Pressure = 0.0F;
        }

        if (IsPenDown)
        {
            Debug.Log($"PenPosition: {PenPosition}, Pressure: {Pressure}");
        }
    }
#endif
}
