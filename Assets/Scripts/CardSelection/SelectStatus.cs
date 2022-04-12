using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class SelectStatus
{
    protected CardImage card;
    protected GameObject cardObject;
    protected SelectStatus(CardImage cardImage) 
    {
        card = cardImage;
    }

    abstract public SelectStatus ChangePosition();
    abstract public bool IsCardSelected();

}
