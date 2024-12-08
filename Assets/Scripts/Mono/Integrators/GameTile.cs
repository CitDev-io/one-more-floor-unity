using UnityEngine;
using TMPro;
using Spine.Unity;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public delegate void ActingTileDelegate(GameTile tile);

public class GameTile : MonoBehaviour
{
    public Tile _tile;
    public ActingTileDelegate OnTileClick;
    public ActingTileDelegate OnTileHoverEnter;

    [Header("Plumbing")]
    [SerializeField] SpriteRenderer sr;
    [SerializeField] public TextMeshPro label1;
    [SerializeField] public TextMeshPro label3;
    [SerializeField] public TextMeshPro xoutlabel;
    [SerializeField] GameObject MonsterFace;

    [Space(25)]
    float speed = 7f;

    public void AttachToTile(Tile tile) {
        _tile = tile;
        tile.OnTileTypeChange += HandleTileTypeChange;
        tile.OnPositionChange += HandlePositionChange;
        tile.OnStunned += HandleStunned;
        tile.OnDoAttack += HandleDoAttack;
        tile.OnUnstunned += HandleUnstunned;
        tile.OnDamageTaken += HandleDamageTaken;
        SetTileType();
    }

    /*

        Tile should have a performance for attack animation
        It should be handed a callback for the "hit" moment

        Callback during MonstersAttack will want the callback
        to be the part where we slap the dmg number on the screen

    */
    public IEnumerator DoAttackAnimation(Action orchestratorHitCallback) {
        var anim = MonsterFace.GetComponent<SkeletonAnimation>();
        anim.AnimationState.SetAnimation(0, "attack", false);
        yield return new WaitForSeconds(0.25f);
        orchestratorHitCallback();
        anim.AnimationState.AddAnimation(0, "idle", true, 0f);
    }

    public IEnumerator FadeToOpacity(float targetOpacity) {
        float currentOpacity = GetIconChildAlpha();
        float step = 0.2f;
        while (Mathf.Abs(currentOpacity - targetOpacity) > 0.2f) {
            currentOpacity = Mathf.Lerp(currentOpacity, targetOpacity, step);
            SetIconChildAlpha(currentOpacity);
            yield return new WaitForSeconds(0.1f);
        }
        SetIconChildAlpha(targetOpacity);
    }

    void HandleDamageTaken(int damage) {
    }
    void HandleUnstunned() {
    }

    void HandleStunned() {
    }

    void HandleDoAttack() {
        if (_tile.isStunned()) return;
    }

    void HandlePositionChange() {
        if (_tile.row == 5) {
            SnapToPosition(_tile.col, 7);
        }
    }

    void HandleTileTypeChange() {
        SetTileType();
    }

    void DetachFromTile() {
        if (_tile == null) return;
        _tile.OnTileTypeChange -= HandleTileTypeChange;
        _tile.OnPositionChange -= HandlePositionChange;
    }

    void OnDestroy() {
        DetachFromTile();
    }

    public float GetIconChildAlpha() {
        return transform.Find("Icon").GetComponent<SpriteRenderer>().color.a;
    }

    public void SetIconChildAlpha(float alpha) {
        var icon = transform.Find("Icon").GetComponent<SpriteRenderer>();
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, alpha);
        if (MonsterFace.activeSelf) {
            MonsterFace.GetComponent<SkeletonAnimation>().skeleton.A = alpha;
        }
    }

    void OnGUI() {
        if (_tile == null) return;

        bool isMonster = _tile.tileType == TileType.Monster;

        if (isMonster) {
            label1.text = _tile.IsBeingCollected ? "C" : _tile.CurrentMonster.Hp + "";
            label3.text = _tile.CurrentMonster.CalcBaseDamage() + "";
        } else {
            label1.text = "";
            label3.text = "";
        }

        label1.transform.Find("Circle")?.gameObject.SetActive(isMonster);
        label3.transform.Find("Circle")?.gameObject.SetActive(isMonster);
        xoutlabel.gameObject.SetActive(isMonster && _tile.selectedAgainstDamage >= _tile.CurrentMonster.Hp);

        if (isMonster && _tile.TurnsAlive > 1) {
            sr.sprite = Resources.Load<Sprite>("Tiles/" + _tile.tileType + "2");
        }
    }

    void SetTileType()
    {
        bool isMonster = _tile.tileType == TileType.Monster;
        MonsterFace.SetActive(isMonster);
        sr.enabled = !isMonster;
        if (isMonster) {
            MonsterFace.GetComponent<SkeletonAnimation>().skeleton.A = 1f;
            // MonsterFace.GetComponent<SkeletonAnimation>().skeletonDataAsset = Resources.Load<SkeletonDataAsset>("Monsters/" + _tile.CurrentMonster.MonsterType);
            //MonsterFace.GetComponent<SkeletonAnimation>().Initialize(true);
            MonsterFace.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "stunned", true);
        }
        if (!isMonster) {
            sr.sprite = Resources.Load<Sprite>("Tiles/" + _tile.tileType + "1");
        }
    }

    void Update()
    {
        if (_tile == null) return;

        Vector2 dest = new Vector2(_tile.col, _tile.row);

        if (Vector2.Distance(transform.position, dest) > 0.2f)
        {
            transform.position = Vector2.MoveTowards(transform.position, dest, speed * Time.deltaTime);
        } else
        {
            transform.position = dest;
        }
    }

    void SnapToPosition(float x, float y)
    {
        SnapToPosition(new Vector2(x, y));
    }

    void SnapToPosition(Vector2 newPos)
    {
        transform.position = newPos;
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        OnTileClick?.Invoke(this);
    }
    void OnMouseEnter()
    {
        OnTileHoverEnter?.Invoke(this);
    }
}
