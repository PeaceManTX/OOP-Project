using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using static GameManager;
using static UnityEditor.FilePathAttribute;
using JetBrains.Annotations;
//using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    // Cannot load static from editor; only public
    public GameObject angelPrefabLoad;
    public GameObject devilPrefabLoad;
    public GameObject doctorPrefabLoad;
    public GameObject zombiePrefabLoad;

    //
    // Need static GameObjects to initialize and operate in child classes
    //
    static GameObject angelPrefab;
    static GameObject devilPrefab;
    static GameObject doctorPrefab;
    static GameObject zombiePrefab;

    //
    // Character Toggles: all listening is done in Unity Inspector 'On Value Changed'
    //
    public Toggle angelTog;
    public Toggle devilTog;
    public Toggle doctorTog;
    public Toggle zombieTog;

    public Angel angel;
    public Zombie zombie;
    public Devil devil;
    public Doctor doctor;

    //
    // Bools to ensure valid states to create and destroy
    //
    public bool angelToggled = false;
    public bool devilToggled = false;
    public bool doctorToggled = false;
    public bool zombieToggled = false;

    Vector3 goForward = new Vector3(8.3f, 4.6f, -18.7f); //Not implemented in this version. Used as placeholder

    public enum EntityType { Physical, Spiritual };

    //***************************************************
    //
    // INHERITANCE 
    // Entity base class: Manages core entity characteristics and location of derived class game objects
    //
    //***************************************************
    public abstract class Entity
    {
        //private bool _inCircleZone = false;  // Check location of entity whether in or out of circle zone
        private Vector3 _location;  // entity location
        protected float worldBoundary = 40;
 

        public Entity(string name, string talent, EntityType type,Vector3 location)
        {
            if (string.IsNullOrWhiteSpace(talent))
                throw new ArgumentException("Character's talent is required.");
            Talent = talent;

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Character's name is required.");
            Name = name;

            Type = type;
            Location = location;
        }
        //
        // ENCAPSULATION
        //
        public string Talent { get; }

        public string Name { get; }

        public EntityType Type { get; }
        public Vector3 Location { get; }    

        public Vector3 GetEntityLocation()
        {
                return Location;
        }
        public bool ValidateLocation(Vector3 location)
        {
            if (location.x > -worldBoundary && location.x < worldBoundary &&
                location.y > 0 && location.y < 10 &&
                location.z > -worldBoundary && location.x < worldBoundary)
            {
                return true;
            }
            return false;
        }
        //
        // POLYMORPHISM
        //
        public virtual bool Move(Vector3 moveToLocation)
        {
            if (ValidateLocation(moveToLocation))
            {
                return true;
            } else
            {
                return false;
            }
        }
        public override string ToString() => Name;
    }

    //***************************************************
    //
    // Each class below represents each character in game and are inherited from Entity base class. 
    //
    //  Angel Class
    //
    //***************************************************
    public sealed class Angel : Entity
    {
        private GameObject angelGameObject;
        private Animator angelAnim;

        public Angel(string name, string origin, string talent, Vector3 location) : base(name, talent, EntityType.Spiritual, location)
        {

            // Ensure Origin is correct
            if (string.IsNullOrWhiteSpace(origin))
                throw new ArgumentException("The origin is required.");

            // Ensure location is within world coordinates
            if (ValidateLocation(location))
            {
                Instantiate(angelPrefab, location, angelPrefab.transform.rotation);
            }
    
            Origin = origin;
            angelGameObject = GameObject.FindGameObjectWithTag("Angel");
            angelAnim = angelGameObject.GetComponent<Animator>();
            angelAnim.SetBool("Fly", false);

        }
        //
        // POLYMORPHISM
        //
        public override bool Move(Vector3 moveToLocation)
        {
            if (base.Move(moveToLocation))
            {
                if (base.ValidateLocation(angelGameObject.transform.position))
                {
                    angelAnim.SetBool("Fly", true);
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }

        }
        //
        // ENCAPSULATION
        //
        public string Origin { get; }
        public override string ToString() => $"{(string.IsNullOrEmpty(Origin) ? "" : Origin + ", ")}{Name}";
    }

    //
    //  Devil Class
    //
    public sealed class Devil : Entity
    {
        private GameObject devilGameObject;
        private Animator devilAnim;
        public Devil(string name, string origin, string talent, Vector3 location) : base(name, talent, EntityType.Spiritual, location)
        {

            // Ensure Origin is correct
            if (string.IsNullOrWhiteSpace(origin))
                throw new ArgumentException("The origin is required.");
            
            // Ensure location is within world coordinates
            if (ValidateLocation(location))
            {
                Instantiate(devilPrefab, location, devilPrefab.transform.rotation);
            }

            Origin = origin;
            devilGameObject = GameObject.FindGameObjectWithTag("Devil");
            devilAnim = devilGameObject.GetComponent<Animator>();
            devilAnim.SetBool("Walk", false);
        }

        //
        // POLYMORPHISM
        //
        public override bool Move(Vector3 moveToLocation)
        {
            if (base.Move(moveToLocation))
            {
                if (base.ValidateLocation(devilGameObject.transform.position))
                {
                    devilAnim.SetBool("Walk", true);
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }

        }
        //
        // ENCAPSULATION
        //
        public string Origin { get; }
        public override string ToString() => $"{(string.IsNullOrEmpty(Origin) ? "" : Origin + ", ")}{Name}";
    }

    //
    // Doctor Class
    //

    public sealed class Doctor : Entity
    {
        private GameObject doctorGameObject;
        private Animator doctorAnim;
        public Doctor(string name, string origin, string talent, Vector3 location) : base(name, talent, EntityType.Physical, location)
        {

            // Ensure Origin is correct
            if (string.IsNullOrWhiteSpace(origin))
                throw new ArgumentException("The origin is required.");

            // Ensure location is within world coordinates
            if (ValidateLocation(location))
            {
                Instantiate(doctorPrefab, location, doctorPrefab.transform.rotation);
            }

            Origin = origin;
            doctorGameObject = GameObject.FindGameObjectWithTag("Doctor");
            doctorAnim = doctorGameObject.GetComponent<Animator>();
            doctorAnim.SetBool("HopWalk", false);
        }

        //
        // POLYMORPHISM
        //
        public override bool Move(Vector3 moveToLocation)
        {
            if (base.Move(moveToLocation))
            {
                if (base.ValidateLocation(doctorGameObject.transform.position))
                {
                    doctorAnim.SetBool("HopWalk", true);
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }

        }
        //
        // ENCAPSULATION
        //
        public string Origin { get; }
        public override string ToString() => $"{(string.IsNullOrEmpty(Origin) ? "" : Origin + ", ")}{Name}";
    }

    //
    // Zombie Class
    //
    public sealed class Zombie : Entity
    {
        private GameObject zombieGameObject;
        private Animator zombieAnim;
        public Zombie(string name, string origin, string talent, Vector3 location) : base(name, talent, EntityType.Physical, location)
        {

            // Ensure Origin is correct
            if (string.IsNullOrWhiteSpace(origin))
                throw new ArgumentException("The origin is required.");

            // Ensure location is within world coordinates
            if (ValidateLocation(location))
            {
                Instantiate(zombiePrefab, location, zombiePrefab.transform.rotation);
            }

            Origin = origin;
            zombieGameObject = GameObject.FindGameObjectWithTag("Zombie");
            zombieAnim = zombieGameObject.GetComponent<Animator>();
            zombieAnim.SetBool("DeadWalk", false);
        }

        //
        // POLYMORPHISM
        //
        public override bool Move(Vector3 moveToLocation)
        {
            if (base.Move(moveToLocation))
            {
                if (base.ValidateLocation(zombieGameObject.transform.position))
                {
                    zombieAnim.SetBool("DeadWalk", true);
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }

        }
        //
        // ENCAPSULATION
        //
        public string Origin { get; }
        public override string ToString() => $"{(string.IsNullOrEmpty(Origin) ? "" : Origin + ", ")}{Name}";
    }
    // Start is called before the first frame update
    void Start()
    {
        // Initialize game objects
        angelPrefab = angelPrefabLoad;
        devilPrefab = devilPrefabLoad;
        doctorPrefab  = doctorPrefabLoad;
        zombiePrefab = zombiePrefabLoad;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (angel.Move(goForward))    //move forward
            {
                ShowLocation(angel);
                Debug.Log("Fly Succeeded");  // Placeholder for later..
            }
          
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (devil.Move(goForward))    //move forward
            {
                ShowLocation(devil);
                Debug.Log("Walk Succeeded");// Placeholder for later..
            }

        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (doctor.Move(goForward))    //move forward
            {
                ShowLocation(doctor);
                Debug.Log("Hop Walk Succeeded");// Placeholder for later..
            }

        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (zombie.Move(goForward))    //move forward
            {
                ShowLocation(zombie);
                Debug.Log("DeadWalk Succeeded");// Placeholder for later..
            }

        }
    }

    public void CheckToggles()
    {
        if (angelTog.isOn && !angelToggled)
        {
            CreateAngel();
            angelToggled = true;
        }
        if (devilTog.isOn && !devilToggled)
        {
            CreateDevil();
            devilToggled = true;
        }
        if (doctorTog.isOn && !doctorToggled)
        {
            CreateDoctor();
            doctorToggled = true;
        }
        if (zombieTog.isOn && !zombieToggled)
        {
            CreateZombie();
            zombieToggled = true;
        }
        if (!angelTog.isOn && angelToggled)
        {
            RemoveAngel();
            angelToggled = false;
        }
        if (!devilTog.isOn && devilToggled)
        {
            RemoveDevil();
            devilToggled = false;
        }
        if (!doctorTog.isOn && doctorToggled)
        {
            RemoveDoctor();
            doctorToggled = false;
        }
        if (!zombieTog.isOn && zombieToggled)
        {
            RemoveZombie();
            zombieToggled = false;
        }
    }
    //
    // ABSTRACTION
    //
    void CreateAngel()
    {
        Vector3 angelLocation = new Vector3(8.3f, 4.5f, 3f);
        angel = new Angel("Gabriel", "Heaven", "Messenger Angel", angelLocation);
    }
    void CreateDevil()
    {
        Vector3 demonLocation = new Vector3(28.0f, 4.4f, 3f);
        devil = new Devil("BeetleJuice", "Hell", "Demon", demonLocation);
    }
    void CreateDoctor()
    {
        Vector3 doctorLocation = new Vector3(-9.2f, 4.6f, 3f);
        doctor = new Doctor("Manny", "Earth", "Healer", doctorLocation);
    }
    void CreateZombie()
    {
        Vector3 zombieLocation = new Vector3(-21.1f, 4.2f, 3f);
        zombie = new Zombie("Bud", "Earth", "Brain Eater", zombieLocation);
    }

    //
    // ABSTRACTION
    //
    void RemoveAngel()
    {
        GameObject toRemove;
        toRemove = GameObject.FindGameObjectWithTag("Angel");
        Destroy(toRemove);
    }
    void RemoveDevil()
    {
        GameObject toRemove;
        toRemove = GameObject.FindGameObjectWithTag("Devil");
        Destroy(toRemove);
    }
    void RemoveDoctor()
    {
        GameObject toRemove;
        toRemove = GameObject.FindGameObjectWithTag("Doctor");
        Destroy(toRemove);
    }
    void RemoveZombie()
    {
        GameObject toRemove;
        toRemove = GameObject.FindGameObjectWithTag("Zombie");
        Destroy(toRemove);
    }
    public static void ShowLocation(Entity ent)
    {
        Vector3 inZone = ent.GetEntityLocation();
        Debug.Log($"{ent.Name} found at {inZone} and entity is {ent.Talent}");
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
    }
}
