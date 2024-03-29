using UnityEngine;
using TMPro;
using Spine.Unity;
using UnityEngine.EventSystems;

public delegate void ActingTileDelegate(GameTile tile);

public class GameTile : MonoBehaviour
{
    public Tile _tile;
    public ActingTileDelegate OnTileClick;
    public ActingTileDelegate OnTileHoverEnter;

    [Header("Plumbing")]
    [SerializeField] SpriteRenderer sr;
    [SerializeField] public TextMeshPro label1;
    [SerializeField] public TextMeshPro label2;
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

    void OnGUI() {
        if (_tile == null) return;

        bool isMonster = _tile.tileType == TileType.Monster;
        label1.text = isMonster ? _tile.IsBeingCollected ? "C" : _tile.CurrentMonster.Hp + "" : "";
        label2.text = isMonster ? _tile.TurnsAlive + "" : "";
        label3.text = isMonster ? _tile.CurrentMonster.CalcBaseDamage() + "" : "";
        xoutlabel.gameObject.SetActive(isMonster && _tile.selectedAgainstDamage >= _tile.CurrentMonster.Hp);

        if (isMonster && _tile.TurnsAlive > 1) {
            sr.sprite = Resources.Load<Sprite>("Tiles/" + _tile.tileType + "2");
        }
    }

    void SetTileType()
    {
        sr.sprite = Resources.Load<Sprite>("Tiles/" + _tile.tileType + "1");
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
