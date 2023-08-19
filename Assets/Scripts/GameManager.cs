using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using static GameManager;
using static UnityEditor.FilePathAttribute;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public GameObject angelPrefabLoad;
    public GameObject devilPrefabLoad;
    public GameObject doctorPrefabLoad;
    public GameObject zombiePrefabLoad;
    public Toggle angelTog;
    public Toggle devilTog;
    public Toggle doctorTog;
    public Toggle zombieTog;

    static GameObject angelPrefab;
    static GameObject devilPrefab;
    static GameObject doctorPrefab;
    static GameObject zombiePrefab;

    public Angel angel;
    public Zombie zombie;
    public Devil devil;
    public Doctor doctor;

    public enum EntityType { Physical, Spiritual };

    //***************************************************
    //
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
                location.y > 0 && location.y < 3 &&
                location.z > -worldBoundary && location.x < worldBoundary)
            {
                return true;
            }
            return false;
        }

        public void Move(Vector3 newLocation)
        {
            //_inCircleZone = true;
            _location = newLocation;
        }
        public override string ToString() => Name;
    }

    //***************************************************
    //
    // Each class below represents each character in game and are inherited from Entity base class. 
    //
    //***************************************************
    public sealed class Angel : Entity
    {
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
    }
    public string Origin { get; }
    public override string ToString() => $"{(string.IsNullOrEmpty(Origin) ? "" : Origin + ", ")}{Name}";
    }

    public sealed class Devil : Entity
    {
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
        }
        public string Origin { get; }
        public override string ToString() => $"{(string.IsNullOrEmpty(Origin) ? "" : Origin + ", ")}{Name}";
    }

    public sealed class Doctor : Entity
    {
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
        }
        public string Origin { get; }
        public override string ToString() => $"{(string.IsNullOrEmpty(Origin) ? "" : Origin + ", ")}{Name}";
    }

    public sealed class Zombie : Entity
    {
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
        }
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

        //ShowLocation(angel);
        //angel.Move(new Vector3());

        //Debug.Log($"{angel.Name} and {devil.Name} are the same entity: " +
        //      $"{((Entity)angel).Equals(devil)}");
        //ShowLocation(devil);
    }

    // Update is called once per frame
    void Update()
    {
        if(angelTog.isOn)
        {
            Vector3 angelLocation = new Vector3(3.2f, 2.8f, 3f);
            angel = new Angel("Gabriel", "Heaven", "Messenger Angel", angelLocation);
        }
        if(devilTog.isOn)
        {
            Vector3 demonLocation = new Vector3(11.0f, 1.8f, 3f);
            devil = new Devil("BeetleJuice", "Hell", "Demon", demonLocation);
        }
        if(doctorTog.isOn)
        {
            Vector3 doctorLocation = new Vector3(-5.2f, 2.3f, 3f);
            doctor = new Doctor("Manny", "Earth", "Healer", doctorLocation);
        }
        if(zombieTog.isOn)
        {
            Vector3 zombieLocation = new Vector3(-9.9f, 2.2f, 3f);
            zombie = new Zombie("Bud", "Earth", "Brain Eater", zombieLocation);
        }
        
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
