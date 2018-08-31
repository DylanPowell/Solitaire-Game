using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrderedDeck : MonoBehaviour {

	[SerializeField] private List<Card> _cards = new List<Card>();

	[SerializeField] private Vector3 _offSet;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool TryAdd(Card card)
	{
		if (_cards.Count == 0 && card.rank != Card.Rank.Ace)
		{
			return false;		
		}

		if (_cards.Count > 0)
		{
			Card lastCard = _cards [_cards.Count - 1];

			if (lastCard.suit != card.suit)
			{
				return false;		
			}

			if(lastCard.rank != card.rank - 1)
			{
				return false;		
			}
		}

		card.transform.SetParent (this.transform);
		card.transform.localPosition = _offSet * _cards.Count;

		_cards.Add (card);

		return true;
	}

	public bool Contains(Card card)
	{
		return _cards.Contains (card);
	}
}

