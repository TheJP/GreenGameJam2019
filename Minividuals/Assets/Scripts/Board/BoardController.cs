using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Board
{
    public class BoardController : MonoBehaviour
    {
        public Tiles tiles;
        public PlayerController players;
        public Die die;

        private void Start()
        {
            players.Setup();
            tiles.Setup(players.Players); // TODO: Pass minigame information to tiles.Setup
            StartCoroutine(GameLoop());
        }

        private IEnumerator GameLoop()
        {
            while (true)
            {
                die.PrepareRoll(players.ActivePlayer);
                do
                {
                    yield return null;
                } while (!Input.GetMouseButtonUp(0)); // TODO: Player Input
                yield return die.RollCoroutine();

                yield return new WaitForSeconds(1f);
                yield return MovePlayer(players.ActivePlayer, die.DieResult);

                die.HideDie();

                // TODO: Reveal map and start minigame

                players.NextPlayer();
            }
        }


        public IEnumerator MovePlayer(Player player, int steps)
        {
            while (steps != 0)
            {
                var target = steps < 0 ? tiles.TileBefore(player.Location) : tiles.TileAfter(player.Location);
                player.Location = target;
                yield return player.MoveToPlayerLocation();
                steps -= System.Math.Sign(steps);
            }
        }
    }
}
