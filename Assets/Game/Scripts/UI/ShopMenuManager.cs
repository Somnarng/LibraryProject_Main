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
    public List<BookScript> BooksToBuy = new List<BookScript>();

    //UI Panels
    [SerializeField] private GameObject ShopWindow;
    //[SerializeField] GameObject NotEnoughMoneyWindow;
    //[SerializeField] GameObject PurchaseDisplayWindow;

    [SerializeField] TextMeshProUGUI moneyDisplay;
    [SerializeField] TextMeshProUGUI costDisplay;

    //purchase display stuff
    [SerializeField] GameObject PurchaseDisplayPanel;
    [SerializeField] GameObject InventoryDisplayPanel;
    [SerializeField] GameObject ShopDisplayPanel;
    //[SerializeField] GameObject PurchaseDisplayPrefab;

    [SerializeField] Button ShopButtonPrefab;

    //lists track placement of buttons
    private List<Button> purchaseDisplays = new List<Button>();
    private List<Button> inventoryDisplays = new List<Button>();
    private List<Button> shopDisplays = new List<Button>();

    //open the shop menu based what shop called this
    public void OpenShop(ShopInteract ts)
    {
        theShop = ts;
        ShopWindow.SetActive(true);

        Debug.Log("start: " + BooksToBuy.Count);


        CreateButtons();
        ClearCart();
        UpdateDisplay();

    }

    //close the shop
    public void CloseShop()
    {
        theShop = null;
        ShopWindow.SetActive(false);
    }

    //select a book
    public void PickBook(BookScript which, Button what)
    {
        if (BooksToBuy.Contains(which))
        {
            shopDisplays.Add(what);
            purchaseDisplays.Remove(what);

            BooksToBuy.Remove(which);
        }
        else
        {
            purchaseDisplays.Add(what);
            shopDisplays.Remove(what);
            BooksToBuy.Add(which);
        }
        UpdateDisplay();
    }

    public void ClearCart()
    {
        while (BooksToBuy.Count > 0)
        {
            BooksToBuy.Remove(BooksToBuy[0]);
        }
        while (purchaseDisplays.Count > 0)
        {
            purchaseDisplays.Remove(purchaseDisplays[0]);
        }
    }

    //update what books are available, selected, etc
    public void UpdateDisplay()
    {
        float z = 0;
        Debug.Log("COUNT: " + BooksToBuy.Count);
        if (BooksToBuy.Count > 0)
        {
            for (int a = 0; a < BooksToBuy.Count; a++)
            {
                float holder = BooksToBuy[a].currentPrice;
                z += holder;
            }
        }

        costDisplay.text = "Total: " + z.ToString();
        moneyDisplay.text = "Money: " + inventory.money.ToString();
        if (z > inventory.money)
        {
            costDisplay.color = Color.red;
            moneyDisplay.color = Color.red;
        }
        else
        {
            costDisplay.color = Color.black;
            moneyDisplay.color = Color.black;
        }

    }

    public void CreateButtons()
    {
        
        foreach(BookScript avaiB in theShop.availableBooks)
        {
            Button shopB = null;
            shopB = Instantiate(ShopButtonPrefab);
            shopB.GetComponent<ShopButtonDetailerScript>().GiveAnchors(InventoryDisplayPanel, ShopDisplayPanel, PurchaseDisplayPanel, this);
            shopB.GetComponent<ShopButtonDetailerScript>().ShopButtonDetail(avaiB);
            shopDisplays.Add(shopB);
            shopB.GetComponent<ShopButtonDetailerScript>().MoveToShop();
        }

        foreach(BookScript invenB in inventory.BooksInInventory)
        {
            Button invB = null;
            invB = Instantiate(ShopButtonPrefab);
            invB.GetComponent<ShopButtonDetailerScript>().GiveAnchors(InventoryDisplayPanel, ShopDisplayPanel, PurchaseDisplayPanel, this);
            invB.GetComponent<ShopButtonDetailerScript>().InventoryButtonDetail(invenB);
            inventoryDisplays.Add(invB);
            invB.GetComponent<ShopButtonDetailerScript>().MoveToInventory();
        }
        while (BooksToBuy.Count > 0)
        {
            BooksToBuy.Remove(BooksToBuy[0]);
        }
        Debug.Log("CB Count: " + BooksToBuy.Count);

    }

    //attempt to purchase the books
    public void BuyBooks()
    {
        float checkouttotal = 0;
        if (BooksToBuy.Count > 0)
        {
            foreach (BookScript purchase in BooksToBuy)
            {
                float ch = purchase.currentPrice;
                checkouttotal += ch;
            }
        }

        if (inventory.money < checkouttotal)
        {
            //NotEnoughMoneyWindow.SetActive(true);
            
            //BooksToBuy.RemoveRange(0, BooksToBuy.Count - 1);

        }
        else
        {
            inventory.money -= checkouttotal;
            DisplayPurchases();
        }
    }

    //show the player the purchases just made 
    public void DisplayPurchases()
    {
        //PurchaseDisplayWindow.SetActive(true);

        foreach(BookScript book in BooksToBuy)
        {
            //create a image showing bought book
            //Button bookDis = null;
            //bookDis = Instantiate(ShopButtonPrefab);
            //bookDis.transform.SetParent(PurchaseDisplayPanel.transform, false);

            //show the purchase details
            //purchaseDisplays.Add(bookDis);
            //bookDis.GetComponentInChildren<TextMeshProUGUI>().text = book.bookName;

            //shuffle the book around
            inventory.BooksInInventory.Add(book);
            inventory.UnsortedBooks.Add(book);
            theShop.availableBooks.Remove(book);
        }


        foreach(Button bookB in purchaseDisplays)
        {
            bookB.GetComponent<ShopButtonDetailerScript>().MoveToInventory();
            inventoryDisplays.Add(bookB);
        }

        //remove bought books
        ClearCart();

    }

    //exit any popups
    public void ExitPopups()
    {
        //NotEnoughMoneyWindow.SetActive(false);
        //PurchaseDisplayWindow.SetActive(false);
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
