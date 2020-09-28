using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndingManager : MonoBehaviour
{
    public TextMeshProUGUI resultTxt;
    private void Start() 
    {
        resultTxt.text = UIManager.Instance.GetPoints().ToString();    
    }
}
