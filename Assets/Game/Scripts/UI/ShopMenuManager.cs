using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopMenuManager : MonoBehaviour
{
    private InventoryManager inventory;
    private ShopInteract theShop;

    //books that the player has selected to buy
    public List<BookScript> BooksToBuy;

    //UI Panels
    [SerializeField] private GameObject ShopWindow;
    [SerializeField] GameObject NotEnoughMoneyWindow;
    [SerializeField] GameObject PurchaseDisplayWindow;

    [SerializeField] TextMeshProUGUI moneyDisplay;
    [SerializeField] TextMeshProUGUI costDisplay;

    //purchase display stuff
    [SerializeField] GameObject PurchaseDisplayPanel;
    [SerializeField] GameObject PurchaseDisplayPrefab;
    private List<GameObject> purchaseDisplays;

    //open the shop menu based what shop called this
    public void OpenShop(ShopInteract ts)
    {
        theShop = ts;
        ShopWindow.SetActive(true);
        UpdateDisplay();
    }

    //close the shop
    public void CloseShop()
    {
        theShop = null;
        ShopWindow.SetActive(false);
    }

    //select a book
    public void PickBook(BookScript which)
    {
        if (BooksToBuy.Contains(which))
        {
            BooksToBuy.Remove(which);
        }
        else
        {
            BooksToBuy.Add(which);
        }
        UpdateDisplay();
    }

    //update what books are available, selected, etc
    public void UpdateDisplay()
    {
        float z = 0;
        for(int a = 0; a < BooksToBuy.Count; a++)
        {
            float holder = BooksToBuy[a].currentPrice;
            z += holder;
        }
        costDisplay.text = "Total: " + z.ToString();
        moneyDisplay.text = inventory.money.ToString();

    }

    //attempt to purchase the books
    public void BuyBooks()
    {
        float checkouttotal = 0;
        foreach(BookScript purchase in BooksToBuy)
        {
            checkouttotal += purchase.currentPrice;
        }

        if (inventory.money < checkouttotal)
        {
            NotEnoughMoneyWindow.SetActive(true);

            BooksToBuy.RemoveRange(0, BooksToBuy.Count - 1);

        }
        else
        {
            inventory.money -= checkouttotal;
            DisplayPurchases();
        }
    }

    //show the player the purchases made
    public void DisplayPurchases()
    {
        PurchaseDisplayWindow.SetActive(true);

        foreach(BookScript book in BooksToBuy)
        {
            //create a image showing bought book
            GameObject bookDis = null;
            bookDis = Instantiate(PurchaseDisplayPrefab);
            bookDis.transform.SetParent(PurchaseDisplayPanel.transform, false);

            //show the purchase details
            purchaseDisplays.Add(bookDis);
            bookDis.GetComponentInChildren<TextMeshProUGUI>().text = book.bookName;

            //shuffle the book around
            inventory.BooksInInventory.Add(book);
            inventory.UnsortedBooks.Add(book);
            theShop.availableBooks.Remove(book);
        }

        //remove bought books from selected list
        BooksToBuy.RemoveRange(0, BooksToBuy.Count - 1);

    }

    //exit any popups
    public void ExitPopups()
    {
        NotEnoughMoneyWindow.SetActive(false);
        PurchaseDisplayWindow.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
        inventory = GameObject.FindObjectOfType<InventoryManager>();

        ExitPopups();
        CloseShop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
