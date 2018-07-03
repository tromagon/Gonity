using System.Linq;
using UnityEditor;

namespace Gonity.Profiler
{
    internal class ECSProfilerMenuItem
    {
        [MenuItem("ECS/Enable Profiler")]
        public static void EnableProfiler()
        {
            AddScriptingDefineSymbolToAllTargets(BuildTargetGroup.Standalone, "ECS_PROFILER_ENABLED");
        }

        [MenuItem("ECS/Disable Profiler")]
        public static void DisableProfiler()
        {
            RemoveScriptingDefineSymbolFromAllTargets(BuildTargetGroup.Standalone, "ECS_PROFILER_ENABLED");
        }

        public static void AddScriptingDefineSymbolToAllTargets(BuildTargetGroup group, string defineSymbol)
        {
            {
                var defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';').Select(d => d.Trim()).ToList();
                if (!defineSymbols.Contains(defineSymbol))
                {
                    defineSymbols.Add(defineSymbol);
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", defineSymbols.ToArray()));
                }
            }
        }

        public static void RemoveScriptingDefineSymbolFromAllTargets(BuildTargetGroup group, string defineSymbol)
        {
            {
                var defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';').Select(d => d.Trim()).ToList();
                if (defineSymbols.Contains(defineSymbol))
                {
                    defineSymbols.Remove(defineSymbol);
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", defineSymbols.ToArray()));
                }
            }
        }
    }
}
