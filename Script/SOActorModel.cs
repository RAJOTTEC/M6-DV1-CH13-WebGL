using UnityEngine;
[CreateAssetMenu(fileName = "Create Actor", menuName = "Create Actor")]
public class SOActorModel : ScriptableObject

{
    public string actorName;
    public AttackType attackType;
    public enum AttackType { wave, player, flee, bullet }
    public string description;
    public int health;
    public int speed;
    public int hitPower;
    public GameObject actor;
    public GameObject actorsBullets;
    public int score;
}