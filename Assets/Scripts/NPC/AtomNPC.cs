using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using Conversation;

namespace NPC
{
	/// <summary>
	/// Sentiment towards player
	/// </summary>
	public enum Sentiment
	{
		NeverMet,
		Met,
		Trusting
	}

	/// <summary>
	/// An atom's state
	/// </summary>
	public enum State
	{
		Neutral,
		Excited,
		InLove
	}

	[Serializable]
	public class AtomStateChanged : UnityEvent<AtomNPC, State> { }

	[Serializable]
	public class AtomSentimentChanged : UnityEvent<AtomNPC, Sentiment> { }

	public class AtomNPC : MonoBehaviour
	{
		// Name of the NPC
		public string NPC_Name = "A Generic Atom";

		#region State
		[SerializeField]
		public State state;
		public AtomStateChanged onStateChange;
		public State State
		{
			get { return state; }
			set
			{
				state = value;
				onStateChange.Invoke(this, value);
			}
		}
		#endregion

		#region Sentiment
		[SerializeField]
		public Sentiment sentiment;
		public AtomSentimentChanged onSentimentChange;
		public Sentiment Sentiment
		{
			get { return sentiment; }
			set
			{
				sentiment = value;
				onSentimentChange.Invoke(this, value);
			}
		}
		#endregion

		// working around the broken conversation code for now...
		string option;

        public Converser convo;

        public SpeechBubbleChanger sbc;
				
		bool isTrusting = false;
		bool convoOpen = false;
		bool isFollowing = false;

		// A integer value that holds the electron charge of the atomNPC
		// Right now I'm just setting it to where 8 = perfect. Anything else = failure.
		public int electronCharge;

		void Start()
		{
			// if convo, set name
            this.CheckComponent(ref convo);
				if (convo.Name.Length == 0) convo.Name = NPC_Name;

                this.CheckComponentInChildren(ref sbc);
		}

		void Update()
		{
			// Edited for the sake of having a conversation system
			option = convo.texter;
		}

        void OnTriggerEnter(Collider col)
        {
            if (col.IsPlayer())
            {
                sbc.gameObject.SetActive(true);
                col.GetComponent<Character>().setupUI(sentiment);
            }
        }

        void OnTriggerStay(Collider col)
        {
			if (col.IsPlayer () && Input.GetKeyDown (KeyCode.E) && convoOpen == false) {
				convo.EnableConversation ();
				col.gameObject.GetComponent<Character> ().EnableConversation ();
				convoOpen = true;
			}
			if (col.IsPlayer () && Input.GetKeyDown (KeyCode.F) && !convoOpen && isTrusting) {
				convo.conversation = GameObject.Find("Follow").GetComponent<Container>();
				convo.ResetConversation ();
				convo.UpdateConversation();
				convo.EnableConversation ();
				col.gameObject.GetComponent<Character> ().EnableConversation ();
				isFollowing = true;
			}
			if (col.IsPlayer () && Input.GetKeyDown (KeyCode.G) && isFollowing) {
				convo.conversation = GameObject.Find("Drop").GetComponent<Container>();
				convo.ResetConversation ();
				convo.UpdateConversation();
				convo.EnableConversation ();
				col.gameObject.GetComponent<Character> ().EnableConversation ();
				convoOpen = true;
				isFollowing = false;
			}
			else {
				if (option == "->" && col.IsPlayer()) 
				{
					convo.LeaveConversation ();
					col.gameObject.GetComponent<Character>().LeaveConversation();
					convo.ResetConversation ();
					convo.UpdateConversation();
					convoOpen = false;
					convo.texter = null;
				}
				/*
				else if (option == "I'll do that." && col.IsPlayer()) 
				{
					convo.LeaveConversation ();
					col.gameObject.GetComponent<Character>().LeaveConversation();
					convo.ResetConversation ();
					convo.UpdateConversation();
					convoOpen = false;
					convo.texter = null;
				}
				else if (option == "Thanks for the Info!" && col.IsPlayer()) 
				{
					convo.LeaveConversation ();
					col.gameObject.GetComponent<Character>().LeaveConversation();
					convo.ResetConversation ();
					convo.UpdateConversation();
					convoOpen = false;
					convo.texter = null;
				}
				*/
			}
			conversationManager ();


        }

        void OnTriggerExit(Collider col)
        {
            if (col.IsPlayer())
            {
                col.GetComponent<Character>().teardownUI();
                if (convoOpen == true)
                {
                    convo.LeaveConversation();
                    convo.ResetConversation();
                    col.gameObject.GetComponent<Character>().LeaveConversation();
                    convo.UpdateConversation();
                    convoOpen = false;
                }
            }
			if(state != State.InLove)
			{
				sbc.gameObject.SetActive (false);
			}
		}

		// Only made because I needed a quick workaround to "When Chosen" - Sung
		void conversationManager()
		{
			// Conversation placeholder, manages state change
			// This way is abhorant but it's the best we can do right now. 
			if (option == "Balloons")
			{
				sentiment = Sentiment.Trusting;
			} 
			else if (option == "1") 
			{
				sentiment = Sentiment.Trusting;
			}
			else if (option == "Swimming Pools")
			{
				sentiment = Sentiment.Trusting;
			} 
			else if (option == "Noble") 
			{
				sentiment = Sentiment.Trusting;
			}
			else if (option == "Mercury") 
			{
				sentiment = Sentiment.Trusting;
			}
			else if (option == "Albert Einstein") 
			{
				sentiment = Sentiment.Trusting;
			}
			else if (option == "Metal") 
			{
				sentiment = Sentiment.Trusting;
			}

			// Change the conversations around
			if (sentiment == Sentiment.Trusting) 
			{
				if(isTrusting == false && this.gameObject.name == "Atom")
				{
					sbc.SetAlternating(SpeechBubbleState.Exclaim, "F");
					convo.conversation = GameObject.Find("Generic Trust1").GetComponent<Container>();
					isTrusting = true;
				}
				else if(isTrusting == false && this.gameObject.name == "Atom2")
				{
					sbc.SetAlternating(SpeechBubbleState.Exclaim, "F");
					convo.conversation = GameObject.Find("Generic Trust2").GetComponent<Container>();
					isTrusting = true;
				}
				else if(isTrusting == false && this.gameObject.name == "Atom3")
				{
					sbc.SetAlternating(SpeechBubbleState.Exclaim, "F");
					convo.conversation = GameObject.Find("Generic Trust3").GetComponent<Container>();
					isTrusting = true;
				}
				else if(isTrusting == false && this.gameObject.name == "Atom4")
				{
					sbc.SetAlternating(SpeechBubbleState.Exclaim, "F");
					convo.conversation = GameObject.Find("Generic Trust4").GetComponent<Container>();
					isTrusting = true;
				}
				else if(isTrusting == false && this.gameObject.name == "Atom5")
				{
					sbc.SetAlternating(SpeechBubbleState.Exclaim, "F");
					convo.conversation = GameObject.Find("Generic Trust4").GetComponent<Container>();
					isTrusting = true;
				}
				else if(isTrusting == false && this.gameObject.name == "Atom6")
				{
					sbc.SetAlternating(SpeechBubbleState.Exclaim, "F");
					convo.conversation = GameObject.Find("Generic Trust1").GetComponent<Container>();
					isTrusting = true;
				}
				else if(isTrusting == false && this.gameObject.name == "Atom7")
				{
					sbc.SetAlternating(SpeechBubbleState.Exclaim, "F");
					convo.conversation = GameObject.Find("Generic Trust3").GetComponent<Container>();
					isTrusting = true;
				}
			}
		}

		public void Match()
		{
			sbc.SetState (SpeechBubbleState.Heart);
			sbc.text = "";
			sbc.gameObject.SetActive(true);
		}
	}
}
