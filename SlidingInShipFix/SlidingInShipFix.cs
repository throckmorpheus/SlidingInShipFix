using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;
using System;
using System.Reflection;

namespace SlidingInShipFix;

public class ModMain : ModBehaviour
{
	public static ModMain Instance;

	public static IModConsole Console
	{
		get { return Instance.ModHelper.Console; }
	}

	public void Awake()
	{
		Instance = this;
	}

	public void Start()
	{
		Console.WriteLine($"{nameof(SlidingInShipFix)} is loaded!", MessageType.Success);

		new Harmony("Throckmorpheus.ShipPhysicsFixes").PatchAll(Assembly.GetExecutingAssembly());
	}
}

