using System.Collections.Generic;

[System.Serializable]
public class ShelfData
{
    public int shelfId;
    public int shelfState;
    public int bookMovements;
    public List<BookScript> bookIds;

    public ShelfData()
    {
        this.shelfId = 0;
        this.shelfState = 0;
        this.bookMovements = 0;
        this.bookIds = new List<BookScript>();
    }

}
