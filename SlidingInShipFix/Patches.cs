using HarmonyLib;
using UnityEngine;

namespace SlidingInShipFix;

[HarmonyPatch]
public class Patches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ShipDirectionalForceVolume), nameof(ShipDirectionalForceVolume.CalculateForceAccelerationOnBody))]
    public static bool ShipDirectionalForceVolume_CalculateForceAccelerationOnBody_Prefix
    (
        ShipDirectionalForceVolume __instance, ref Vector3 __result, OWRigidbody targetBody
    )
    {
        // Just the default method, but with the weird arbitrary acceleration cap removed
        Vector3 accel = __instance._attachedBody.GetAcceleration();
		if (!__instance._insideSpeedLimiter)
		{
			targetBody.AddAcceleration(accel);
		}
		if (targetBody == __instance._playerBody && PlayerState.IsInsideShip() && !PlayerState.IsAttached())
		{
            Vector3 fieldForce = -__instance._fieldDirection.normalized * __instance._fieldMagnitude;
            Vector3 force = (__instance.transform.TransformDirection(fieldForce) - accel) * targetBody.GetMass();
            __instance._attachedBody.AddForce(force, targetBody.GetWorldCenterOfMass());
		}
		__result = __instance.CalculateForceAccelerationAtPoint(targetBody.GetWorldCenterOfMass());
        return false;
    }
}