using UnityEngine;

public enum ProType {
    Rock,
    Arrow,
    Fireball
}

public class Projectile : MonoBehaviour {

    [SerializeField] private int attackStrength;
    [SerializeField] private ProType projectileType;

    public int AttackStrength {
        get {
            return attackStrength;
        }
    }

    public ProType ProjectileType {
        get {
            return projectileType;
        }
    }
}
