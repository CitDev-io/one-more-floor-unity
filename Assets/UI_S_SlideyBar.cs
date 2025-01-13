using System;
using Spine;
using Spine.Unity;
using UnityEngine;


public class UI_S_SlideyBar : MonoBehaviour
{
    Bone _bone;
    Bone _previewBone;
    GameBridge _rc;
    [SerializeField] PlayerAvatarCurMaxType _curMaxType;

    void Start()
    {
        _rc = FindObjectOfType<GameBridge>();
        _bone = GetComponent<SkeletonGraphic>().Skeleton.FindBone("value");
        _previewBone = GetComponent<SkeletonGraphic>().Skeleton.FindBone("valuePreview");
        _previewBone.ScaleX = 0;
    }

    private void OnGUI()
    {
        if (_rc.Board == null) return;

        Tuple<int, int> curmax = _rc.Board.State.Player.GetCurMax(_curMaxType);
        _bone.ScaleX = Mathf.Min((float)curmax.Item1, curmax.Item2) / curmax.Item2;
    }
}
