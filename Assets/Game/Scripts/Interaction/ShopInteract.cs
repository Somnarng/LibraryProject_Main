using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private bool interactable;
    [SerializeField] private string interactText;

    //list of all books that the shop may have, list of books currently available
    [SerializeField] List<BookScript> potentialBooks;
    public List<BookScript> availableBooks;

    //idk if we want this one
    public List<BookScript> soldBooks;

    //reference relevant managers
    private InventoryManager inventory;
    private ShopMenuManager ShopMenu;

    public void Interact()
    {

        ShopMenu.OpenShop(this);
    }



    public bool Interactable { get { return interactable; } set { interactable = value; } }
    public string InteractText { get { return interactText; } set { interactText = value; } }
    public Transform objectTransform { get => gameObject.transform; }

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindObjectOfType<InventoryManager>();
        ShopMenu = GameObject.FindObjectOfType<ShopMenuManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
