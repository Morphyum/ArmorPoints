﻿using BattleTech;
using BattleTech.Data;
using BattleTech.Framework;
using Harmony;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace ClassLibrary1
{
    public class ArmorPoints
    {
        internal static string ModDirectory;
        public static void Init(string directory, string settingsJSON) {
            ModDirectory = directory;
            var harmony = HarmonyInstance.Create("de.morphyum.LanceSpawnFixer");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public static void LogLine(String line) {
            string filePath = $"{ModDirectory}/Log.txt";
            (new FileInfo(filePath)).Directory.Create();
            using (StreamWriter writer = new StreamWriter(filePath, true)) {
                writer.WriteLine(line + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }
        }
    }

    [HarmonyPatch(typeof(LanceOverride))]
    [HarmonyPatch("SelectLanceDefFromList")]
    public static class SimGameState_CreateMechArmorModifyWorkOrder_Patch {
        public static void Prefix(ref int ___MAX_DIFF_DIVERGENCE) {
            ___MAX_DIFF_DIVERGENCE = 50;
        }

        public static void Postfix(LanceDef_MDD __result, int requestedDifficulty, int ___MAX_DIFF_DIVERGENCE) {
            if(__result == null) {
                ArmorPoints.LogLine("No Lance found");
                ArmorPoints.LogLine("Diff: " + requestedDifficulty);
                ArmorPoints.LogLine("Conver: " + ___MAX_DIFF_DIVERGENCE);
            }
        }
    }

    
}
