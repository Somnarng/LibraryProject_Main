using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//referenced: https://forum.unity.com/threads/how-to-create-ui-button-dynamically.393160/

public class ShelfMenuManager : MonoBehaviour
{
    private InventoryManager inventory;


    //SHELF EDITING
    [SerializeField] private GameObject shelfEditor;

    //list of sprites to be displayed depending on shelf status
    [SerializeField] private Image shelfBackground;

    [SerializeField] private List<Button> bookSpaces;
    public BookScript selectedBook;
    private ShelfInteract theShelf;

    [SerializeField] private GameObject UnsortedBookGroup;
    [SerializeField] List<Button> bookButtons = new List<Button>();
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
        //displays the shelf's state
        switch (theShelf.shelfState)
        {
            case 0:
                shelfBackground.color = new Color(.45f, .24f, 0f);
                Debug.Log("color change 1");
                break;
            case 1:
                shelfBackground.color = new Color(.4f, .12f, 0f);
                Debug.Log("color change 2");
                break;
            default:
                shelfBackground.color = new Color(.45f, .24f, 0f);
                Debug.Log("POPUPMANAGER SHELFSTATE FAILURE");
                break;
        }


        //display books depending on shelf's string
        int number = 0;
        foreach (BookScript space in theShelf.shelvedBooks)
        {
            if (space.bookName == "BLANK")
            {
                bookSpaces[number].GetComponent<Image>().color = Color.grey;
                bookSpaces[number].GetComponentInChildren<TextMeshProUGUI>().text = "[empty]";
            }
            else
            {
                bookSpaces[number].GetComponent<Image>().color = Color.green;
                bookSpaces[number].GetComponentInChildren<TextMeshProUGUI>().text = space.bookName;
            }
            number++;
        }

        //remove old buttons (THIS IS BAD FIX THIS)
        if(bookButtons.Count!=0)
        {
            //bookButtons.RemoveRange(0, bookButtons.Count - 1);

            while (bookButtons.Count != 0)
            {
                Button e = bookButtons[0];
                bookButtons.Remove(e);
                Destroy(e.gameObject);
            }
        }

        //display unsorted books
 
        for(int book = 0; book < inventory.UnsortedBooks.Count; book++)
        {
            Button newButton = null;
            newButton = Instantiate(UnsortedBookButtonPrefab);
            newButton.transform.SetParent(UnsortedBookGroup.transform, false);
            
            bookButtons.Add(newButton);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = inventory.UnsortedBooks[book].bookName;
        }

        int a = 0;
        foreach(Button aButton in bookButtons)
        {
            int z = a;
            aButton.onClick.AddListener(() => SelectBook(z));
            a++;
        }

        if (bookButtons.Count > inventory.UnsortedBooks.Count)
        {
            Debug.Log("error too long");
            bookButtons.Remove(bookButtons[bookButtons.Count - 1]);
        }
        
    }

    //select a book to be added to the shelf
    public void SelectBook(int which)
    {
        //Debug.Log("which = " + which);
        Debug.Log("tried to select book! " + inventory.UnsortedBooks[which].bookName +" : "+ which); ;
        // Debug.Log("unsorted books count: " + inventory.UnsortedBooks.Count);
        
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
        if (theShelf.shelvedBooks[which].bookName != "BLANK")
        {
            ClearShelf(which);

        }
        //check that a book has been selected to be loaded
        if (selectedBook != null)
        {
            //move book from inventory to shelf
            theShelf.shelvedBooks[which] = selectedBook;
            theShelf.shelvedBookIDs[which] = selectedBook.bookId;
            inventory.UnsortedBooks.Remove(selectedBook);
            selectedBook = null;

            //REMOVABLE 
            theShelf.IncrementBookMovement();

         }
        

        DisplayBooks();
    }


    //remove a book from a space on the shelf
    public void ClearShelf(int which)
    {
        
        inventory.UnsortedBooks.Add(theShelf.shelvedBooks[which]);
        theShelf.enBlankenSpace(which);

        //REMOVABLE 
        theShelf.IncrementBookMovement();

        DisplayBooks();
    }


    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindObjectOfType<InventoryManager>();
        
        CloseShelf();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
