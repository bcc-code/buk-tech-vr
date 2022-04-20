using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Buk.AgeOfWar
{
  public class AgeOfWarController : MonoBehaviour
  {
    public float difficulty = 1.0f;
    public float spawnDistance = 50f;
    public List<GameObject> EnemyPrefabs = new List<GameObject>();
    private System.Random random = new System.Random();
    private IEnumerator<Spawn> spawner;
    private TextMeshPro scoreBoard;
    private float startTime = 0f;

    public float GameTime { get => Time.fixedTime - startTime; }

    public void Awake()
    {
      startTime = Time.fixedTime;
      for (var i = EnemyPrefabs.Count - 1; i >= 0; i--)
      {
        var prefab = EnemyPrefabs[i];
        if (!prefab.TryGetComponent<Enemy>(out var enemy))
        {
          EnemyPrefabs.RemoveAt(i);
          Debug.Log("Prefab is not an enemy, it will not be spawned.", prefab);
          continue;
        }
        enemy.ApplyDifficulty(difficulty);
        spawner = Spawns().GetEnumerator();
        spawner.MoveNext();
      }
      scoreBoard = GetComponentInChildren<TextMeshPro>();
    }

    public void FixedUpdate()
    {
      if (spawner.Current.spawnTime <= Time.fixedTime) {
        Instantiate(spawner.Current.prefab, spawner.Current.position, Quaternion.identity);
        spawner.MoveNext();
      }
    }

    public void Update()
    {
      scoreBoard.text = $"Time: {GameTime / 3600:00}:{GameTime / 60 % 3600:00}:{(int)GameTime % 60:00}";
    }

    public void TargetDied(Team team) {
      if (FindObjectsOfType<Target>().Where(target => target.gameObject.GetComponent<Team>().SameTeam(team)).Count() == 0) {
        EndGame();
      }
    }

    public void EndGame() {
      // TODO: Score etc.
      Destroy(this);
    }

    private IEnumerable<Spawn> Spawns()
    {
      while (true)
      {
        var prefab = EnemyPrefabs[random.Next(EnemyPrefabs.Count)];
        yield return new Spawn
        {
          prefab = prefab,
          spawnTime = Time.fixedTime + prefab.GetComponent<Enemy>().productionTime,
          position = transform.position + Quaternion.AngleAxis((float)random.NextDouble() * 360f, Vector3.up) * new Vector3(spawnDistance, 0, 0)
        };
      }
    }

    private class Spawn
    {
      public float spawnTime;
      public GameObject prefab;
      public Vector3 position;
    }
  }
}
