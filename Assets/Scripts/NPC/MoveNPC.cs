﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Conversation;

namespace NPC
{
	public class MoveNPC : MonoBehaviour
	{
		public Character player;
		// a bool meant to control the event's invoke. 
		public bool isMoving = false;
		public bool playerHere = false;
		public bool isMatched = false;
		public GameObject sparkle1;
		public GameObject sparkle2;
		// Gets the result of two electron charges
		int result = 0;
		bool runOnce = false;


		[ContextMenu("Is the object following the player?")]
		public UnityEvent followMe;
		// Use this for initialization
		void Start()
		{
		}

		// Update is called once per frame
		void Update()
		{
			InvokeFollow();
		}

		void OnTriggerEnter(Collider col)
		{
			if(col.gameObject.tag == "Player")
			{
				playerHere = true;
			}
			if(col.gameObject.tag == "Atom")
			{
				result = this.gameObject.GetComponent<AtomNPC>().electronCharge + col.gameObject.GetComponent<AtomNPC>().electronCharge;
				if(isMoving == true && col.gameObject.GetComponent<AtomNPC>().sentiment == Sentiment.Trusting && col.gameObject.GetComponent<AtomNPC>().state != State.InLove && result == 8)
				{
					this.gameObject.GetComponent<AtomNPC>().state = State.InLove;
					col.gameObject.GetComponent<AtomNPC>().state = State.InLove;
					isMatched = true;
					col.gameObject.GetComponent<MoveNPC>().isMatched = true;
					this.gameObject.GetComponent<AtomNPC>().Match();
					col.gameObject.GetComponent<AtomNPC>().Match();
					if(sparkle1.activeSelf == true)
					{
						Vector3 sparklePos = (this.gameObject.transform.position + col.gameObject.transform.position)/2;
						sparkle2.SetActive(true);
						sparkle2.transform.position = sparklePos;
					}
					else
					{
						Vector3 sparklePos = (this.gameObject.transform.position + col.gameObject.transform.position)/2;
						sparkle1.SetActive(true);
						sparkle1.transform.position = sparklePos;
					}
				}
				if (result != 8 && isMoving && col.gameObject.GetComponent<AtomNPC>().sentiment == Sentiment.Trusting) {
					//Debug.Log("Can't match");
					this.gameObject.GetComponent<AtomNPC> ().convo.conversation = GameObject.Find ("InvalidMatch").GetComponent<Container> ();
					this.gameObject.GetComponent<AtomNPC> ().convo.ResetConversation ();
					this.gameObject.GetComponent<AtomNPC> ().convo.UpdateConversation ();
					this.gameObject.GetComponent<AtomNPC> ().convo.EnableConversation ();
					runOnce = true;
				}
			}
		}


		void OnTriggerExit(Collider col)
		{
			if(col.gameObject.tag == "Player")
			{
				playerHere = false;
			}
			if (col.gameObject.tag == "Atom")
			{
				this.gameObject.GetComponent<AtomNPC> ().convo.LeaveConversation ();
				//runOnce = false;
			}
		}

		/// <summary>
		/// If the distance between the player and the gameObject is too large, transform the gameObject towards the player. 
		/// </summary>
		public void followPlayer()
		{
			// This object's parent is the player.
			if((Vector3.Distance(this.transform.position, player.gameObject.transform.position)) > 1.5f)
			{
				Quaternion startingRotation = transform.rotation;
				transform.LookAt(player.transform.position);
				transform.Translate(Vector3.forward * 0.1f);
				//transform.position.y += Vector3.forward.y * 0.1f;
				transform.rotation = startingRotation;
			}
		}
		void InvokeFollow()
		{
				// If the F key is pressed, the player will carry the object
			if (Input.GetKeyDown(KeyCode.F) && !isMoving && playerHere && !isMatched && this.gameObject.GetComponent<AtomNPC>().sentiment == Sentiment.Trusting) 
			{
				isMoving = true;
                player.startFollowing();
			}
			// if the G key is pressed, the player will release the object.
			else if (Input.GetKeyDown(KeyCode.G) && isMoving == true || isMatched)
			{
				isMoving = false;
                player.stopFollowing();
			}
			// This will invoke the event as long as the player pressed the F key. 
			if (isMoving == true)
			{
				//followMe.Invoke();
				followPlayer ();
			}
			// if the player releases the object, isMoving is then false so the object is allowed to stop. 
		}
		/// <summary>
		/// Checks to see if a match was made between two atoms
		/// </summary>
	}
}