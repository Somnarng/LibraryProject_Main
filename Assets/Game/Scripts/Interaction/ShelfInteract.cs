using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShelfInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private bool interactable;
    [SerializeField] private string interactText;
    public int shelfID; //MUST BE UNIQUE between shelves, used to save/load shelf data.

    //reference relevant managers
    private InventoryManager inventory;
    private ShelfMenuManager shelfMenu;

    //lists for books (and blank spaces when shelf is empty)
    public List<BookScript> shelvedBooks;
    public List<BookScript> blankBooks;

    //shelf state management
    public int shelfState = 0;
    public int bookMovements = 0;

    //[SerializeField] BookScript blankBook;
    private ShelfInteract shelfData;

    void Start()
    {
        inventory = GameObject.FindObjectOfType<InventoryManager>();
        shelfMenu = GameObject.FindObjectOfType<ShelfMenuManager>();

        //LOAD BLANK SPACES INTO SHELF
        if (shelvedBooks == null || shelvedBooks.Count == 0) //a check to ensure no books are already loaded on the shelves
        {
            for (int b = 0; b < blankBooks.Count; b++)
            {
                shelvedBooks.Add(blankBooks[b]);
            }
        }
    }

    public void enBlankenSpace(int which)
    {
        //Debug.Log("enblanken " + which);

        shelvedBooks[which] = blankBooks[which];

        //if all spots are clear, shelf is not disorganized (change shelfstate)
        bool allClear = true;
        foreach (BookScript b in shelvedBooks)
        {
            if (b.bookName != "BLANK")
            {
                //Debug.Log("clear fail");
                allClear = false;
            }
        }
        Debug.Log("all clear: " + allClear);
        if (allClear)
        {
            Debug.Log("clear?");
            ChangeShelfState(0);
        }
    }

    //book movements add up to a shelf state change
    public void IncrementBookMovement()
    {
        //arbitrary cap here, remove if statement if more states are added
        if (shelfState == 0)
        {
            //this could be a boolean, but is a int switch in case we want more states later
            bookMovements++;
            if (bookMovements >= 10)
            {
                int state = shelfState + 1;
                bookMovements = 0;
                ChangeShelfState(state);

            }
        }

    }

    //change appearance based on shelf state
    public void ChangeShelfState(int state)
    {
        Debug.Log("change state: " + state);
        bookMovements = 0;

        switch (state)
        {
            case 0:
                shelfState = 0;
                GetComponentInParent<SpriteRenderer>().color = new Color(.45f, .24f, 0f);
                break;
            case 1:
                shelfState = 1;
                GetComponentInParent<SpriteRenderer>().color = new Color(.4f, .12f, 0f);
                break;
            default:
                shelfState = 0;
                GetComponentInParent<SpriteRenderer>().color = new Color(.45f, .24f, 0f);
                break;
        }
    }

    public void Interact()
    {

        shelfMenu.OpenShelf(this);
    }



    public bool Interactable { get { return interactable; } set { interactable = value; } }
    public string InteractText { get { return interactText; } set { interactText = value; } }
    public Transform objectTransform { get => gameObject.transform; }



    // Update is called once per frame
    void Update()
    {

    }

}
