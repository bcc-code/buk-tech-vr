using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Buk.PhysicsLogic.Implementation
{
  public class ChargeablePhysicsGun : PhysicsGun
  {
    public float minMuzzleVelocity = 1.0f;
    public float maxChargeTime = 1.0f;
    // Seconds before you can shoot a new bullet.
    private float triggerTime = float.PositiveInfinity;

    protected override void TriggerPressed(InputAction.CallbackContext _)
    {
      triggerTime = Time.fixedTime;
    }

    protected override void TriggerReleased(InputAction.CallbackContext _)
    {
      if (CanShoot) {
        Shoot(Math.Max(Time.fixedTime - triggerTime, 0) / maxChargeTime * (maxMuzzleVelocity - minMuzzleVelocity) + minMuzzleVelocity);
        triggerTime = float.PositiveInfinity;
      }
    }
  }
}
