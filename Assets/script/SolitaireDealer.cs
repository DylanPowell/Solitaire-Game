using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SolitaireDealer : MonoBehaviour
{
	[SerializeField] Deck _mainDeck;
	[SerializeField] Deck _discardDeck;
	[SerializeField] List<Deck> _piles = new List<Deck>(7);
	[SerializeField] List<OrderedDeck> _ordered = new List<OrderedDeck>(4);
	// Use this for initialization
	void Start ()
	{
		Random.seed = (int)System.DateTime.Now.Ticks;

		for (int i = 0; i < _mainDeck.Count; i++)
		{
			_mainDeck.Peek(i).OnUserClick += OnCardSelected;
		}

		_mainDeck.OnUserClick += OnMainDeckClicked;

		foreach (Deck deck in _piles)
		{
			deck.OnUserClick += OnPileClicked;
		}

		Deal ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Deal()
	{
		_mainDeck.PileCards ();

		int startIndex = 0;

		while (startIndex < _piles.Count)
		{
			int currIndex = startIndex;

			while (currIndex < _piles.Count)
			{
				Card card = _mainDeck.RemoveTopCard ();
				card.showing = (currIndex == startIndex);

				_piles [currIndex].Add (card);

				currIndex++;
			}

			startIndex++;
		}
	}

	void MoveCardToDeck(Card card, Deck deck)
	{
		if (_discardDeck.Contains (_selectedCard))
		{
			_discardDeck.RemoveTopCard ();
			deck.Add (_selectedCard);
			_selectedCard = null;

			return;
		}

		foreach (Deck selectedDeck in _piles)
		{
			if (selectedDeck.Contains (_selectedCard))
			{
				Stack<Card> cardsToMove = new Stack<Card> ();

				Card topCard = null;

				while (topCard != _selectedCard)
				{
					topCard = selectedDeck.RemoveTopCard ();
					cardsToMove.Push (topCard);
				}

				while (cardsToMove.Count > 0)
				{
					deck.Add (cardsToMove.Pop());
				}

				_selectedCard = null;

				return;
			}
		}
	}

	void OnMainDeckClicked(Deck deck)
	{
		if (deck.Count <= 0) {
			
			while (_discardDeck.Count > 0) {
				var card = _discardDeck.RemoveTopCard ();
				card.showing = false;
				deck.Add (card);
			}
		}
		LogSelected ();
	}

	void OnPileClicked(Deck clickedDeck)
	{
		if (clickedDeck.Count <= 0 && _selectedCard != null && _selectedCard.rank == Card.Rank.King)
		{
			MoveCardToDeck (_selectedCard, clickedDeck);
		}

		LogSelected ();
	}

	void LogSelected()
	{
		if (_selectedCard) {
			Debug.Log ("Selected = " + _selectedCard.rank + " of " + _selectedCard.suit + " clicked");
		} else {
			Debug.Log ("Selected = null");
		}
	}

	void OnCardSelected(Card selectedCard)
	{
		if (_mainDeck.Contains (selectedCard)) 
		{
			HandleMainDeckClick (selectedCard);
			LogSelected ();
			return;
		}

		if (_discardDeck.Contains (selectedCard)) 
		{
			HandleDiscardDeckClick (selectedCard);
			LogSelected ();
			return;
		}

		foreach (Deck deck in _piles)
		{
			if (deck.Contains (selectedCard))
			{
				HandleDeckClick (selectedCard, deck);
				LogSelected ();
				return;
			}
		}

		foreach (OrderedDeck deck in _ordered)
		{
			if (deck.Contains (selectedCard))
			{
				HandleOrderedDeckClick (selectedCard, deck);
				LogSelected ();
				return;
			}
		}
	}

	[SerializeField] Card _selectedCard = null;

	void HandleMainDeckClick(Card card)
	{
		_selectedCard = null;
		_mainDeck.RemoveTopCard ();
		card.showing = true;
		_discardDeck.Add (card);


	}

	void HandleDiscardDeckClick(Card card)
	{
		_selectedCard = _discardDeck.Peek (_discardDeck.Count - 1);
	}

	void HandleDeckClick(Card clickedCard, Deck clickedDeck)
	{
		if (clickedCard == _selectedCard)
		{
			return; 
		}
			
		Card topCard = clickedDeck.Peek (clickedDeck.Count - 1);

		//if we want to turn over the top card
		if (clickedCard == topCard && !clickedCard.showing)
		{
			clickedCard.showing = true;
			return;
		}
			
		// if we click on an Ace, send it to an ordered deck
		if (clickedCard == topCard && clickedCard.rank == Card.Rank.Ace)
		{
			clickedDeck.RemoveTopCard ();

			foreach (OrderedDeck orderedDeck in _ordered)
			{
				if (orderedDeck.TryAdd (clickedCard))
				{
					return;
				}
			}
		}

		// if we havent selected a card, select this card
		if (_selectedCard == null && clickedCard.showing == true)
		{
			_selectedCard = clickedCard;
			return; 
		}


		// if we have a card selected check to see that we can move the selected card to this card
		if (_selectedCard != null && clickedCard.showing == true && clickedCard == topCard)
		{
			if (_selectedCard.IsRed != clickedCard.IsRed && _selectedCard.rank == clickedCard.rank - 1)
			{
				MoveCardToDeck (_selectedCard, clickedDeck);
			}
		}
	}

	void HandleOrderedDeckClick(Card clickedCard, OrderedDeck clickedDeck)
	{
		if (_selectedCard != null)
		{
			Deck selectedDeck = null;

			if (_discardDeck.Contains (_selectedCard))
			{
				selectedDeck = _discardDeck;
			}

			foreach (Deck pile in _piles)
			{
				if (pile.Contains (_selectedCard))
				{
					selectedDeck = pile;

					break;
				}
			}

			if (selectedDeck != null)
			{
				Card topCard = selectedDeck.Peek (selectedDeck.Count - 1);

				if (_selectedCard == topCard && _selectedCard.showing && clickedDeck.TryAdd(_selectedCard))
				{
					selectedDeck.RemoveTopCard ();
				}
			}
		}

		_selectedCard = null;
		LogSelected ();
	}
}
