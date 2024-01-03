using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //shift these to a list of book scripts later
    public List<BookScript> UnsortedBooks;
    public List<BookScript> BooksInInventory;

    public int money;

    public List<ShelfInteract> shelves;

    // Start is called before the first frame update
    void Start()
    {
        tempLoadBooks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void tempLoadBooks()
    {
        
        for(int oo = 0; oo<7; oo++)
        {
            Debug.Log("created book! " + oo);
            BookScript book = BookScript.CreateInstance<BookScript>();
            book.name = "Book" + oo;
            book.bookName = "Book " + oo;
            UnsortedBooks.Add(book);
            BooksInInventory.Add(book);

            /**
            UnsortedBooks.Add("Book " + oo);
            BooksInInventory.Add("Book " + oo);**/
        }
    }
}
