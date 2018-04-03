using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Enemy_4 will start offscreen and then pick a random point on screen to
/// move to. Once it has arrived, it will pick another random point and
/// continue until the player has shot it down.
/// </summary>
/// 

[System.Serializable]
public class Part
{
    // These three fields need to be defined in the Inspector pane
    public string name; // The name of this part
    public float health; // The amount of health this part has
    public string[] protectedBy; // The other parts that protect this
                                 // These two fields are set automatically in Start().
                                 // Caching like this makes it faster and easier to find these later
    [HideInInspector] // Makes field on the next line not appear in the Inspector
    public GameObject go; // The GameObject of this part
    [HideInInspector]
    public Material mat; // The Material to show damage
}

public class Enemy_4 : Enemy
{
    [Header("Set in Inspector: Enemy_4")]
    public Part[] parts; // The array of ship Parts
    private Vector3 p0, p1; // The two points to interpolate
    private float timeStart; // Birth time for this Enemy_4
    private float duration = 4; // Duration of movement

    private int score;
    public GameObject projectilePrefab;


    void Start()
    {

        score = Settings.getScore(4);

        // There is already an initial position chosen by Main.SpawnEnemy()
        // so add it to points as the initial p0 & p1
        p0 = p1 = pos; // a
        InitMovement();
        // Cache GameObject & Material of each Part in parts
        Transform t;
        foreach (Part prt in parts)
        {
            t = transform.Find(prt.name);
            if (t != null)
            {
                prt.go = t.gameObject;
                prt.mat = prt.go.GetComponent<Renderer>().material;
            }
        }

        foreach (Transform child in this.transform){
            switch(Settings.getColor(4)){
                case 1: child.gameObject.GetComponent<Renderer>().material = Main.S.blue; break;
                case 2: child.gameObject.GetComponent<Renderer>().material = Main.S.green; break;
                case 3: child.gameObject.GetComponent<Renderer>().material = Main.S.red; break;
            }
        }

    }

    public int getScore(){
        return score;
    }

    void Fire(){
        GameObject projGO = Instantiate<GameObject>(projectilePrefab);
        projGO.transform.position = new Vector3(transform.position.x, transform.position.y-3f, transform.position.z) ;
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
        //rigidB.velocity = Vector3.up * projectileSpeed;

        Projectile proj = projGO.GetComponent<Projectile>(); // h
        proj.type = WeaponType.blaster;
        float tSpeed = Main.GetWeaponDefinition( proj.type ).velocity;
        rigidB.velocity = Vector3.down * tSpeed;

    }

    void InitMovement()
    { // b
        p0 = p1; // Set p0 to the old p1
                 // Assign a new on-screen location to p1
        float widMinRad = bndCheck.camWidth - bndCheck.radius;
        float hgtMinRad = bndCheck.camHeight - bndCheck.radius;
        p1.x = Random.Range(-widMinRad, widMinRad);
        p1.y = Random.Range(-hgtMinRad, hgtMinRad);
        // Reset the time
        timeStart = Time.time;
    }
    public override void Move()
    { // c
      // This completely overrides Enemy.Move() with a linear interpolation
        float u = (Time.time - timeStart) / duration;
        if (u >= 1)
        {
            InitMovement();
            Fire();
            u = 0;
        }
        u = 1 - Mathf.Pow(1 - u, 2); // Apply Ease Out easing to u // d
        pos = (1 - u) * p0 + u * p1; // Simple linear interpolation // e
    }

    // These two functions find a Part in parts based on name or GameObject
    Part FindPart(string n)
    { // a
        foreach (Part prt in parts)
        {
            if (prt.name == n)
            {
                return (prt);
            }
        }
        return (null);
    }
    Part FindPart(GameObject go)
    { // b
        foreach (Part prt in parts)
        {
            if (prt.go == go)
            {
                return (prt);
            }
        }
        return (null);
    }
    // These functions return true if the Part has been destroyed
    bool Destroyed(GameObject go)
    { // c
        return (Destroyed(FindPart(go)));
    }
    bool Destroyed(string n)
    {
        return (Destroyed(FindPart(n)));
    }
    bool Destroyed(Part prt)
    {
        if (prt == null)
        { // If no real ph was passed in
            return (true); // Return true (meaning yes, it was destroyed)
        }
        // Returns the result of the comparison: prt.health <= 0
        // If prt.health is 0 or less, returns true (yes, it was destroyed)
        return (prt.health <= 0);
    }
    // This changes the color of just one Part to red instead of the whole ship.
    void ShowLocalizedDamage(Material m)
    { // d
        m.color = Color.red;
        damageDoneTime = Time.time + showDamageDuration;
        showingDamage = true;
    }
    // This will override the OnCollisionEnter that is part of Enemy.cs.

    // This will override the OnCollisionEnter that is part of Enemy.cs.
    void OnCollisionEnter(Collision coll)
    { // e
        GameObject other = coll.gameObject;
        switch (other.tag)
        {
            case "ProjectileEnemy":
            case "ProjectilePlayer":
                Projectile p = other.GetComponent<Projectile>();
                
                GameObject goHit = coll.contacts[0].thisCollider.gameObject; // f
                Part prtHit = FindPart(goHit);
                if (prtHit == null)
                { // If prtHit wasn't found… // g
                  
                    goHit = coll.contacts[0].otherCollider.gameObject;
                    prtHit = FindPart(goHit);
                }
                // Check whether this part is still protected
                if (prtHit.protectedBy != null)
                { // h
                 
                    foreach (string s in prtHit.protectedBy)
                    {
                        // If one of the protecting parts hasn't been destroyed...
                        if (!Destroyed(s))
                        {
                            // ...then don't damage this part yet
                            Destroy(other); // Destroy the ProjectileHero
                            return; // return before damaging Enemy_4
                        }
                    }
                }

                
                prtHit.health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                // Show damage on the part
                ShowLocalizedDamage(prtHit.mat);
                if (prtHit.health <= 0)
                { // i
                  // Instead of destroying this enemy, disable the damaged part
                    prtHit.go.SetActive(false);
                }
                // Check to see if the whole ship is destroyed
                bool allDestroyed = true; // Assume it is destroyed
                foreach (Part prt in parts)
                {
                    if (!Destroyed(prt))
                    { // If a part still exists...
                        allDestroyed = false; // ...change allDestroyed to false
                        break; // & break out of the foreach loop
                    }
                }
                if (allDestroyed)
                { // If it IS completely destroyed... // j
                  // ...tell the Main singleton that this ship was destroyed
                    Main.S.ShipDestroyed(this, score);
                    // Destroy this Enemy
                    Destroy(this.gameObject);
                }
                Destroy(other); // Destroy the ProjectileHero
                break;

        }
    }


}
