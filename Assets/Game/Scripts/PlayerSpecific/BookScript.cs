using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]

[CreateAssetMenu]

public class BookScript : ScriptableObject
{
    //the book's name and description
    public string bookName;
    public string bookDesc;

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


   
    // Start is called before the first frame update
    void Start()
    {
        //bookGenres = new List<string>();
        
    }
    /**
   // Update is called once per frame
   void Update()
   {
       
   }**/
}
