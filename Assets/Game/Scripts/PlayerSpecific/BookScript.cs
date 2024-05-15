using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]

public class BookScript : ScriptableObject
{
    //the book's name and description
    public string bookName;
    public string bookDesc;

    //a unique ID used for saving book loaded data (0 is blank)
    [Tooltip("This must be a unique number compared to other book IDs for saving to properly function.")]
    [Header("Ensure this ID is unique.")]
    public int bookId;
    [Space(10f)]

    public Image bookImage;

    //a list of the book's genres (~2?)
    public List<string> bookGenres = new List<string>();

    //the book's state - 0= good, 5= unreadable?
    public int bookState;

    //the book's rating
    public float bookBaseRating;
    public float bookActiveRating;

    public float basePrice;
    public float currentPrice;

}
