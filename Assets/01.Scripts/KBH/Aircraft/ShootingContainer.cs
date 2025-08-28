using System;
using System.Collections;
using UnityEngine;
using PlayerInput = Kbh.NewInput.PlayerInput;

namespace Kbh.Aircraft
{
    public class ShootingContainer : MonoBehaviour
    {
        [Header("Default Setting")]
        [SerializeField] private UIManager _uiManager;
        private PlayerInput _playerInput;

        private int _aircraftMaxCount;
        private Vector3[] _aircraftPositions;
        private AbstractAircraft[] _aircraftObjects;

        private const int UNSELECTED_IDX = -1;
        private int _recentSelectedIdx = UNSELECTED_IDX;
        private int _overCheckingIdx = UNSELECTED_IDX;
        private int _statusWindowOpenedIdx = UNSELECTED_IDX;


        [Header("Visual")]
        [SerializeField, Range(0f, 1f)] private float _aircraftDragLerp;
        private IEnumerator _moveRoutine;


        private void Awake()
        {
            _uiManager.Init();
            InputInit();

            var trms = _uiManager.GetAircraftPositionTrms();
            _aircraftMaxCount = _uiManager.GetAircraftPositionCount();

            _aircraftPositions = new Vector3[_aircraftMaxCount];
            _aircraftObjects = new AbstractAircraft[_aircraftMaxCount];

            int idx = 0;
            foreach (Transform trm in trms)
            {
                AircraftPosition aircraft = trm.GetComponent<AircraftPosition>();

                int unreferencedIdx = idx;

                aircraft.MouseDownEvt += () => HandleAircraftMouseDown(unreferencedIdx);
                aircraft.MouseUpEvt += () => HandleAircraftMouseUp(unreferencedIdx);
                aircraft.MouseOverEvt += () => HandleAircraftMouseOver(unreferencedIdx);

                _aircraftPositions[idx++] = trm.position;
            }
        }

#region NEW_INPUT
        private void InputInit()
        {
            _playerInput = NewInput.ConnectPlayer(this);
            _playerInput.OnTouchEvt += HandleTouch;
        }

        private void HandleTouch(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            // 이펙트라든가 그런거는 여기서 나오게 하기.
        }
#endregion

#region AIRCRAFT_INPUT_HANDLE
        private bool IsValidAircraftInIdx(int idx) => _aircraftObjects[idx] != null;

        private void HandleAircraftMouseDown(int idx)
        {
            if (!IsValidAircraftInIdx(idx)) return;

            _recentSelectedIdx = idx;
            ShowSynergyPreview();
            StartMoveAircraft();

            if (idx == _statusWindowOpenedIdx) // status window를 닫습니다. 
            {
                CloseAirplaneStatusWindow();
                _statusWindowOpenedIdx = UNSELECTED_IDX;
            }
        }

        private void HandleAircraftMouseOver(int idx)
        {
            _overCheckingIdx = idx;
        }
        
        private void HandleAircraftMouseUp(int idx)
        {
            if (_recentSelectedIdx == UNSELECTED_IDX) return;
            DisableSynergyPreview();
            StopMoveAircraft();

            if (_overCheckingIdx != UNSELECTED_IDX) // 다른 선택된 비행선이 있다면
                SwapAircraft(_recentSelectedIdx, _overCheckingIdx); // 서로 swap해줍니다.

            OpenAirplaneStatusWindow(); // status window를 열어줍니다.

            _recentSelectedIdx = UNSELECTED_IDX;
            _overCheckingIdx = UNSELECTED_IDX;
            _statusWindowOpenedIdx = idx;
        }


        private void SwapAircraft(int recentSelectedIdx, int overCheckingIdx)
        {
            AbstractAircraft aircraft0 = _aircraftObjects[recentSelectedIdx],
                             aircraft1 = _aircraftObjects[overCheckingIdx];

            aircraft0.transform.position = _aircraftPositions[overCheckingIdx];
            aircraft1.transform.position = _aircraftPositions[recentSelectedIdx];

            (_aircraftPositions[recentSelectedIdx], _aircraftPositions[overCheckingIdx])
            = (_aircraftPositions[overCheckingIdx], _aircraftPositions[recentSelectedIdx]);
        }
        #endregion

#region AIRCRAFT_STATUS
        private void OpenAirplaneStatusWindow()
        {
            var aircraft = _aircraftObjects[_statusWindowOpenedIdx];

            // aircraft에 상태를 가져오는 함수 구현

            _uiManager.OpenStatusPanel(/* 해당 reference 상태 전달 */);
        }

        private void CloseAirplaneStatusWindow()
        {
            // 걍 닫아주면 되는 수.
        }


#endregion

#region MOVE_ACTION
        private void StartMoveAircraft()
        {
            _moveRoutine = AircraftMoveRoutine();
            StartCoroutine(_moveRoutine);
        }

        private void StopMoveAircraft()
        {
            StopCoroutine(_moveRoutine);
        }

        private IEnumerator AircraftMoveRoutine()
        {
            while (true)
            {
                var selectedAircraft = _aircraftObjects[_recentSelectedIdx];
                var previousPosition = selectedAircraft.transform.position;
                var nextPosition = _playerInput.PointerPosition;

                selectedAircraft.transform.position
                    = Vector3.Lerp(previousPosition, nextPosition, _aircraftDragLerp);

                yield return new WaitForFixedUpdate();
            }
        }
        #endregion

#region SYNERGY_EFFECT
        private void ShowSynergyPreview()
        {

        }

        private void DisableSynergyPreview()
        {

        }
        #endregion

#region CONTROL_AIRCRAFT

        #endregion

        private void OnDestroy()
        {
            NewInput.DisconnectPlayer(this);
        }
    }
}
