//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class BackupCard : SelectStatus
//{

//    private Transform returnTable;
//    public BackupCard(RectTransform transform) : base(transform)
//    {
//        returnTable = transform.parent;
//    }

//    public override SelectStatus ReturnCard(out Transform table)
//    {
//        table = returnTable;
//        return new UnselectedCard(card);
//    }

//    public override SelectStatus SetUnselected(Transform cardTransform)
//    {
//        return new UnselectedCard(card);
//    }
//}