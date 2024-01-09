using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopButtonDetailerScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI price;
    [SerializeField] GameObject priceBG;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI blurb;
    [SerializeField] GameObject blurbObject;
    [SerializeField] Image bImage;
    [SerializeField] TextMeshProUGUI genre1Text;
    [SerializeField] GameObject genre1Object;
    [SerializeField] TextMeshProUGUI genre2Text;
    [SerializeField] GameObject genre2Object;

    private GameObject InventoryAnchor;
    private GameObject ShopAnchor;
    private GameObject CartAnchor;

    public ShopMenuManager shopManager;

    public BookScript theBook;

    //show the details of a book in the shop
    public void ShopButtonDetail(BookScript book)
    {
        theBook = book;
        //change dimensions to fit a shop button
        transform.SetParent(ShopAnchor.transform, false);

        blurbObject.SetActive(true);

        genre1Text.gameObject.transform.position = new Vector3(264f, 0f, genre1Text.gameObject.transform.position.z);

        if (book.bookGenres.Count > 1)
        {
            genre2Text.gameObject.transform.position = new Vector3(374f, 0f, genre2Text.gameObject.transform.position.z);
        }


        //display details of selected book
        price.text = book.currentPrice.ToString();
        title.text = book.bookName;
        blurb.text = book.bookDesc;
        bImage = book.bookImage;
        genre1Text.text = book.bookGenres[0];
        if (book.bookGenres.Count > 1)
        {
            genre2Object.SetActive(true);
            genre2Text.text = book.bookGenres[1];
        }
        else
        {
            genre2Object.SetActive(false);
        }
    }

    public void GiveAnchors(GameObject invenAnch, GameObject shopAnch, GameObject cartAnch, ShopMenuManager shopM)
    {
        InventoryAnchor = invenAnch;
        ShopAnchor = shopAnch;
        CartAnchor = cartAnch;
        shopManager = shopM;
    }

    //display details of a book in the inventory
    public void InventoryButtonDetail(BookScript book)
    {
        theBook = book;

        //change dimensions to fit an inventory button
        transform.SetParent(InventoryAnchor.transform, false);
        title.gameObject.transform.position = new Vector3(title.gameObject.transform.position.x+40, title.gameObject.transform.position.y, 0);

        genre1Text.gameObject.transform.position = new Vector3(genre1Text.gameObject.transform.position.x - 110, genre1Text.gameObject.transform.position.y - 45, 0);
        if (book.bookGenres.Count > 1)
        {
            genre2Text.gameObject.transform.position = new Vector3(genre2Text.gameObject.transform.position.x - 110, genre2Text.gameObject.transform.position.y - 45, 0);
        }

            priceBG.SetActive(false);
        blurbObject.SetActive(false);

        title.text = book.bookName;

        bImage = book.bookImage;
        genre1Text.text = book.bookGenres[0];

        if (book.bookGenres.Count > 1)
        {
            genre2Object.SetActive(true);
            genre2Text.text = book.bookGenres[1];
        }
        else
        {
            genre2Object.SetActive(false);
        }
    }

    //display details of a book in the player's cart
    public void CartButtonDetail(BookScript book)
    {
        theBook = book;
        transform.SetParent(CartAnchor.transform, false);

        //change dimensions to fit a cart button
        blurbObject.SetActive(false);
        title.gameObject.transform.position = new Vector3(title.gameObject.transform.position.x + 40, title.gameObject.transform.position.y, 0);

        genre1Text.gameObject.transform.position = new Vector3(genre1Text.gameObject.transform.position.x - 110, genre1Text.gameObject.transform.position.y - 45, 0);
        if (book.bookGenres.Count > 1)
        {
            genre2Text.gameObject.transform.position = new Vector3(genre2Text.gameObject.transform.position.x - 110, genre2Text.gameObject.transform.position.y - 45, 0);
        }

            //display details of selected book
            price.text = book.currentPrice.ToString();
        title.text = book.bookName;
        bImage = book.bookImage;

        genre1Text.text = book.bookGenres[0];
        if (book.bookGenres.Count > 1)
        {
            genre2Object.SetActive(true);
            genre2Text.text = book.bookGenres[1];
        }
        else 
        {
            genre2Object.SetActive(false);
        }
    }

    //button moved to inventory display
    public void MoveToInventory()
    {
        //no use for button in inventory display
        this.GetComponent<Button>().onClick.RemoveAllListeners();

        InventoryButtonDetail(theBook);
    }

    public void MoveToCart()
    {
        //remove past listeners
        this.GetComponent<Button>().onClick.RemoveAllListeners();
        this.GetComponent<Button>().onClick.AddListener(MoveToShop);

        CartButtonDetail(theBook);
    }

    public void MoveToShop()
    {
        //remove past listeners
        this.GetComponent<Button>().onClick.RemoveAllListeners();
        this.GetComponent<Button>().onClick.AddListener(MoveToCart);

        ShopButtonDetail(theBook);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
