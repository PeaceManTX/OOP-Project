using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using static GameManager;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public enum EntityType { Physical, Spiritual, Living, Dead };

    public abstract class Entity
    {
        private bool _inCircleZone = false;  // Check location of entity whether in or out of circle zone
        private Vector3 _location;  // entity location
 

        public Entity(string name, string talent, EntityType type)
        {
            if (string.IsNullOrWhiteSpace(talent))
                throw new ArgumentException("The talent is required.");
            Talent = talent;

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The name is required.");
            Name = name;

            Type = type;
        }

        public string Talent { get; }

        public string Name { get; }

        public EntityType Type { get; }

        public string GetEntityLocation()
        {
            if (!_inCircleZone)
                return "NIZ";
            else
                return _location.ToString(" ");
        }

        public void Move(Vector3 newLocation)
        {
            _inCircleZone = true;
            _location = newLocation;
        }



        public override string ToString() => Name;
    }


public sealed class Angel : Entity
    {
    //public Angel(string name, string origin, string talent) :
     //      this(name, string.Empty, origin, talent)
    //{ }

    public Angel(string name, string origin, string talent) : base(name, talent, EntityType.Spiritual)
    {

            // Ensure Origin is correct
            if (string.IsNullOrWhiteSpace(origin))
                throw new ArgumentException("The origin is required.");
    
        Origin = origin;
    }
    public string Origin { get; }

    public decimal Price { get; private set; }

    // A three-digit ISO currency symbol.
    public string Currency { get; private set; }

    public override string ToString() => $"{(string.IsNullOrEmpty(Origin) ? "" : Origin + ", ")}{Name}";
}
// Start is called before the first frame update
void Start()
    {
        var angel = new Angel("Gabriel", "Heaven","Messenger Angel");
        ShowEntityLocation(angel);
        angel.Move(new Vector3());
        ShowEntityLocation(angel);

        var angel2 = new Angel("Michael", "Heaven", "Warrior Angel");
        Debug.Log($"{angel.Name} and {angel2.Name} are the same entity: " +
              $"{((Entity)angel).Equals(angel2)}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void ShowEntityLocation(Entity ent)
    {
        string inZone = ent.GetEntityLocation();
        Debug.Log($"{ent.Name}, " +
                  $"{(inZone == "NIZ" ? "Not in Zone" : "found at " + inZone)} and entity is {ent.Talent}");
    }
    public void Quit()
    {
        SceneManager.LoadScene(0);
    }
}
