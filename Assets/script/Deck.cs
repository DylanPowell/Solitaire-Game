using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Deck : MonoBehaviour
{
	const int NUM_SWAPS = 1000;

	[SerializeField] private List<Card> _cards = new List<Card>(52);
	[SerializeField] private Vector3 _offSet;

	void OnValidate()
	{
		#if UNITY_EDITOR
		if (_cards != null)
		{
//			foreach (Card card in _cards)
//			{
//				float row = (int) card.suit;
//				float column = (int) card.rank;
//
//				card.transform.SetParent (this.transform);
//				card.transform.localPosition = new Vector3 (column, row, 0);
//			}
			PileCards();
		}
		#endif
	}

	void Start()
	{
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.W))
		{
			Shuffle();
		}
	}

	public void PileCards()
	{
		for (int i = 0; i < _cards.Count; i++)
		{
			_cards[i].transform.SetParent (this.transform);
			_cards[i].transform.localPosition = _offSet * i;
		}
	}

	public void Shuffle()
	{
		if (_cards.Count <= 0) {
			return;
		}


		int swaps = 0;
		while (swaps < NUM_SWAPS)
		{
			var random1 = UnityEngine.Random.Range (0, _cards.Count);
			var random2 = UnityEngine.Random.Range (0, _cards.Count);

			var card1 = _cards [random1];
			var card2 = _cards [random2];

			_cards [random1] = card2;
			_cards [random2] = card1;

			swaps++;
		}
	}

	public void Add(Card card)
	{
		card.transform.SetParent (this.transform);
		card.transform.localPosition = _offSet * _cards.Count;

		_cards.Add (card);
	}

	public Card RemoveTopCard()
	{
		Card card = _cards[_cards.Count - 1];

		_cards.Remove (card);

		return card;
	}

	public int Count {
		get{ return _cards.Count; }
	}

	public Card Peek(int index)
	{
		if (index >= 0 && index < _cards.Count)
		{
			return _cards [index];
		}

		return null;
	}

	public bool Contains(Card card)
	{
		return _cards.Contains (card);
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

	public Action<Deck> OnUserClick = delegate {};
	void OnMouseDown()
	{
		UnityEngine.Debug.Log ("Clicked The Empty Deck");
		OnUserClick (this);
	}
}

