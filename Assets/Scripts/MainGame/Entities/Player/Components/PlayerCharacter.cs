using Player.Base;
using UnityEngine;

namespace Components
{
    public class PlayerCharacter : PlayerBase
    {

        protected override void Awake()
        {
            base.Awake();
            Debug.Log("Player is Here");
        }
    }
}
