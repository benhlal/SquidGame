using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CameraFreeLook : MonoBehaviour
{
    private CinemachineFreeLook cinemachinefl;

    private PlayerControls playerCTL;

    [SerializeField] private float lookSpeed = 1;

    private void Awake()
    {
        playerCTL = new PlayerControls();
        cinemachinefl = GetComponent<CinemachineFreeLook>();
    }


    void Update()
    {
        Vector2 delta = playerCTL.Player.Look.ReadValue<Vector2>();
        cinemachinefl.m_XAxis.Value += delta.x * 200 * lookSpeed * Time.deltaTime;
        cinemachinefl.m_YAxis.Value += delta.y * Time.deltaTime;
    }

    private void OnEnable()
    {
        playerCTL.Enable();
    }

    private void OnDisable()
    {
        playerCTL.Disable();
    }
}