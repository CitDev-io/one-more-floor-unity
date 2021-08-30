using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Spine.Unity;

namespace citdev {
    public delegate void TileDelegate(GameTile tile);

    public class GameTile : MonoBehaviour
    {
        public TileDelegate OnTileClick;
        public TileDelegate OnTileHoverEnter;

        [Header("Plumbing")]
        [SerializeField] GameObject highlight;
        [SerializeField] SpriteRenderer sr;
        [SerializeField] public TextMeshProUGUI label1;
        [SerializeField] public TextMeshProUGUI label2;
        [SerializeField] GameObject MonsterFace;

        [Header("State")]
        public TileType tileType;
        public int HitPoints = 0;
        public int MaxHitPoints = 2;
        public int TurnsAlive = 0;
        public int Damage = 2;
        public int StunnedRounds = 0;

        [Space(25)]
        float speed = 7f;

        [Header("Assignment")]
        public int row = 5; // Y Y Y Y Y Y 
        public int col = 5; // X X X X X X

        void OnGUI() {
            label1.text = tileType == TileType.Monster ? HitPoints + "" : "";
            label2.text = tileType == TileType.Monster ? Damage + "" : "";
        }

        public void Reset() {
            HitPoints = MaxHitPoints;
            TurnsAlive = 0;
            StunnedRounds = 0;
            MonsterFace.GetComponent<SkeletonAnimation>().AnimationState.AddAnimation(0, "idle", true, 0f);
        }

        public void SetTileType(TileType tt)
        {
            tileType = tt;
            sr.sprite = Resources.Load<Sprite>("Tiles/" + tt + "1");

            bool isAMonster = tt == TileType.Monster;
            MonsterFace.SetActive(isAMonster);
            sr.enabled = !isAMonster;
        }

        public void MonsterMenace()
        {
            if (StunnedRounds > 0) return;

            MonsterFace.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "attack", false);
            MonsterFace.GetComponent<SkeletonAnimation>().AnimationState.AddAnimation(0, "idle", true, 0f);
        }

        public void Stun() {
            StunnedRounds = 3;
            MonsterFace.GetComponent<SkeletonAnimation>().AnimationState.AddAnimation(0, "stunned", true, 0f);
        }

        public void ResolveStunRound(){
            if (StunnedRounds == 1) {
                MonsterFace.GetComponent<SkeletonAnimation>().AnimationState.AddAnimation(0, "idle", true, 0f);
            }
            StunnedRounds -= 1;
        }

        void Start()
        {
            highlight.SetActive(false);   
        }

        void Update()
        {
            Vector2 dest = new Vector2(col, row);

            if (Vector2.Distance(transform.position, dest) > 0.2f)
            {
                transform.position = Vector2.MoveTowards(transform.position, dest, speed * Time.deltaTime);
            } else
            {
                transform.position = dest;
            }
        }

        public void RecycleAsType(TileType tileType, bool RecyclePosition = false) {
            SetTileType(tileType);
            SnapToPosition(col, 7);
            if (RecyclePosition) {
                AssignPosition(col, 5);
            }
            Reset();
        }

        public void SnapToPosition(float x, float y)
        {
            SnapToPosition(new Vector2(x, y));
        }

        public void SnapToPosition(Vector2 newPos)
        {
            transform.position = newPos;
        }

        public void AssignPosition(float x, float y)
        {
            AssignPosition(new Vector2(x, y));
        }

        public void AssignPosition(Vector2 newPos)
        {
            row = (int) newPos.y;
            col = (int) newPos.x;
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
        
}
