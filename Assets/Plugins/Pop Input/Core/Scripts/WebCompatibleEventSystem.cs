using UnityEngine;
using UnityEngine.EventSystems;

/*
    On Chrome for Android and only on Chrome for Android there
    is a bug with some Unity versions, wherein the unity event system does not think it receives focus again
    should you experience this bug, simply use this "Fixed Event System", where nothing should happen on focus changes.
    To be entirely on the save side, ensure that you application can run in the background.
*/

public class WebCompatibleEventSystem : EventSystem
{
    protected override void OnApplicationFocus(bool hasFocus)
    {
        if(!Application.runInBackground) Application.runInBackground = true;
    }
}