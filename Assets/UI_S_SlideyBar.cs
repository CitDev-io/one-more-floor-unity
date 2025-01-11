using System;
using Spine;
using Spine.Unity;
using UnityEngine;


public class UI_S_SlideyBar : MonoBehaviour
{
    Bone _bone;
    GameBridge _rc;
    [SerializeField] PlayerAvatarCurMaxType _curMaxType;

    void Start()
    {
        _rc = FindObjectOfType<GameBridge>();
        _bone = GetComponent<SkeletonGraphic>().Skeleton.FindBone("value");
    }

    private void OnGUI()
    {
        if (_rc.Board == null) return;

        Tuple<int, int> curmax = _rc.Board.State.Player.GetCurMax(_curMaxType);
        _bone.ScaleX = (float)curmax.Item1 / curmax.Item2;
    }
}
