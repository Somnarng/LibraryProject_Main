using System.Collections.Generic;
using UnityEngine;
//implemented IDataPersitence interface for calling save/load on scene changes
public class InventoryManager : MonoBehaviour, IDataPersistence
{
    //shift these to a list of book scripts later
    public List<BookScript> UnsortedBooks;
    public List<BookScript> BooksInInventory;
    public bool debugInventory; //toggle false on if you want to save the inventory or true to always keep the base 6 books

    public float money;

    public List<ShelfInteract> shelves;

    // Start is called before the first frame update
    void Start()
    {
        tempLoadBooks(); //replaced the instance books with actual SOs, instanced SOs can't be saved and therefore cannot be used for testing the save system.
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void tempLoadBooks()
    {
        if (UnsortedBooks.Count > 0 && BooksInInventory.Count > 0) { return; }
        else
        {
            for (int oo = 0; oo < 7; oo++)
            {
                //Debug.Log("created book! " + oo);
                BookScript book = BookScript.CreateInstance<BookScript>();
                book.name = "Book" + oo;
                book.bookName = "Book " + oo;

                book.basePrice = oo;
                book.currentPrice = oo + 1;

                book.bookDesc = "This is book " + oo;

                book.bookGenres.Add("genre1");
                book.bookGenres.Add("genre2");

                UnsortedBooks.Add(book);
                BooksInInventory.Add(book);

            }
        }
    }

    public void LoadData(PlayerStats data) //called on scene changes or if a generic "load" function is called on the data persitence script
    {
        foreach (KeyValuePair<int, ShelfInteract> iSI in data.ShelfData) //searches through the dictionary list for every shelfdata saved, compares to current shelfdata on inventory manager.
        {
            foreach (ShelfInteract shelfInteract in shelves)
            {
                data.ShelfData.TryGetValue(iSI.Key, out ShelfInteract shelfData);
                if (shelfInteract.shelfID != shelfData.shelfID) { return; } //there is some null ref exception here that is causing the entire save to not work and I DONT KNOW WHY PLEASE GOD JUST SMITE THIS STUPID EXCEPTION
                else
                {
                    shelfInteract.shelvedBooks = shelfData.shelvedBooks;
                    shelfInteract.shelfState = shelfData.shelfState;
                    shelfInteract.bookMovements = shelfData.bookMovements;
                    Debug.Log("Shelfdata loaded " + shelfInteract.shelfID);//if the shelf data id matches, the saved data is written onto the shelf.
                }
            }
        }

        if (debugInventory) { return; }
        UnsortedBooks = data.UnsortedBooks;
        BooksInInventory = data.BooksInInventory;
    }

    public void SaveData(ref PlayerStats data) //called before scenes deload to keep data between scene movement, also called when the day is progressed through sleeping
    {
        foreach (ShelfInteract SI in shelves) //for every shelf in the shelves list, remove any existing data from the dictionary and re-add the updated data.
        {
            if (data.ShelfData.ContainsKey(SI.shelfID))
            {
                data.ShelfData.Remove(SI.shelfID);
            }
            data.ShelfData.Add(SI.shelfID, SI);
        }

        if (debugInventory) { return; }
        data.UnsortedBooks = UnsortedBooks;
        data.BooksInInventory = BooksInInventory;
    }
}
