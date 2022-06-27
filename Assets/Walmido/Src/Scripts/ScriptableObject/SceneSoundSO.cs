using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage data", menuName = "Scriptable Object/SceneSound")]
public class SceneSoundSO : ScriptableObject
{
    [SerializeField]
    string lobbySceneSoundName;
    public string LobbySceneSoundName { get => lobbySceneSoundName; }

    [SerializeField]
    string stageSelectSceneSoundName;
    public string StageSelectSceneSoundName { get => stageSelectSceneSoundName; }

    [SerializeField]
    string endSceneSoundName;
    public string EndSceneSoundName { get => endSceneSoundName; }

    [SerializeField]
    string ingameSceneSoundName;
    public string IngameSceneSoundName { get => ingameSceneSoundName; }
}
