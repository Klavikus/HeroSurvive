using UnityEngine;
using System.IO;

namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuildManager
    {
        public static void FlagsModul()
        {
            string initFunction = "flasgsData = await GetFlags();\nconsole.log('Init Flags ysdk');";
            AddIndexCode(initFunction, CodeType.init);

            string donorPatch = Application.dataPath + "//YandexGame/ScriptsYG/Editor/Flags/Flags_js.js";
            string donorText = File.ReadAllText(donorPatch);

            AddIndexCode(donorText, CodeType.js);
        }
    }
}
