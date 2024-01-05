using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopButtonDetailerScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI price;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI blurb;
    [SerializeField] Image bImage;
    [SerializeField] TextMeshProUGUI genre1Text;
    [SerializeField] TextMeshProUGUI genre2Text;

    public GameObject InventoryAnchor;
    public GameObject ShopAnchor;
    public GameObject CartAnchor;

    public BookScript theBook;

    //show the details of a book in the shop
    public void ShopButtonDetail(BookScript book)
    {
        theBook = book;
        //change dimensions to fit a shop button

        transform.SetParent(ShopAnchor.transform, false);

        //display details of selected book
        price.text = book.currentPrice.ToString();
        title.text = book.bookName;
        blurb.text = book.bookDesc;
        bImage = book.bookImage;
        genre1Text.text = book.bookGenres[0];
        if (book.bookGenres.Count > 0)
        {
            genre2Text.text = book.bookGenres[1];
        }
        else
        {
            genre2Text.GetComponent<GameObject>().SetActive(false);
        }
    }

    //display details of a book in the inventory
    public void InventoryButtonDetail(BookScript book)
    {
        theBook = book;
        //change dimensions to fit an inventory button

        transform.SetParent(InventoryAnchor.transform, false);

        price.GetComponent<GameObject>().SetActive(false);
        blurb.GetComponent<GameObject>().SetActive(false);

        title.text = book.bookName;
        bImage = book.bookImage;
        genre1Text.text = book.bookGenres[0];
        if (book.bookGenres.Count > 0)
        {
            genre2Text.text = book.bookGenres[1];
        }
        else
        {
            genre2Text.GetComponent<GameObject>().SetActive(false);
        }
    }

    //display details of a book in the player's cart
    public void CartButtonDetail(BookScript book)
    {
        theBook = book;
        //change dimensions to fit a cart button

        transform.SetParent(CartAnchor.transform, false);

        blurb.GetComponent<GameObject>().SetActive(false);

        //display details of selected book
        price.text = book.currentPrice.ToString();
        title.text = book.bookName;
        bImage = book.bookImage;
        genre1Text.text = book.bookGenres[0];
        if (book.bookGenres.Count > 0)
        {
            genre2Text.text = book.bookGenres[1];
        }
        else
        {
            genre2Text.GetComponent<GameObject>().SetActive(false);
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
