using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Inventory : MonoBehaviour
{
    private NPCPreferenceScriptable prefs;
    private List<BookScript> holdingBooks;

    // Start is called before the first frame update
    void Start()
    {
        prefs = GetComponent<NPCPreferenceScriptable>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GiveBook(BookScript theBook) // gives a book to the npc
    {
        holdingBooks.Add(theBook); //adds book to inventory

        int numGenres = 0;
        int bookfavor = 0;
        foreach (string genre in theBook.bookGenres) // calculates favorability (0-5)
        {
            numGenres++;
            bookfavor += prefs.CheckGenrePrefs(genre);
        }
        bookfavor = bookfavor / numGenres;
    }
}

