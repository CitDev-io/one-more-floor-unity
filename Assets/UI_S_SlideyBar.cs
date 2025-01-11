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
        // if (_bone == null) _bone = GetComponent<SkeletonGraphic>().Skeleton.FindBone("value");

        Tuple<int, int> curmax = _rc.Board.State.Player.GetCurMax(_curMaxType);
        // float newWidth = ((float)  curmax.Item1 / curmax.Item2) * 100;

        // RectTransform rt = GetComponent<RectTransform>();
        // rt.sizeDelta = new Vector2(newWidth, rt.sizeDelta.y);
        Debug.Log("CURRENTLY AT " + curmax.Item1 + " / " + curmax.Item2 + " SCALEX " + _bone.ScaleX);
        _bone.ScaleX = ((float)curmax.Item1 / curmax.Item2);
    }
}
