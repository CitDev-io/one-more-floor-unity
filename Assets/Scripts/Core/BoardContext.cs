using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace citdev {
    public class BoardContext
    {
        private readonly PlayerCharacter _pc;
        private readonly int _stage;
        private readonly BoardGameBox _box;

        public BoardContext(PlayerCharacter playerCharacter, int stage, BoardGameBox box) {
            _pc = playerCharacter;
            _stage = stage;
            _box = box;
        }

        public PlayerCharacter PC {
            get { return _pc; }
        }

        public int Stage {
            get { return _stage; }
        }

        public BoardGameBox Box {
            get { return _box; }
        }
    }
}
