using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Syateki;

public class AlignmentContorller : BestPracticeSingleton<AlignmentContorller> {

    private Alignment[] alignments;

    // Use this for initialization

    private void Awake()
    {
        alignments = new Alignment[GameManager.Instance.PlayabelePersons];

    }
    public void SetAlignment(int number, Alignment alignment){
        alignments[number - 1] = alignment;
    }

    public Alignment GetAlignment(int number){
        return alignments[number - 1];
    }
}
