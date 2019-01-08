using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperationManager : MonoBehaviour {

    public List<OperationSequence> OperationLists;
    public int operationCount = 0;

    private Operation currentOperation;
    private GameObject currentTarget;
    private Transform initialTransform;

	// Use this for initialization
	void Start () {
        UpdateOperation();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateOperation ()
    {
        currentOperation = OperationLists[0].Operations[operationCount];
        currentTarget = currentOperation.Target;
        initialTransform = currentTarget.transform;
    }
}
