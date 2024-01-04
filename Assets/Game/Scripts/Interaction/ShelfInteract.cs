using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShelfInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private bool interactable;
    [SerializeField] private string interactText;

    //reference relevant managers
    private InventoryManager inventory;
    private ShelfMenuManager shelfMenu;

    //lists for books (and blank spaces when shelf is empty)
    public List<BookScript> shelvedBooks;
    public List<BookScript> blankBooks;

    //shelf state management
    public int shelfState;
    public int bookMovements;

    //[SerializeField] BookScript blankBook;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindObjectOfType<InventoryManager>();
        shelfMenu = GameObject.FindObjectOfType<ShelfMenuManager>();

        shelfState = 0;
        bookMovements = 0;

        //LOAD BLANK SPACES INTO SHELF
        for (int b = 0; b < blankBooks.Count; b++){
            shelvedBooks.Add(blankBooks[b]);
        }

    }

    public void enBlankenSpace(int which)
    {
        Debug.Log("enblanken " + which);
        
        shelvedBooks[which] = blankBooks[which];

        //if all spots are clear, shelf is not disorganized (change shelfstate)
        bool allClear = true;
        foreach(BookScript b in shelvedBooks)
        {
            if(b.bookName != "BLANK")
            {
                allClear = false;
            }
        }
        if (allClear)
        {
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
                shelfState++;
                bookMovements = 0;
                ChangeShelfState(shelfState);
                
            }
        }

    }

    //change appearance based on shelf state
    public void ChangeShelfState(int state)
    {
        bookMovements = 0;

        switch (shelfState)
        {
            case 0:
                GetComponentInParent<Image>().color = new Color(115, 62, 1);
                break;
            case 1:
                GetComponentInParent<Image>().color = new Color(102, 31, 1);
                break;
            default:
                shelfState = 0;
                GetComponentInParent<Image>().color = new Color(115, 62, 1);
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
