using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//referenced: https://forum.unity.com/threads/how-to-create-ui-button-dynamically.393160/

public class PopUpsManager : MonoBehaviour
{
    private InventoryManager inventory;


    //SHELF EDITING
    [SerializeField] private GameObject shelfEditor;
    //list of sprites to be displayed depending on shelf status
    //[SerializeField] private Image shelfBackground;
    [SerializeField] private List<Button> bookSpaces;
    public string selectedBook;
    private ShelfInteract theShelf;

    [SerializeField] private GameObject UnsortedBookGroup;
    private List<Button> bookButtons = new List<Button>();
    [SerializeField] private Button UnsortedBookButtonPrefab;

    //open the shelf editor
    public void OpenShelf(ShelfInteract ts)
    {
        theShelf = ts;
        shelfEditor.SetActive(true);

        DisplayBooks();
        selectedBook = null;
         
    }

    //close the shelf editor
    public void CloseShelf()
    {
        theShelf = null;
        shelfEditor.SetActive(false);
    }

    
    private void DisplayBooks()
    {
        //Debug.Log("DISPLAY BOOKS");
        //display books depending on shelf's string
        int number = 0;
        foreach (string space in theShelf.shelvedBooks)
        {
            if (space == "BLANK")
            {
                bookSpaces[number].GetComponent<Image>().color = Color.grey;
                bookSpaces[number].GetComponentInChildren<TextMeshProUGUI>().text = "[empty]";
            }
            else
            {
                bookSpaces[number].GetComponent<Image>().color = Color.green;
                bookSpaces[number].GetComponentInChildren<TextMeshProUGUI>().text = space;
            }
            number++;
        }

        //remove old buttons (THIS IS BAD FIX THIS)
        if(bookButtons!= null)
        {
            while(bookButtons.Count!=0)
            {
                Debug.Log("?");
                //e.SetActive(false);
                Button e = bookButtons[0];
                bookButtons.Remove(e);
                Destroy(e.gameObject);
            }
        }

        //display unsorted books
        for(int book = 0; book < inventory.UnsortedBooks.Count; book++)
        {
            Debug.Log("unsorted book " + book);
            Button newButton = null;
            newButton =  Instantiate(UnsortedBookButtonPrefab) as Button;
            newButton.onClick.AddListener(() => this.SelectBook(book));
            newButton.transform.SetParent(UnsortedBookGroup.transform, false);

            
            bookButtons.Add(newButton);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = inventory.UnsortedBooks[book];
        }
        if (bookButtons.Count > inventory.UnsortedBooks.Count)
        {
            Debug.Log("error too long");
            bookButtons.Remove(bookButtons[bookButtons.Count - 1]);
        }
        Debug.Log("check");
    }

    //select a book to be added to the shelf
    public void SelectBook(int which)
    {
        which--;
        Debug.Log("tried to select book!" + which);
        Debug.Log("unsorted books count: " + inventory.UnsortedBooks.Count);
        selectedBook = inventory.UnsortedBooks[which];
        foreach(Button b in bookButtons)
        {
            b.GetComponentInChildren<Image>().color = Color.white;
        }
        bookButtons[which].GetComponentInChildren<Image>().color = Color.cyan;
    }

    //add a book to a space on the shelf
    public void LoadShelf(int which)
    {
        //check if the shelf already has a book
        if (theShelf.shelvedBooks[which] != "BLANK")
        {
            ClearShelf(which);

        }
            //check that a book has been selected to be loaded
            if (selectedBook != null)
            {


                //move book from inventory to shelf
                theShelf.shelvedBooks[which] = selectedBook;
                inventory.UnsortedBooks.Remove(selectedBook);
            selectedBook = null;

            }
        

        DisplayBooks();
    }


    //remove a book from a space on the shelf
    public void ClearShelf(int which)
    {

        inventory.UnsortedBooks.Add(theShelf.shelvedBooks[which]);
        theShelf.shelvedBooks[which] = "BLANK";

        DisplayBooks();
    }


    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindObjectOfType<InventoryManager>();
        //bookButtons = null;
        CloseShelf();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
