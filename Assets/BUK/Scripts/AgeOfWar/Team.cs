using UnityEngine;

namespace Buk.AgeOfWar
{
  public class Team : MonoBehaviour
  {
    public string team;

    public bool OtherTeam(Team compareTeam) => team != compareTeam?.team;
    public bool SameTeam(Team compareTeam) => team == compareTeam?.team;
  }
}
