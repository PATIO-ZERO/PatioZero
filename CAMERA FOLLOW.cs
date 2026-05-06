using UnityEngine;

public class HollowKnightCamera : MonoBehaviour
{
    [Header("?? Objetivo")]
    public Transform target;   // El jugador

    [Header("?? Movimiento Suave")]
    public float smoothSpeed = 4f;

    [Header("?? Dead Zone (Zona Muerta)")]
    public float deadZoneX = 2f;
    public float deadZoneY = 1.2f;

    [Header("?? Límites")]
    public bool useLimits = false;
    public float minX;
    public float maxX;
    public float minY; // Límite máximo hacia abajo

    private Vector3 currentVelocity;

    void LateUpdate()
    {
        if (!target) return;

        Vector3 camPos = transform.position;

        float diffX = target.position.x - camPos.x;
        float diffY = target.position.y - camPos.y;

        // ======================================================
        //                MOVIMIENTO HORIZONTAL
        // ======================================================

        // Movimiento horizontal
        if (Mathf.Abs(diffX) > deadZoneX)
        {
            float targetX = target.position.x - Mathf.Sign(diffX) * deadZoneX;
            camPos.x = Mathf.SmoothDamp(
                camPos.x,
                targetX,
                ref currentVelocity.x,
                1f / smoothSpeed
            );
        }

        // Movimiento vertical (sigue al subir y al bajar)
        if (diffY > deadZoneY || diffY < -deadZoneY)
        {
            float targetY = target.position.y - Mathf.Sign(diffY) * deadZoneY;
            camPos.y = Mathf.SmoothDamp(
                camPos.y,
                targetY,
                ref currentVelocity.y,
                1f / smoothSpeed
            );
        }

        // Mantener Z fijo
        camPos.z = -10f;

        // Aplicar límites
        if (useLimits)
        {
            camPos.x = Mathf.Clamp(camPos.x, minX, maxX);
            camPos.y = Mathf.Max(camPos.y, minY); // no bajar más que minY
        }

        transform.position = camPos;
    }
}
