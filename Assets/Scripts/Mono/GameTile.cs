using UnityEngine;
using TMPro;
using Spine.Unity;

public delegate void ActingTileDelegate(GameTile tile);

public class GameTile : MonoBehaviour
{
    public Tile _tile;
    public ActingTileDelegate OnTileClick;
    public ActingTileDelegate OnTileHoverEnter;

    [Header("Plumbing")]
    [SerializeField] SpriteRenderer sr;
    [SerializeField] public TextMeshProUGUI label1;
    [SerializeField] public TextMeshProUGUI label2;
    [SerializeField] public TextMeshProUGUI xoutlabel;
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

    void HandleDamageTaken(int damage, DamageSource src) {
       // Debug.Log("I should make some animated stuff to show that this tile took " + damage + " dmg from " + src);
    }
    void HandleUnstunned() {
        MonsterFace.GetComponent<SkeletonAnimation>().AnimationState.AddAnimation(0, "idle", true, 0f);
    }

    void HandleStunned() {
        MonsterFace.GetComponent<SkeletonAnimation>().AnimationState.AddAnimation(0, "stunned", true, 0f);
    }

    void HandleDoAttack() {
        if (_tile.isStunned()) return;

        MonsterFace.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "attack", false);
        MonsterFace.GetComponent<SkeletonAnimation>().AnimationState.AddAnimation(0, "idle", true, 0f);
    }

    void HandlePositionChange() {
        if (_tile.row == 5) {
            MonsterFace.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "idle", true);
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

    void OnGUI() {
        if (_tile == null) return;

        bool isMonster = _tile.tileType == TileType.Monster;
        label1.text = isMonster ? _tile.CurrentMonster.Vitality + "" : "";
        label2.text = isMonster ? _tile.CurrentMonster.Strength + "" : "";
        xoutlabel.gameObject.SetActive(isMonster && _tile.selectedAgainstDamage >= _tile.CurrentMonster.Vitality);
    }

    void SetTileType()
    {
        sr.sprite = Resources.Load<Sprite>("Tiles/" + _tile.tileType + "1");

        bool isAMonster = _tile.tileType == TileType.Monster;
        MonsterFace.SetActive(isAMonster);
        sr.enabled = !isAMonster;
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
        OnTileClick?.Invoke(this);
    }
    void OnMouseEnter()
    {
        OnTileHoverEnter?.Invoke(this);
    }
}
