using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class PlayerController
{
    [Header("Grappling State")]
    [SerializeField] LayerMask _grapplingLayer;
    [SerializeField] float _grapplingRayLength = 10f;
    [SerializeField] float _grapplingPower = 5f;
    [SerializeField] float _grapplingPointMargin = 1f;
    [SerializeField] Transform _topPosition;

    public class PlayerGrapplingState : StateBase
    {
        RaycastHit _hit;
        Vector3 _endPosition;
        PlayerController _player;

        public override void OnEnter(StatePatternBase entity, StateBase state)
        {
            if(!_player)
            {
                _player = entity.GetComponent<PlayerController>();
            }

            var t = Camera.main.transform;
            var hit = Physics.Raycast(t.position, t.forward, out _hit, _player._grapplingRayLength, _player._grapplingLayer);
            
            if(!hit)
            {
                _player.ChangeState(_moveState);
                return;
            }

            _player._rb.useGravity = false;
            _player.transform.rotation = Quaternion.LookRotation(_endPosition - _player._topPosition.position);
            _endPosition = _hit.point;
        }
        public override void OnUpdate(StatePatternBase entity)
        {
            if(Vector3.Distance(_player._topPosition.position, _endPosition) < _player._grapplingPointMargin)
            {
                _player.ChangeState(_moveState);
            }

            var dir = _endPosition - _player._topPosition.position;

            _player._rb.AddForce(dir * _player._grapplingPower, ForceMode.Acceleration);
        }
        public override void OnExit(StatePatternBase entity, StateBase nextState)
        {
            _player._rb.useGravity = true;
        }
    }
}
