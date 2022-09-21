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

        public override void OnEnter(PlayerController player, StateBase state)
        {
            var t = Camera.main.transform;
            var hit = Physics.Raycast(t.position, t.forward, out _hit, player._grapplingRayLength, player._grapplingLayer);
            
            if(!hit)
            {
                player.ChangeState(_moveState);
                return;
            }

            player._rb.useGravity = false;
            player.transform.rotation = Quaternion.LookRotation(_endPosition - player._topPosition.position);
            _endPosition = _hit.point;
        }
        public override void OnUpdate(PlayerController player)
        {
            if(Vector3.Distance(player._topPosition.position, _endPosition) < player._grapplingPointMargin)
            {
                player.ChangeState(_moveState);
            }

            var dir = _endPosition - player._topPosition.position;

            player._rb.AddForce(dir * player._grapplingPower, ForceMode.Acceleration);
        }
        public override void OnExit(PlayerController player, StateBase nextState)
        {
            player._rb.useGravity = true;
        }
    }
}
