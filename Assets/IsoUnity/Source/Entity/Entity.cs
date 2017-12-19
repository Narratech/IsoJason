﻿using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Entity : MonoBehaviour {
	/// <summary>
	/// Used to know if you can be blocked in paths
	/// </summary>
	public bool canBlockMe = true;
	/// <summary>
	/// Used to know if this blocks
	/// </summary>
	public bool blocks = true;
	/// <summary>
	/// Max cell difference jump
	/// </summary>
	public float maxJumpSize = 1.5f;
	/// <summary>
	/// The direction.
	/// </summary>
	public Mover.Direction direction;
    /// <summary>
    /// Normal sprite used for standard animations
    /// </summary>
	public IsoDecoration normalSprite;
    /// <summary>
    /// Sprite used for jump animations
    /// </summary>
	public IsoDecoration jumpingSprite;
    /// <summary>
    /// Face used for dialogs
    /// </summary>
	public Texture2D face;
    /// <summary>
    /// ASL Agent's name
    /// </summary>
	public string entityName;

    [SerializeField]
	private Cell position;
	public Cell Position {
		get{
			return position;
		}
		set {
			position = value;
			this.transform.parent = position.transform;
			my_transform.position = position.transform.position + new Vector3(0, position.WalkingHeight + my_transform.localScale.y/2f, 0);
		}
	}

	public void tick(){
		foreach(EntityScript es in this.GetComponents<EntityScript>())
			es.tick();
	}

	public void eventHappened(GameEvent ge){
		EntityScript[] scripts = this.GetComponents<EntityScript>();

		//TODO Preference system

		foreach(EntityScript es in scripts)
			es.eventHappened(ge);
	}

	public Option[] getOptions(){
		EntityScript[] scripts = this.GetComponents<EntityScript>();
		List<Option> options = new List<Option>();
		
		foreach(EntityScript es in scripts)
			options.AddRange (es.getOptions());

		return options.ToArray() as Option[];
	}

	// Use this for initialization
	void Start () {
        if (Application.isPlaying) {
            Mover mover = this.gameObject.AddComponent<Mover>();
            mover.canBlockMe = canBlockMe;
            mover.blocks = blocks;
            mover.maxJumpSize = maxJumpSize;
            mover.direction = direction;
            mover.normalSprite = normalSprite;
            mover.jumpingSprite = jumpingSprite;
        }
    }

    [SerializeField]
    Transform my_transform;
	public Decoration decoration {
		get{
			return this.GetComponent<Decoration>();
		}
	}

	public Mover mover {
		get{
			return this.GetComponent<Mover>();
		}
	}

	// Update is called once per frame
	void Update () {
		if(my_transform ==null)
			my_transform = this.transform;

		if(!Application.isPlaying && Application.isEditor){

			this.decoration.Tile = ((int)direction)*this.decoration.IsoDec.nCols;

			Transform parent = my_transform.parent;
			Transform actual = null;
			if(position != null)
				actual = position.transform;

			if(parent != actual){
				Cell probablyParent = parent.GetComponent<Cell>();
				if(probablyParent!=null)
					position = probablyParent;
				else if(actual!=null)
					my_transform.parent = actual;

			}

			if(this.position != null){
				my_transform.position = position.transform.position + new Vector3(0, position.WalkingHeight + my_transform.localScale.y/2f, 0);
			}
		}


	}

	public void onDestroy(){

	}
}
