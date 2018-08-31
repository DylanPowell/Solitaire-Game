using UnityEngine;
using System.Collections;
using System;

public class Card : MonoBehaviour
{
	public enum Suit
	{
		Clubs,
		Spades,
		Hearts,
		Diamonds
	}
	public enum Rank
	{
	 	Ace,
		Two,
		Three,
		Four,
		Five,
		Six,
		Seven,
		Eight,
		Nine,
		Ten,
		Jack,
		Queen,
		King
	}

	private bool _showing;
	public bool showing
	{
		get { return _showing; }
		set
		{
			_showing = value;
			GetComponent<Animator>().SetBool ("show", _showing);
		}
	}

	public Suit suit;
	public Rank rank;

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Q))
		{
			showing = !showing;
		}
	}

	void OnValidate()
	{
		gameObject.name = suit.ToString () + "_" + rank.ToString ();
	}

	public Action<Card> OnUserClick = delegate {};
	void OnMouseDown()
	{
		OnUserClick (this);
	}

	public bool IsRed
	{
		get { return (suit == Suit.Diamonds || suit == Suit.Hearts);}
	}
}
